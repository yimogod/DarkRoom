using System;
using System.Collections.Generic;

namespace DarkRoom.UI
{
    /// <summary>
    /// UI回调
    /// </summary>
    /// <param name="objs"></param>
    public delegate void UICallBack(UIWindowBase UI, params object[] objs);

    public delegate void UIAnimCallBack(UIWindowBase UIbase, UICallBack callBack, params object[] objs);

    public enum UIType
    {
        HUD,
        Normal,
        Dialog,
        Tips,
        Mask,
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

}
