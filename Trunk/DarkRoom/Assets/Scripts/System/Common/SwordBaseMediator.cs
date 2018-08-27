using System;
using System.Collections.Generic;
using DarkRoom.UI;
using PureMVC.Patterns;

namespace Assets.Scripts.System.Common
{
    public class SwordBaseMediator : Mediator
    {
        public SwordBaseMediator(string mediatorName) : base(mediatorName)
        {
        }

        protected void OpenPanel<T>() where T : CUIWindowBase
        {
            CUIManager.Instance.OpenUI<T>(OnOpenComplete);
        }

        protected void OnOpenComplete(CUIWindowBase window, params object[] objs)
        {
            m_viewComponent = window;
        }
    }
}
