using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.UI
{
	public class CUIStackManager : MonoBehaviour
	{
		private List<CUIWindowBase> m_hudStack = new List<CUIWindowBase>();
		private List<CUIWindowBase> m_normalStack = new List<CUIWindowBase>();
		private List<CUIWindowBase> m_dialogStack = new List<CUIWindowBase>();
		private List<CUIWindowBase> m_tipsStack = new List<CUIWindowBase>();
		private List<CUIWindowBase> m_maskStack = new List<CUIWindowBase>();

		public void OnUIOpen(CUIWindowBase window)
		{
			switch (window.UIType)
			{
				case UIType.HUD:
					m_hudStack.Add(window);
					break;
				case UIType.Normal:
					m_normalStack.Add(window);
					break;
				case UIType.Dialog:
					m_dialogStack.Add(window);
					break;
				case UIType.Tips:
					m_tipsStack.Add(window);
					break;
				case UIType.Mask:
					m_maskStack.Add(window);
					break;
			}
		}

		public void OnUIClose(CUIWindowBase ui)
		{
			switch (ui.UIType)
			{
				case UIType.HUD:
					m_hudStack.Remove(ui);
					break;
				case UIType.Normal:
					m_normalStack.Remove(ui);
					break;
				case UIType.Dialog:
					m_dialogStack.Remove(ui);
					break;
				case UIType.Tips:
					m_tipsStack.Remove(ui);
					break;
				case UIType.Mask:
					m_tipsStack.Remove(ui);
					break;
			}
		}

		public void CloseLastUIWindow(UIType uiType = UIType.Normal)
		{
			CUIWindowBase window = GetLastUI(uiType);
			if (window != null) CUIManager.Instance.CloseUI(window);
		}

		public CUIWindowBase GetLastUI(UIType uiType)
		{
			switch (uiType)
			{
				case UIType.Normal:
					if (m_normalStack.Count > 0)
						return m_normalStack[m_normalStack.Count - 1];
					else
						return null;
				case UIType.Dialog:
					if (m_dialogStack.Count > 0)
						return m_dialogStack[m_dialogStack.Count - 1];
					else
						return null;
			}

			return null;
		}
	}
}