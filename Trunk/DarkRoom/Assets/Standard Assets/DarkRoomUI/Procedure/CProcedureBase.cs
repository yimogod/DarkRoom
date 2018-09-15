using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DarkRoom.Core;

namespace DarkRoom.UI
{
	/// <summary>
	/// 流程基类
	/// 在切换场景的时候, 我们会加载loadingscene
	/// </summary>
	public abstract class CProcedureBase : CBaseState
	{
		public static string LoadingSceneName = "LoadingScene";

		//切换的目标的场景名称
		protected String m_targetSceneName = null;

		//预加载的go,主要就是为了把bundle加载进入内存.
		//加快后面的读取速度
		//重要, 没有配表, 配表要在前面全部加载完毕
		protected List<string> m_preCreatePrefabAddress = new List<string>();

		//全部加载完毕切换场景需要加载的资源数量
		protected int m_enterSceneAssetMaxNum = 0;

		//当前已经加载的资源
		protected int m_enterSceneAssetLoadedNum = 0;

		//以防万一, 如果加载超过30s, 则视为全部加载成功
		protected int m_maxLoadingSecond = 30;

		//以防万一, 如果加载超过30s, 则视为全部加载成功
		protected CTimeRegulator m_loadingTimeReg;

		public CProcedureBase(string name) : base(name)
		{
			m_loadingTimeReg = new CTimeRegulator(m_maxLoadingSecond, 1);
		}

		/// <summary>
		/// 进入场景的默认行为
		/// 1. 启动加载时间限制
		/// 2. 开始加载prefab
		/// </summary>
		public override void Enter(CStateMachine sm)
		{
			PreEnter(sm);
			DoEnter(sm);
			PostEnter(sm);
		}

		public override void Exit(CStateMachine sm)
		{
			base.Exit(sm);

			//清理预加载资源
			m_preCreatePrefabAddress.Clear();
			CUIManager.Instance.DestroyAllUI();
		}

		protected virtual void PreEnter(CStateMachine sm)
		{
			m_enterSceneAssetMaxNum = 0;
		}

		protected virtual void DoEnter(CStateMachine sm)
		{
			m_loadingTimeReg.Restart();
			// 1是加载的目标场景
			m_enterSceneAssetMaxNum += m_preCreatePrefabAddress.Count + 1;
			StartLoading();
		}

		protected virtual void PostEnter(CStateMachine sm)
		{
		}

		public override void Execute(CStateMachine sm)
		{
			bool b = m_loadingTimeReg.Update();
			if (b) CheckAllAssetsLoadComplete();
		}

		// 开始加载, 一般情绪下, 在enter中调用
		protected virtual void StartLoading()
		{
			AddTargetScene();
			AddLoadingScene();
			StartLoadingPrefab();
		}

		/// <summary>
		/// 加载loadingscene
		/// </summary>
		protected virtual void AddLoadingScene()
		{
			SceneManager.LoadScene(LoadingSceneName, LoadSceneMode.Additive);
		}

		/// <summary>
		/// 异步加载目标scene
		/// </summary>
		protected virtual void AddTargetScene()
		{
			if (string.IsNullOrEmpty(m_targetSceneName))
			{
				Debug.LogErrorFormat("{0} Procedure Should override m_targetSceneName Method", Name);
				return;
			}

			AsyncOperation op = SceneManager.LoadSceneAsync(m_targetSceneName);
			op.completed += OnSceneLoadComplete;
		}

		/// <summary>
		/// 开始加载依赖的资源, 在DoEnter中调用
		/// </summary>
		protected virtual void StartLoadingPrefab()
		{
		}

		/// <summary>
		/// 目标场景加载完毕
		/// </summary>
		protected virtual void OnSceneLoadComplete(AsyncOperation op)
		{
			m_enterSceneAssetLoadedNum++;
			CheckAllAssetsLoadComplete();
		}

		/// <summary>
		/// 所有的加载都完毕了, 包含资源, 目标场景本身
		/// 这时, 我们要关闭loadingscene
		/// </summary>
		protected virtual void CheckAllAssetsLoadComplete()
		{
			Debug.LogWarningFormat("curr load num is {0}", m_enterSceneAssetLoadedNum);
			Debug.LogWarningFormat("max load num is {0}", m_enterSceneAssetMaxNum);
			if (m_enterSceneAssetLoadedNum < m_enterSceneAssetMaxNum) return;

			SceneManager.UnloadSceneAsync(LoadingSceneName);
			m_loadingTimeReg.Stop();

			OnEntireLevelComplete();
		}

		/// <summary>
		/// 完整进入场景后调用
		/// </summary>
		protected virtual void OnEntireLevelComplete()
		{
		}
	}
}