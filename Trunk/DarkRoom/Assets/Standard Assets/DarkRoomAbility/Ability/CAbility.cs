using System;
using DarkRoom.Core;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 技能组件. 技能开始的地方. 
	/// 根据SC,相当于命令. 一般有采集, 传送, 复活, 攻击, 合体, 建造
	/// 以及最重要的 效果即时, 效果目标
	/// 
	/// 
	/// 根据UE4的注释. 技能定义了可以执行的游戏逻辑
	/// 另外也提供了如下功能
	/// 	是否可以使用
	///			Cooldown
	///			资源(mana, 耐力, 钱 等)
	///			其他的限制
	///		网络复制支持
	///		实例化支持
	///			技能可以不实例化
	///			针对每个角色一个实例(大部分技能都是这个)
	///			针对每个不同的执行一个实例(比如蓄力攻击的技能. 不同蓄力形成的表现和攻击力不一样)
	///		其他
	///			比如获取能力
	/// 需要注意的是对于不需要实例化的技能需要在c++端实现.然后设计师可以用他们作为数据文件做扩展
	/// 比如GameplayAbility_Montage, 播放动画且在目标身上施加效果. 动画结束, 效果删除
	/// 
	/// 重要的方法:
	///	CanActivateAbility() 技能是否可以激活. 会被ui调用
	///	TryActivateAbility() 试图激活技能. 输入事件直接调用. 也处理(每次执行都实例化)的逻辑
	///	CallActivate() protect, 非虚函数. 做一些常见的'pre activate'内容. 然后调用 Activate()
	///	Activate() 技能具体做的事情. 继承类必须覆盖
	/// Commit()提交资源或者cd. Activate()必须调用这个方法
	/// CancelAbility()取消技能, 比如持续施法会被打断
	/// EndAbility()技能完成. 技能自身调用告知自己技能完毕了
	/// </summary>
	[RequireComponent(typeof(CController))]
	public class CAbility : MonoBehaviour {
		public enum AffectDectectResult{
			Success, //成功碰到目标
			NotReady, //技能没有准备好
			TargetDeadOrNull, //目标死亡或者为空
			TargetGroupNotMatch, //目标组不匹配
			TargetInvalid, //目标无效
            OutOfRange, //超出作用范围
		}

		//本技能在角色身上的索引
		[NonSerialized]
		public int Index = -1;

		//技能的图标
		[HideInInspector]
		public Texture2D Icon;

		/// <summary>
		/// 技能的名称, 要跟meta名称对应
		/// </summary>
		public string AbilityName;

		/// <summary>
		/// 技能的等级
		/// </summary>
		public int Level;

		/// <summary>
		/// 是否需要编辑器的动画文件
		/// </summary>
		public bool NeedEditorShow = false;

		//技能相关的效果
		//我们会在创建技能的时候创建相关效果
		protected CEffect m_effect;

		//技能的owner, 做个简单的约定, 只能是人
		protected CAIController m_owner;

		//技能的目标, 会是人物
		protected CAIController m_target;
		//或者坐标(在unit layer上的坐标)
		protected Vector3 m_targetLocalPosition;

		//上次施法的时间, 基于毫秒
		private long m_lastFireTime = -1;

		/// <summary>
		/// 技能配置信息
		/// </summary>
		public CAbilityMeta MetaBase{
			get{ return CAbilityMetaManager.GetMeta(AbilityName); }
		}

		public bool NeedTarget
		{
			get { return MetaBase.Type == CAbilityMeta.AbilityType.EffectTarget ||
							MetaBase.Type == CAbilityMeta.AbilityType.Attack; }
		}

		/// <summary>
		/// cd是否好了
		/// </summary>
		public bool Ready{
			get{
				if (m_lastFireTime < 0)return true;
				long delta = CTimeUtil.GetCurrentMillSecondStamp() - m_lastFireTime;

				return (float)delta >= (MetaBase.Period * 1000f);
			}
		}

		/// <summary>
		/// 重置/消除cd
		/// </summary>
		public void ResetCD(){
			m_lastFireTime = -1;
		}

		/// <summary>
		/// 让技能被动进入cd
		/// </summary>
		public void MakeCD(){
			m_lastFireTime = CTimeUtil.GetCurrentMillSecondStamp();
		}

		/// <summary>
		/// 通知本技能不能使用
		/// 在ui上会有消息条
		/// </summary>
		public void ReportNotReady(){
			if (Ready)return;

			//Debug.Log("CD not Ready");
		}

		//owner的gameobject
		protected GameObject m_ownerGO{
			get { return m_owner.gameObject; }
		}

		protected GameObject m_targetGO {
			get{
				if (m_target == null) return null;
				return m_target.gameObject;
			}
		}

		//挂在角色身上, 所以他的主人就是该角色
		protected virtual void Awake()
		{
			m_owner = gameObject.GetComponent<CAIController>();
		}

		protected virtual void Start()
		{
		}

		protected virtual void Update(){

		}

		public virtual AffectDectectResult Activate(){
			return Activate(m_owner);
		}

		public virtual AffectDectectResult Activate(CAIController target){
			AffectDectectResult result = CanAffectOnTarget(target);
			if (result != AffectDectectResult.Success) {
				ReportNotReady();
				return result;
			}

			m_target = target;
			m_lastFireTime = CTimeUtil.GetCurrentMillSecondStamp();
			return result;
		}

		public virtual AffectDectectResult Activate(Vector3 target){
			AffectDectectResult result = CanAffectOnTarget(target);
			if (result != AffectDectectResult.Success) {
				ReportNotReady();
				return result;
			}

			m_targetLocalPosition = target;
			m_lastFireTime = CTimeUtil.GetCurrentMillSecondStamp();
			return result;
		}

		/// <summary>
		/// 技能是否能够对目标施展
		/// </summary>
		public virtual AffectDectectResult CanAffectOnTarget(CAIController target){
			if(!Ready)return AffectDectectResult.NotReady;
			
			if (target == null || target.Pawn.DeadOrDying)
				return AffectDectectResult.TargetDeadOrNull;

			bool b = false;
			CAbilityTargetFilter filter = MetaBase.TargetFilter;
			switch (filter.TargetValue) {
				case CAbilityTargetFilter.TargetType.All:
					b = true;
					break;
				case CAbilityTargetFilter.TargetType.Me:
					b = (target == m_owner);
					break;
				case CAbilityTargetFilter.TargetType.Friend:
					b = m_owner.Pawn.IsFriendTeam(target.Pawn);
					break;
				case CAbilityTargetFilter.TargetType.Enemy:
					b = m_owner.Pawn.IsEnemyTeam(target.Pawn);
					break;
			}
			if (!b)return AffectDectectResult.TargetGroupNotMatch;
			return AffectDectectResult.Success;
		}

		/// <summary>
		/// 技能是否能够对目的地
		/// </summary>
		public virtual AffectDectectResult CanAffectOnTarget(Vector3 target) {
			if (!Ready)return AffectDectectResult.NotReady;
			return AffectDectectResult.Success;
		}


		public static CAbility Create(string meta, GameObject go)
		{
			CAbility ability = null;

			CAbilityMeta emeta = CAbilityMetaManager.GetMeta(meta);
			switch (emeta.Type) {
				case CAbilityMeta.AbilityType.Attack:
					ability = go.AddComponent<CAbilAttack>();
					ability.AbilityName = meta;
					break;
				case CAbilityMeta.AbilityType.EffectTarget:
					ability = go.AddComponent<CAbilEffectTarget>();
					ability.AbilityName = meta;
					break;
			}

			if (ability == null) {
				Debug.LogError(meta + " ability we are not handle");
			}

			return ability;
		}
	}

}