using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkRoom.GamePlayAbility {
	//Atk会寻找武器, 使用武器的effect来进行真正的攻击判断
	public class CAbilAttackMeta : CAbilityMeta {
		//调整攻击速度的最大最小范围
		public float MinAttackSpeedMultiplier = 0.25f;
		public float MaxAttackSpeedMultiplier = 128f;

		public CAbilAttackMeta(string idKey) : base(idKey) {
		}
	}
}
