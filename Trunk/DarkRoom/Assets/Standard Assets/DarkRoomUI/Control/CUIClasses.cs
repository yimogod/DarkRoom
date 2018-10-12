using System;
using System.Collections.Generic;

namespace DarkRoom.UI
{
	/// <summary>
	/// UI回调
	/// </summary>
	/// <param name="objs"></param>
	public delegate void UICallBack(CUIWindowBase UI, params object[] objs);

	public delegate void UIAnimCallBack(CUIWindowBase UIbase, UICallBack callBack);

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