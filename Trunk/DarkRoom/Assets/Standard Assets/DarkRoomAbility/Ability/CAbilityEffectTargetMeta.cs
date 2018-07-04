using System;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 需要选择目标的技能， 就是对他人使用的技能
	/// </summary>
	public class CAbilityEffectTargetMeta : CAbilityMeta {
		/// <summary>
		/// 准备阶段的时间.
		/// 比如人族的大和炮要蓄能3秒
		/// 比如生命强化期
		/// </summary>
		public float PrepTime = 0f;

		/// <summary>
		/// 在准备阶段产生的效果
		/// </summary>
		public string PrepEffect;

		/// <summary>
		/// 结束阶段产生的时间
		/// 该阶段是该效果被应用后发生的阶段
		/// </summary>
		public float FinishTime = 0f;

		/// <summary>
		/// 技能(命中)产生的效果, 星际2里面是一个列表
		/// 他的index对应技能等级锁产生的效果
		/// 我们暂时不用考虑值这个
		/// </summary>
		public string Effect;

		/// <summary>
		/// AI通知的效果 TODO 然而并不知道咋用
		/// </summary>
		public string AINotifyEffect;

		/// <summary>
		/// 效果运行前施加的行为
		/// TODO 根据具体的技能看
		/// </summary>
		public string PreEffectBehavior;

		/// <summary>
		/// 效果运行后施加的行为
		/// TODO 根据具体的技能看
		/// </summary>
		public string PostEffectBehavior;

		/// <summary>
		/// 可以选择的范围, 基于米
		/// 如果数据小于等于0, 则无限选择
		/// </summary>
		public float Range = 4.0f;

		/// <summary>
		/// 可以选择的范围, 基于米
		/// 如果数据小于等于0, 则不做判断
		/// 
		/// 施法距离必须大于等于 min range
		/// 骑兵的冲锋就是5-10米
		/// </summary>
		public float MinRange = 0f;

		/// <summary>
		/// TODO 技能施法角度/或者弧度. 具体看那个计算更省
		/// 注意这两个值是 技能是否能使用的条件
		/// 通俗的讲就是敌人进入这个范围才能施法. 
		/// 至于伤害范围或者说其他的范围是由其他的effect定义
		/// </summary>
		public float Arc = 4.0f;

		/// <summary>
		/// 如果技能目标离开Range, 那么会有额外缓冲范围避免立刻取消技能
		/// 如果小于0, 则永不取消
		/// Slop 液体的溢出/晃动
		/// </summary>
		public float RangeSlop = 4f;

		/// <summary>
		/// 如果技能目标离开Arc, 那么会有额外缓冲范围避免立刻取消技能
		/// 如果小于0, 则永不取消
		/// </summary>
		public float ArcSlop;

		/// <summary>
		/// 显示技能选择范围.星际2用的直接是一个effect
		/// TODO 找到对应的例子, 深入了解
		/// </summary>
		public string CursorEffect;


		public CAbilityEffectTargetMeta(string idKey) : base(idKey) {
			Type = AbilityType.EffectTarget;
		}
	}
}