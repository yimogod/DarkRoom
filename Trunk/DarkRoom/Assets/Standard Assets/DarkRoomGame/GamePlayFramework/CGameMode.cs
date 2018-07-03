﻿using System;
using System.Collections.Generic;
using DarkRoom.Core;

namespace DarkRoom.Game {
	/// <summary>
	/// GamePlay的类. 定义游戏玩法. 逻辑规则. 创建角色. 
	/// </summary>
	public class CGameMode
	{
		/// <summary>
		/// 用于测试的标记符
		/// </summary>
		public static bool TestFlag = false;

		//游戏数据
		protected CGameState m_gameState;

		//目前玩家操纵的角色
		//当选中我的时候，使用技能是选中的其他人或位置存在这里
		//回头看是否放在ai里面更合适一些
		//毕竟即使你控制的主角也需要一些基本AI, 只是不需要做决策
		//这里的selectedActor是玩家主动选择的, 或者是AI选择的
		//而ActorAIComp的target是ai寻找并选择的
		protected CAIController m_selectedPawnCtrl = null;

		//public Vector3 selectedWorldPositon = RayConst.INVALID_VEC3;

		//鼠标划过的单位, 为了高亮
		protected CController m_mouseOverUnit = null;

		//游戏是否启动
		protected bool m_enabled = false;

		//每1s我们移除死亡的单位
		private CTimeRegulator m_removeDeadReg = new CTimeRegulator(1);

		//内部对选中pawn的快捷引用
		protected CPawnEntity m_selectPawn {
			get {
				if (m_selectedPawnCtrl == null) {
					return null;
				}

				return m_selectedPawnCtrl.Pawn;
			}
		}

		/// <summary>
		/// 初始化游戏
		/// </summary>
		public virtual void InitGame(int mapId, CGameState data)
		{
			m_enabled = true;
			m_gameState = data;
		}

		/// <summary>
		/// 选择一个角色为当前操作的角色
		/// </summary>
		/// <param name="ctrl"></param>
		public void SelectPawnCtrl(CPlayerController ctrl)
		{
			if (ctrl == null)return;
			ctrl.IgnoreInput = false;
			ctrl.InterestingTarget = null;

			
			m_selectedPawnCtrl = ctrl;
		}

		/// <summary>
		/// 暂停游戏, 比如暂停键或者某些逻辑需要的
		/// </summary>
		public virtual void Pause()
		{
			
		}

		/// <summary>
		/// 清理游戏
		/// </summary>
		public virtual void Clear()
		{
			if (m_gameState != null) {
				m_gameState.Clear();
				m_gameState = null;
			}

			m_enabled = false;
			m_selectedPawnCtrl = null;
			m_mouseOverUnit = null;
		}

		/// <summary>
		/// 每帧需要被调用
		/// </summary>
		public virtual void Update()
		{
			
        }

		public virtual void LateUpdate()
		{
			bool v = m_removeDeadReg.Update();
			if (v && m_gameState != null) m_gameState.RemoveDeadUnit();
		}
	}
}