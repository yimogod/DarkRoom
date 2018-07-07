using System;
using System.Collections;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
    /// <summary>
    /// 技能系统, 挂在角色身上管理着身上的具体技能.
    /// 根据SC会有三个主要组件Ability, Behavior, Effect
    /// 
    /// 
    /// 根据UE会有Ability, Effect, Attribute.
    /// 来源于UE的注释, system提供的主要方法有
    /// Ability
    ///		提供赋予技能的接口
    ///		管理着技能实例
    /// 
    /// Effect
    ///		提供一个FActiveGameplayEffectsContainer容器来管理所有活动着的Effect
    ///		提供接口应用Effect到目标身上
    ///		FActiveGameplayEffectsContainers里封装查询信息
    ///		提供接口清理Effect
    /// 
    /// Attribute
    ///		提供属性初始化/设置器
    ///		提供方法获取属性
    /// 
    /// 在UE中, Cability是通过调用CAbilitySystem来 ApplyEffect 的
    /// CAbilitySystem相当于一个代理, 提供了技能系统的入口和很多实用方法, 比如
    /// 设置owner和avatar
    /// 获取owner身上的指定buff, 以及buff的开始时间, 剩余时间, 叠加层数/或者实例个数
    /// 移除buff, 可以根据类型, id等
    /// 查询tag
    /// 
    /// 特别奇怪(好像也不奇怪). 针对CAbility,  此类的责任有
    /// 1. 管理技能的实例(因为技能分3个instance类型), 我之前的ability都是一个实例
    ///     怀疑这里的instance类型可以匹配到我的Effect instance 类型--不能对应. 
    ///     因为Effect的对象不一定是自己, 而Ability肯定是自己
    /// 2. ActiviteAbility
    /// 3. CancelAbility
    /// 4. 通过Tag禁言技能
    /// </summary>
    public class CAbilitySystem : MonoBehaviour
	{
	    protected IGameplayAbilityUnit m_owner;

        //角色目前选中的技能
        private CAbility m_selectedAbility;

        //在ability go的下面有两个child, 分别用来挂在effect和buff
	    private GameObject m_effectGo;
	    private GameObject m_buffGo;

        //角色的技能列表
        private List<CAbility> m_abilityList = new List<CAbility>();
		private Dictionary<string, CAbility> m_abilityDict = new Dictionary<string, CAbility>();

		/// <summary>
		/// 所有的技能
		/// </summary>
		public List<CAbility> AbilityList{
			get { return m_abilityList; }
		}

		void Start()
		{
            //添加挂在effect和buff的go. 
		    var effect = transform.Find("Effect");
		    if (effect == null)
		    {
		        m_effectGo = new GameObject("Effect");
		        CDarkUtil.AddChild(transform, m_effectGo.transform, Vector3.zero);
		    }
		    else
		    {
		        m_effectGo = effect.gameObject;
		    }

		    var buff = transform.Find("Buff");
		    if (buff == null)
		    {
		        m_buffGo = new GameObject("Buff");
		        CDarkUtil.AddChild(transform, m_buffGo.transform, Vector3.zero);
		    }
		    else
		    {
		        m_buffGo = buff.gameObject;
		    }
        }

        #region Ability
	    public void InitAbilityActorInfo(IGameplayAbilityUnit actor)
	    {
	        m_owner = actor;
	    }

        /// <summary>
        /// actor掌握新技能
        /// </summary>
        /// <param name="meta"></param>
        public CAbility MasterAbility(string name)
		{
			if (string.IsNullOrEmpty(name))return null;

			CAbility abi = CAbility.Create(name, gameObject);
            if (abi == null)return null;

		    abi.InitAbilityActorInfo(m_owner, m_owner);
            abi.Index = m_abilityList.Count;

			m_abilityList.Add(abi);
			m_abilityDict[name] = abi;

			return abi;
		}

		/// <summary>
		/// 掌握具体的技能
		/// </summary>
		/// <param name="ability"></param>
		public void MasterAbility(CAbility ability) {
			if (m_abilityList.Contains(ability)) return;
			ability.Index = m_abilityList.Count;
			m_abilityList.Add(ability);
			m_abilityDict[ability.AbilityName] = ability;
		}

		public CAbility SelectedAbility {
			get { return m_selectedAbility; }
		}

		/// <summary>
		/// 选择使用某个技能
		/// </summary>
		/// <param name="index"></param>
		public void SelectAbility(int index) {
			if (index >= m_abilityList.Count) {
				m_selectedAbility = null;
                return;
			}
			m_selectedAbility = m_abilityList[index];
		}

		/// <summary>
		/// 选择使用某个技能
		/// </summary>
		/// <param name="index"></param>
		public void SelectAbility(string name) {
			m_selectedAbility = m_abilityDict[name];
		}

		public void Launch(int index, IGameplayAbilityUnit target){
			SelectAbility(0);
			Launch(target);
		}

		public void Launch(int index, Vector3 target) {
			SelectAbility(0);
			Launch(target);
		}

		/// <summary>
		/// 使用已经选中的技能
		/// </summary>
		public void Launch(IGameplayAbilityUnit target){
			m_selectedAbility.TryActivateAbility(target);
        }

		/// <summary>
		/// 使用已经选中的技能
		/// </summary>
		public void Launch(Vector3 target) {
			m_selectedAbility.TryActivateAbility(target);
		}

		/// <summary>
		/// 取消选中的技能
		/// </summary>
		public void CancelSelectedAbility() {
			m_selectedAbility = null;
		}
        #endregion

        #region Effect
	    /// <summary>
	    /// 朝某个位置应用效果
	    /// </summary>
	    public void ApplyGameplayEffectToPosition(CEffectMeta effectMeta, Vector3 localPostion)
	    {
	        var effect = GetSleepingEffectOnSelf(effectMeta);
	        if (effect == null)
	        {
	            Debug.Log("Effect MUST NOT Null here. Check the code or config");
	            return;
	        }

	        effect.ApplyToPosition(localPostion);
        }

        /// <summary>
        /// 给目标添加效果
        /// </summary>
	    public void ApplyGameplayEffectToTarget(CEffectMeta effectMeta, IGameplayAbilityUnit target)
	    {
	        if (target == null)return;
	        target.AbilitySystem.ApplyGameplayEffectToSelf(effectMeta, m_owner);
	    }

        /// <summary>
        /// 给自己添加效果, 要指定效果来源
        /// </summary>
	    private void ApplyGameplayEffectToSelf(CEffectMeta effectMeta, IGameplayAbilityUnit instigator)
        {
            var effect = GetSleepingEffectOnSelf(effectMeta);
            if (effect == null)
            {
                Debug.Log("Effect MUST NOT Null here. Check the code or config");
                return;
            }

            effect.AppliedFrom(instigator);
        }

        /// <summary>
        /// 获取自己身上的可用的CEffect
        /// </summary>
	    private CEffect GetSleepingEffectOnSelf(CEffectMeta effectMeta)
	    {
	        return null;
	    }

        #endregion

        void OnDestroy()
		{
			m_abilityDict.Clear();
			m_abilityDict = null;

			m_abilityList.Clear();
			m_abilityList = null;
		}
	}
}
