using System;
using System.Collections.Generic;
using DarkRoom.GamePlayAbility;

namespace Sword
{
    /// <summary>
    /// 主属性合集
    /// </summary>
    public class SwordPrimaryAttribute
    {
        public CAbilityAttribute Strength;
        public CAbilityAttribute Dexterity;
        public CAbilityAttribute Constitution;
        public CAbilityAttribute Magic;
        public CAbilityAttribute Willpower;
        public CAbilityAttribute Cunning;
        public CAbilityAttribute Luck;
    }

    /// <summary>
    /// 二级属性合集
    /// </summary>
    public class SwordSubAttribute
    {
        public CAbilityAttribute Accuracy;
        public CAbilityAttribute Encumbrance;
        public CAbilityAttribute Defense;
        public CAbilityAttribute RangedDefense;
        public float CriticalChance;
        public float ShrugOffCriticalsChance;
        public float HealingModification;
    }

    /// <summary>
    /// 消耗的资源合集
    /// </summary>
    public class SwordResourceAttribute
    {
        public CAbilityAttribute MaxHealth;
        public CAbilityAttribute MaxMana;
        public CAbilityAttribute MaxStamina;
    }

    public class SwordPowerAttribute
    {
        public CAbilityAttribute Physical;
        public CAbilityAttribute Spell;
        public CAbilityAttribute Mind;
        public CAbilityAttribute PhysicalSave;
        public CAbilityAttribute SpellSave;
        public CAbilityAttribute MindSave;
    }

    public class SwordArmorAttribute
    {
        public CAbilityAttribute Armour;
        public float ArmourPenetration;
        public float ArmourHardiness;
    }

    /// <summary>
    /// 抗性数据合集
    /// </summary>
    public class SwordResistanceAttribute
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
    public class SwordImmunityAttribute
    {

    }
}
