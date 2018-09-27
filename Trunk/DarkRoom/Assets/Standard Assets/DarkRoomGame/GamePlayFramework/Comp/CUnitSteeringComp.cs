using System.Collections.Generic;
using DarkRoom.AI;
using UnityEngine;

namespace DarkRoom.Game
{
	/// <summary>
	/// CUnitSteeringComp ֻ�ṩ��Ϊ����, ���ǲ�����ȥupdate����������Ϊ
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

		//��������Щ��Ϊ�������һ��
		private int m_flags = 0;

		//���ٿ���, Ӧ����ϸ����. ������ٶ��й�ϵ. Ӧ��
		//��Ŀǰ��������ֵΪ1ͦ�õ�
		private const float m_maxForce = 1f;

		//�Ը�����Ϊ����������Ȩ�ص���
		private float m_weightSeek = 2f;
		private float m_weightFlee = 1f;
		private float m_weightFollowPath = 4f;


		//private float m_weightAlignment = 1f;
		//������ֵ�ǵ������о�����
		private float m_weightSeparation = 2f;
		private float m_weightCohesion = 1f;

		//��ǰ֡����ĺ���
		private Vector3 m_steeringForce = Vector3.zero;

		//ֻ�п������ǲŻ῿���Ͷ���
		private float m_inSightDist = 4f;
		//ֻ��̫����ʱ��, ���ǲŻῼ��Զ��, ��������������
		private float m_tooCloseDist = 4f;

		//����ռ����
		private CUnitSpacialComp m_spacial;
		//�����ƶ����
		private CPawnMovementComp m_mover;
		//����·������
		private CPawnPathFollowingComp m_pathFollower;

		private bool m_idleInRVO = false;

		private int m_agent;

		/// <summary>
		/// ��ǰ�ٶ�
		/// </summary>
		public Vector3 Velocity {
			get { return m_mover.Velocity; }
		}

		/// <summary>
		/// ��ǰλ��
		/// </summary>
		public Vector3 Position {
			get { return m_spacial.localPosition; }
		}

		/// <summary>
		/// ��ǰ����, �����ϳ���Ӧ�ú��ٶ�һ��
		/// </summary>
		//public Vector3 Heading {
		//	get { return m_spacial.direction; }
		//}

		/// <summary>
		/// ���ʹ���˸���·��,ÿ֡�����и���״̬�ı仯
		/// </summary>
		public CPawnPathFollowingComp.FinishResultType PathFinishResult {
			get { return m_pathFollower.FinishResult; }
		}

		//����ٶ�
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
		/// ���õ�λ�����С����
		/// </summary>
		public void SetLeastDistanceBetweenUnits(float value) { m_tooCloseDist = value * value; }

		/// <summary>
		/// ����Ⱥ����Ϊ, ���ڹ���Ӧ��, һ�����Ǻ͸���·��һ��ʹ��.
		/// �������ǲ����������. ����·����Ӱ�����
		/// </summary>
		public void TurnOnFlock(float weight)
		{
			m_flags |= (int)(SteeringType.FLock);

			//�����ǺϾ۵������ǵ�����������ֵ. �о�����
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
		/// �ر����е���Ϊ
		/// </summary>
		public void TurnOffSteering()
		{
			m_flags = 0;
			m_pathFollower.ResetFinishResult();
		}

		/// <summary>
		/// ���㵱ǰ֡�ĺ���, ����Ҫ����Ŀ������Ϊ Ŀ����ǵ�ǰ����·���Ľڵ�
		/// �������Ҫ���� PathFollow
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

		//Ŀǰ����Ϊ�Ƿ������״̬
		private bool On(SteeringType type)
		{
			int value = (int) type;
			return (m_flags & value) == value;
		}

		// �������ȼ���Ȩ�ĺ���
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

		//�����ܷ����������
		//TODO ��������ƽ����, ��ͷ�Ż�
		private bool AccumulateForce(Vector3 forceToAdd)
		{
			//����Ŀǰ���˶��ٲٿ���
			float forceSoFar = m_steeringForce.sqrMagnitude;

			//���㻹�ж���
			float forceRemain = m_maxForce - forceSoFar;
			if (forceRemain <= 0f) return false;
			
			//�������
			float toAdd = forceToAdd.sqrMagnitude;
			if (toAdd < forceRemain) {
				m_steeringForce += forceToAdd;
			} else {
				m_steeringForce += forceToAdd.normalized * forceRemain;
			}

			return true;
		}

		//��ȡ��Ѱtarget���Ŀ����Ҫ����
		//ԭ����������Ŀ��������, Ȼ���ȥĿǰ���ٶȷ���, ������Ҫʩ�ӵ���
		private Vector3 Seek(Vector3 target)
		{
			Vector3 desiredVelocity = target - Position;
			desiredVelocity = desiredVelocity.normalized * m_maxSpeed;

			Vector3 force = desiredVelocity + Velocity;
			return force;
		}


		//��Seek�෴
		private Vector3 Flee(Vector3 target)
		{
			Vector3 desiredVelocity = target - Position;
			desiredVelocity = desiredVelocity.normalized * m_maxSpeed;

			Vector3 force = desiredVelocity - Velocity;
			return force;
		}


		//����Ⱥ��Ҫ�����
		private Vector3 Separation(List<CUnitSteeringComp> neighbors)
		{
			Vector3 force = Vector3.zero;

			for (int a = 0; a < neighbors.Count; a++) {
				CUnitSteeringComp item = neighbors[a];
				if (item == this) continue;
				if (!TooClose(item.Position)) continue;

				Vector3 toAgent = Position - item.Position;
				//���������. ���Զ���neighbors��Ҫ����ɸѡ
				//���Ĵ�С���� ���ھӵľ���
				force += toAgent.normalized;
			}

			return force;
		}

		//�������--Ŀǰ�ò���
		/*private Vector3 Alignment(List<CUnitSteeringComp> neighbors)
		{
			//�洢ƽ������
			Vector3 averageHeading = Vector3.zero;

			//�洢��Ҫ������˵ĸ���
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

		//����ۺ�. �ҵ�����, Ȼ���ҹ�ȥ
		private Vector3 Cohesion(List<CUnitSteeringComp> neighbors)
		{
			//���������
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

		//�Ƿ����ҵ���Ұ����
		private bool InSight(Vector3 vehicle) {
			float dist = Vector3.Distance(Position, vehicle);
			if (dist > m_inSightDist) return false;


			Vector3 heading = Velocity.normalized;
			Vector3 difference = vehicle - Position;
			float dotProd = Vector3.Dot(difference, heading);

			if (dotProd < 0) return false;
			return true;
		}

		// ������λ�Ƿ�̫��
		private bool TooClose(Vector3 vehicle) {
			float dist = Vector3.SqrMagnitude(Position - vehicle);
			return dist < m_tooCloseDist;
		}
	}
}