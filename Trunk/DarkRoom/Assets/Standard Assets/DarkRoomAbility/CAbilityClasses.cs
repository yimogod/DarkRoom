using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
    /// <summary>
    /// 技能消耗的资源种类
    /// </summary>
    public enum AbilityCostType
    {
        MP, //蓝
        HP, //红
        Vital, //活力
        Count,
    }

    public enum AbilityTargetTeam
    {
        None, //不需要目标, 可以选择地面
        Me,
        Friend,
        Enemy,
        All
    }

    /// <summary>
    /// 作为武器攻击或者指向性技能的过滤器
    /// TODO 可以通过tag来实现
    /// </summary>
    /*public class CAbilityTargetFilter {
		/// <summary>
		/// 过滤器枚举
		/// TODO 可以通过tag来实现
		/// </summary>
		public enum Filter
		{
			Missile, //飞行物
			UnitOnGround, //地面单位
			UnitInAir, //空中单位
			Building, //建筑
			Visible, //可见的单位
			Light, //轻甲
			Armored, //重甲
			Biological, //生物单位
			Invulnerable, //无敌的
			Cloaked, //隐身的
			Self, //自己
			Count,
		}

		public enum TargetTeam
		{
            None, //不需要目标, 可以选择地面
			Me,
			Friend,
			Enemy,
			All
		}

		/// <summary>
		/// 单个枚举的值
		/// </summary>
		public enum Value
		{
			Allowable, //满足一个就可以, 相当于 ||
            Essential, //必须要满足的, 相当于 &&
			PermissionDeny, //拒绝, 相当于 !( || )
		}

		/// <summary>
		/// 过滤器的值. index是各个filter enum的值
		/// </summary>
		public Value[] FilterValue;

		/// <summary>
		/// 技能的目标, 默认我们是all
		/// </summary>
		public TargetTeam TargetValue;

		public CAbilityTargetFilter()
		{
			int count = (int) Filter.Count;
			FilterValue = new Value[count];

			for (int i = 0; i < count; i++) {
				FilterValue[i] = Value.Allowable;
			}

			TargetValue = TargetTeam.All;
		}
	}*/

	public class CAbilityEnum
	{
		/// <summary>
		/// 目标类型
		/// 用于 effsetset 选择目标的方式
		/// </summary>
		public enum TargetLocationType {
			Point,
			Unit,
			PointOrUnit
		}

		public enum Order {
			Invalid, //无效指令
			Move, //移动
			StopMove, //停止移动
			InstantEffect, //立刻施法, 是为了统一接口添加的指令类型
			DelayEffect, //延迟施法
			NotifyEffect, //接受通知的效果
		}

		/// <summary>
		/// 效果的目标谁身上
		/// 用于配置接收该指令的单位
		/// 根据需求进行调整
		/// </summary>
		public enum Location {
			TargetUnit, //目标的中心
			TargetPoint, //传入的目标坐标点
			TargetDirection, //目标的方向
			//TargetFloor, //目标脚下
			//TargetTop, //目标头顶
			//TargetHand, //目标的手上

			CasterUnit, //来源中心
			CasterPoint, //来源发起时的坐标(来源可能在不断位移)
			//SourceFloor, //来源脚下
			//SourceTop, //来源头顶
			//SourceHand, //来源的手上
		}

		/// <summary>
		/// 属性类型. TODO 会修正位置
		/// </summary>
		public enum AttributeType {
			Light, //轻甲
			Armored, //重甲
			Biological, //生物
			Mechanical, //机械
			Psionic, //灵能
			Massive, //巨型单位, 比如雷神
			Structure, //建筑
			Heroic, //英雄
			Summoned, //召唤物
			UserDef_1, //用户自定义
			UserDef_2, //用户自定义
			UserDef_3, //用户自定义
			Count,
		}

		/// <summary>
		/// buff对属性的修改
		/// 人物属性里面也有. 重复定义的原因是方便技能编辑器里面形成数据
		/// </summary>
		public class BuffModification {
			/// <summary>
			/// 对速度的修正
			/// </summary>
			public float MoveSpeedMultiplier = 1f;

			/// <summary>
			/// 对速度的修正
			/// </summary>
			public float MoveSpeedBonus = 0f;

			/// <summary>
			/// 杀敌经验系数
			/// </summary>
			public float KillXPMultiplier = 1f;

			/// <summary>
			/// 杀敌经验增加
			/// </summary>
			public float KillXPBonus = 0f;

			/// <summary>
			/// 生命系数
			/// </summary>
			public float LifeMultiplier = 1f;
			/// <summary>
			/// 生命增加
			/// </summary>
			public float LifeXPBonus = 0f;

			/// <summary>
			/// 魔法系数
			/// </summary>
			public float ManaMultiplier = 1f;
			/// <summary>
			/// 魔法增加
			/// </summary>
			public float ManaBonus = 0f;

			/// <summary>
			/// 活力系数
			/// </summary>
			public float VitalMultiplier = 1f;
			/// <summary>
			/// 活力增加
			/// </summary>
			public float VitalBonus = 0f;

			/// <summary>
			/// 视野增加
			/// </summary>
			public float SightBonus = 0f;
		}
	}

	/// <summary>
	/// 交互位置. 用来确认该效果的中心点
	/// 比如伤害命中的位置是对方
	/// buff的位置有可能是自己
	/// </summary>
	public struct CVisualEffect
	{
		public CAbilityEnum.Location LocationValue;

		/// <summary>
		/// 视觉效果的prefab
		/// </summary>
		public string Prefab;

		/// <summary>
		/// 是否有效
		/// </summary>
		public bool Valid{
			get { return !string.IsNullOrEmpty(Prefab); }
		}
	}

    public enum CGameplayAbilityTriggerSource
    {
        // Triggered from a gameplay event, will come with payload
        GameplayEvent,

        // Triggered if the ability's owner gets a tag added, triggered once whenever it's added
        OwnedTagAdded,

        // Triggered if the ability's owner gets a tag removed, triggered once whenever it's removed
        OwnedTagRemoved,
    }

    /// <summary>
    /// 技能被动激活的触发器数据结构
    /// </summary>
    public struct CAbilityTriggerData
    {
        /// <summary>
        /// 相关tag
        /// </summary>
        public CGameplayTag TriggerTag;

        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName;

        /** The type of trigger to respond to */
        CGameplayAbilityTriggerSource TriggerSource;
    };
}
