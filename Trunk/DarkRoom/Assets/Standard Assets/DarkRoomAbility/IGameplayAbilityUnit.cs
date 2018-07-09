using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.GamePlayAbility
{
    /// <summary>
    /// 技能组件会挂在实现本接口的unitentity或者controller身上
    /// 我们的技能目标或者来源都必须实现这个接口
    /// </summary>
    public interface IGameplayAbilityUnit
    {
        /// <summary>
        /// 获取本组件挂在的owner的gameobject
        /// </summary>
        GameObject GameObject { get; }

        Vector3 LocalPosition { get; }

        /// <summary>
        /// 两者是否朋友 
        /// </summary>
        bool IsFriendTeam(IGameplayAbilityUnit target);

        /// <summary>
        /// 两者是否敌人
        /// </summary>
        bool IsEnemyTeam(IGameplayAbilityUnit target);

        /// <summary>
        /// 本类是否合法, 比如死亡就不合法
        /// </summary>
        bool InValid { get; }

        //------------------- for aiblity ---------------------------------
        CAbilitySystem AbilitySystem { get; }
        bool CostEnoughForAbility(CAbilityCostType type, int count);
        void AbilityUseCost(CAbilityCostType type, int count);
        //------------------- for aiblity end ---------------------------------

        //------------------- for effect ---------------------------------
        GameObject EffectLayer { get; }

        /// <summary>
        /// 查询符合的对象, 放入列表
        /// </summary>
        void SearchUnitsWithQuery(List<IGameplayAbilityUnit> searchResult);
        //------------------- for effect end ---------------------------------

        //------------------- for buff ---------------------------------
        GameObject BuffLayer { get; }
        //------------------- for effect end ---------------------------------

        /// <summary>
        /// 获取到点的距离的平方
        /// </summary>
        float GetSquaredXZDistanceTo_NoRadius(Vector3 target);

        /// <summary>
        /// 获取到单位的距离的平方
        /// </summary>
        float GetSquaredXZDistanceTo_NoRadius(IGameplayAbilityUnit target);
    }
}
