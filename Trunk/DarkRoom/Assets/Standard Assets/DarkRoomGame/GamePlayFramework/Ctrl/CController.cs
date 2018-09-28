using System.Collections.Generic;
using DarkRoom.AI;
using UnityEngine;

namespace DarkRoom.Game
{
	/// <summary>
	/// 跟UE不同. 没有继承CUnitEntity -- 因为复杂度没有那么高
	/// mvc中的控制器. 接收输入控制pawn的各种行为
	/// 比如行走, 攻击和施法, 交互等
	/// 对于一些可替换的逻辑，或者智能决策的，就应该归Controller管辖
	/// controller -> manipulate -> model
	/// controller -> fire event -> view
	/// model -> update -> view
	/// 
	/// 同时也是个门面, 管理各个组件并提供代理功能
	/// 
	/// 根据我们的设定, 
	/// 每个controller必须有pawn -- 目的是为了简化设计!!!!!!! IMPORTANT
	/// 
	/// TODO
	/// 如果以逻辑来划分游戏，得到的就是一个个World的概念；如果以表示来划分，得到就是一个个Level
	/// </summary>

	[RequireComponent(typeof(CPawnPathFollowingComp))]
	public class CController : MonoBehaviour, IHaveLocalPosition, ICanSearchUnits
	{
		/// <summary>
		/// 创建pawn成功后的回调
		/// </summary>
		//public event EventHandler<XEventArgs> OnPawnCreatedCallBack;

		/// <summary>
		/// pawn死亡的通知
		/// 比如猎人死了, 要带走一个
		/// </summary>
		//public event EventHandler<XEventArgs> OnPawnDieCallBack;

		//控制着的pawn视图
		protected CPawnEntity m_pawn;

		//是否是player controller
		protected bool m_isPlayerController = false;

		/// <summary>
		/// controller持有的view对象
		/// </summary>
		public CPawnEntity Pawn {
			get { return m_pawn; }
		}

		/// <summary>
		/// 返回controller的位置. 快捷引用
		/// </summary>
		public virtual Vector3 LocalPosition {
			get { return m_pawn.LocalPosition; }
		}

		protected virtual void Awake()
		{
			//我们在这里视图读取下CPawnEntity, 但一般我们的子类都有对应的具体的CPawnEntity子类
			//我们在子类里面做具体的CPawnEntity required
			m_pawn = GetComponent<CPawnEntity>();
		}

		protected virtual void Start(){}

		protected virtual void Update(){}

		protected virtual void LateUpdate(){}

		protected virtual void OnDestroy() {
			//OnPawnCreatedCallBack = null;
			//OnPawnDieCallBack = null;

			m_pawn = null;
		}

		/// <summary>
		/// 初始化pawn的位置和旋转
		/// </summary>
		/// <param name="localLocation">位置</param>
		/// <param name="localRotation">旋转, 注意!rotation是基于NDC空间</param>
		public void SetInitialLocationAndRotation(Vector3 localLocation, Vector3 localRotation)
		{
			m_pawn.SpacialComp.SetLocalPos(localLocation);
			m_pawn.SpacialComp.SetDirection(localRotation);
        }

		/// <summary>
		/// 遭受到伤害, 具体的伤害计算写在这里
		/// 不要忘记调用伤害回调
		/// </summary>
		public virtual void InstigatedAnyDamage(float damage, CDamagePacket damageType, CController instigatedBy, CController damageCauser)
		{
		}

		/// <summary>
		/// 搜索在center周围radius范围内的单位
		/// </summary>
		public virtual List<CController> SearchUnits(Vector3 center, float radius)
		{
			return new List<CController>();
		}

		/// <summary>
		/// 寻找我整个团队的成员
		/// </summary>
		public virtual List<CController> SearchUnits()
		{
			return new List<CController>();
		}

		/// <summary>
		/// 重置mvc到初始状态
		/// </summary>
		public virtual void Reset()
		{
			
		}

		/// <summary>
		/// 通过导航系统移动到某个地点
		/// IMPORTANT, 本方法会打断之前的运动. 导致会有一帧的停顿
		/// 之前遇到bug, 有单位使用他的速度进行更新. 但因为这一帧的停顿.导致位移有累计差错
		/// </summary>
		/// <param name="goal"></param>
		public virtual void MoveToLocation(Vector3 goal)
		{
			CTileNavigationSystem.Instance.SimpleMoveToLocation(this, goal);
		}

		/// <summary>
		/// 跟随路径点行走
		/// </summary>
		/// <param name="wayPoints"></param>
		public virtual void MoveWithPath(CTilePathResult wayPoints)
		{
			m_pawn.Follower.RequestMove(wayPoints);
		}

		/// <summary>
		/// 获取两个角色间距离的平方, 要考虑自己的半径
		/// </summary>
		public float GetSquaredDistanceTo(CController otherActor) {
			float d = Vector3.SqrMagnitude(otherActor.LocalPosition - LocalPosition);
			float c = d - m_pawn.Radius - otherActor.Pawn.Radius;
			if (c < 0) c = 0;
			return c * c;
		}


		/// <summary>
		/// 忽略y轴的两个角色的距离的平方, 要考虑自己的半径
		/// </summary>
		public float GetSquaredXZDistanceTo(CController otherActor) {
			Vector3 a = otherActor.LocalPosition;
			Vector3 b = LocalPosition;
			a.y = 0;
			b.y = 0;
			//减去两者的半径
			float c = Vector3.Magnitude(a - b);
			c = c - m_pawn.Radius - otherActor.Pawn.Radius;
			if (c < 0) c = 0;
			return c * c;
		}

		/// <summary>
		/// 忽略y轴的两个角色的距离的平方, 要考虑自己的半径
		/// </summary>
		public float GetSquaredXZDistanceTo(Vector3 pos) {
			Vector3 a = pos;
			Vector3 b = LocalPosition;
			a.y = 0;
			b.y = 0;

			float c = Vector3.Magnitude(a - b);
			c = c - m_pawn.Radius;
			if (c < 0) c = 0;
			return c * c;
		}

		/// <summary>
		/// 忽略y轴的两个角色的距离的平方, 要考虑自己的半径
		/// </summary>
		public float GetSquaredXZDistanceTo_NoRadius(CController otherActor){
			return GetSquaredXZDistanceTo_NoRadius(otherActor.LocalPosition);
		}

		/// <summary>
		/// 忽略y轴的两个角色的距离的平方, 不考虑自己的半径
		/// </summary>
		public float GetSquaredXZDistanceTo_NoRadius(Vector3 otherActor) {
			Vector3 a = otherActor;
			Vector3 b = LocalPosition;
			a.y = 0;
			b.y = 0;
			float c = Vector3.SqrMagnitude(a - b);
			return c;
		}

		public bool SameTeam(CController target){
			return Pawn.Team == target.Pawn.Team;
		}
	}
}