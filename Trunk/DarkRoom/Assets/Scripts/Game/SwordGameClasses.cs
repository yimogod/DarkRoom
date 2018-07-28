using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace Sword
{
    /// <summary>
    /// 伤害包
    /// </summary>
    public class SwordDamagePacket : CDamagePacket
    {
        public float OutDamage;

        public float OutLifeSteal;

        public bool OutIsDodged;

        public float SourceOriginalDamage;

        public float SourceCritChance;

        public float SourceCritMultiplier;

        public float TargetDodgeChance;

        public float TargetArmorReduction;
    }
}
