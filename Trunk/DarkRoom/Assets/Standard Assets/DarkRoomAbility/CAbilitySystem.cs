using System;
using System.Collections;
using System.Collections.Generic;
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
	///		网络复制功能
	///			技能状态必须可以自己复制. 另外CAbilitySystem提供RPC复制
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
	/// </summary>
	public class CAbilitySystem : MonoBehaviour
	{
		protected CAIController m_owner;
		//角色目前选中的技能
		private CAbility m_selectedAbility;

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
			m_owner = gameObject.GetComponent<CAIController>();
		}

		/// <summary>
		/// actor掌握新技能
		/// </summary>
		/// <param name="meta"></param>
		public CAbility MasterAbility(string name, int level)
		{
			if (string.IsNullOrEmpty(name))return null;

			CAbility abi = CAbility.Create(name, gameObject);
			if (abi == null)return null;

			abi.Level = level;
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

		public void Launch(int index, CAIController target){
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
		public void Launch(CAIController target){
			m_selectedAbility.Activate(target);
        }

		/// <summary>
		/// 使用已经选中的技能
		/// </summary>
		public void Launch(Vector3 target) {
			m_selectedAbility.Activate(target);
		}

		/// <summary>
		/// 取消选中的技能
		/// </summary>
		public void CancelSelectedAbility() {
			m_selectedAbility = null;
		}

		void OnDestroy()
		{
			m_abilityDict.Clear();
			m_abilityDict = null;

			m_abilityList.Clear();
			m_abilityList = null;
		}
	}
}
