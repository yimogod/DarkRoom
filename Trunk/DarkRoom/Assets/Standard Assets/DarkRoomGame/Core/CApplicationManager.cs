﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.UI;
using DarkRoom.Utility;

namespace DarkRoom.Game
{
	public enum CAppMode
	{
		Developing,
		QA,
		Release
	}

	public class CApplicationManager : CSingletonMono<CApplicationManager>
	{
		public delegate void ApplicationVoidCallback();

		public delegate void ApplicationBoolCallback(bool b);

		public static ApplicationVoidCallback s_OnApplicationQuit = null;
		public static ApplicationBoolCallback s_OnApplicationPause = null;
		public static ApplicationBoolCallback s_OnApplicationFocus = null;
		public static ApplicationVoidCallback s_OnApplicationUpdate = null;
		public static ApplicationVoidCallback s_OnApplicationFixedUpdate = null;
		public static ApplicationVoidCallback s_OnApplicationOnGUI = null;
		public static ApplicationVoidCallback s_OnApplicationOnDrawGizmos = null;
		public static ApplicationVoidCallback s_OnApplicationLateUpdate = null;

		public CAppMode m_AppMode = CAppMode.Developing;

		public CAppMode AppMode
		{
			get
			{
#if APPMODE_DEV
            return AppMode.Developing;
#elif APPMODE_QA
            return AppMode.QA;
#elif APPMODE_REL
            return AppMode.Release;
#else
				return m_AppMode;
#endif
			}
		}

		[Tooltip("是否记录输入到本地")] public bool m_recordInput = true;

		//快速启动
		public bool m_quickLunch = true;

		[HideInInspector] public List<string> m_globalLogic;
		[HideInInspector] public string currentStatus;

		/// <summary>
		/// 显示括号标识多语言转换的字段
		/// </summary>
		public bool showLanguageValue = false;


		//管理游戏流程
		private CProcedureManager m_procedure = new CProcedureManager();


		/// <summary>
		/// 初始化游戏流程数据
		/// </summary>
		public void InitializeProcedure(params CProcedureBase[] procedures)
		{
			m_procedure.Initialize(procedures);
		}

		/// <summary>
		/// 在一些必要的数据设置完毕后
		/// 程序启动
		/// </summary>
		public void AppLaunch()
		{
			//SetResourceLoadType(); //设置资源加载类型
			//ResourcesConfigManager.Initialize(); //资源路径管理器启动

			//MemoryManager.Init(); //内存管理初始化
			//Timer.Init(); //计时器启动
			//InputManager.Init(); //输入管理器启动

			CUIManager.Instance.Initialize(); //UIManager启动

			//GlobalLogicManager.Init(); //初始化全局逻辑

			/*if (AppMode != AppMode.Release)
			{
			    //GUIConsole.Init(); //运行时Console

			    DevelopReplayManager.OnLunchCallBack += () =>
			    {
			        InitGlobalLogic(); //全局逻辑
			        ApplicationStatusManager.EnterTestModel(m_Status); //可以从此处进入测试流程
			    };

			    DevelopReplayManager.Init(m_quickLunch); //开发者复盘管理器                              
			}
			else
			{
			    //Log.Init(false); //关闭 Debug

			    InitGlobalLogic(); //全局逻辑
			    //ApplicationStatusManager.EnterStatus(m_Status); //游戏流程状态机，开始第一个状态
			}*/
		}

		/// <summary>
		/// 开始流程
		/// </summary>
		public void ChangeProcedure(string procedureName)
		{
			m_procedure.ChangeProcedure(procedureName);
		}

		void OnApplicationQuit()
		{
			if (s_OnApplicationQuit != null)
			{
				try
				{
					s_OnApplicationQuit();
				}
				catch (Exception e)
				{
					Debug.LogError(e.ToString());
				}
			}
		}

		/*
		 * 强制暂停时，先 OnApplicationPause，后 OnApplicationFocus
		 * 重新“启动”游戏时，先OnApplicationFocus，后 OnApplicationPause
		 */
		void OnApplicationPause(bool pauseStatus)
		{
			if (s_OnApplicationPause != null)
			{
				try
				{
					s_OnApplicationPause(pauseStatus);
				}
				catch (Exception e)
				{
					Debug.LogError(e.ToString());
				}
			}
		}

		void OnApplicationFocus(bool focusStatus)
		{
			if (s_OnApplicationFocus != null)
			{
				try
				{
					s_OnApplicationFocus(focusStatus);
				}
				catch (Exception e)
				{
					Debug.LogError(e.ToString());
				}
			}
		}

		void Update()
		{
			m_procedure.Update();
			CMouseInput.Instance.Update();

			if (s_OnApplicationUpdate != null)
				s_OnApplicationUpdate();
		}

		private void LateUpdate()
		{
			if (s_OnApplicationLateUpdate != null)
			{
				s_OnApplicationLateUpdate();
			}
		}

		private void FixedUpdate()
		{
			if (s_OnApplicationFixedUpdate != null)
				s_OnApplicationFixedUpdate();
		}

		void OnGUI()
		{
			if (s_OnApplicationOnGUI != null)
				s_OnApplicationOnGUI();
		}


		/// <summary>
		/// 设置资源加载方式
		/// </summary>
		void SetResourceLoadType()
		{
		}

		/// <summary>
		/// 初始化全局逻辑
		/// </summary>
		void InitGlobalLogic()
		{
		}
	}
}