using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	public class CBuffStatusMeta : CBuffMeta {
		/// <summary>
		/// 更改单位自身的时间流速
		/// </summary>
		public float TimeScale = 1f;

		/// <summary>
		/// 状态标记. 没明白为何星际2分开了不同的flag
		/// 内置的状态state有
		/// 暴漏进程, 被动, 不可拖动, 不可选择, 不可阻止, 不可作为目标, 沉默
		/// 单位属性修改--轻甲, 重甲, 生物, 机械, 灵能, 建筑, 英雄
		/// 不让攻击, 不让隐形, 不让碰撞, 不让战斗, 不让侦测, 不让提供/使用人口
		/// 不让乘客, 不让有视野. 
		/// </summary>
		public Dictionary<CPawnVO.State, int> StateFlags = new Dictionary<CPawnVO.State, int>();

		/// <summary>
		/// 禁用技能类别, 比如目标技能, 普通攻击, 采集, 合体, 变形, 建造
		/// </summary>
		public List<CAbilityMeta.AbilityType> AbilClassDisableList = 
			new List<CAbilityMeta.AbilityType>();

		/// <summary>
		/// buff修改的单位属性
		/// </summary>
		public CAbilityEnum.BuffModification ModifyProperty = 
			new CAbilityEnum.BuffModification();

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