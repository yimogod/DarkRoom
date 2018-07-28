using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	public class CBuffStatusMeta : CBuffMeta {
		/// <summary>
		/// 更改单位自身的时间流速
		/// </summary>
		public float TimeScale = 1f;

		public CBuffStatusMeta(string idKey) : base(idKey) {}

		/// <summary>
		/// 控制有该buff的单位受到伤害时会如何反映的参数
		/// 比如无敌, 最小伤害就为0
		/// </summary>
		public class DamageResponse {
			/// <summary>
			/// response 起作用的几率
			/// 0代表完全不会起作用
			/// 1代表完全起作用
			/// </summary>
			public float Chance = 1f;


			public float ModifyFraction = 0f;

			/// <summary>
			/// 最小伤害
			/// </summary>
			public float ModifyMinimumDamage = 1f;

			/// <summary>
			/// 是否闪避
			/// </summary>
			public bool Evade = false;

			/// <summary>
			/// 给攻击者的效果
			/// 比如荆棘护甲的反弹伤害
			/// </summary>
			public string EffectToInstigator;
		}
	}
}