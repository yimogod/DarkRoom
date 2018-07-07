using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.GamePlayAbility
{
    /// <summary>
    /// 技能组件会挂在实现本接口的unitentity或者controller身上
    /// 我们的技能目标或者来源都必须实现这个接口
    /// </summary>
    public interface IGameplayAbilityActor
    {
        /// <summary>
        /// 获取本组件挂在的owner的gameobject
        /// </summary>
        GameObject GameObject { get; }

        /// <summary>
        /// 两者是否朋友 
        /// </summary>
        bool IsFriendTeam(IGameplayAbilityActor target);

        /// <summary>
        /// 两者是否敌人
        /// </summary>
        bool IsEnemyTeam(IGameplayAbilityActor target);

        /// <summary>
        /// 本类是否合法, 比如死亡就不合法
        /// </summary>
        bool InValid { get; }

        //------------------- for aiblity ---------------------------------
        CAbilitySystem AbilitySystem { get; }
        bool CostEnoughForAbility(AbilityCostType type, int count);
        void AbilityUseCost(AbilityCostType type, int count);
        //------------------- for aiblity end ---------------------------------

        //------------------- for effect ---------------------------------
        GameObject EffectLayer { get; }
        //------------------- for effect end ---------------------------------

        //------------------- for buff ---------------------------------
        GameObject BuffLayer { get; }
        //------------------- for effect end ---------------------------------

        float GetSquaredXZDistanceTo_NoRadius(Vector3 target);
        float GetSquaredXZDistanceTo_NoRadius(IGameplayAbilityActor target);
    }
}
