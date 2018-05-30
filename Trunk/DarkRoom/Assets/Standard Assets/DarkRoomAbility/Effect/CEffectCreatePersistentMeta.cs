using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 持续效果的配置数据
	/// </summary>
	public class CEffectCreatePersistentMeta : CEffectMeta {
		/// <summary>
		/// 持续效果执行次数
		/// </summary>
		public int PeriodCount;

		/// <summary>
		/// TODO 保留 看编辑器的内容然后推断功能
		/// </summary>
		public CAbilityEnum.TargetLocationType WhichLocation = CAbilityEnum.TargetLocationType.Point;

		/// <summary>
		/// 持续周期执行的效果id列表
		/// </summary>
		public List<string> PeriodicEffectArray = new List<string>();

		/// <summary>
		/// 持续周期的时间列表
		/// 第一个数据一般配置0, 表明立刻执行
		/// 
		/// 注意, 执行的次数是 max(PeriodicEffectArray, PeriodicPeriodArray)
		/// 另外一个循环执行
		/// </summary>
		public List<float> PeriodicPeriodArray = new List<float>();

		public CEffectCreatePersistentMeta(string id) : base(id) {
			Type = EffectType.ApplyBehaviour;
		}
	}
}
