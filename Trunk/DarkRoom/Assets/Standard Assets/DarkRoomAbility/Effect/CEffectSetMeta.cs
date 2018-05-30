using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 包含多个同时起作用的效果器
	/// </summary>
	public class CEffectSetMeta : CEffectMeta {
		/// <summary>
		/// 效果列表
		/// </summary>
		public List<string> EffectList = new List<string>();

		/// <summary>
		/// 效果作用的目标
		/// </summary>
		public CAbilityEnum.TargetLocationType LocationType;

		public CEffectSetMeta(string id) : base(id) {
			Type = EffectType.EffectSet;
		}

		public void AddEffect(string effId){
			EffectList.Add(effId);
        }
	}
}