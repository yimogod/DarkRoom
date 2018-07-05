using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	public class CEffectRemoveBuffMeta : CEffectMeta {
		/// <summary>
		/// 应用的行为id
		/// </summary>
		public string Behavior;

		/// <summary>
		/// 0到1之间. 0表示永远不执行. 1表示永远执行
		/// </summary>
		public string Chance;

		/// <summary>
		/// 某些"行为"所持续的时间通常会在"行为"中指定. 也会在本类中指定
		/// 如果在本类中指定, 则会覆盖行为自身配置的数据
		/// 如果是0, 则表明按照count来执行
		/// </summary>
		public float Duration = 0f;

		/// <summary>
		/// 行为如果Count大于0, 则指代两次buff的时间间隔
		/// </summary>
		public float Period = -1f;

		/// <summary>
		/// 该行为应用的次数
		/// </summary>
		public int Count = 0;

		public CEffectRemoveBuffMeta(string id) : base(id) {
			Type = EffectType.ApplyBehaviour;
		}
	}
}

