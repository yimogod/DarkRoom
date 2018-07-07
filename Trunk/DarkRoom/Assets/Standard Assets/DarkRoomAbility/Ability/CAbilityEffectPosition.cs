using System;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.GamePlayAbility {
	public class CAbilityEffectPosition : CAbility
	{
		private CAbilityEffectTargetMeta m_meta{
			get{ return MetaBase as CAbilityEffectTargetMeta; }
		}

		protected override void Start()
		{
			
		}

	    protected override void Activate()
		{
            //作用于目标
		    if (m_target != null)
		    {
		        m_owner.AbilitySystem.ApplyGameplayEffectToTarget(null, m_target);
            }
		    else //作用于地址
		    {
		        m_owner.AbilitySystem.ApplyGameplayEffectToPosition(null, m_targetLocalPosition);
            }
			
		}

		public override AffectDectectResult CanAffectOnTarget(IGameplayAbilityUnit target){
			AffectDectectResult result = base.CanAffectOnTarget(target);
			if (result != AffectDectectResult.Success)return result;

			if (m_meta.Range > 0) {
				float dist = m_owner.GetSquaredXZDistanceTo_NoRadius(target);
				if (dist > (m_meta.Range * m_meta.Range))
					return AffectDectectResult.OutOfRange;
			}

			return AffectDectectResult.Success;
		}

		public override AffectDectectResult CanAffectOnTarget(Vector3 target) {
			AffectDectectResult result = base.CanAffectOnTarget(target);
			if (result != AffectDectectResult.Success) return result;

			if (m_meta.Range > 0) {
				//技能不考虑自己和对方的半径, 否则在AI阶段如果是以
				//CanAffectOnTarget(CController target)为考量, 这样考虑到了对方的半径
				//但具体技能是以地点为目标, 使用的是CanAffectOnTarget(Vector3 target)
				//也就是说没有考虑对方的半径. 这样会造成结果不统一
				float dist = m_owner.GetSquaredXZDistanceTo_NoRadius(target);
				if (dist > m_meta.Range * m_meta.Range)
					return AffectDectectResult.OutOfRange;
			}

			return AffectDectectResult.Success;
		}
	}
}