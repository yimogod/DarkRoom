using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace Sword
{
    public class SwordGameMode : CGameMode
    {
        /// <summary>
        /// 角色的最高等级
        /// </summary>
        public const int MAX_LEVEL = 9;

        public void ModifyDamage(SwordDamagePacket damagePacket)
        {
            if (damagePacket.TargetDodgeChance > 50.0f)
            {
                Debug.LogWarning("Target DodgeChance is bigger than 0.5f");
                return;
            }

            float randDoage = Random.Range(0.0f, 100.0f);
            if (randDoage < damagePacket.TargetDodgeChance)
            {
                damagePacket.OutDamage = 0;
                damagePacket.OutIsDodged = true;
                return;
            }

            if (damagePacket.SourceCritChance > 90.0f)
            {
                Debug.LogWarning("Source CritChance is bigger than 0.9f");
                return;
            }

            float value = damagePacket.SourceOriginalDamage;
            float randCrit = Random.Range(0.0f, 100.0f);
            if (randCrit < damagePacket.SourceCritChance)
            {
                if (damagePacket.SourceCritMultiplier < 1.5f) damagePacket.SourceCritMultiplier = 1.5f;
                value *= damagePacket.SourceCritMultiplier;
            }

            float minDamage = value * 0.1f;
            float modifyDamage = value - damagePacket.TargetArmorReduction;
            if (modifyDamage < minDamage) modifyDamage = minDamage;
            damagePacket.OutDamage = modifyDamage;
        }

        /** 当前的经验是否可以升级 */
        public bool CanLevelUp(int currLevel, int currExp)
        {
            int nextLevel = currLevel + 1;
            if (nextLevel > MAX_LEVEL) nextLevel = MAX_LEVEL;
            int requiredExp = 0;//HeroExpList[nextLevel];
            return currExp >= requiredExp;
        }

        //----------------------------------- 属性计算规则 --------------------------------
        public float GetStrengthBase(ActorClass attri, int level)
        {
            if (level <= 0) return 0;

            float valueInit = 0;
            float valueStep = 0;

            switch (attri)
            {
                case ActorClass.Warrier:
                    valueInit = 8.0f;
                    valueStep = 2.4f;
                    break;
                case ActorClass.Ranger:
                    valueInit = 6.0f;
                    valueStep = 1.8f;
                    break;
                case ActorClass.Wizard:
                    valueInit = 7.0f;
                    valueStep = 1.6f;
                    break;
            }

            return valueInit + valueStep * level;
        }

        public float GetAgilityBase(ActorClass attri, int level)
        {
            if (level <= 0) return 0;

            float valueInit = 0;
            float valueStep = 0;

            switch (attri)
            {
                case ActorClass.Warrier:
                    valueInit = 16.0f;
                    valueStep = 1.8f;
                    break;
                case ActorClass.Ranger:
                    valueInit = 24.0f;
                    valueStep = 2.8f;
                    break;
                case ActorClass.Wizard:
                    valueInit = 14.0f;
                    valueStep = 1.5f;
                    break;
            }

            return valueInit + valueStep * level;
        }

        public float GetIntelligenceBase(ActorClass attri, int level)
        {
            if (level <= 0) return 0;

            float valueInit = 0;
            float valueStep = 0;

            switch (attri)
            {
                case ActorClass.Warrier:
                    valueInit = 4.0f;
                    valueStep = 1.4f;
                    break;
                case ActorClass.Ranger:
                    valueInit = 4.0f;
                    valueStep = 1.6f;
                    break;
                case ActorClass.Wizard:
                    valueInit = 8.0f;
                    valueStep = 2.9f;
                    break;
            }

            return valueInit + valueStep * level;
        }

        public float GetArmorReductionBase(ActorClass attri, float strength, float initArmor)
        {
            if (strength <= 0) return initArmor;

            float initValue = 0;
            if (attri == ActorClass.Warrier)initValue = 12.0f;
            if (attri == ActorClass.Ranger)initValue = 6.0f;
            if (attri == ActorClass.Wizard)initValue = 5.0f;

            return initValue + strength * 0.2f + initArmor;
        }

        public float GetPhysicalDamageBase(ActorClass attri, float strength, float agility,
            float intelligence, float initDamage)
        {
            if (strength <= 0 || agility <= 0 || intelligence <= 0) return initDamage;

            float initValue = 0;
            if (attri == ActorClass.Warrier)initValue = 54.0f + strength * 1.6f;
            if (attri == ActorClass.Ranger)initValue = 50.0f + agility * 2.0f;
            if (attri == ActorClass.Wizard)initValue = 48.0f + intelligence * 1.5f;

            return initValue + initDamage;
        }

        public float GetDodgeChanceBase(ActorClass attri, float agility)
        {
            if (agility <= 0) return 0.0f;

            float initValue = 0;
            if (attri == ActorClass.Warrier)initValue = 4.0f;
            if (attri == ActorClass.Ranger)initValue = 6.0f;
            if (attri == ActorClass.Wizard)initValue = 5.0f;

            return initValue + agility * 0.1f;
        }

        public float GetCritChanceBase(ActorClass attri, float agility)
        {
            if (agility <= 0) return 0.1f;

            float initCrit = 0;
            if (attri == ActorClass.Warrier)initCrit = 2.0f;
            if (attri == ActorClass.Ranger)initCrit = 2.0f;
            if (attri == ActorClass.Wizard)initCrit = 1.0f;

            return initCrit + agility * 0.1f;
        }

        public float GetCritMultiplierBase(ActorClass attri, float strength, float agility,
            float Intelligence)
        {
            return 2.0f;
        }

 
        public float GetMaxHealthBase(ActorClass attri, float strength, float initHealth)
        {
            if (strength <= 0) return initHealth;

            float initValue = 0;
            if (attri == ActorClass.Warrier)initValue = 100.0f;
            if (attri == ActorClass.Ranger)initValue = 80.0f;
            if (attri == ActorClass.Wizard)initValue = 75.0f;

            return initValue + strength * 50.0f + initHealth;
        }

        public float GetHealthRecoverBase(ActorClass attri, float strength, float initHealthRecover)
        {
            if (strength <= 0) return initHealthRecover;
            return -2.0f + strength * 0.2f + initHealthRecover;
        }

        public float GetMaxManaBase(ActorClass attri, float intelligence, float initMana)
        {
            if (intelligence <= 0) return initMana;

            float initValue = 0;
            if (attri == ActorClass.Warrier)initValue = 20.0f;
            if (attri == ActorClass.Ranger)initValue = 22.0f;
            if (attri == ActorClass.Wizard)initValue = 30.0f;

            return initValue + intelligence * 25.0f + initMana;
        }

        public float GetManaRecoverBase(ActorClass attri, float intelligence, float initManaRecover)
        {
            if (intelligence <= 0) return initManaRecover;
            return 0.2f + intelligence * 0.1f + initManaRecover;
        }

        //---------------------------------------------------------------------------------
    }
}