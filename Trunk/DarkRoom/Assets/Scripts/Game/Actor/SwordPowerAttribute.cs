using System;
using System.Collections.Generic;
using DarkRoom.GamePlayAbility;

namespace Sword
{
	/// <summary>
	/// 次级属性集合
	/// </summary>
	public class SwordPowerAttributeSet
	{
		/// <summary>
		/// 暴击几率
		/// </summary>
		public CriticalChanceAttribute CriticalChance;

		/// <summary>
		/// 暴击伤害加成
		/// </summary>
		public CriticalMultiplierAttribute CriticalMultiplier;

		/// <summary>
		/// 猛击几率
		/// </summary>
		public SmashChanceAttribute SmashChance;

		/// <summary>
		/// 物理伤害加成
		/// </summary>
		public PhysicalPowerMultiplierAttribute PhysicalPowerMultiplier;

		/// <summary>
		/// 元素伤害加成
		/// </summary>
		public SpellPowerMultiplierAttribute SpellPowerMultiplier;

		/// <summary>
		/// 意念伤害加成
		/// </summary>
		public SpellPowerMultiplierAttribute MindPowerMultiplier;

		/// <summary>
		/// 护甲穿透
		/// </summary>
		public CAbilityAttribute ArmorPenetration;

		public SwordPowerAttributeSet(SwordPrimaryAttributeSet primary)
		{
			CriticalChance = new CriticalChanceAttribute(primary);
			CriticalMultiplier = new CriticalMultiplierAttribute(primary);
			PhysicalPowerMultiplier = new PhysicalPowerMultiplierAttribute(primary);
			SpellPowerMultiplier = new SpellPowerMultiplierAttribute(primary);
			MindPowerMultiplier = new SpellPowerMultiplierAttribute(primary);
			ArmorPenetration = new ArmorPenetrationAttribute(primary);
			SmashChance = new SmashChanceAttribute(primary);
		}
	}

	/// <summary>
	/// 暴击几率
	/// </summary>
	public class CriticalChanceAttribute : SwordPowerAttribute
	{
		public override float InitialValue => m_primary.Dexterity.Value * 0.002f +
		                                      m_primary.Luck.Value * 0.004f;

		public CriticalChanceAttribute(SwordPrimaryAttributeSet primary) : base(primary, AttackPower.CriticalChance)
		{
		}
	}

	/// <summary>
	/// 暴击伤害加成百分比
	/// </summary>
	public class CriticalMultiplierAttribute : SwordPowerAttribute
	{
		public override float InitialValue => 0.5f + m_primary.Strength.Value * 0.004f;

		public CriticalMultiplierAttribute(SwordPrimaryAttributeSet primary) : base(primary, AttackPower.CriticalMultiplierr)
		{
		}
	}

	/// <summary>
	/// 猛击几率
	/// </summary>
	public class SmashChanceAttribute : SwordPowerAttribute
	{
		public SmashChanceAttribute(SwordPrimaryAttributeSet primary) : base(primary, AttackPower.SmashChance)
		{
		}
	}

	/// <summary>
	/// 物理伤害百分百加成
	/// </summary>
	public class PhysicalPowerMultiplierAttribute : SwordPowerAttribute
	{
		public override float InitialValue => m_primary.Strength.Value * 0.005f;

		public PhysicalPowerMultiplierAttribute(SwordPrimaryAttributeSet primary)
			: base(primary, AttackPower.PhysicalPowerMultiplier)
		{
		}
	}

	/// <summary>
	/// 元素伤害百分百加成
	/// </summary>
	public class SpellPowerMultiplierAttribute : SwordPowerAttribute
	{
		public override float InitialValue => m_primary.Strength.Value * 0.005f;

		public SpellPowerMultiplierAttribute(SwordPrimaryAttributeSet primary)
			: base(primary, AttackPower.SpellPowerMultiplier)
		{
		}
	}

	/// <summary>
	/// 护甲穿透类
	/// </summary>
	public class ArmorPenetrationAttribute : SwordPowerAttribute
	{
		public ArmorPenetrationAttribute(SwordPrimaryAttributeSet primary) : base(primary, AttackPower.ArmorPenetration)
		{
		}
	}

	/// <summary>
	/// 次级属性基础类
	/// </summary>
	public class SwordPowerAttribute : CAbilityAttribute
	{
		protected SwordPrimaryAttributeSet m_primary;

		public SwordPowerAttribute(SwordPrimaryAttributeSet primary, AttackPower id) : base((int) id)
		{
			m_primary = primary;
		}
	}
}