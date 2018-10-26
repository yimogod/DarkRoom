using System;
using System.Collections.Generic;
using DarkRoom.GamePlayAbility;

namespace Sword
{
	public class SwordArmorAttributeSet
	{
		//物防
		public PhysicalSaveAttribute PhysicalSave;
		//法防
		public SpellSaveAttribute SpellSave;
		//意念防
		public MindSaveAttribute MindSave;

		//格挡几率
		public ParryChanceAttribute ParryChance;
		//闪避几率
		public ParryChanceAttribute DodgeChance;

		//物理伤害减免百分比
		public PhysicalHardinessAttribute PhysicalHardiness;
		//法伤伤害减免百分比
		public SpellHardinessAttribute SpellHardiness;
		//意念伤害减免百分比
		public MindHardinessAttribute MindHardiness;

		//反弹伤害几率
		public DamageReflectChanceAttribute DamageReflectChance;
		//反弹伤害的伤害比例; 反弹伤害是混乱伤害
		public DamageReflectMultiplierAttribute DamageReflectMultiplier;

		public SwordArmorAttributeSet(SwordPrimaryAttributeSet primary)
		{
			PhysicalSave = new PhysicalSaveAttribute(primary);
			SpellSave = new SpellSaveAttribute(primary);
			MindSave = new MindSaveAttribute(primary);
			ParryChance = new ParryChanceAttribute(primary);
			DodgeChance = new ParryChanceAttribute(primary);
			PhysicalHardiness = new PhysicalHardinessAttribute(primary);
			SpellHardiness = new SpellHardinessAttribute(primary);
			MindHardiness = new MindHardinessAttribute(primary);
			DamageReflectChance = new DamageReflectChanceAttribute(primary);
			DamageReflectMultiplier = new DamageReflectMultiplierAttribute(primary);
		}
	}

	/// <summary>
	/// 物防
	/// </summary>
	public class PhysicalSaveAttribute : SwordArmorAttribute
	{
		public override float InitialValue => m_primary.Constitution.Value * 0.025f;

		public PhysicalSaveAttribute(SwordPrimaryAttributeSet primary) : base(primary, DefencePower.PhysicalSave)
		{
		}
	}

	/// <summary>
	/// 法防
	/// </summary>
	public class SpellSaveAttribute : SwordArmorAttribute
	{
		public override float InitialValue => m_primary.Constitution.Value * 0.025f;

		public SpellSaveAttribute(SwordPrimaryAttributeSet primary) : base(primary, DefencePower.SpellSave)
		{
		}
	}

	/// <summary>
	/// 意念
	/// </summary>
	public class MindSaveAttribute : SwordArmorAttribute
	{
		public override float InitialValue => m_primary.Constitution.Value * 0.025f;

		public MindSaveAttribute(SwordPrimaryAttributeSet primary) : base(primary, DefencePower.MindSave)
		{
		}
	}

	/// <summary>
	/// 格挡几率
	/// </summary>
	public class ParryChanceAttribute : SwordArmorAttribute
	{
		public override float InitialValue => m_primary.Constitution.Value * 0.002f;

		public ParryChanceAttribute(SwordPrimaryAttributeSet primary) : base(primary, DefencePower.ParryChance)
		{
		}
	}

	/// <summary>
	/// 格挡几率
	/// </summary>
	public class DodgeChanceAttribute : SwordArmorAttribute
	{
		public DodgeChanceAttribute(SwordPrimaryAttributeSet primary) : base(primary, DefencePower.ParryChance)
		{
		}
	}

	/// <summary>
	/// 物伤抵抗百分比
	/// </summary>
	public class PhysicalHardinessAttribute : SwordArmorAttribute
	{
		public PhysicalHardinessAttribute(SwordPrimaryAttributeSet primary) : base(primary, DefencePower.PhysicalHardiness)
		{
		}
	}

	/// <summary>
	/// 法伤抵抗百分比
	/// </summary>
	public class SpellHardinessAttribute : SwordArmorAttribute
	{
		public SpellHardinessAttribute(SwordPrimaryAttributeSet primary) : base(primary, DefencePower.SpellHardiness)
		{
		}
	}

	/// <summary>
	/// 伤害反弹几率
	/// </summary>
	public class DamageReflectChanceAttribute : SwordArmorAttribute
	{
		public DamageReflectChanceAttribute(SwordPrimaryAttributeSet primary)
			: base(primary, DefencePower.DamageReflectMultiplier)
		{
		}
	}

	/// <summary>
	/// 意伤抵抗百分比
	/// </summary>
	public class MindHardinessAttribute : SwordArmorAttribute
	{
		public MindHardinessAttribute(SwordPrimaryAttributeSet primary) : base(primary, DefencePower.MindHardiness)
		{
		}
	}

	/// <summary>
	/// 反弹伤害百分比
	/// </summary>
	public class DamageReflectMultiplierAttribute : SwordArmorAttribute
	{
		public DamageReflectMultiplierAttribute(SwordPrimaryAttributeSet primary)
			: base(primary, DefencePower.DamageReflectMultiplier)
		{
		}
	}


	/// <summary>
	/// 次级属性基础类
	/// </summary>
	public class SwordArmorAttribute : CAbilityAttribute
	{
		protected SwordPrimaryAttributeSet m_primary;

		public SwordArmorAttribute(SwordPrimaryAttributeSet primary, DefencePower id) : base((int) id)
		{
			m_primary = primary;
		}
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