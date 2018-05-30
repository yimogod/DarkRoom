using System;
using UnityEngine;

namespace DarkRoom.Core
{
	public class CAutoDestoryComp : MonoBehaviour
	{
		private CTimeRegulator m_reg;
		private bool m_enable = false;

		void Awake()
		{
			m_reg = new CTimeRegulator();
			m_enable = false;
		}

		void Update()
		{
			if (!m_enable)return;

			bool b = m_reg.Update();
			if (b) {
				m_enable = false;
				DestroySelf();
			}
		}

		public void Run(float time)
		{
			m_reg.Period = time;
			m_enable = true;
		}

		private void DestroySelf() {
			Destroy(gameObject);
		}
	}
}