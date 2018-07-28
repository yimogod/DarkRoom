using System;
using System.Collections.Generic;
using System.ComponentModel;
using DarkRoom.Game;
using DarkRoom.GamePlayAbility;
using UnityEngine;

namespace Sword
{
    public enum AttributeType
    {
        StrengthUnit, //力量单位
        AgilityUnit, //敏捷单位
        IntelligenceUnit, //智力单位
    }

    public class SwordAttributeSet : CAbilityAttributeSet
    {
        private enum AttributeId
        {
            Exp,
            Health,
            Mana,
        }
        //------------------------------------ 角色当前的属性 ----------------
        /// <summary>
        /// 角色成长类型
        /// </summary>
        public AttributeType AttributeType;

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

        /** 技能/buff/道具对最大血量的修改, 加法 */
        public int MaxHealthAddOn;

        /** 技能/buff/道具对最大血量的修改, 乘法 */
        public float MaxHealthFactor;

        /** 技能/buff/道具对自动恢复血量的修改 */
        public float HealthRecoverAddOn;

        /** 技能/buff/道具对魔法的修改, 加法 */
        public int MaxManaAddOn;

        /** 技能/buff/道具对魔法的修改, 乘法 */
        public float MaxManaFactor;

        /** 技能/buff/道具对魔法自动恢复的修改 */
        public float ManaRecoverAddOn;

        /** 技能/buff/道具对物理攻击修改 */
        public float PhysicalDamageAddOn;

        /** 暴击几率 0 ~ 100 */
        public float CritChanceAddOn;

        /** 暴击伤害系数 */
        public float CritMultiplierAddOn;

        /** 技能/buff/道具对防御的修改, 加法 */
        public float ArmorReductionAddOn;

        /** 闪避 0-100 */
        public float DodgeChanceAddOn;

        /** 技能/buff/道具对力量的修改 */
        public float StrengthAddOn;

        /** 技能/buff/道具对敏捷的修改 */
        public float AgilityAddOn;

        /** 技能/buff/道具对智力的修改 */
        public float IntelligenceAddOn;

        public int Exp
        {
            get { return (int)m_expAttr.Value; }
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
        // 初始化的最大hp
        private float m_initMaxHealth = 0;

        // 初始血量恢复
        private float m_initHealthRecover = 0;

        // 初始化的魔法
        private float m_initMaxMana = 0;

        // 初始魔法恢复
        private float m_initManaRecover = 0;

        // 初始化物理伤害
        private float m_initPhysicDamage = 0;

        // 初始化防御
        private float m_initArmorReduction;

        private SwordGameMode m_gm => CWorld.Instance.GetGameMode<SwordGameMode>();

        public SwordAttributeSet()
        {
            m_expAttr = new CAbilityAttribute((int)AttributeId.Exp);
            m_healthAttr = new CAbilityAttribute((int)AttributeId.Health);
            m_manaAttr = new CAbilityAttribute((int)AttributeId.Mana);
        }

        /// <summary>
        /// 赋值初始化的属性
        /// </summary>
        public void InitAttribute(int maxHealth, int maxMana, int healthRecover, int manaRecover,
            int physicDamage, int armorReduction)
        {
            m_initMaxHealth = maxHealth;
            m_initMaxMana = maxMana;
            m_initHealthRecover = healthRecover;
            m_initManaRecover = manaRecover;
            m_initPhysicDamage = physicDamage;
            m_initArmorReduction = armorReduction;
        }

        public void InitHealthAndMana()
        {
            Health = MaxHealth;
            Mana = MaxMana;
        }

        public void InitLevel(int value)
        {
            if (m_gm == null)Debug.LogError("SwordGameMode m_gm MUST NOT NULL!!");
            m_level = value;
        }

        public void LevelUp()
        {
            m_level++;
            Exp = 0;
            InitHealthAndMana();
        }

        /** 力量 */
        public float Strength => StrengthBase + StrengthAddOn;
        public float StrengthBase=> m_gm.GetStrengthBase(AttributeType, m_level);

        /** 敏捷 */
        public float Agility => AgilityBase + AgilityAddOn;
        public float AgilityBase=> m_gm.GetAgilityBase(AttributeType, m_level);

        /** 智力 */
        public float Intelligence => IntelligenceBase + IntelligenceAddOn;
        public float IntelligenceBase=> m_gm.GetIntelligenceBase(AttributeType, m_level);

        /// <summary>
        /// 物理防御
        /// </summary>
        public float ArmorReduction=> ArmorReductionBase + ArmorReductionAddOn;
        public float ArmorReductionBase => m_gm.GetArmorReductionBase(AttributeType, Strength, m_initArmorReduction);

        /** 物理攻击 */
        public float PhysicalDamage=> PhysicalDamageBase + PhysicalDamageAddOn;
        public float PhysicalDamageBase=> m_gm.GetPhysicalDamageBase(AttributeType, Strength, Agility, Intelligence, m_initPhysicDamage);

        /** 暴击率  0 -- 100*/
        public float CritChance=> CritChanceBase + CritChanceAddOn;
        public float CritChanceBase=> m_gm.GetCritChanceBase(AttributeType, Agility);

        /** 暴击系数 */
        public float CritMultiplier=> CritMultiplierBase + CritMultiplierAddOn;
        public float CritMultiplierBase=> m_gm.GetCritMultiplierBase(AttributeType, Strength, Agility, Intelligence);

        /** 闪避 0 -- 100 */
        public float DodgeChance=> DodgeChanceBase + DodgeChanceAddOn;
        public float DodgeChanceBase=> m_gm.GetDodgeChanceBase(AttributeType, Agility);

        /** 最大血量 */
        public float MaxHealth=> MaxHealthBase * (1.0f + MaxHealthFactor) + MaxHealthAddOn;
        public float MaxHealthBase=> m_gm.GetMaxHealthBase(AttributeType, Strength, m_initMaxHealth);

        /** 血量自动恢复 */
        public float HealthRecover=> HealthRecoverBase + HealthRecoverAddOn;
        public float HealthRecoverBase=> m_gm.GetHealthRecoverBase(AttributeType, Strength, m_initHealthRecover);

        /** 最大蓝量 */
        public float MaxMana=> MaxManaBase * (1.0f + MaxManaFactor) + MaxManaAddOn;
        public float MaxManaBase=> m_gm.GetMaxManaBase(AttributeType, Intelligence, m_initMaxMana);

        /** 蓝量自动恢复 */
        public float ManaRecover=> ManaRecoverBase + ManaRecoverAddOn;
        public float ManaRecoverBase=> m_gm.GetManaRecoverBase(AttributeType, Intelligence, m_initManaRecover);

        public override void PreAttributeChange(CAbilityAttribute attribute, float newValue)
        {
            if (attribute.Propery == (int)AttributeId.Health)
            {
                float maxHealth = MaxHealth;
                newValue = Mathf.Clamp(newValue, 0, maxHealth);
                attribute.SetValue(newValue);
            }

            if (attribute.Propery == (int)AttributeId.Mana)
            {
                float maxMana = MaxMana;
                newValue = Mathf.Clamp(newValue, 0, maxMana);
                attribute.SetValue(newValue);
            }
        }

        public override void PostAttributeExecute(CAbilityAttribute attribute)
        {
            if (attribute.Propery == (int)AttributeId.Health)
            {
                if (Health < 0)
                {
                    //target dead
                }
            }
        }
    }
}