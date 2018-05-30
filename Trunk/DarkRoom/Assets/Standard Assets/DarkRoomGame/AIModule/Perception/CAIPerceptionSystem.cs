using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.AI {
	/// <summary>
	/// 感知系统, 敌对单位间的感知
	/// 感知系统为单例, 管理着所有的感知器
	/// </summary>
	public class CAIPerceptionSystem {
		private static CAIPerceptionSystem s_instance = null;

		//存储所有的会产生刺激的单位
		//key是sense的类型(也是刺激的类型), value是所有会产生刺激的单位列表
		private Dictionary<string, List<CAIController>> m_stimusDict = new Dictionary<string, List<CAIController>>();

		//所有的具体的感知
		private List<CAISense> m_senseList = new List<CAISense>();

		public static CAIPerceptionSystem Instance {
			get {
				if (s_instance == null) {
					s_instance = new CAIPerceptionSystem();
				}
				return s_instance;
			}
		}

		/// <summary>
		/// 添加具体的感应器
		/// </summary>
		/// <param name="sense"></param>
		public void RegisterSense(CAISense sense)
		{
			m_senseList.Add(sense);
        }

		/// <summary>
		/// 获取会产生特定刺激的单位
		/// </summary>
		/// <param name="stimusName"></param>
		/// <returns></returns>
		public List<CAIController> GetStimusSourceList(string stimusName)
		{
			if (m_stimusDict.ContainsKey(stimusName)) {
				return m_stimusDict[stimusName];
			}
			return null;
		}

		/// <summary>
		/// 注册刺激产生者
		/// </summary>
		public void RegisterStimusSource(string stimus, CAIController src)
		{
			if (!m_stimusDict.ContainsKey(stimus)) {
				m_stimusDict[stimus] = new List<CAIController>();
            }

			if (m_stimusDict[stimus].Contains(src)) {
				Debug.LogError("U duplicate RegisterStimusGenerator with " + stimus + " " + src.name);
				return;
			}

			m_stimusDict[stimus].Add(src);
        }

		public void Update()
		{
			//remove dead src
			foreach (CAISense item in m_senseList) {
				List<CAIController> srcList = GetStimusSourceList(item.Name);
				if (srcList == null)continue;
				item.Update(srcList);
			}

			//每帧移除一个无效的感应器
			for (int i = m_senseList.Count - 1; i >= 0; i--) {
				if (m_senseList[i].Invalid) {
					m_senseList.RemoveAt(i);
					break;
				}
			}

			//每帧移除一个无效的产生器
			foreach (var pair in m_stimusDict) {
				List<CAIController> list = pair.Value;
				if (list == null) continue;

				for(int i = list.Count - 1; i >= 0; i--) {
					if (list[i].Pawn.DeadOrDying) {
						list.RemoveAt(i);
						break;
					}
				}
			}

			//end of for
		}

	}
}
