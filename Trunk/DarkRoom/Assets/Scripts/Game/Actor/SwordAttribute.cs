using System;
using System.Collections.Generic;
using DarkRoom.GamePlayAbility;

namespace Sword
{
	/// <summary>
	/// 主属性合集
	/// </summary>
	public class SwordPrimaryAttributeSet
	{
		public CAbilityAttribute Strength;
		public CAbilityAttribute Dexterity;
		public CAbilityAttribute Intelligence;
		public CAbilityAttribute Constitution;
		public CAbilityAttribute Willpower;
		public CAbilityAttribute Cunning;
		public CAbilityAttribute Luck;

		public void InitFromClassAndRace(ActorBornAttributeMeta classMeta, ActorBornAttributeMeta raceMeta)
		{
			//所有一级属性的基础值都是10, 然后根据种族和职业进行修正
			float baseValue = 10f;
			float v1 = 0;
			v1 = classMeta.Strength + raceMeta.Strength + baseValue;
			Strength = new CAbilityAttribute((int) PrimaryAttribute.Strength, v1);

			v1 = classMeta.Dexterity + raceMeta.Dexterity + baseValue;
			Dexterity = new CAbilityAttribute((int) PrimaryAttribute.Dexterity, v1);

			v1 = classMeta.Constitution + raceMeta.Constitution + baseValue;
			Constitution = new CAbilityAttribute((int) PrimaryAttribute.Constitution, v1);

			v1 = classMeta.Intelligence + raceMeta.Intelligence + baseValue;
			Intelligence = new CAbilityAttribute((int) PrimaryAttribute.Intelligence, v1);

			v1 = classMeta.Willpower + raceMeta.Willpower + baseValue;
			Willpower = new CAbilityAttribute((int) PrimaryAttribute.Willpower, v1);

			v1 = classMeta.Cunning + raceMeta.Cunning + baseValue;
			Cunning = new CAbilityAttribute((int) PrimaryAttribute.Cunning, v1);

			v1 = classMeta.Luck + raceMeta.Luck + baseValue;
			Luck = new CAbilityAttribute((int) PrimaryAttribute.Luck, v1);
		}
	}

	/// <summary>
	/// 二级属性合集
	/// 这里有个注意的点是, 二级属性的初始化值是由一级属性提供的
	/// </summary>
	public class SwordSubAttributeSet
	{
		public class SwordAccuracyAttribute : CAbilityAttribute
		{
			private SwordPrimaryAttributeSet m_primary;

			public SwordAccuracyAttribute(SwordPrimaryAttributeSet primary) : base((int) SubAttribute.Accuracy)
			{
				m_primary = primary;
			}

			public override float BaseValue => base.BaseValue + 4f + 
				(m_primary.Dexterity.Value - 10f) + m_primary.Luck.Value * 0.4f;
		}

		public class SwordDefenseAttribute : CAbilityAttribute
		{
			private SwordPrimaryAttributeSet m_primary;

			public SwordDefenseAttribute(SwordPrimaryAttributeSet primary, SubAttribute id) : base((int) id)
			{
				m_primary = primary;
			}

			public override float BaseValue => base.BaseValue + (m_primary.Dexterity.Value - 10f) * 0.35f +
					       m_primary.Luck.Value * 0.4f;
		}

		public SwordAccuracyAttribute Accuracy;
		public CAbilityAttribute Encumbrance;
		public SwordDefenseAttribute Defense;
		public SwordDefenseAttribute RangedDefense;
		public float CriticalChance;
		public float ShrugOffCriticalsChance;
		public float HealingModification;

		public SwordSubAttributeSet(SwordPrimaryAttributeSet primary)
		{
			Accuracy = new SwordAccuracyAttribute(primary);
			Encumbrance = new CAbilityAttribute((int) SubAttribute.Encumbrance);
			Defense = new SwordDefenseAttribute(primary, SubAttribute.Defense);
			RangedDefense = new SwordDefenseAttribute(primary, SubAttribute.RangedDefense);

			CriticalChance = 0;
			ShrugOffCriticalsChance = 0;
			HealingModification = 0;
		}
	}

	/// <summary>
	/// 消耗的资源合集
	/// </summary>
	public class SwordResourceAttributeSet
	{
		public class SwordHealthAttribute : CAbilityAttribute
		{
			private SwordPrimaryAttributeSet m_primary;

			// 每次升级增加的生命
			private float m_healthRating;

			//为npc用的计算生命的系数
			private float m_healthRank = 1f;

			//由职业和种族提供的初始化属性
			private float m_initValueFromClassRace;

			//由等级获取到初始值
			private float m_initValueFromLevel;

			public SwordHealthAttribute(SwordPrimaryAttributeSet primary) : base((int) Resource.Health)
			{
				m_primary = primary;
			}

			public override float BaseValue => base.BaseValue + m_healthRank * 
				(m_initValueFromClassRace + m_initValueFromLevel + m_primary.Constitution.Value * 4f);

			public void InitHealthFromClassRace(float classHealth, float raceHealth)
			{
				m_initValueFromClassRace = classHealth + raceHealth;
			}

			public void InitHealthRatingRank(float healthRating, float healthRank)
			{
				m_healthRating = healthRating;
				m_healthRank = healthRank;
			}

			public void SetLevel(int lv)
			{
				//每升一级, 增加的血量 Life Rating * (1.1+(current_level/40)) 
				//相当于lv!来计算血量 lv从2开始计算

				m_initValueFromLevel = 0;
				for (int i = 2; i <= lv; i++)
				{
					m_initValueFromLevel += i * 0.025f;
				}

				m_initValueFromLevel += 1.1f * (lv - 1);
				m_initValueFromLevel *= m_healthRating;
			}
		}

		public SwordHealthAttribute MaxHealth { private set; get; }
		public CAbilityAttribute MaxMana { private set; get; }
		public CAbilityAttribute MaxStamina { private set; get; }

		private SwordPrimaryAttributeSet m_primary;


		public SwordResourceAttributeSet(SwordPrimaryAttributeSet primary)
		{
			m_primary = primary;
			MaxHealth = new SwordHealthAttribute(m_primary);

			//TODO 临时
			MaxMana = new CAbilityAttribute(1000, 100, 10);
			MaxStamina = new CAbilityAttribute(1001, 200, 20);
		}

		public void InitClassAndRace(ActorBornAttributeMeta classMeta, ActorBornAttributeMeta raceMeta, float healthRank)
		{
			MaxHealth.InitHealthRatingRank(classMeta.HealthRating + raceMeta.HealthRating, healthRank);
			MaxHealth.InitHealthFromClassRace(classMeta.Health, raceMeta.Health);
		}

		public void SetLevel(int level)
		{
			MaxHealth.SetLevel(level);
		}
	}

	public class SwordPowerAttributeSet
	{
		public CAbilityAttribute Physical;
		public CAbilityAttribute Spell;
		public CAbilityAttribute Mind;
		public CAbilityAttribute PhysicalSave;
		public CAbilityAttribute SpellSave;
		public CAbilityAttribute MindSave;
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
			public float ResistancePenetration;
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