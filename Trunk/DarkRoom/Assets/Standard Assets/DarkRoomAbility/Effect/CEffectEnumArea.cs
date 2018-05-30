using UnityEngine;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Core;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	public class CEffectEnumArea : CEffect
	{
		/// <summary>
		/// 本效果感兴趣的目标
		/// </summary>
		protected CController m_interested;

		protected CEffectEnumAreaMeta m_meta
		{
			get { return MetaBase as CEffectEnumAreaMeta; }
		}

		public override void Apply(CAIController from, CAIController to)
		{
			base.Apply(from, to);

			//Debug.Log("apply ceffect enum area");
			switch (m_meta.ImpactLocation) {
				case CAbilityEnum.Location.CasterUnit:
					SearchAndApply(m_owner, m_from, m_from.LocalPosition);
					break;
				case CAbilityEnum.Location.CasterPoint:
					SearchAndApply(m_owner, m_from, m_from.LocalPosition);
					break;
				case CAbilityEnum.Location.TargetUnit:
					SearchAndApply(m_owner, m_to, m_to.LocalPosition);
					break;
				case CAbilityEnum.Location.TargetPoint:
					SearchAndApply(m_owner, m_to, m_to.LocalPosition);
					break;
				case CAbilityEnum.Location.TargetDirection:
					break;
			}
		}

		//说实话, 这里不知道怎么样, 先放着
		public override void Apply(CAIController from, Vector3 to) {
			Debug.LogError("EffectEnumArea must have a unit to param");

			switch (m_meta.ImpactLocation) {
				case CAbilityEnum.Location.CasterUnit:
					break;
				case CAbilityEnum.Location.CasterPoint:
					break;
				case CAbilityEnum.Location.TargetUnit:
					break;
				case CAbilityEnum.Location.TargetPoint:
					break;
				case CAbilityEnum.Location.TargetDirection:
					break;
			}
		}

		private void SearchAndApply(CAIController owner, CAIController target, Vector3 center)
		{
			List<CController> list = null;
			switch (m_meta.SearchMethod) {
					case CEffectEnumAreaMeta.Method.TeamAll:
					list = target.SearchUnits();
                    break;
			case CEffectEnumAreaMeta.Method.RandomInRange:
				list = target.SearchUnits ();
				if (list.Count > 1) {
					int i = CDarkRandom.Next (0, list.Count - 1);
					m_interested = list [i];
					list = owner.SearchUnits (m_interested.LocalPosition, m_meta.Area.Radius);
				}
					
					break;
					case CEffectEnumAreaMeta.Method.ImpactRange:
					list = owner.SearchUnits(center, m_meta.Area.Radius);
					break;
			}

			if (list != null) {
				foreach (var item in list) {
					if (item.Pawn.DeadOrDyingOrInvalid) continue;
					CEffect.DefaultCreateAndApply(m_meta.Area.Effect, owner, item as CAIController);
				}
			}

			if (m_interested != null) {
				CAIMessage m = new CAIMessage (0, "NotiyEffectCallBack", m_owner, m_interested);
				CAIMessage.Send(m_owner, m);
			}

		}
	}
}
