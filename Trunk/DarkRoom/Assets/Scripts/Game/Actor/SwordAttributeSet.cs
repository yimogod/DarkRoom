using System;
using System.Collections.Generic;
using DarkRoom.Game;
using DarkRoom.GamePlayAbility;
using UnityEngine;

namespace Sword
{
	public class SwordAttributeSet : CAbilityAttributeSet
	{
		//------------------------------------ 角色当前的属性 ----------------

		private SwordPrimaryAttribute m_primaryAttr;
		private SwordSubAttribute m_subAttr;
		private SwordResourceAttribute m_resAttr;
		private SwordPowerAttribute m_powerAttr;
		private SwordArmorAttribute m_armorAttr;
		private SwordResistanceAttribute m_resistAttr;

		/// <summary>
		/// 当前回合的物理伤害
		/// </summary>
		public float Damage;

		/// <summary>
		/// 当前的法伤
		/// </summary>
		public float SpellDamage;

		// ------------------buff或者等级对属性的修改------------------------
		/** 吸血, 按照伤害的百分比 */
		public float LifeStealAddOn;

		public int Exp
		{
			get { return (int) m_expAttr.Value; }
			set { PreAttributeChange(m_expAttr, value); }
		}

		public float Health
		{
			get { return m_healthAttr.Value; }
			set { PreAttributeChange(m_healthAttr, value); }
		}

		public float Mana
		{
			get { return m_manaAttr.Value; }
			set { PreAttributeChange(m_manaAttr, value); }
		}

		private int m_level;

		private CAbilityAttribute m_expAttr;
		private CAbilityAttribute m_healthAttr;
		private CAbilityAttribute m_manaAttr;

		//------------------------------------ 角色初始化的属性 ----------------
		// 角色职业
		private ActorClass m_class;
		//角色种族
		private ActorRace m_race;


		private SwordGameMode m_gm => CWorld.Instance.GetGameMode<SwordGameMode>();

		public SwordAttributeSet()
		{
			//m_expAttr = new CAbilityAttribute((int)AttributeId.Exp);
			//m_healthAttr = new CAbilityAttribute((int)AttributeId.Health);
			//m_manaAttr = new CAbilityAttribute((int)AttributeId.Mana);
		}

		/// <summary>
		/// 根据种族和职业赋值初始化的属性
		/// </summary>
		public void InitAttribute(ActorClass actorClass, ActorRace actorRace)
		{
			m_class = actorClass;
			m_race = actorRace;
			var classMeta = ClassMetaManager.GetMeta((int)m_class);
			var raceMeta = RaceMetaManager.GetMeta((int)m_race);

			m_primaryAttr = new SwordPrimaryAttribute();
			m_primaryAttr.InitFromClassAndRace(classMeta, raceMeta);

			m_resAttr = new SwordResourceAttribute();
			m_resAttr.InitFromClassAndRace(classMeta, raceMeta);
		}

		public void InitHealthAndMana()
		{
			Health = m_resAttr.MaxHealth.Value;
		}

		public void InitLevel(int value)
		{
			if (m_gm == null) Debug.LogError("SwordGameMode m_gm MUST NOT NULL!!");
			m_level = value;
		}

		public void LevelUp()
		{
			InitLevel(m_level + 1);
			Exp = 0;
			InitHealthAndMana();
		}

		/** 力量 */
		//public float Strength => StrengthBase + StrengthAddOn;
		// public float StrengthBase=> m_gm.GetStrengthBase(m_class, m_level);

		/** 敏捷 */
		// public float Agility => AgilityBase + AgilityAddOn;
		// public float AgilityBase=> m_gm.GetAgilityBase(m_class, m_level);
		///
		/** 智力 */
		// public float Intelligence => IntelligenceBase + IntelligenceAddOn;
		// public float IntelligenceBase=> m_gm.GetIntelligenceBase(m_class, m_level);
		/// <summary>
		/// 物理防御
		/// </summary>
		//   public float ArmorReduction=> ArmorReductionBase + ArmorReductionAddOn;
		//   public float ArmorReductionBase => m_gm.GetArmorReductionBase(m_class, Strength, m_initArmorReduction);
		/** 物理攻击 */
		//  public float PhysicalDamage=> PhysicalDamageBase + PhysicalDamageAddOn;
		// public float PhysicalDamageBase=> m_gm.GetPhysicalDamageBase(m_class, Strength, Agility, Intelligence, m_initPhysicDamage);
		/** 暴击率  0 -- 100*/
		// public float CritChance=> CritChanceBase + CritChanceAddOn;
		// public float CritChanceBase=> m_gm.GetCritChanceBase(m_class, Agility);
		/** 暴击系数 */
		//   public float CritMultiplier=> CritMultiplierBase + CritMultiplierAddOn;
		//   public float CritMultiplierBase=> m_gm.GetCritMultiplierBase(m_class, Strength, Agility, Intelligence);
		/** 闪避 0 -- 100 */
		//   public float DodgeChance=> DodgeChanceBase + DodgeChanceAddOn;
		//  public float DodgeChanceBase=> m_gm.GetDodgeChanceBase(m_class, Agility);
		/** 最大血量 */
		//  public float MaxHealth=> MaxHealthBase * (1.0f + MaxHealthFactor) + MaxHealthAddOn;
		//   public float MaxHealthBase=> m_gm.GetMaxHealthBase(m_class, Strength, m_initMaxHealth);
		/** 血量自动恢复 */
		//   public float HealthRecover=> HealthRecoverBase + HealthRecoverAddOn;
		//  public float HealthRecoverBase=> m_gm.GetHealthRecoverBase(m_class, Strength, m_initHealthRecover);
		/** 最大蓝量 */
		//  public float MaxMana=> MaxManaBase * (1.0f + MaxManaFactor) + MaxManaAddOn;
		//  public float MaxManaBase=> m_gm.GetMaxManaBase(m_class, Intelligence, m_initMaxMana);
		/** 蓝量自动恢复 */
		//  public float ManaRecover=> ManaRecoverBase + ManaRecoverAddOn;
		// public float ManaRecoverBase=> m_gm.GetManaRecoverBase(m_class, Intelligence, m_initManaRecover);
		public override void PreAttributeChange(CAbilityAttribute attribute, float newValue)
		{
			//  if (attribute.Id == (int)AttributeId.Health)
			//  {
			//     float maxHealth = MaxHealth;
			//   newValue = Mathf.Clamp(newValue, 0, maxHealth);
			//attribute.SetValue(newValue);
			//  }

			//  if (attribute.Id == (int)AttributeId.Mana)
			//  {
			//     float maxMana = MaxMana;
			//     newValue = Mathf.Clamp(newValue, 0, maxMana);
			//attribute.SetValue(newValue);
			//    }
		}

		public override void PostAttributeExecute(CAbilityAttribute attribute)
		{
			//     if (attribute.Id == (int)AttributeId.Health)
			//   {
			//    if (Health < 0)
			//   {
			//target dead
			//  }
			//   }
		}
	}
}