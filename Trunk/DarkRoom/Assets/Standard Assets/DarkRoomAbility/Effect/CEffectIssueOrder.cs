using UnityEngine;
using DarkRoom.AI;
using DarkRoom.Core;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 让单位执行指令的效果--比如行走到某地
	/// </summary>
	public class CEffectIssueOrder : CEffect
	{
		private CTimeRegulator m_reg = new CTimeRegulator(1f);

		//指令是否准备好
		private bool m_orderReady = false;

		protected CEffectIssueOrderMeta m_meta{
			get { return MetaBase as CEffectIssueOrderMeta; }
		}

		protected override void Start(){
			m_reg.Period = m_meta.Delay;
			m_orderReady = false;
		}

		//预处理指令, 获取关键数据
		private void PreProcessOrder(){
			m_orderReady = true;
			switch (m_meta.EffectOrder) {
			case CAbilityEnum.Order.DelayEffect:
				m_orderReady = false;
				if (m_meta.Delay <= 0)m_meta.Delay = 1f; //做数据保护
				break;
			case CAbilityEnum.Order.NotifyEffect:
				m_orderReady = false;
				//m_owner.ObserveMessage ("NotiyEffect", OnNotify);
				break;
			}
		}

		//目前只测试了, which unit = caster
		/*public override void Apply(CAIController from, CAIController to){
			PreProcessOrder();
			base.Apply(from, to);
			if (m_orderReady)PostApplyCtrl();
		}

		public override void Apply(CAIController from, Vector3 to) {
			PreProcessOrder();
			base.Apply(from, to);
			if (m_orderReady)PostApplyPosition();
		}

		private void PostApplyCtrl()
		{
			switch (m_meta.WhichUnit) {
				case CAbilityEnum.Location.TargetUnit:
					DoOrder(m_owner, m_to, m_to);
					break;
				case CAbilityEnum.Location.TargetPoint:
					DoOrder(m_owner, m_to.LocalPosition);
					break;
				case CAbilityEnum.Location.TargetDirection:
					Vector3 dir = m_to.LocalPosition - m_owner.LocalPosition;
					DoOrder(m_owner, m_to.LocalPosition + dir * 100f);
					break;
				case CAbilityEnum.Location.CasterPoint:
					DoOrder(m_owner, m_from.LocalPosition);
					break;
				case CAbilityEnum.Location.CasterUnit:
					DoOrder(m_owner, m_from, m_to);
					break;
				}
		}

		private void PostApplyPosition() 
		{
			switch (m_meta.WhichUnit) {
				case CAbilityEnum.Location.TargetPoint:
					DoOrder(m_owner, m_toPos);
					break;
				case CAbilityEnum.Location.TargetUnit:
					DoOrder(m_owner, m_to, m_to);
					break;
				case CAbilityEnum.Location.TargetDirection:
					Vector3 dir = m_toPos - m_owner.LocalPosition;
					m_toPos = m_toPos + dir * 2f;
					DoOrder(m_owner, m_toPos);
					break;
				case CAbilityEnum.Location.CasterPoint:
					m_toPos = m_from.LocalPosition;
					DoOrder(m_owner, m_toPos);
					break;
				case CAbilityEnum.Location.CasterUnit:
					DoOrder(m_owner, m_from, m_to);
					break;
			}
		}

		private void DoOrder(CAIController owner, CAIController onWhich, CAIController originTarget) {
			switch (m_meta.EffectOrder) {
				case CAbilityEnum.Order.Move:
					owner.MoveToLocation(onWhich.LocalPosition);
					break;
				case CAbilityEnum.Order.StopMove:
					owner.Pawn.StopMovement();
					break;
				case CAbilityEnum.Order.DelayEffect:
					CEffect.DefaultCreateAndApply(m_meta.Param, onWhich, originTarget);
					break;
				case CAbilityEnum.Order.NotifyEffect:
					var e = CEffect.DefaultCreateAndApply (m_meta.Param, onWhich, originTarget);
					e.Index = Index;
					break;
			}
		}

		private void DoOrder(CAIController owner, Vector3 target) {
			switch (m_meta.EffectOrder) {
				case CAbilityEnum.Order.Move:
					owner.MoveToLocation(target);
					break;
				case CAbilityEnum.Order.StopMove:
					owner.Pawn.StopMovement();
					break;
				case CAbilityEnum.Order.DelayEffect:
					CEffect.DefaultCreateAndApply(m_meta.Param, owner, target);
					break;
				case CAbilityEnum.Order.NotifyEffect:
				var e = CEffect.DefaultCreateAndApply(m_meta.Param, owner, target);
				e.Index = Index;
					break;
			}
		}

		private void OnNotify(CAIMessage message){
			int i = (int)message.Data;
			if (i != Index)return;
			m_owner.UnobserveMessage("NotiyEffect", OnNotify);


			m_orderReady = true;
			if (m_to != null)PostApplyCtrl();
			else PostApplyPosition();
			JobDown();
		}

		//时间到了, 自己删除自己
		protected override void Update(){
			if (m_meta.Delay <= 0)return;
			if (m_orderReady)return;

			bool b = m_reg.Update();
			if (!b)return;

			m_orderReady = true;
			if (m_to != null)PostApplyCtrl();
			else PostApplyPosition( );
			JobDown();
		}

		/// <summary>
		/// 在有指令延迟执行时间的情况下, 不能立刻销毁
		/// </summary>
		public override void JobDown(){
			if (m_orderReady) base.JobDown();
		}*/
	}
}
