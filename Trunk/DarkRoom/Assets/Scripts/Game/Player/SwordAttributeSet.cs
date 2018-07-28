using System;
using System.Collections.Generic;
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
        //------------------------------------ 角色初始化的属性 ----------------
        /// <summary>
        /// 初始化的最大hp
        /// </summary>
        public float InitMaxHealth = 0;

        /// <summary>
        /// 初始血量恢复
        /// </summary>
        public float InitHealthRecover = 0;

        /// <summary>
        /// 初始化的魔法
        /// </summary>
        public float InitMaxMana = 0;

        /// <summary>
        /// 初始魔法恢复
        /// </summary>
        public float InitManaRecover = 0;

        /// <summary>
        /// 初始化物理伤害
        /// </summary>
        public float InitPhysicDamage = 0;

        /// <summary>
        /// 初始化防御
        /// </summary>
        public float InitArmorReduction;

        //------------------------------------ 角色当前的属性 ----------------
        /// <summary>
        /// 角色成长类型
        /// </summary>
        public AttributeType AttributeType;

        /// <summary>
        /// 经验, 升级清0, 设为public是因为希望有的技能可以直接增加经验. 做些好玩的
        /// </summary>
        public int Exp;

        public float Health;

        public float Mana;

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
        public float LifeSteal;

        /** 技能/buff/道具对最大血量的修改, 加法 */
        public float MaxHealthAddOn;

        /** 技能/buff/道具对最大血量的修改, 乘法 */
        public float MaxHealthFactor;

        /** 技能/buff/道具对自动恢复血量的修改 */
        public float HealthRecoverAddOn;

        /** 技能/buff/道具对魔法的修改, 加法 */
        public float MaxManaAddOn;

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

        private int m_level;

        private SwordGameMode m_gm => CWorld.Instance.GetGameMode<SwordGameMode>();

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
        public float ArmorReductionBase => m_gm.GetArmorReductionBase(AttributeType, Strength, InitArmorReduction);

        /** 物理攻击 */
        public float PhysicalDamage=> PhysicalDamageBase + PhysicalDamageAddOn;
        public float PhysicalDamageBase=> m_gm.GetPhysicalDamageBase(AttributeType, Strength, Agility, Intelligence, InitPhysicDamage);

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
        public float MaxHealthBase=> m_gm.GetMaxHealthBase(AttributeType, Strength, InitMaxHealth);

        /** 血量自动恢复 */
        public float HealthRecover=> HealthRecoverBase + HealthRecoverAddOn;
        public float HealthRecoverBase=> m_gm.GetHealthRecoverBase(AttributeType, Strength, InitHealthRecover);

        /** 最大蓝量 */
        public float MaxMana=> MaxManaBase * (1.0f + MaxManaFactor) + MaxManaAddOn;
        public float MaxManaBase=> m_gm.GetMaxManaBase(AttributeType, Intelligence, InitMaxMana);

        /** 蓝量自动恢复 */
        public float ManaRecover=> ManaRecoverBase + ManaRecoverAddOn;
        public float ManaRecoverBase=> m_gm.GetManaRecoverBase(AttributeType, Intelligence, InitManaRecover);
    }
}