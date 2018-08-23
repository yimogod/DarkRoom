using System;

namespace DarkRoom.Core
{
    /// <summary>
    /// 流程管理器。
    /// </summary>
    public class ProcedureManager : CSingleton<ProcedureManager>
    {
        private CStateMachine m_ProcedureFsm;

        /// <summary>
        /// 初始化流程管理器的新实例。
        /// </summary>
        public ProcedureManager()
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
        public ProcedureBase CurrentProcedure
                => m_ProcedureFsm.CurrState as ProcedureBase;

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
        /// <param name="stateMachine">有限状态机管理器。</param>
        /// <param name="procedures">流程管理器包含的流程。</param>
        public void Initialize(params ProcedureBase[] procedures)
        {
            foreach (var item in procedures)
            {
                m_ProcedureFsm.RegisterState(item);
            }
        }

        /// <summary>
        /// 开始流程。
        /// </summary>
        /// <param name="procedureName">要开始的流程类型。</param>
        public void StartProcedure(string procedureName)
        {
            m_ProcedureFsm.ChangeState(procedureName);
        }

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <param name="procedureName">要检查的流程名称。</param>
        /// <returns>是否存在流程。</returns>
        public bool HasProcedure(string procedureName)
        {
            return m_ProcedureFsm.ContainState(procedureName);
        }
    }
}
