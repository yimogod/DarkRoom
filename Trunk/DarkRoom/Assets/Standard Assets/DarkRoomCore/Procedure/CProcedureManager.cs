using System;
using UnityEngine;

namespace DarkRoom.Core
{
    /// <summary>
    /// 流程管理器。
    /// </summary>
    public class CProcedureManager
    {
        private CStateMachine m_ProcedureFsm;

        /// <summary>
        /// 初始化流程管理器的新实例。
        /// </summary>
        public CProcedureManager()
        {
            m_ProcedureFsm = new CStateMachine();
        }

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        //internal override int Priority => -10;

        /// <summary>
        /// 获取当前流程。
        /// </summary>
        public CProcedureBase CurrentProcedure
                => m_ProcedureFsm.CurrState as CProcedureBase;

        /// <summary>
        /// 获取当前流程持续时间。
        /// </summary>
        public float CurrentProcedureTime => 0;

        public void Update()
        {
            m_ProcedureFsm.Update();
        }

        /// <summary>
        /// 关闭并清理流程管理器。
        /// </summary>
        public void Shutdown()
        {
            m_ProcedureFsm.Destroy();
        }

        /// <summary>
        /// 初始化流程管理器。
        /// </summary>
        public void Initialize(params CProcedureBase[] procedures)
        {
            foreach (var item in procedures)
            {
                m_ProcedureFsm.RegisterState(item);
            }
        }

        /// <summary>
        /// 开始流程。
        /// </summary>
        public void ChangeProcedure(string procedureName)
        {
            m_ProcedureFsm.ChangeState(procedureName);
        }

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        public bool ContainProcedure(string procedureName)
        {
            return m_ProcedureFsm.ContainState(procedureName);
        }
    }
}
