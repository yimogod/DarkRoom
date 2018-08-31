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
        //---------------------------------------------------------------------------------
    }
}