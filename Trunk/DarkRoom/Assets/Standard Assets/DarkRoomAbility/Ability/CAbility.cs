using System;
using DarkRoom.Core;
using UnityEngine;
using DarkRoom.Game;
using UnityEditorInternal;

namespace DarkRoom.GamePlayAbility {
    /// <summary>
    /// 技能组件. 技能开始的地方. 
    /// 根据SC,相当于命令. 一般有采集, 传送, 复活, 攻击, 合体, 建造
    /// 以及最重要的 效果即时, 效果目标--我们统一成了效果目标
    /// 
    /// 根据UE4的注释. 技能定义了可以执行的游戏逻辑
    /// 另外也提供了如下功能
    /// 	是否可以使用
    ///			Cooldown
    ///			资源(mana, 耐力, 钱 等)
    ///			其他的限制
    ///		实例化支持--我们就只实现第二种情况
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
    /// 
    /// 我需要技能自带一些可能和Effect重合的信息, 比如说tag, 用于GamePlay的判断. 这样可以做到Ability和Effect分离
    /// Ability只ApplyEffect, 创建Effect的职责交给Target的AbilitySystem
    /// 
    /// 另外, 如果技能可以升级, 那么我们其实是替换为不同的技能
    /// 
    /// 我们的ability的理念跟之前有点不一样
    /// 我们是可以随意调用TryActivateAbility, 然后在技能内部判断, 此技能是否可以使用
    /// 如果不能, 就CallActivate, 如果技能成功, 我们在Commit中开始cd, 进行实际消耗
    /// 
    /// 如果我们不调用EndAbility, 那么此技能就不能再被调用
    /// </summary>
	public class CAbility : MonoBehaviour {
		public enum AffectDectectResult{
			Success, //成功碰到目标
			CDNotReady, //技能没有准备好
            StillRunning, //技能还在执行
			TargetDeadOrNull, //目标死亡或者为空
			TargetGroupNotMatch, //目标组不匹配
			TargetInvalid, //目标无效
		    TargetTagInvalid, //自身的tag不符合
            OwnerTagInvalid, //自身的tag不符合
            OutOfRange, //超出作用范围
		}

		//本技能在角色身上的索引
		[NonSerialized]
		public int Index = -1;

        /// <summary>
        /// 技能的名称, 要跟meta名称对应
        /// </summary>
        [NonSerialized]
        public string AbilityName;

		//技能的owner, 做个简单的约定, 只能是人
		protected IGameplayAbilityOwner m_owner;
        //谁发射了技能, 比如枪
        protected IGameplayAbilityOwner m_avatar;

        //技能的目标, 会是人物
        protected IGameplayAbilityOwner m_target;
		//或者坐标(在unit layer上的坐标)
		protected Vector3 m_targetLocalPosition;

		//上次施法的时间, 基于毫秒
		private long m_lastFireTime = -1;

        //是否正在运行, 调用EndAbility/CancelAbility结束技能
        private bool m_running = false;

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
		public bool CD_Ready{
			get
			{
			    int delta = CD_TimeRemaining;
                return delta <= 0;
			}
		}

        /// <summary>
        /// cd还剩的时间
        /// </summary>
        public int CD_TimeRemaining
        {
            get
            {
                if (m_lastFireTime < 0) return 0;

                long delta = CTimeUtil.GetCurrentMillSecondStamp() - m_lastFireTime;
                delta = (long)MetaBase.Period * 1000 - delta;
                if (delta < 0) return 0;

                return (int)delta;
            }
        }

        /// <summary>
        /// 重置/消除cd
        /// </summary>
        public void ResetCD(){
			m_lastFireTime = -1;
		}

		/// <summary>
		/// 通知本技能不能使用
		/// 在ui上会有消息条
		/// </summary>
		public void ReportNotReady(){
			if (CD_Ready) return;

			//Debug.Log("CD not Ready");
		}

        /// <summary>
        /// 传入本技能的owner和avatar
        /// </summary>
        public void InitAbilityActorInfo(IGameplayAbilityOwner owner, IGameplayAbilityOwner avatar)
        {
            m_owner = owner;
            m_avatar = avatar;
        }

        //owner的gameobject
        protected GameObject m_ownerGO{
			get { return m_owner.GameObject; }
		}

		protected GameObject m_targetGO {
			get{
				if (m_target == null) return null;
				return m_target.GameObject;
			}
		}

		protected virtual void Awake()
		{
		}

		protected virtual void Start()
		{
		}

		protected virtual void Update(){

		}

        /// <summary>
        /// 没有参数意味着对自己施法
        /// </summary>
        public virtual AffectDectectResult TryActivateAbility(){
			return TryActivateAbility(m_owner);
		}

        /// <summary>
        /// 对传入的目标施法
        /// </summary>
		public virtual AffectDectectResult TryActivateAbility(IGameplayAbilityOwner target){
			AffectDectectResult result = CanAffectOnTarget(target);
			if (result != AffectDectectResult.Success) {
				ReportNotReady();
				return result;
			}

			m_target = target;
			Activate();

			return result;
		}

        /// <summary>
        /// 对指定的地点施法
        /// 注意是, localPos的位置
        /// </summary>
		public virtual AffectDectectResult TryActivateAbility(Vector3 target){
			AffectDectectResult result = CanAffectOnTarget(target);
			if (result != AffectDectectResult.Success) {
				ReportNotReady();
				return result;
			}

			m_targetLocalPosition = target;
            Activate();
            return result;
		}

        /// <summary>
        /// 技能正常结束
        /// </summary>
        public void EndAbility()
        {
            //TODO 添加结束通知
            if (!m_running)return;

            m_running = false;
        }

		/// <summary>
		/// 技能是否能够对目标施展
		/// </summary>
		public virtual AffectDectectResult CanAffectOnTarget(IGameplayAbilityOwner target){
			if(!CD_Ready) return AffectDectectResult.CDNotReady;
		    if (m_running) return AffectDectectResult.StillRunning;

            if (target == null || target.InValid)
				return AffectDectectResult.TargetDeadOrNull;

		    bool b = IsTargetGroupMatch(target);
			if (!b)return AffectDectectResult.TargetGroupNotMatch;

		    b = IsActivationTagMatchForSelf();
		    if (!b) return AffectDectectResult.OwnerTagInvalid;

		    b = IsActivationTagMatchForTarget(target);
		    if (!b) return AffectDectectResult.TargetTagInvalid;

            return AffectDectectResult.Success;
		}

		/// <summary>
		/// 技能是否能够对目的地
		/// </summary>
		public virtual AffectDectectResult CanAffectOnTarget(Vector3 localPosition) {
			if (!CD_Ready) return AffectDectectResult.CDNotReady;
		    if (m_running) return AffectDectectResult.StillRunning;

		    bool b = IsActivationTagMatchForSelf();
		    if (!b) return AffectDectectResult.OwnerTagInvalid;

            return AffectDectectResult.Success;
		}

        /// <summary>
        /// target的分组是否匹配
        /// </summary>
        protected bool IsTargetGroupMatch(IGameplayAbilityOwner target)
        {
            bool b = false;
            switch (MetaBase.TargetTeamRequire)
            {
                case AbilityTargetTeam.All:
                    b = true;
                    break;
                case AbilityTargetTeam.Me:
                    b = (target == m_owner);
                    break;
                case AbilityTargetTeam.Friend:
                    b = m_owner.IsFriendTeam(target);
                    break;
                case AbilityTargetTeam.Enemy:
                    b = m_owner.IsEnemyTeam(target);
                    break;
            }

            return b;
        }

        /// <summary>
        /// require 或者 block tag是否满足
        /// </summary>
        protected bool IsActivationTagMatchForSelf()
        {
            return false;
        }

        protected bool IsActivationTagMatchForTarget(IGameplayAbilityOwner target)
        {
            return false;
        }

        /// <summary>
        /// 技能实际执行逻辑的地方, 比如造成伤害啊, 应用效果啊
        /// 子类效果必须覆盖
        /// </summary>
        protected virtual void Activate()
        {
            Debug.LogError("Child Must Override this Method");
        }

        /// <summary>
        /// 消耗资源和cd, Activate必须调用此方法
        /// </summary>
        protected void Commit()
        {
            ApplyCooldown();
            ApplyCost();
        }

        /// <summary>
        /// 开始cd
        /// </summary>
        private void ApplyCooldown()
        {
            m_lastFireTime = CTimeUtil.GetCurrentMillSecondStamp();
        }

        /// <summary>
        /// 消耗资源
        /// </summary>
        private void ApplyCost()
        {
            foreach (var item in MetaBase.CostList)
            {
                m_owner.AbilityUseCost((AbilityCostType)item.x, item.y);
            }
        }

        /// <summary>
        /// 创建技能组件, 并挂在owner身上
        /// </summary>
        public static CAbility Create(string meta, GameObject owner)
		{
			CAbility ability = null;

			CAbilityMeta emeta = CAbilityMetaManager.GetMeta(meta);
			switch (emeta.Type) {
				case CAbilityMeta.AbilityType.Attack:
					ability = owner.AddComponent<CAbilityAttack>();
					ability.AbilityName = meta;
					break;
				case CAbilityMeta.AbilityType.EffectTarget:
					ability = owner.AddComponent<CAbilitylEffectTarget>();
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