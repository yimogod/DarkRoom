using System;
using System.Collections.Generic;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
    /// 普通的近战或开枪等瞬伤单次单人攻击
    /// </summary>
	public class CAbilityAttackMeta : CAbilityMeta
	{
        /// <summary>
        /// 普通命中应用的属性计算器
        /// </summary>
	    public string AffectCalculation = "DefaultPhysicalCalculation";

        public CAbilityAttackMeta(string idKey) : base(idKey)
		{
		    Type = CAbilityType.Attack;
		}
	}
}
