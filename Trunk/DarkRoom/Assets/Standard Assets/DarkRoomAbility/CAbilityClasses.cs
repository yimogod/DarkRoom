using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility
{
    //----------------------- not sure delete in future -------------------------
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
        public enum TargetLocationType
        {
            Point,
            Unit,
            PointOrUnit
        }

        public enum Order
        {
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
        /// 
        /// TODO 看是否能干掉这个location
        /// </summary>
        public enum Location
        {
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
    }

    //----------------------- used for now -------------------------

    /// <summary>
    /// 技能类型的枚举定义
    /// </summary>
    public enum CAbilityType
    {
        Attack, //攻击
        EffectTarget, //需要选定目标的effect
        EffectPosition, //对于目标点或者方向实施的技能
    }

    /// <summary>
    /// 技能消耗的资源种类
    /// </summary>
    public enum CAbilityCostType
    {
        MP, //蓝
        HP, //红
        Vital, //活力
        Count,
    }

    /// <summary>
    /// 用何种方法进行属性修改
    /// </summary>
    public enum CAttributeModMethod
    {
        ConstValue, //固定值
        CustomCalculation, //通过传入的计算机进行的修改
    }

    /// <summary>
    /// 对属性的操作的算数
    /// </summary>
    public enum CAttributeModOp
    {
        Additive, //加法
        Multiplicitive, //乘法
        Division, //出发
        Override, //覆盖
        Max,
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
    }
}
