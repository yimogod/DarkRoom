using System;
using System.Collections.Generic;
using DarkRoom.GamePlayAbility;

namespace Sword
{
	/// <summary>
	/// 二级属性合集
	/// 这里有个注意的点是, 二级属性的初始化值是由一级属性提供的
	/// </summary>
	public class SwordSubAttributeSet
	{
		public class SwordDefenseAttribute : CAbilityAttribute
		{
			private SwordPrimaryAttributeSet m_primary;

			public SwordDefenseAttribute(SwordPrimaryAttributeSet primary, SubAttribute id) : base((int)id)
			{
				m_primary = primary;
			}

			public override float BaseValue => base.BaseValue + (m_primary.Dexterity.Value - 10f) * 0.35f +
						   m_primary.Luck.Value * 0.4f;
		}

		public CAbilityAttribute Encumbrance;
		public SwordDefenseAttribute Defense;
		public float CriticalChance;
		public float HealingModification;

		public SwordSubAttributeSet(SwordPrimaryAttributeSet primary)
		{
			Encumbrance = new CAbilityAttribute((int)SubAttribute.Encumbrance);
			Defense = new SwordDefenseAttribute(primary, SubAttribute.Defense);

			CriticalChance = 0;
			HealingModification = 0;
		}
	}

	public class SwordPowerAttributeSet
	{
		/// <summary>
		/// 暴击几率
		/// </summary>
		public float CriticalChance;

		/// <summary>
		/// 暴击伤害加成
		/// </summary>
		public float CriticalMultiplier;

		/// <summary>
		/// 猛击几率
		/// </summary>
		public float SmashChance;

		/// <summary>
		/// 物理伤害加成, 又技能,武器获得
		/// 非属性点加成
		/// </summary>
		public float PhysicalPowerMultiplier;

		/// <summary>
		/// 元素伤害加成, 又技能,武器获得
		/// 非属性点加成
		/// </summary>
		public float SpellPowerMultiplier;

		/// <summary>
		/// 意念伤害加成, 又技能,武器获得
		/// 非属性点加成
		/// </summary>
		public float MindPowerMultiplier;

		/// <summary>
		/// 护甲穿透
		/// </summary>
		public float ArmorPenetration;
	}

	public class SwordArmorAttributeSet
	{
		public CAbilityAttribute Armour;
		public float ArmourPenetration;
		public float ArmourHardiness;
	}

	/// <summary>
	/// 抗性数据合集
	/// </summary>
	public class SwordResistanceAttributeSet
	{
		public class ResistanceGroup
		{
			public CAbilityAttribute Resistance;
			/// <summary>
			/// 抗性穿透
			/// </summary>
			public float ResistancePenetration;
			/// <summary>
			/// 抗性减少
			/// </summary>
			public float ResistanceReduction;
		}

		public List<ResistanceGroup> ResistanceAttribute = new List<ResistanceGroup>();

		public ResistanceGroup this[int index]
		{
			get { return ResistanceAttribute[index]; }
		}
	}

	/// <summary>
	/// 免疫数据集合
	/// </summary>
	public class SwordImmunityAttributeSet
	{
	}
}