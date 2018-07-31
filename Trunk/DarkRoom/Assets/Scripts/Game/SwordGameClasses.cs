using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace Sword
{
    public enum ActorClass
    {
        Warrier, //力量单位
        Ranger, //敏捷单位
        Wizard, //智力单位
    }

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
