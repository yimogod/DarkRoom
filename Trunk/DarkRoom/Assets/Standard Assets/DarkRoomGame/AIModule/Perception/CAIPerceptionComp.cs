using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.AI {
	/// <summary>
	/// AI感知组件
	/// </summary>
	public class CAIPerceptionComp : MonoBehaviour
	{
		/// <summary>
		/// 感知组件的具体感知器
		/// 目前看不出来记录在这里的好处
		/// </summary>
		//protected List<CAISense> m_sensesList = new List<CAISense>();

		/// <summary>
		/// 我记忆中的刺激
		/// </summary>
		protected List<CAIStimulus> m_stimulusList = new List<CAIStimulus>();

		protected CAIController m_owner;

		void Start()
		{
			m_owner = gameObject.GetComponent<CAIController>();
		}

		public void RegisterSense(CAISense sense)
		{
			sense.Owner = m_owner;
            CAIPerceptionSystem.Instance.RegisterSense(sense);
		}

		public void RegisterStimulus(CAIStimulus stimulus)
		{
			m_stimulusList.Add(stimulus);
        }


		/// <summary>
		/// 忘记这个人, 外部调用. 可能因为逻辑或者技能道具啥的
		/// </summary>
		/// <param name="ctrlToForget"></param>
		public void ForgetActor(CController ctrlToForget)
		{
			
		}

		/// <summary>
		/// 忘记所有的人
		/// </summary>
		public void ForgetAll()
		{
			
		}

		protected void RemoveDeadData()
		{
			
		}

		protected void RefreshStimulus(CAIStimulus StimulusStore, CAIStimulus NewStimulus)
		{
			
		}
	}
}
