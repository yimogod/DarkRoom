using System;
using System.Collections.Generic;
using DarkRoom.UI;
using PureMVC.Patterns;

namespace Sword
{
	public class SwordBaseMediator : Mediator
	{
		public SwordBaseMediator(string mediatorName) : base(mediatorName)
		{
		}

		protected void OpenPanel<T>(params object[] objs) where T : CUIWindowBase
		{
			CUIManager.Instance.OpenUI<T>(OnOpenComplete, objs);
		}

		protected void OnOpenComplete(CUIWindowBase window, params object[] objs)
		{
			m_viewComponent = window;
		}
	}
}