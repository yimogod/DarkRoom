using System;
using DarkRoom.AI;
using UnityEngine;

namespace DarkRoom.Game {
	/// <summary>
	/// 由AI控制角色的控制器, 需要移动
	/// 这里还没有具体想法. 通过demo反推吧
	/// </summary>
	[RequireComponent(typeof(CBrainComp))]
	[RequireComponent(typeof(CAIPerceptionComp))]
	public class CAIController : CController
	{
		/// <summary>
		/// 我关心的目标
		/// 可以作为 技能选择对象存储的地方
		/// 注意, 在技能里面也会有类似的这个对象
		/// </summary>
		[HideInInspector, NonSerialized]
		public CAIController InterestingTarget;

		/// <summary>
		/// 我关心的地点
		/// 用于技能的地点或者方向
		/// 注意, 在技能里面也会有类似的这个对象
		/// </summary>
		[HideInInspector, NonSerialized]
		public Vector3 InterestingLocation;

		/// <summary>
		/// 我是否之前选中了技能, 然后选择目标
		/// 比如按下A之后的状态
		/// </summary>
		public bool InTargetSelectCommand;

		// 我的脑子
		protected CBrainComp m_brainComp;

		/// <summary>
		/// 我的感知系统
		/// </summary>
		protected CAIPerceptionComp m_perceptionComp;

		/// <summary>
		/// 寻路的结果
		/// </summary>
		protected CPathResult m_pathResult;

		/// <summary>
		/// 我自己的黑板, 跟世界黑板不是一个东西
		/// 目的是用来存储一些自己需要的数据
		/// 比如说, 人物系统里面NPC记住的event(例子: 说一个关于神秘岛的传说. 刚看见罗密欧亲了朱丽叶)
		/// 也可以记录自己对其他人的 relationship
		/// </summary>
		public CBlackBoard BlackBorad = new CBlackBoard();

		/// <summary>
		/// TODO 跟技能系统有关 做技能时再仔细考虑
		/// </summary>
		public GameObject GameplayTaskComp;

		/// <summary>
		/// 脑子来处理各种消息
		/// </summary>
		public CBrainComp BrainComp
		{
			get { return m_brainComp; }
		}

		protected override void Awake()
		{
			base.Awake();
			m_pathResult = new CPathResult();
			m_brainComp = gameObject.GetComponent<CBrainComp>();
			m_perceptionComp = gameObject.GetComponent<CAIPerceptionComp>();
		}

		protected override void Start()
		{
			base.Start();
		}

		/// <summary>
		/// 本ctrl对此消息感兴趣
		/// </summary>
		public void ObserveMessage(string type, Action<CAIMessage> action){
			CAIMessageObserver observer = new CAIMessageObserver(type, action);
			m_brainComp.RegisterMessageObserver(observer);
		}

		/// <summary>
		/// 解除消息兴趣
		/// </summary>
		public void UnobserveMessage(string type){
			m_brainComp.UnregisterMessageObserver(type);
		}

		public void UnobserveMessage(string type, Action<CAIMessage> action){
			m_brainComp.UnregisterMessageObserver(type, action);
		}

		/// <summary>
		/// target 是否在我视野内
		/// 目前只考虑xz平面. 未来做到有高差的游戏, 我们需要添加高度可视
		/// </summary>
		/// <param name="target">是否在视野内的目标</param>
		/// <param name="viewPoint">眼睛朝向, 如果viewPoint传入的是Vector3.zero, 我们就使用自身的朝向</param>
		/// <returns></returns>
		public bool LineOfSightTo(CController target, Vector3 viewPoint) {
			return m_pawn.FOV.ContainPoint(target.LocalPosition);
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			m_brainComp = null;
			BlackBorad = null;
			InterestingTarget = null;
			m_perceptionComp = null;
		}
	}
}