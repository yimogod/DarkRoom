using UnityEngine;

namespace DarkRoom.Core {
	/// <summary>
	/// 基于真实时间的间隔调用
	/// 请注意这里有个默认的规则. 即时间间隔一定会大于帧间隔. 否则没啥意义
	/// </summary>
	public class CTimeRegulator {
		//时间间隔, 基于秒
		private float m_period;
		//走过了多少时间
		private float m_timePast = 0f;

		private float m_deltaTime = 0f;

		//是否间隔到了
		private bool m_isReady = false;

		public CTimeRegulator(float period) {
			m_period = period;
		}

		/// <summary>
		/// 默认我们一秒执行10次
		/// </summary>
		public CTimeRegulator() {
			m_period = 0.1f;
		}

		/// <summary>
		/// 时间间隔, 基于秒
		/// </summary>
		public float Period {
			get { return m_period; }
			set {
				//period不能小于0.05
				m_period = System.Math.Max(value, 0.05f);
				m_timePast = 0;
				m_isReady = false;
			}
		}

		/// <summary>
		/// 两帧间隔
		/// </summary>
		public float DeltaTime {
			get { return m_deltaTime; }
		}

		/// <summary>
		/// 时间间隔走了多少百分比(0, 1)
		/// </summary>
		public float PastProcess {
			get {
				return m_timePast / m_period;
			}
		}

		public bool IsReady {
			get { return m_isReady; }
		}

		/// <summary>
		/// 重置规划器的数据
		/// </summary>
		public void Restart()
		{
			m_timePast = 0;
			m_isReady = false;
		}

		public bool Update() {
			m_deltaTime = Time.deltaTime;
			//计算时间间隔, 我们不需要每帧都计算
			m_timePast += m_deltaTime;

			//这么写的原因是给机会让百分比大于等于1有被访问的机会
			if (m_timePast >= m_period && !m_isReady) {
				m_isReady = true;
				return m_isReady;
			}

			if (m_isReady) {
				m_isReady = false;
				m_timePast -= m_period;
			}

			return m_isReady;
		}
	}
}