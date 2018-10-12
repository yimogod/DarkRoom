using UnityEngine;
using System.Collections;
using System;

namespace DarkRoom.UI
{
	public class CUIAnimManager : MonoBehaviour
	{
		//开始调用进入动画
		public void StartEnterAnim(CUIWindowBase l_UIbase, UICallBack callBack)
		{
			StartCoroutine(l_UIbase.EnterAnim(EndEnterAnim, callBack));
		}

		//进入动画播放完毕回调
		public void EndEnterAnim(CUIWindowBase l_UIbase, UICallBack callBack)
		{
			l_UIbase.OnCompleteEnterAnim();

			try
			{
				callBack?.Invoke(l_UIbase);
			}
			catch (Exception e)
			{
				Debug.LogError(e.ToString());
			}
		}

		//开始调用退出动画
		public void StartExitAnim(CUIWindowBase l_UIbase, UICallBack callBack)
		{
			StartCoroutine(l_UIbase.ExitAnim(EndExitAnim, callBack));
		}

		//退出动画播放完毕回调
		public void EndExitAnim(CUIWindowBase l_UIbase, UICallBack callBack)
		{
			l_UIbase.OnCompleteExitAnim();

			try
			{
				callBack?.Invoke(l_UIbase);
			}
			catch (Exception e)
			{
				Debug.LogError(e.ToString());
			}
		}
	}
}