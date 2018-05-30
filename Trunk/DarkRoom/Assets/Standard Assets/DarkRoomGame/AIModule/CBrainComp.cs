using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.AI {
	/// <summary>
	/// 消息接收器, 消息处理器的中转中心
	/// </summary>
	public class CBrainComp : MonoBehaviour
	{
		//双缓冲的消息列表
		private List<CAIMessage> m_messagesToProcessCache = null;
		//要被处理的消息列表
		private List<CAIMessage> m_messagesToProcess = null;

		//双缓冲的执行器的列表
		private List<CAIMessageObserver> m_observersCache = null;
		//处理消息的执行器的列表
		private List<CAIMessageObserver> m_observers = null;

		/// <summary>
		/// 处理消息.
		/// 原理是讲本消息放入列表, 然后在后面的update中进行处理
		/// </summary>
		/// <param name="message"></param>
		public void HandleMessage(CAIMessage message)
		{
			if (m_messagesToProcessCache == null) {
				m_messagesToProcessCache = new List<CAIMessage>();
				m_messagesToProcess = new List<CAIMessage>();
            }

			m_messagesToProcessCache.Add(message);
        }

		/// <summary>
		/// 添加消息处理器
		/// </summary>
		/// <param name="observer"></param>
		public void RegisterMessageObserver(CAIMessageObserver observer)
		{
			if (m_observers == null) {
				m_observers = new List<CAIMessageObserver>();
				m_observersCache = new List<CAIMessageObserver>();
			}

			m_observersCache.Add(observer);
		}

		/// <summary>
		/// 清理消息处理器
		/// 一个消息可能会有多个监听器
		/// 这个方法清理本消息的所有监听器
		/// </summary>
		public void UnregisterMessageObserver(string type)
		{
			if (m_observers == null || m_observers.Count == 0)return;

			foreach(var ob in m_observers){
				bool b = ob.SameObserver (type, null);
				if (b){ ob.Die (); return; }
			}

			foreach(var ob in m_observersCache){
				bool b = ob.SameObserver (type, null);
				if (b){ ob.Die (); return; }
			}
		}

		public void UnregisterMessageObserver(string type, Action<CAIMessage> action)
		{
			if (m_observers == null || m_observers.Count == 0)return;

			foreach(var ob in m_observers){
				bool b = ob.SameObserver (type, action);
				if (b) { ob.Die (); return; }
			}

			foreach(var ob in m_observersCache){
				bool b = ob.SameObserver (type, action);
				if (b) { ob.Die (); return; }
			}
		}

		void LateUpdate()
		{
			if (m_observers == null) return;

			//先清理死亡的监听器
			for (int i = m_observers.Count - 1; i >= 0; i--) {
				var ob = m_observers [i];
				if(ob.Dead)m_observers.RemoveAt(i);
			}

			//添加两帧间新加的观察器
			if (m_observersCache.Count > 0) {
				m_observers.AddRange(m_observersCache);
				m_observersCache.Clear();
			}
        }

		void Update(){
			if (m_messagesToProcess == null)return;

			foreach (var message in m_messagesToProcess) {
				//一个message会多个监听器
				foreach (var ob in m_observers) {
					ob.OnMessage(message);
				}
			}

			m_messagesToProcess.Clear();

			//双缓冲交换
			var temp = m_messagesToProcess;
			m_messagesToProcess = m_messagesToProcessCache;
			m_messagesToProcessCache = temp;
		}

		void OnDestroy()
		{
			if (m_messagesToProcess != null) {
				m_messagesToProcess.Clear();
				m_messagesToProcess = null;
			}

			if (m_observers != null) {
				foreach (var observer in m_observers) {
					observer.Dispose();
				}

				m_observers.Clear();
				m_observers = null;
			}
		}
	}
}
