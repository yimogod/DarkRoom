using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.AI {
	public class CAISense
	{
		public string Name;

		public CAIController Owner;

		/// <summary>
		/// 感知更新类型, 目前没用到
		/// </summary>
		public enum AISenseNotifyType {
			/** Continuous update whenever target is perceived. */
			OnEveryPerception,
			/** From "visible" to "not visible" or vice versa. */
			OnPerceptionChange,
		};

		/// <summary>
		/// 感知记忆的时长. 基于秒
		/// </summary>
		protected float m_defaultExpirationAge = 10f;

		//感知通知类型
		protected AISenseNotifyType m_notifyType;

		//当新的pawn出生时, 此scense是否通知它
		protected bool m_wantsNewPawnNotify = true;

		//是否自动注册的标记,在pawn出生时
		protected bool m_autoRegisterAllPawnsAsSources = true;

		//忘记时是否通知的标记
		protected bool m_needsForgettingNotify = true;

		//目前的记忆
		protected Dictionary<int, CAIStimulus> m_stimuDict = new Dictionary<int, CAIStimulus>();

		/// <summary>
		/// 是否只在sense更改的时候才更新
		/// </summary>
		protected bool m_wantsUpdateOnlyOnPerceptionValueChange
		{
			get { return m_notifyType == AISenseNotifyType.OnPerceptionChange; }
		}

		/// <summary>
		/// 当一个sense要被忘记的时候通知
		/// </summary>
		public bool NeedsNotificationOnForgetting
		{
			get { return m_needsForgettingNotify; }
		}

		/// <summary>
		/// 当新的pawn出生时, 此scense是否通知它. 
		/// </summary>
		public bool WantsNewPawnNotify {
			get { return m_wantsNewPawnNotify; }
		}

		/// <summary>
		/// 如果是true, 所有新的pawn都会自动注册自己(作为源)到这个sense
		/// </summary>
		public bool AutoRegisterAllPawnsAsSources {
			get { return m_autoRegisterAllPawnsAsSources; }
		}

		/// <summary>
		/// 失效标记, 接下来会在感知系统中移除
		/// </summary>
		public bool Invalid
		{
			get { return Owner.Pawn.DeadOrDying; }
		}

		public CAISense()
		{
			m_defaultExpirationAge = 30f;
			m_needsForgettingNotify = false;
		}

		/// <summary>
		/// 添加或者更新刺激
		/// </summary>
		/// <param name="stimu"></param>
		public void AddOrUpdateStimulu(CAIStimulus stimu)
		{
			if (m_stimuDict.ContainsKey(stimu.Id)) {
				//拷贝第一次看见的值以及其他旧对象需要保留下来的值
				CAIStimulus src = m_stimuDict[stimu.Id];
				stimu.TimeBecameVisible = src.TimeBecameVisible;
			}

			m_stimuDict[stimu.Id] = stimu;
		}

		/// <summary>
		/// 感知器具体的逻辑
		/// </summary>
		/// <param name="stimusSrcList"></param>
		public virtual void Update(List<CAIController> stimusSrcList)
		{
			CheckExpirationOrInvalid();
			CleanInvalidSources();
		}

		//检测记忆是否过期
		protected void CheckExpirationOrInvalid()
		{
			foreach (var pair in m_stimuDict) {
				CAIStimulus item = pair.Value;
				if (item.Source == null || item.Source.Pawn.DeadOrDying) {
					item.Expired = true;
					m_stimuDict[item.Id] = item;
					continue;
				}


				float currTime = Time.realtimeSinceStartup;
				float deltaTime = currTime - item.TimeLastSensed;
				if (deltaTime <= m_defaultExpirationAge)continue;

				item.Expired = true;
				m_stimuDict[item.Id] = item;
			}
		}

		/// <summary>
		/// 删除过期思想成本较高, 所以我们添加超过20个才删除一次
		/// </summary>
		private List<int> m_stimusToDie = new List<int>();
		protected virtual void CleanInvalidSources()
		{
			if (m_stimuDict.Count <= 20)return;

			m_stimusToDie.Clear();
            foreach (var pair in m_stimuDict) {
				CAIStimulus item = pair.Value;
				if (item.Expired)m_stimusToDie.Add(item.Id);
			}

			foreach (var item in m_stimusToDie) {
				m_stimuDict.Remove(item);
			}
		}

		/// <summary>
		/// 在感知系统获取新pawn出生的通知时调用
		/// </summary>
		/// <param name="pawn"></param>
		protected virtual void OnNewPawn(CPawnEntity pawn)
		{
			if (m_wantsNewPawnNotify) {
				Debug.LogError("Declars m_wantsNewPawnNotify true but does not override OnNewPawn");
			}
		}
    }

	
	/// <summary>
	/// 刺激的数据结构
	/// </summary>
	public struct CAIStimulus
	{
		public int Id{
			get{ return Source.Pawn.cid; }
		}

		public bool Invalid
		{
			get{
				if (Expired) return true;
				if (Source == null) return true;
				if (Source.Pawn.DeadOrDying) return true;
				return false;
			}
		}

		/// <summary>
		/// 产生刺激的来源
		/// </summary>
		public CAIController Source;

		/// <summary>
		/// 产生刺激的原因
		/// </summary>
		public CAISense Sense;

		/// <summary>
		/// 刺激的强度
		/// </summary>
		public float Strength;

		/// <summary>
		/// 产生刺激的位置
		/// </summary>
		public Vector3 StimulusLocation;

		/// <summary>
		/// 产生刺激时我的位置
		/// </summary>
		public Vector3 ReceiverLocation;

		/// <summary>
		/// 是否过期
		/// </summary>
		public bool Expired;

		/// <summary>
		/// 你最近看到这个目标的时间
		/// </summary>
		public float TimeLastSensed;

		/// <summary>
		/// 你第一次看到这个目标的时间
		/// </summary>
		public float TimeBecameVisible;
	}
}
