using DarkRoom.AI;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.Game {
	public class CUnitMovementBaseState : CBaseState
	{
		protected CUnitSpacialComp m_spacial;
		protected Vector3 m_velocity = Vector3.zero;

		protected GameObject m_go {
			get { return m_data as GameObject; }
		}

		public CUnitMovementBaseState(string name, GameObject go) : base(name, go)
		{
			m_spacial = go.GetComponent<CUnitSpacialComp>();
		}

		public virtual void TurnOn()
		{
			
		}

		public virtual void TurnOff()
		{
			
		}

		public virtual void Stop() {
			m_velocity = Vector3.zero;
        }

		/// <summary>
		/// 获取当前运动速度
		/// </summary>
		/// <returns></returns>
		public virtual Vector3 GetVelocity()
		{
			return m_velocity;
		}

		/// <summary>
		/// 给当前状态设置运动速度
		/// </summary>
		/// <param name="velocity"></param>
		public void SetVelicity(Vector3 velocity)
		{
			m_velocity = velocity;
		}

		public virtual void SyncSpeed(float value)
		{
			Vector3 v = m_velocity;
			if (!CMathUtil.ZeroVector3(v)) {
				SetVelicity(value * v.normalized);
			}
		}

		public override void Destroy()
		{
			base.Destroy();
			m_spacial = null;
		}
	}

	/// <summary>
	/// 移动器不知道用什么方式来移动. 
	/// 典型的原因是外部在直接操控transform来移动
	/// </summary>
	public class CUnitMovementUnknow : CUnitMovementBaseState {
		public const string STATE = "CUnitMovementUnknow";

		public CUnitMovementUnknow(GameObject go) : base(STATE, go) { }
	}

	/// <summary>
	/// 直接移动的状态, 就是传入速度方向. 然后进行移动
	/// </summary>
	public class CUnitMovementDirectly : CUnitMovementBaseState {
		public const string STATE = "CUnitMovementDirectly";

		public CUnitMovementDirectly(GameObject go) : base(STATE, go) { }

		public override void Enter(CStateMachine sm) {
		}

		public override void Execute(CStateMachine sm) {
			//重新计算v, 因为有可能会更改速度
			Vector3 v = m_velocity * Time.deltaTime;

			//坐标只在平面上移动, y轴的坐标由物理引擎来控制
			m_spacial.Translate(v);
		}

		public override void Exit(CStateMachine sm)
		{
		}
	}
}
