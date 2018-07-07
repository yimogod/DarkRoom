using System;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 给目标添加Buff
	/// </summary>
	public class CEffectApplyBuff : CEffect
	{
		protected CEffectApplyBuffMeta m_meta {
			get { return MetaBase as CEffectApplyBuffMeta; }
		}

	    public override void AppliedFrom(IGameplayAbilityActor instigator)
	    {
	        base.AppliedFrom(instigator);
	        //buff自己销毁自己
	        //CBuff beh = CBuff.Create(m_meta.Behavior, m_owner.gameObject);
	        //beh.Apply(from, to);
        }

	    public override void ApplyToPosition(Vector3 localPosition)
	    {
            Debug.LogError("CEffectApplyBuff Can not Apply To a Position. Check Config");
	    }
	}
}

