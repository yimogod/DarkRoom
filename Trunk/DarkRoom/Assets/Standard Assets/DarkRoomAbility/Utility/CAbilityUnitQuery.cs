using System;
using System.Collections.Generic;

namespace DarkRoom.GamePlayAbility
{

    public enum CAbilityUnitTeamType
    {
        None, //不需要目标, 可以选择地面
        Me,
        Friend,
        Enemy,
        All
    }

    /// <summary>
    /// 查询目标的方法
    /// </summary>
    public enum CAbilityUnitQueryType
    {
        Undefined = 0,

        TeamAll, //如果有团队的概念的话, 那么就整个团队
        ImpactRange, //在圆/矩形里面的所有单位
        RandomInRange, //在圆/矩形里面的单个随机单位
    }

    public class CAbilityUnitQuery
    {
        public CAbilityUnitTeamType TeamRequire;
        public CAbilityUnitQueryType QueryType;

        /// <summary>
        /// 谁发起的这次查询
        /// </summary>
        public IGameplayAbilityUnit Instigater;

        /// <summary>
        /// 执行查询, 查询结果放入unitList
        /// </summary>
        public virtual void Excute(List<IGameplayAbilityUnit> unitList)
        {

        }
    }
}
