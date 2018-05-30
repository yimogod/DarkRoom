using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.AI {
	/// <summary>
	/// 让owner pawn自动注册为感知系统的刺激源
	/// </summary>
	public class CAIPerceptionStimuliSourceComponent : MonoBehaviour
	{
		/// <summary>
		/// 我有可能产生的刺激的类型列表
		/// </summary>
		public List<string> StimusListWhichICanGenerate = new List<string>();

		private CAIController m_owner;
		//是否已经注册
		private bool m_successfullyRegistered = false;

		void Awake()
		{
			m_owner = gameObject.GetComponent<CAIController>();
		}


		/// <summary>
		/// 将自己作为刺激源注册到感知系统中
		/// </summary>
		public void RegisterToPerceptionSystem()
		{
			if (m_successfullyRegistered) {
				Debug.LogError("U have Register me Before");
				return;
			}

			foreach (string name in StimusListWhichICanGenerate) {
				CAIPerceptionSystem.Instance.RegisterStimusSource(name, m_owner);
			}
		}
    }
}
