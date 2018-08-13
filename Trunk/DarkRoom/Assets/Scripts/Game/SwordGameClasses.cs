using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace Sword
{
    /// <summary>
    /// 基础职业
    /// </summary>
    public enum MetaClass
    {
        //战士类, 活力为主要资源, 主要天赋是Technique, 主要伤害类型是物理伤害
        Warrier = 1,
        //法师类, mana为主要资源, 主要天赋是Spell, 主要伤害类型是arcane, 法术伤害
        Mage,
    }

    /// <summary>
    /// 实际的职业
    /// </summary>
    public enum SubClass
    {
        //狂战, 双手武器, 牺牲防御增加攻击, 最重要的属性Strength and Constitution
        Berserker = 1,
    }

    /// <summary>
    /// 基础种族
    /// </summary>
    public enum MetaRace
    {
        Human = 1,
        Elf,
    }

    /// <summary>
    /// 实际的种族
    /// </summary>
    public enum SubRace
    {
        //------- human ------
        Cornac = 1,
        Higher,
        //------- elf ------
        Shalore,
        Thalore,
    }

    /// <summary>
    /// 主要属性
    /// 每级有3个属性点可以分配给这六个属性, 另外每个属性有两个上限
    /// 1. 小于 60
    /// 2. 小于 level * 1.4 + 20
    /// 但npc是可以超过这些限制的
    /// Luck 比较特殊, 你不能给他加点, 只能通过技能和装备
    /// 天赋和装备对属性都有最低要求
    /// </summary>
    public enum PrimaryAttribute
    {
        /// 力量会增加你的近战武器物理攻击和物理防御和负重
        /// Gains per point:
        /// Physical power:   1.00
        /// Max encumbrance:  1.80
        /// Physical save:     .35
        Strength,
        /// 敏捷会增加你的敏捷度和命中率, 闪避率. 提高匕首和远程武器的伤害
        /// Gains per point:
        /// Defense:                     .35
        /// Ranged defense:              .35
        /// Accuracy:                   1.00
        /// Shrug off criticals chance:  .30 //无视暴击的几率
        Dexterity,
        /// 体质会增加max hp, 减少物理伤害, 增加治疗效果
        /// Gains per point:
        /// Max life:       4.00 (*)
        /// Physical save:  .35
        /// Healing modification: 0.7%
        Constitution,
        /// 魔力增加各种魔法物品的能力, 减少mana消耗
        /// Gains per point:
        /// Spell save:  .35
        /// Spellpower:  1.00
        Magic,
        /// 意志力减少思想和魔法的攻击力
        /// Gains per point:
        /// Max Mana:       5.00
        /// Max Stamina:    2.50
        /// Max Psi:        1.00 (*)
        /// Mindpower:      .70
        /// Mental save:    .35
        /// Spell save:     .35
        /// Accuracy:       .35 (only when using Psi combat)
        Willpower,
        /// 灵巧影响学习思考和交互. 提高暴击率, 和抛射物的伤害, 
        /// Gains per point:
        /// Critical hit chance:  .30
        /// Mental save:          .35
        /// Mindpower:            .40
        /// Accuracy:             .35 (only when using Psi combat)
        Cunning,
        /// 运气对很多战斗属性都有加成, luck是个隐藏属性
        /// Gains per point:
        /// Accuracy:             .40
        /// Defense:              .40
        /// Critical hit chance:  .30
        /// Physical save:        .175
        /// Mental save:          .175
        /// Spell save:           .175
        Luck,
    }

    public enum SubAttribute
    {
        /// 精度, 属于防御的部分属性
        /// 4 
        /// + (Dexterity - 10) * 1.00 
        /// + Luck * 0.40 
        /// + Accuracy bonuses from Combat Accuracy
        /// + Accuracy bonuses from equipment 
        /// + Accuracy bonuses or penalties from effects
        Accuracy,
        /// <summary>
        /// 防御和穿透防御不同, 但公式一样
        /// Defense bonuses from equipment
        /// + (Dexterity - 10) * 0.35 
        /// + Defense bonuses from talents 
        /// + Luck * 0.4
        /// </summary>
        Defense,
    }

    /// <summary>
    /// 攻击力和豁免
    /// </summary>
    public enum PowerAndSave
    {
        Physical,
        Spell,
        Mind,
        PhysicalSave,
        SpellSave,
        MentalSave,
    }

    /// <summary>
    /// 抗性
    /// </summary>
    public enum Resistance
    {
        Physical,
        Fire,
        Cold,
        Lightning,
        Nature,
        BLight,
        Light,
        Darkness,
        Mind,
        Temporal,
        All,
    }

    /// <summary>
    /// 消耗的资源
    /// </summary>
    public enum Resource
    {
        Health, //红
        Mana, //蓝
        /// <summary>
        /// Stamina 活力/精力
        /// 大部分物理天赋都会用到. 默认每回合恢复0.3, 有buff可以修改本值. 
        /// 休息的时候可以加快恢复速度
        /// </summary>
        Stamina,
    }

    /// <summary>
    /// 伤害类型
    /// </summary>
    public enum DamageType
    {
        Physical,
        Fire,
        Cold,
        Lightning,
        Nature,
        BLight,
        Light,
        Darkness,
        Mind,
        Temporal,
    }

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
