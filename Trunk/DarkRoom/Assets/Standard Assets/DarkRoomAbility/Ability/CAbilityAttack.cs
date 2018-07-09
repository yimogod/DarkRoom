using System;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
    /// <summary>
    /// 挂在角色身上, 角色的默认攻击技能
    /// 进行攻击的目标我们是有AI/鼠标进行选定的，除非取消这个技能， 否则会进行攻击判定
    /// 即使target已经逃出攻击范围
    /// 
    /// 本技能其实应该调用attack effect的. 但普攻所有的游戏都有, 所有我们提供了快捷引用
    /// </summary>
    public class CAbilityAttack : CAbility
	{
	    private CAbilityAttackMeta m_meta
	    {
	        get { return MetaBase as CAbilityAttackMeta; }
	    }

        //普攻, 那只能对敌人使用
        public override  AffectDectectResult CanAffectOnTarget(IGameplayAbilityUnit target){
			AffectDectectResult result = base.CanAffectOnTarget(target);
			if (result != AffectDectectResult.Success)return result;

		    return AffectDectectResult.Success;
		}

	    public override AffectDectectResult CanAffectOnTarget(Vector3 localPosition)
	    {
            Debug.LogErrorFormat("{0} is one AbilityAttack which Just handle unit target", MetaBase.Id);
	        return AffectDectectResult.TargetInvalid;
	    }

	    protected override void Activate()
	    {
	        base.Activate();
	        var cal = CAbilityManager.Instance.FindCalculation(m_meta.AffectCalculation);
            cal.Execute(m_owner, m_target);
	    }
	}
}