using System;
using System.Collections.Generic;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
    /// 普通的近战或开枪等瞬伤单次单人攻击
    /// </summary>
	public class CAbilityAttackMeta : CAbilityMeta {
		public CAbilityAttackMeta(string idKey) : base(idKey)
		{
		    Type = CAbilityType.Attack;
		}
	}
}
