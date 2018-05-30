using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	public class CEffectDamageMeta : CEffectMeta {
		/// <summary>
		/// 伤害类型.总觉得不对
		/// </summary>
		public enum EffectDamageType {
			Melee, //近战
			Ranged, //远程
			Splash, //割伤
					//钝击
					//穿刺


			Spell, //咒语
		}

		/// <summary>
		/// 伤害大小
		/// </summary>
		public int Amount = 1;

		/// <summary>
		/// 伤害类型
		/// </summary>
		public EffectDamageType DamageType;

		/// <summary>
		/// 命中的视觉效果
		/// </summary>
		public CVisualEffect m_impact;


		/// <summary>
		/// 对不同目标类型的伤害加成(加法)
		/// </summary>
		public AttributeBonus[] AttributeBonusList;

		/// <summary>
		/// 对不同目标类型的伤害加成(乘法)
		/// </summary>
		public AttributeBonus[] AttributeMultiplierBonusList;


		public CEffectDamageMeta(string id) : base(id) {
			Type = EffectType.Damage;
			DamageType = EffectDamageType.Melee;

			int max = (int)CAbilityEnum.AttributeType.Count;
			AttributeBonusList = new AttributeBonus[max];
			AttributeMultiplierBonusList = new AttributeBonus[max];

			for (int i = 0; i < max; i++) {
				AttributeBonusList[i].Index = (CAbilityEnum.AttributeType)i;
				AttributeBonusList[i].Value = 0;
			}

			for (int i = 0; i < max; i++) {
				AttributeBonusList[i].Index = (CAbilityEnum.AttributeType)i;
				AttributeBonusList[i].Value = 1f;
			}
		}

		/// <summary>
		/// 伤害对不同目标类型的加成
		/// </summary>
		public struct AttributeBonus {
			/// <summary>
			/// 根据sc2, 有轻甲,重甲, 重型(massive),
			/// 生物, 机械, 灵能, 重型, 建筑, 英雄, 召唤, 地图boss, 地图对象
			/// </summary>
			public CAbilityEnum.AttributeType Index;
			public float Value;
		}
	}
}