using DarkRoom.AI;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.Game
{
	/// <summary>
	/// 场景单位的移动功能组件.
	/// 会组合一些其他具体移动功能组件(比如PathFollowComp)来实现单位的移动
	/// 自身是个组合模式. 会通过调用具体的行为来切换行为方式
	/// </summary>
	[RequireComponent(typeof(CUnitSpacialComp))]
	public class CPawnMovementComp : MonoBehaviour {

		/// <summary>
		/// 移动方式, 是mover的直接移动， 还是由rvo驱动的移动
		/// 对于Unknow(默认值), 的意思是外部操控
		/// </summary>
		public enum MoveType {
			Unknow, //外部操控
			Direct, //直接给速度, 然后移动
		}

		//本组件挂在谁身上
		protected CPawnEntity m_owner;

		//移动方式
		private MoveType m_moveType = MoveType.Direct;

		//临时让本组件停止运动
		private bool m_notControlMove = false;

		//运动时的速率
		private float m_speed = 10f;

		private CUnitSpacialComp m_spacial;

		//做具体运动的状态机
		private CStateMachine m_sm;

		/// <summary>
		/// 不带方向的速率, 从外面传进来的数据
		/// 比如从表或者经过技能影响的
		/// </summary>
		public float Speed { get { return m_speed; } }

		/// <summary>
		/// 获取最大速度, 我们的速度就是最大速度
		/// </summary>
		public float MaxSpeed { get { return Speed; } }

		/// <summary>
		/// 同步速度, 对速度进行赋值,
		/// 拆分speed的get,set是想表明, speed的来源是vo
		/// </summary>
		public void SyncSpeed(float value){
			m_speed = value;
			m_currState.SyncSpeed(value);
		}

		/// <summary>
		/// 本组件本帧的速度. 如果不受控制, 则是0速度
		/// </summary>
		public Vector3 Velocity {
			get{
				if (NotControlMove)return Vector3.zero;
				return m_currState.GetVelocity();
			}
		}

		/// <summary>
		/// 给外部调用的快捷方法
		/// </summary>
		public bool IsMoving {
			get { return !CMathUtil.ZeroVector3(Velocity); }
		}

		/// <summary>
		/// 本组件是否控制移动
		/// 比如把控制器临时叫出来
		/// </summary>
		protected bool NotControlMove
		{
			get { return m_notControlMove; }
			set { m_notControlMove = value; }
		}

		/// <summary>
		/// 当前的运动状态
		/// </summary>
		protected CPawnMovementBaseState m_currState{
			get { return m_sm.CurrState as CPawnMovementBaseState; }
		}

		void Awake()
		{
			m_spacial = GetComponent<CUnitSpacialComp>();
			if (m_spacial == null) {
				m_spacial = gameObject.AddComponent<CUnitSpacialComp>();
			}

			//默认我们用外部的移动器
			m_sm = new CStateMachine();
			m_sm.RegisterState(new CPawnMovementUnknow(gameObject));
			m_sm.RegisterState(new CPawnMovementDirectly(gameObject));
			m_sm.ChangeState(CPawnMovementUnknow.STATE);
        }

		void Start()
		{
			m_owner = GetComponent<CPawnEntity>();
		}

		/// <summary>
		/// 设置移动类型
		/// </summary>
		/// <param name="type"></param>
		public void SetMoveType(MoveType type)
		{
			switch (type) {
				case MoveType.Direct:
				m_sm.ForceChangeState(CPawnMovementDirectly.STATE);
                break;
				case MoveType.Unknow:
				m_sm.ForceChangeState(CPawnMovementUnknow.STATE);
				break;
			}

			m_moveType = type;
		}

		/// <summary>
		/// 开始奔跑, 传入单位速度矢量
		/// 但记住如果之前开始了不受控制, 那请关闭它
		/// </summary>
		/// <param name="value">方向矢量</param>
		/// <param name="changeDirection">是否更新朝向</param>
		public void Move(Vector3 value, bool changeDirection = true) {
			if (CMathUtil.ZeroVector3(value)) {
				Stop();
				return;
			}

			m_currState.SetVelicity(Speed * value);
			if (changeDirection)m_spacial.SetDirection(value);
		}

		/// <summary>
		/// 暂停行走, 速度置为0
		/// </summary>
		public void Stop() {
			m_currState.Stop();
		}

		/// <summary>
		/// 关闭自己的所有行为
		/// </summary>
		public void TurnOff()
		{
			NotControlMove = true;
			m_currState.Stop();
			m_currState.TurnOff();
		}

		/// <summary>
		/// 开启自己
		/// </summary>
		public void TurnOn()
		{
			NotControlMove = false;
			m_currState.TurnOn();
		}

		void LateUpdate(){
			if (NotControlMove || m_moveType == MoveType.Unknow)return;
			m_sm.Update();
		}

		void OnDestroy() {
			m_spacial = null;
			m_sm.Destroy();
		}
	}
}