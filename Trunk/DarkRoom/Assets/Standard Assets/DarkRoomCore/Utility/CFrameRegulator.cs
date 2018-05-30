using System;
namespace DarkRoom.Core {
	/// <summary>
	/// 基于帧的间隔调用
	/// </summary>
	public class CFrameRegulator {
		//帧数间隔
		private int m_totalFrame;
		//剩余多少帧
		private int m_leftTime;

		//是否间隔到了
		private bool m_isReady = false;

		public CFrameRegulator(int totalFrame) {
			m_totalFrame = totalFrame;
			m_leftTime = m_totalFrame;
		}

		public bool IsReady {
			get { return m_isReady; }
		}

		public void Restart() {
			m_leftTime = m_totalFrame;
			m_isReady = false;
		}

		public bool Update() {
			m_leftTime--;

			if (m_leftTime < 0) {
				m_leftTime = m_totalFrame;
				m_isReady = true;
			} else if (m_isReady) {
				m_isReady = false;
			}

			return m_isReady;
		}
	}
}