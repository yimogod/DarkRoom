using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.Core
{
	public class CStateMachine {
		private Dictionary<string, CBaseState> m_stateDict;

		private CBaseState m_globalState;
		private CBaseState m_currState;
		private CBaseState m_lastState;

		public CStateMachine() {
			m_stateDict = new Dictionary<string, CBaseState>();
		}

		public CBaseState LastState
		{
			get { return m_lastState; }
		}

		public CBaseState CurrState {
			get { return m_currState; }
		}

		/// <summary>
		/// 状态机中是否注册过了
		/// </summary>
		public bool ContainState(string name)
		{
			return m_stateDict.ContainsKey(name);
		}

		public void RegisterGlobalState(CBaseState state) {
			//目前认为 全局状态不应该注册进去
			m_globalState = state;
        }

		/// <summary>
		/// 全局状态机开始运行
		/// </summary>
		public void StartGlbalState(){
			m_globalState.Enter(this);
		}

		public void RegisterState(CBaseState state) {
			if (m_stateDict.ContainsKey(state.Name)) {
				Debug.LogError("Duplicate RegisterState With State Name " + state.Name);
				return;
			}

			m_stateDict[state.Name] = state;
		}

		/// <summary>
		/// 切换状态， 如果是本身就不切换
		/// </summary>
		public void ChangeState(CBaseState state) {
			if (state == m_currState) return;
			SetCurrState(state);
		}

		/// <summary>
		/// 切换状态， 如果是本身就不切换
		/// </summary>
		/// <param name="name">状态机的名称</param>
		public void ChangeState(string name) {
			if (!m_stateDict.ContainsKey(name)) {
				Debug.LogError("State Machine does not contain " + name);
				return;
			}

			ChangeState(m_stateDict[name]);
		}

		/// <summary>
		/// 强制切换状态，即使是本身也切换到自己. 及会调用自身的exit和enter方法
		/// </summary>
		/// <param name="name">Name.</param>
		public void ForceChangeState(string name) {
			SetCurrState(m_stateDict[name]);
		}

		private void SetCurrState(CBaseState state) {
			m_lastState = m_currState;
			m_currState = state;
			if (m_lastState != null) m_lastState.Exit(this);
			m_currState.Enter(this);
		}

		/// <summary>
		/// 进入上个状态
		/// </summary>
		public void RevertToPreviousState(){
			ChangeState(m_lastState);
		}

		public void Update() {
			if (m_globalState != null) {
				m_globalState.Execute(this);
			}

			if (m_currState != null) {
				m_currState.Execute(this);
			}
		}

		public void Destroy() {
			foreach (KeyValuePair<string, CBaseState> item in m_stateDict) {
				item.Value.Destroy();
			}
			m_stateDict.Clear();

			m_currState = null;
			m_lastState = null;
			m_stateDict = null;
			m_globalState = null;
		}
	}
}