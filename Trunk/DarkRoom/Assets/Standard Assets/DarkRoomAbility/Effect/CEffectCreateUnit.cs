using System;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.GamePlayAbility {
	public class CEffectCreateUnit : CEffect
	{
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

