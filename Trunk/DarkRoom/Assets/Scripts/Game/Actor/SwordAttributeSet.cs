﻿using System;
using System.Collections.Generic;
using DarkRoom.Game;
using DarkRoom.GamePlayAbility;
using UnityEngine;

namespace Sword
{
	public class SwordAttributeSet : CAbilityAttributeSet
	{
		//----------------------- 单个属性的get接口 ----------------------
		//--------------------- 这里我们平铺所有的属性--------------------
		//资源
		public float MaxHealth => m_resAttr.MaxHealth.Value;
		public float MaxMana => m_resAttr.MaxMana.Value;
		public float MaxStamina => m_resAttr.MaxStamina.Value;

		//主属性
		public float Strength => m_primaryAttr.Strength.Value;
		public float Dexterity => m_primaryAttr.Dexterity.Value;
		public float Intelligence => m_primaryAttr.Intelligence.Value;
		public float Constitution => m_primaryAttr.Constitution.Value;
		public float Willpower => m_primaryAttr.Willpower.Value;
		public float Luck => m_primaryAttr.Luck.Value;

		//攻击属性


		//防御属性
		public float DodgeChance => m_armorAttr.DodgeChance.Value;

		/// <summary>
		/// 移动力 TODO 移动到side attribute
		/// </summary>
		public int MoveRange = 3;

		/// <summary>
		/// 视野 TODO 移动到side attribute
		/// </summary>
		public int ViewRange = 3;

		/// <summary>
		/// 升级到下级需要的经验
		/// </summary>
		public int NextLevelExp => m_gm.GetNextLevelUpExp(m_level);

		public int Exp { get; set; }
		public float Health { get; set; }
		public float Mana { get; set; }
		public float Stamina { get; set; }

		//------------------------------------ 角色当前的属性 ----------------
		private SwordPrimaryAttributeSet m_primaryAttr;
		private SwordResourceAttributeSet m_resAttr;
		private SwordPowerAttributeSet m_powerAttr;
		private SwordArmorAttributeSet m_armorAttr;
		private SwordResistanceAttributeSet m_resistAttr;

		//当前等级
		private int m_level;

		// 角色职业
		private ActorClass m_class;

		//角色种族
		private ActorRace m_race;


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

		private SwordGameMode m_gm => CWorld.Instance.GetGameMode<SwordGameMode>();

		public SwordAttributeSet()
		{
		}

		/// <summary>
		/// 根据种族和职业赋值初始化的属性
		/// </summary>
		public void InitAttr(ActorClass actorClass, ActorRace actorRace, float healthRank)
		{
			m_class = actorClass;
			m_race = actorRace;
			var classMeta = ClassMetaManager.GetMeta((int) m_class);
			var raceMeta = RaceMetaManager.GetMeta((int) m_race);

			m_primaryAttr = new SwordPrimaryAttributeSet();
			m_primaryAttr.InitFromClassAndRace(classMeta, raceMeta);

			m_powerAttr = new SwordPowerAttributeSet(m_primaryAttr);

			m_armorAttr = new SwordArmorAttributeSet(m_primaryAttr);

			m_resAttr = new SwordResourceAttributeSet(m_primaryAttr);
			m_resAttr.InitClassAndRace(classMeta, raceMeta, healthRank);
		}

		/// <summary>
		/// 从数据库读取的, 一级属性的持久化的点数
		/// </summary>
		public void SetPrimaryAttrPersistentValue(float strength, float dexterity, float intelligence, float constitution,
			float willpower, float luck)
		{
			m_primaryAttr.Strength.AddPersistentValue(strength);
			m_primaryAttr.Dexterity.AddPersistentValue(dexterity);
			m_primaryAttr.Intelligence.AddPersistentValue(intelligence);
			m_primaryAttr.Constitution.AddPersistentValue(constitution);
			m_primaryAttr.Willpower.AddPersistentValue(willpower);
			m_primaryAttr.Luck.AddPersistentValue(luck);
		}

		public void InitLevel(int value)
		{
			if (m_gm == null) Debug.LogError("SwordGameMode m_gm MUST NOT NULL!!");
			m_level = value;

			//升级重置最大血量
			m_resAttr.SetLevel(value);
			Health = MaxHealth;
			Mana = MaxMana;
			Stamina = MaxStamina;
		}


		public void LevelUp()
		{
			InitLevel(m_level + 1);
			Exp = 0;
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