using System;
using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.Game;

namespace Sword
{

	//--------------------- Hero Attribute ----------------
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
	public enum ActorClass
	{
		//--------Warrier--------
		//狂战, 双手武器, 牺牲防御增加攻击, 最重要的属性Strength and Constitution
		[StringValueAttribute("Berserker")]
		Berserker = 1,

		//-------- mage ------------
		// 术士
		[StringValueAttribute("Alchemist")]
		Alchemist
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
	public enum ActorRace
	{
        //------- human ------
        [StringValueAttribute("Cornac")]
        Cornac = 1,
        [StringValueAttribute("Higher")]
        Higher,

        //------- elf ------
        [StringValueAttribute("Shalore")]
        Shalore,
        [StringValueAttribute("Thalore")]
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
		/// 力量会增加你的武器物理攻击和暴击伤害
		/// Gains per point:
		/// Physical power:   0.5%
		/// Max encumbrance:  1.80
		/// 0.4%的暴击伤害
		Strength,

		/// 敏捷会增加暴击几率和闪避几率
		/// Gains per point:
		/// Crit chance: 0.2%
		/// Dodge chance: 0.2%
		Dexterity,

		/// 魔力增加各种魔法物品的能力, 增加mp, 增加猛击几率
		/// Gains per point:
		/// Spell Power:  0.5%
		/// Smash chance: 0.2%
		/// mp 1.00
		Intelligence,

		/// 体质会增加max hp, 减少物理伤害, 增加治疗效果, 增加格挡几率
		/// Gains per point:
		/// Max life:       4.00 (*)
		/// Physical save:  2.5%
		/// Healing modification: 0.7%
		/// Parry Chance: 0.2% (如果有盾牌的话)
		Constitution,

		/// 意志力减少思想和魔法的攻击力
		/// Gains per point:
		/// Max Stamina:    2.50
		/// Max Psi:        1.00 (*)
		/// Mindpower:      .70
		/// Mental save:    .35
		/// Spell save:     .35
		Willpower,

		/// 运气对很多战斗属性都有加成, luck是个隐藏属性
		/// Gains per point:
		/// Defense:              .40
		/// Critical hit chance:  .30
		/// Physical save:        .175
		/// Mental save:          .175
		/// Spell save:           .175
		Luck,
	}

	public enum SubAttribute
	{


		/// <summary>
		/// 负重
		/// </summary>
		Encumbrance,

		/// <summary>
		/// 防御
		/// Defense bonuses from equipment
		/// + (Dexterity - 10) * 0.35 
		/// + Defense bonuses from talents 
		/// + Luck * 0.4
		/// </summary>
		Defense,



		/// <summary>
		/// 治疗效果的百分比提升
		/// </summary>
		HealingModification
	}

	/// <summary>
	/// 另外一种平行属性
	/// </summary>
	public enum ThirdAttribute
	{
		/// <summary>
		/// 移动能力
		/// </summary>
		MoveRange,
		/// <summary>
		/// 视野范围
		/// </summary>
		ViewRange,
	}

	/// <summary>
	/// 攻击力
	/// </summary>
	public enum AttackPower
	{
		/// <summary>
		/// 暴击几率
		/// </summary>
		CriticalChance,

		/// <summary>
		/// 暴击伤害加成
		/// </summary>
		CriticalMultiplierr,

		/// <summary>
		/// 物理伤害加成
		/// </summary>
		PhysicalPowerMultiplier,

		/// <summary>
		/// 元素伤害加成
		/// </summary>
		SpellPowerMultiplier,

		/// <summary>
		/// 意念伤害加成
		/// </summary>
		MindPowerMultiplier,

		/// <summary>
		/// 护甲穿透
		/// </summary>
		ArmorPenetration,

		/// <summary>
		/// 猛击几率
		/// </summary>
		SmashChance,
	}

	/// <summary>
	/// 防御
	/// </summary>
	public enum DefencePower
	{
		//物防
		PhysicalSave,
		//法防
		SpellSave,
		//意念防
		MindSave,

		//物理伤害减免百分比
		PhysicalHardiness,
		//法伤伤害减免百分比
		SpellHardiness,
		//意念伤害减免百分比
		MindHardiness,
		
		//格挡几率
		ParryChance,

		//反弹伤害几率
		DamageReflectChance,
		//反弹伤害的伤害比例, 反弹伤害是混乱伤害
		DamageReflectMultiplier,
	}

	/// <summary>
	/// 抗性
	/// </summary>
	public enum Resistance
	{
		/// <summary>
		/// 抗性
		/// </summary>
		Resistance,

		/// <summary>
		/// 抗性穿透
		/// 减少对方的抗性的百分比
		/// </summary>
		ResistancePenetration,

		/// <summary>
		/// 抗性削弱
		/// 减少对方的免疫数据
		/// </summary>
		ResistanceReduction,
	}

	/// <summary>
	/// 抗性的类型
	/// </summary>
	public enum ResistanceType
	{
		All, //all比较特殊. 和剩下的类型有加成关系
		Physical,
		Fire,
		Cold,
		Lightning,
		Nature,
		Light,
		Darkness,
		Mind,
		Temporal,
	}

	/// <summary>
	/// 对某伤害类型的免疫百分比
	/// 估计这些类型会修改定义
	/// </summary>
	public enum ImmunityType
	{
		Confusion,
		Stun,
		Freeze,
		Fear,
		Knockback,
		Instadeath,
		Disarm,
		Poison,
		Disease,
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
		Ice,
		Fire,
		Lightning,
		Poison,
		Mind,
	}

	//--------------------- item ----------------------

	public enum ItemType
	{
		/// <summary>
		/// 道具(血瓶, 饼干)
		/// </summary>
		Props = 1,

		/// <summary>
		/// 材料(宝石,毛皮)
		/// </summary>
		Material,

		/// <summary>
		/// 装备
		/// </summary>
		Equipment,
	}

	//Slot 和 type是多对多的关系, 所以不能通过slot获得type, 反之亦然
	public enum EquipSlot
	{
		Head,//头盔
		Neck,//项链
		Chest,//盔甲
		Waist,//腰带
		Hand, //手套
		LeftRing,
		RightRing,
		Leg,
		Feet,//鞋
		PrimaryWeapon,
		SecondaryWeapon,
		Count,
	}

	public enum EquipType
	{
		Null,
		Cap,
		Necklace,
		Armor,
		Belt,
		Glove, //手套
		Ring,
		Trouser,
		Shoe,
		Weapon,
		Sheild,
		Count
	}

	public enum EquipMethod
	{
		Null,
		LinkAsChild,
		ReplaceSkinMesh,
		UseSelfAnimator,
	}

	public enum WeaponHand
	{
		/// <summary>
		/// 单手武器
		/// </summary>
		SingleHand,

		/// <summary>
		/// 双手武器
		/// </summary>
		TwoHand,
	}

	/// <summary>
	/// 武器类型
	/// 武器类型定了, 意味着单双手, 基础攻击距离定了就
	/// </summary>
	public enum WeaponType
	{
		//赤手空拳
		Fist = 0,
		//单手剑
		Sword,
		//爪
		Claw,
		//匕首
		Dagger,
		//盾牌
		Shield,

		//大剑, 一般双手
		GreateSword,
		//枪之类的长柄武器, 一般双手
		Polearm,

		//弓箭, 一般双手
		Bow,
		//弩, 一般单手
		Crossbow,
		//手枪
		Pistol,
		//散弹枪
		Shotgun,
		//大炮, 双手
		Cannon,
		//魔杖, 单手
		Wand,
		//法杖, 双手
		Staff,
	}

	//---------------------- damage -----------------------
	/// <summary>
	/// 包含6中属性的结构体包
	/// 用于攻击, 防御, 抗性
	/// </summary>
	public struct DamageTypePacket
	{
		public float Physical;
		public float Ice;
		public float Fire;
		public float Lightning;
		public float Poison;
		public float Mind;
	}

	/// <summary>
	/// 平铺开所有计算伤害需要的属性
	/// 估计最后会有几十个属性
	/// </summary>
	public class SwordDamagePacket : CDamagePacket
	{
		// ------------------计算后的伤害数值-------------------
		public float OutDamage;

		public float OutLifeSteal;

		public bool OutIsDodged;

		// ------------- 攻击者的属性 --------------
		public float SourceOriginalDamage;

		public float SourceCritChance;

		public float SourceCritMultiplier;


		// ------------ 受害者的属性 -------------
		public float TargetDodgeChance;

		public float TargetArmorReduction;
	}
}