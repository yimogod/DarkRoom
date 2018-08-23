using System;
using System.Collections.Generic;

namespace DarkRoom.UI
{
    #region UI事件 代理 枚举

    /// <summary>
    /// UI回调
    /// </summary>
    /// <param name="objs"></param>
    public delegate void UICallBack(UIWindowBase UI, params object[] objs);

    public delegate void UIAnimCallBack(UIWindowBase UIbase, UICallBack callBack, params object[] objs);

    public enum UIType
    {
        GameUI,

        Fixed,
        Normal,
        TopBar,
        PopUp
    }

    public enum UIEvent
    {
        OnOpen,
        OnClose,
        OnHide,
        OnShow,

        OnInit,
        OnDestroy,

        OnRefresh,

        OnStartEnterAnim,
        OnCompleteEnterAnim,

        OnStartExitAnim,
        OnCompleteExitAnim,
    }

    #endregion
}
