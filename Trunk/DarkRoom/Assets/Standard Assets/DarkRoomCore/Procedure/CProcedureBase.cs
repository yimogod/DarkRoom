using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkRoom.Core
{
    /// <summary>
    /// 流程基类
    /// 在切换场景的时候, 我们会加载loadingscene
    /// </summary>
    public abstract class CProcedureBase : CBaseState
    {
        public static string LoadingSceneName = "LoadingScene";

        //切换的目标的场景名称
        protected virtual String m_targetSceneName => null;

        public CProcedureBase(string name) : base(name) { }

        public override void Enter(CStateMachine sm)
        {
            AddLoadingScene();
            AddTargetScene();
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

        protected virtual void OnSceneLoadComplete(AsyncOperation op)
        {

        }

        /// <summary>
        /// 所有的加载都完毕了, 包含资源, 目标场景本身
        /// 这时, 我们要关闭loadingscene
        /// </summary>
        protected virtual void EnterTargetSceneComplete()
        {
            SceneManager.UnloadSceneAsync(LoadingSceneName);
        }
    }
}
