using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility
{


    public enum CGameplayModOp
    {
        Additive, //加法
        Multiplicitive, //乘法
        Division, //出发
        Override, //覆盖
        Max,
    }

    /// <summary>
    /// 对属性的修改操作
    /// </summary>
    public class CBuffModifierInfo
    {
        /// <summary>
        /// 属性的英文字符, 我们用反射找对应的属性
        /// </summary>
        public string Attribute;

        /// <summary>
        /// 对属性的操作
        /// </summary>
        public CGameplayModOp ModifierOp;

        /// <summary>
        /// buff来源的身上必须满足的tag条件
        /// </summary>
        public CGameplayTagRequirement SourceTagsRequire;

        /// <summary>
        /// buff应用的目标身上必须满足的tag条件
        /// </summary>
        public CGameplayTagRequirement TargetTagsRequire;
    }
}
