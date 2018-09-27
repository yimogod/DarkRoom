using System.Collections.Generic;
using DarkRoom.AI;
using UnityEngine;

namespace DarkRoom.Game
{
	/// <summary>
	/// CUnitSteeringComp 只提供行为方法, 我们不自身去update产生具体行为
	/// </summary>
	[RequireComponent(typeof(CUnitSpacialComp))]
	[RequireComponent(typeof(CPawnMovementComp))]
	[RequireComponent(typeof(CPawnPathFollowingComp))]
	public class CUnitSteeringComp : MonoBehaviour
	{
		enum SteeringType
		{
			None = 0x00000,
			Seek = 0x00002,
			Flee = 0x00004,
			Arrive = 0x00008,
			Wander = 0x00010,
			FLock = 0x00020,
			PathFollow = 0x00100,
		}

		//现在有哪些行为组合在了一起
		private int m_flags = 0;

		//最大操控力, 应该仔细调教. 跟最大速度有关系. 应该
		//但目前开起来总值为1挺好的
		private const float m_maxForce = 1f;

		//对各种行为产生的力的权重调整
		private float m_weightSeek = 2f;
		private float m_weightFlee = 1f;
		private float m_weightFollowPath = 4f;


		//private float m_weightAlignment = 1f;
		//这两个值是调整过感觉不错
		private float m_weightSeparation = 2f;
		private float m_weightCohesion = 1f;

		//当前帧计算的合力
		private Vector3 m_steeringForce = Vector3.zero;

		//只有看见我们才会靠近和对齐
		private float m_inSightDist = 4f;
		//只有太近的时候, 我们才会考虑远离, 质心离质心两米
		private float m_tooCloseDist = 4f;

		//缓存空间组件
		private CUnitSpacialComp m_spacial;
		//缓存移动组件
		private CPawnMovementComp m_mover;
		//跟随路径行走
		private CPawnPathFollowingComp m_pathFollower;

		private bool m_idleInRVO = false;

		private int m_agent;

		/// <summary>
		/// 当前速度
		/// </summary>
		public Vector3 Velocity {
			get { return m_mover.Velocity; }
		}

		/// <summary>
		/// 当前位置
		/// </summary>
		public Vector3 Position {
			get { return m_spacial.localPosition; }
		}

		/// <summary>
		/// 当前朝向, 理论上朝向应该和速度一样
		/// </summary>
		//public Vector3 Heading {
		//	get { return m_spacial.direction; }
		//}

		/// <summary>
		/// 如果使用了跟随路径,每帧都会有跟随状态的变化
		/// </summary>
		public CPawnPathFollowingComp.FinishResultType PathFinishResult {
			get { return m_pathFollower.FinishResult; }
		}

		//最大速度
		private float m_maxSpeed {
			get { return m_mover.MaxSpeed; }
		}

		void Awake()
		{
			m_spacial = gameObject.GetComponent<CUnitSpacialComp>();
			if (m_spacial == null) {
				m_spacial = gameObject.AddComponent< CUnitSpacialComp>();
			}

			m_mover = gameObject.GetComponent<CPawnMovementComp>();
			if (m_mover == null) {
				m_mover = gameObject.AddComponent<CPawnMovementComp>();
			}

			m_pathFollower = gameObject.GetComponent<CPawnPathFollowingComp>();
			if (m_pathFollower == null) {
				m_pathFollower = gameObject.AddComponent<CPawnPathFollowingComp>();
			}
		}

		/// <summary>
		/// 设置单位间的最小距离
		/// </summary>
		public void SetLeastDistanceBetweenUnits(float value) { m_tooCloseDist = value * value; }

		/// <summary>
		/// 开启群落行为, 基于工程应用, 一般我们和跟随路径一起使用.
		/// 所以我们不做对齐操作. 跟随路径会影响对齐
		/// </summary>
		public void TurnOnFlock(float weight)
		{
			m_flags |= (int)(SteeringType.FLock);

			//分离是合聚的两倍是调整出来的数值. 感觉不错
			m_weightSeparation = weight * 0.66f;
			m_weightCohesion = weight * 0.33f;
		}

		public void TurnOnPathFollow(CPathResult wayPoints, float weight)
		{
			m_flags |= (int)(SteeringType.PathFollow);
			m_weightFollowPath = weight;
			m_pathFollower.RequestMove(wayPoints);
		}

		/// <summary>
		/// 关闭所有的行为
		/// </summary>
		public void TurnOffSteering()
		{
			m_flags = 0;
			m_pathFollower.ResetFinishResult();
		}

		/// <summary>
		/// 计算当前帧的合力, 不需要传入目标是因为 目标就是当前跟随路径的节点
		/// 所以这个要求开启 PathFollow
		/// </summary>
		public Vector3 Calculate(List<CUnitSteeringComp> neighbors)
		{
			if (m_flags == 0) return Vector3.zero;

			Vector3 target = m_pathFollower.StepDestinationForNow;
			m_steeringForce = Vector3.zero;
			m_steeringForce = CalculatePrioritized(target, neighbors);
			m_steeringForce = Vector3.ClampMagnitude(m_steeringForce, m_maxForce);

			return m_steeringForce;
		}

		//目前的行为是否有这个状态
		private bool On(SteeringType type)
		{
			int value = (int) type;
			return (m_flags & value) == value;
		}

		// 计算优先级加权的合理
		private Vector3 CalculatePrioritized(Vector3 target, List<CUnitSteeringComp> neighbors)
		{
			Vector3 force = Vector3.zero;

			if (On(SteeringType.PathFollow)) {
				force = m_pathFollower.ForceVector * m_weightFollowPath;
				if (!AccumulateForce(force)) return m_steeringForce;
			}

			if (On(SteeringType.Flee)) {
				force = Flee(target) * m_weightFlee;
				if (!AccumulateForce(force)) return m_steeringForce;
			}


			if (On(SteeringType.FLock)) {
				force = Separation(neighbors) * m_weightSeparation;
                force += Cohesion(neighbors) * m_weightCohesion;
				if (!AccumulateForce(force)) return m_steeringForce;
			}

			if (On(SteeringType.Seek)) {
				force = Seek(target) * m_weightSeek;
				if (!AccumulateForce(force)) return m_steeringForce;
			}

			return m_steeringForce;
		}

		//计算能否添加新力了
		//TODO 这里用了平方根, 回头优化
		private bool AccumulateForce(Vector3 forceToAdd)
		{
			//计算目前用了多少操控力
			float forceSoFar = m_steeringForce.sqrMagnitude;

			//计算还有多少
			float forceRemain = m_maxForce - forceSoFar;
			if (forceRemain <= 0f) return false;
			
			//添加新力
			float toAdd = forceToAdd.sqrMagnitude;
			if (toAdd < forceRemain) {
				m_steeringForce += forceToAdd;
			} else {
				m_steeringForce += forceToAdd.normalized * forceRemain;
			}

			return true;
		}

		//获取搜寻target这个目标需要的力
		//原理就是算出里目标点的向量, 然后减去目前的速度方向, 就是需要施加的力
		private Vector3 Seek(Vector3 target)
		{
			Vector3 desiredVelocity = target - Position;
			desiredVelocity = desiredVelocity.normalized * m_maxSpeed;

			Vector3 force = desiredVelocity + Velocity;
			return force;
		}


		//与Seek相反
		private Vector3 Flee(Vector3 target)
		{
			Vector3 desiredVelocity = target - Position;
			desiredVelocity = desiredVelocity.normalized * m_maxSpeed;

			Vector3 force = desiredVelocity - Velocity;
			return force;
		}


		//各个群体要求独立
		private Vector3 Separation(List<CUnitSteeringComp> neighbors)
		{
			Vector3 force = Vector3.zero;

			for (int a = 0; a < neighbors.Count; a++) {
				CUnitSteeringComp item = neighbors[a];
				if (item == this) continue;
				if (!TooClose(item.Position)) continue;

				Vector3 toAgent = Position - item.Position;
				//这个很慢啊. 所以对于neighbors还要进行筛选
				//力的大小反比 与邻居的距离
				force += toAgent.normalized;
			}

			return force;
		}

		//队伍对齐--目前用不到
		/*private Vector3 Alignment(List<CUnitSteeringComp> neighbors)
		{
			//存储平均方向
			Vector3 averageHeading = Vector3.zero;

			//存储需要对齐的人的个数
			int neighborCount = 0;
			for (int a = 0; a < neighbors.Count; a++) {
				CUnitSteeringComp item = neighbors[a];
				if (item == this) continue;
				if (!InSight(item.Position)) continue;

				averageHeading += item.Heading;
				neighborCount++;
			}

			if (neighborCount > 0) {
				averageHeading /= neighborCount;
				averageHeading -= Heading;
			}

			return averageHeading;
		}*/

		//队伍聚合. 找到质心, 然后找过去
		private Vector3 Cohesion(List<CUnitSteeringComp> neighbors)
		{
			//队伍的质心
			Vector3 centerOfMass = Vector3.zero;
			Vector3 force = Vector3.zero;

			int neighborCount = 0;
			for (int a = 0; a < neighbors.Count; a++) {
				CUnitSteeringComp item = neighbors[a];
				if (item == this) continue;
				if (!InSight(item.Position))continue;

				centerOfMass += item.Position;

				neighborCount++;
			}

			if (neighborCount > 0) {
				centerOfMass /= neighborCount;
				force = Seek(centerOfMass);
			}

			return force.normalized;
		}

		//是否在我的视野里面
		private bool InSight(Vector3 vehicle) {
			float dist = Vector3.Distance(Position, vehicle);
			if (dist > m_inSightDist) return false;


			Vector3 heading = Velocity.normalized;
			Vector3 difference = vehicle - Position;
			float dotProd = Vector3.Dot(difference, heading);

			if (dotProd < 0) return false;
			return true;
		}

		// 两个单位是否太近
		private bool TooClose(Vector3 vehicle) {
			float dist = Vector3.SqrMagnitude(Position - vehicle);
			return dist < m_tooCloseDist;
		}
	}
}