﻿using UnityEngine;
using System.Collections.Generic;
using System;
using DarkRoom.Core;
using DarkRoom.Utility;

/// <summary>
/// ui板子总体的管理器, 每个板子存在6中行为
/// create, open, close, show, hide, destroy
/// 注意, 我们的create和第一次open都是异步的
/// </summary>
namespace DarkRoom.UI
{
	[RequireComponent(typeof(CUIStackManager))]
	[RequireComponent(typeof(CUILayerManager))]
	[RequireComponent(typeof(CUIAnimManager))]
	public class CUIManager : CSingletonMono<CUIManager>
	{
		public Transform UIRoot => transform;

		/// <summary>
		/// UI层级管理器
		/// </summary>
		public CUILayerManager LayerManager;

		/// <summary>
		/// UI动画管理器
		/// </summary>
		public CUIAnimManager AnimManager;

		/// <summary>
		/// UI栈管理器
		/// </summary>
		public CUIStackManager StackManager;

		/// <summary>
		/// 当前打开的UI列表
		/// </summary>
		public Dictionary<string, List<CUIWindowBase>>
			ActiveDict = new Dictionary<string, List<CUIWindowBase>>();

		/// <summary>
		/// 当前隐藏的UI列表
		/// </summary>
		public Dictionary<string, List<CUIWindowBase>>
			HiddenDict = new Dictionary<string, List<CUIWindowBase>>();

		public void Initialize()
		{
			LayerManager = GetComponent<CUILayerManager>();
			AnimManager = GetComponent<CUIAnimManager>();
			StackManager = GetComponent<CUIStackManager>();
		}

		/// <summary>
		/// 创建UI, 放在Hide列表中, 用于预创建ui
		/// </summary>
		public void CreateUI<T>(Action<GameObject> complete = null) where T : CUIWindowBase
		{
			string address = typeof(T).Name;
			CResourceManager.InstantiatePrefab(address, UIRoot, go =>
			{
				InternalCreateUI<T>(go);
				complete?.Invoke(go);
			});
		}

		private void InternalCreateUI<T>(GameObject go) where T : CUIWindowBase
		{
			string winName = typeof(T).Name;
			CUIWindowBase window = go.GetComponent<CUIWindowBase>();
			try
			{
				int id = CUIManagerHelper.GetUIIDFromCache(winName, ActiveDict);
				window.Init(id);
				window.OnCreated();
			}
			catch (Exception e)
			{
				Debug.LogError("OnInit Exception: " + e.ToString());
			}

			window.OnHide();
			CUIManagerHelper.AddUIToCache(window, HiddenDict);
			LayerManager.SetLayer(window); //设置层级
		}

		public void OpenUI<T>(UICallBack callback = null, params object[] objs) where T : CUIWindowBase
		{
			string winName = typeof(T).Name;
			CUIWindowBase window = CUIManagerHelper.GetUIFromCache(winName, HiddenDict);
			if (window == null)
			{
				CreateUI<T>(go =>
				{
					window = go.GetComponent<T>();
					InternalOpenUI(window, callback, objs);
				});
				return;
			}

			InternalOpenUI(window, callback, objs);
		}

		private void InternalOpenUI(CUIWindowBase window, UICallBack callback, params object[] objs)
		{
			if (window == null) throw new Exception("UIManager: InternalOpenUI window is null");

			CUIManagerHelper.RemoveUIFromCache(window, HiddenDict);
			CUIManagerHelper.AddUIToCache(window, ActiveDict);

			StackManager.OnUIOpen(window);
			try
			{
				window.OnOpen(objs);
			}
			catch (Exception e)
			{
				Debug.LogError(window.UIName + " OnOpen Exception: " + e);
			}

			AnimManager.StartEnterAnim(window, callback); //播放动画
		}

		/// <summary>
		/// 关闭传入的ui实例
		/// </summary>
		public void CloseUI(CUIWindowBase window, bool isPlayAnim = true, UICallBack callback = null,
			params object[] objs)
		{
			CUIManagerHelper.RemoveUIFromCache(window, ActiveDict);

			if (callback != null) callback += InternalCloseUI;
			else callback = InternalCloseUI;

			//不播放动画直接调用回调
			if (!isPlayAnim)
			{
				callback(window, objs);
				return;
			}

			AnimManager.StartExitAnim(window, callback);
		}

		/// <summary>
		/// 根据类型关闭一个ui
		/// </summary>
		public void CloseUI<T>(bool isPlayAnim = true, UICallBack callback = null, params object[] objs)
			where T : CUIWindowBase
		{
			string winName = typeof(T).Name;
			CUIWindowBase window = CUIManagerHelper.GetUIFromCache(winName, ActiveDict);

			if (window == null)
			{
				Debug.LogError("CloseUIWindow Error UI ->" + winName + "<-  not Exist!");
				return;
			}

			CloseUI(window, isPlayAnim, callback, objs);
		}

		private void InternalCloseUI(CUIWindowBase window, params object[] objs)
		{
			try
			{
				window.OnClose();
			}
			catch (Exception e)
			{
				Debug.LogError(window.UIName + " OnClose Exception: " + e.ToString());
			}

			StackManager.OnUIClose(window);
			CUIManagerHelper.AddUIToCache(window, HiddenDict);
		}

		/// <summary>
		/// 移除全部UI
		/// </summary>
		public void CloseAllUI()
		{
			List<string> keys = new List<string>(ActiveDict.Keys);
			for (int i = 0; i < keys.Count; i++)
			{
				List<CUIWindowBase> list = ActiveDict[keys[i]];
				for (int j = 0; j < list.Count; j++)
				{
					CloseUI(list[i], false);
				}
			}
		}

		/// <summary>
		/// 关闭最后打开的一个ui
		/// </summary>
		public void CloseLastUI(UIType uiType = UIType.Normal)
		{
			StackManager.CloseLastUIWindow(uiType);
		}

		public CUIWindowBase ShowUI(string viewName)
		{
			CUIWindowBase ui = CUIManagerHelper.GetUIFromCache(viewName, ActiveDict);
			return RevealUI(ui);
		}

		/// <summary>
		/// 从隐藏状态中 显示ui,
		/// </summary>
		public CUIWindowBase RevealUI(CUIWindowBase view)
		{
			try
			{
				view.OnReveal();
			}
			catch (Exception e)
			{
				Debug.LogError(view.UIName + " OnShow Exception: " + e.ToString());
			}

			return view;
		}

		public CUIWindowBase HideUI(string winName)
		{
			CUIWindowBase ui = CUIManagerHelper.GetUIFromCache(winName, ActiveDict);
			return HideUI(ui);
		}

		public CUIWindowBase HideUI(CUIWindowBase window)
		{
			try
			{
				window.OnHide();
			}
			catch (Exception e)
			{
				Debug.LogError(window.UIName + " OnShow Exception: " + e.ToString());
			}

			return window;
		}

		public void HideOtherUI(string viewName)
		{
			List<string> keys = new List<string>(ActiveDict.Keys);
			for (int i = 0; i < keys.Count; i++)
			{
				List<CUIWindowBase> list = ActiveDict[keys[i]];
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].UIName != viewName)
					{
						HideUI(list[j]);
					}
				}
			}
		}

		public void DestroyUI(CUIWindowBase UI)
		{
			Debug.Log("UIManager DestroyUI " + UI.name);

			CUIManagerHelper.RemoveUIFromCache(UI, HiddenDict);
			CUIManagerHelper.RemoveUIFromCache(UI, ActiveDict);

			Destroy(UI.gameObject);
		}

		public void DestroyAllUI()
		{
			CUIManagerHelper.DestroyUIInCache(ActiveDict);
			CUIManagerHelper.DestroyUIInCache(HiddenDict);
		}
	}
}