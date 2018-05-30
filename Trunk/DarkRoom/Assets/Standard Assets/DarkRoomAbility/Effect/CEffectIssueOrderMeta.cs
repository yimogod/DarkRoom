using UnityEngine;
using System.Collections;

namespace DarkRoom.GamePlayAbility {
	public class CEffectIssueOrderMeta : CEffectMeta {
		/// <summary>
		/// 具体的指令
		/// </summary>
		public CAbilityEnum.Order EffectOrder;

		/// <summary>
		/// 是否需要延迟执行order
		/// </summary>
		public float Delay = -1;

		/// <summary>
		/// 指令的参数,
		/// 1. 如果是效果指令, 那么param = effect id
		/// </summary>
		public string Param;

		public CEffectIssueOrderMeta(string id) : base(id) {
			Type = EffectType.IssueOrder;
		}
	}
}
