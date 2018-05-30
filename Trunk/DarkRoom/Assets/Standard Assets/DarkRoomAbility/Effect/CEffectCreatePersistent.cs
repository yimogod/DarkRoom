using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 持续效果基础类。非常重要的基础类，用来实现"在指定点创建持续效果"的效果。
	/// 比如灵能风暴，它可以周期性地引发其他效果。
	/// </summary>
	public class CEffectCreatePersistent : CEffect {
		public override void Apply(CAIController owner, CAIController target) {
		}
	}
}
