﻿using UnityEngine;
using System.Collections;

namespace DarkRoom.UI
{
    public class UIWindowBase : UIBase
    {
        public UIType m_UIType;

        public GameObject m_bgMask;
        public GameObject m_uiRoot;

        public virtual void OnOpen()
        {
        }

        public virtual void OnClose()
        {

        }

        public virtual void OnHide()
        {

        }

        public virtual void OnShow()
        {

        }

        public virtual void OnRefresh()
        {
        }

        public virtual IEnumerator EnterAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
        {
            //默认无动画
            l_animComplete(this, l_callBack, objs);

            yield break;
        }

        public virtual void OnCompleteEnterAnim()
        {
        }

        public virtual IEnumerator ExitAnim(UIAnimCallBack l_animComplete, UICallBack l_callBack, params object[] objs)
        {
            //默认无动画
            l_animComplete(this, l_callBack, objs);

            yield break;
        }

        public virtual void OnCompleteExitAnim()
        {
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        //刷新是主动调用
        public void Refresh(params object[] args)
        {
            OnRefresh();
        }
    }
}

