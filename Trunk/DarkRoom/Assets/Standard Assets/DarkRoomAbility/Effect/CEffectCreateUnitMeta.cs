using System;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	public class CEffectCreateUnitMeta : CEffectMeta {
		//创建单位的个数
		public int SpawnCount = 1;
		//基于米的可以产崽的距离
		public int SpawnRange = 4;

		public CEffectCreateUnitMeta(string id) : base(id) {
			Type = EffectType.CreateUnit;
		}
	}
}

