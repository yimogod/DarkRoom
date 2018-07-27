using System;
using System.Collections.Generic;
using DarkRoom.GamePlayAbility;

namespace Sword
{
    public class SwordAttributeSet : CAbilityAttributeSet
    {
        //------------------------------------ 角色初始化的属性 ----------------
        /** 初始化的hp */
        float InitMaxHealth;

        /** 初始血量恢复 */
        float InitHealthRecover;

        /** 初始化的魔法 */
        float InitMaxMana;

        /** 初始魔法恢复 */
        float InitManaRecover;

        /// <summary>
        /// 初始化物理伤害
        /// </summary>
        float InitPhysicDamage;

        /// <summary>
        /// 初始化防御
        /// </summary>
        float InitArmorReduction;

        //------------------------------------ 角色当前的属性 ----------------
        /** 经验, 升级清0, 设为public是因为希望有的技能可以直接增加经验. 做些好玩的 */
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

        public void InitHealthAndMana()
        {
        }

        public void InitLevel(int Value)
        {
        }

        public void LevelUp()
        {
        }

        /** 力量 */
        public float GetStrengthBase()
        {
            return 0;
        }

        public float GetStrength()
        {
            return 0;
        }

        /** 敏捷 */
        public float GetAgilityBase()
        {
            return 0;
        }

        public float GetAgility()
        {
            return 0;
        }

        /** 智力 */
        public float GetIntelligenceBase()
        {
            return 0;
        }

        public float GetIntelligence()
        {
            return 0;
        }

        /** 物理防御 */
        public float GetArmorReductionBase()
        {
            return 0;
        }

        public float GetArmorReduction()
        {
            return 0;
        }

        /** 物理攻击 */
        public float GetPhysicalDamageBase()
        {
            return 0;
        }

        public float GetPhysicalDamage()
        {
            return 0;
        }

        /** 暴击率  0 -- 100*/
        public float GetCritChanceBase()
        {
            return 0;
        }

        public float GetCritChance()
        {
            return 0;
        }

        /** 暴击系数 */
        public float GetCritMultiplierBase()
        {
            return 0;
        }

        public float GetCritMultiplier()
        {
            return 0;
        }

        /** 闪避 0 -- 100 */
        public float GetDodgeChanceBase()
        {
            return 0;
        }

        public float GetDodgeChance()
        {
            return 0;
        }

        /** 攻击速度 */
        public float GetAttackSpeedBase()
        {
            return 0;
        }

        public float GetAttackSpeed()
        {
            return 0;
        }

        /** 最大血量 */
        public float GetMaxHealthBase()
        {
            return 0;
        }

        public float GetMaxHealth()
        {
            return 0;
        }

        /** 血量自动恢复 */
        public float GetHealthRecoverBase()
        {
            return 0;
        }

        public float GetHealthRecover()
        {
            return 0;
        }

        /** 最大蓝量 */
        public float GetMaxManaBase()
        {
            return 0;
        }

        public float GetMaxMana()
        {
            return 0;
        }

        /** 蓝量自动恢复 */
        public float GetManaRecoverBase()
        {
            return 0;
        }

        public float GetManaRecover()
        {
            return 0;
        }
    }
}