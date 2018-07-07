using System;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.GamePlayAbility {
	public class CAbilitylEffectTarget : CAbility
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
		    if (m_target == null)return;

		    m_owner.AbilitySystem.ApplyGameplayEffectToTarget(null, m_target);
		}

		public override AffectDectectResult CanAffectOnTarget(IGameplayAbilityActor target){
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
            Debug.LogError("Ability Target do not handle Position");
			return AffectDectectResult.TargetInvalid;
		}
	}
}