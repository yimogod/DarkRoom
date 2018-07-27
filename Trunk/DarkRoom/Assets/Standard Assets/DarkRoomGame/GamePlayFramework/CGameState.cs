using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.Game {
	/// <summary>
	/// 保存游戏场景数据
	/// 
	/// </summary>
	public class CGameState
	{
        /// <summary>
        /// 游戏开始的时间
        /// </summary>
	    private int m_gameStartTime = 0;

		/// <summary>
		/// 存储着场景中的所有数据
		/// 比如玩家数据.任务数据.地图数据
		/// </summary>

		protected Dictionary<int, CAIController> m_pawnEntityList = new Dictionary<int, CAIController>();

		//我方的单位
		protected List<CAIController> m_redGroup = new List<CAIController>();
		//敌方单位
		protected List<CAIController> m_blueGroup = new List<CAIController>();

		private DataCleaner m_cleaner;

		/// <summary>
		/// 存储着场景所有的角色
		/// </summary>
		public Dictionary<int, CAIController> PawnDict{
			get{ return m_pawnEntityList; }
		}

		public CGameState()
		{
			m_cleaner = new DataCleaner(this);
        }

		public List<CAIController> RedGroup {
			get { return m_redGroup; }
		}

		public List<CAIController> BlueGroup {
			get { return m_blueGroup; }
		}

		/// <summary>
		/// 添加单位到场景
		/// </summary>
		/// <param name="unit"></param>
		public void AddUnit(CAIController unit) {
			m_pawnEntityList.Add(unit.Pawn.CId, unit);

			if (unit.Pawn.Team == CPawnEntity.TeamSide.Red) {
				m_redGroup.Add(unit);
			} else if (unit.Pawn.Team == CPawnEntity.TeamSide.Blue) {
				m_blueGroup.Add(unit);
			}
		}

		/// <summary>
		/// 通过cid获取单位
		/// </summary>
		public CAIController GetUnit(int cid){
			CAIController value = null;
			bool b = m_pawnEntityList.TryGetValue(cid, out value);
			return value;
		}

		/// <summary>
		/// 获取我敌对的团队
		/// </summary>
		public List<CAIController> GetEnemyList(CAIController actor) {
			if (actor.Pawn.Team == CPawnEntity.TeamSide.Blue) {
				return m_redGroup;
			}

			return m_blueGroup;
		}

		/// <summary>
		/// 删除死亡的单位, 可以每1s执行一次
		/// </summary>
		public void RemoveDeadUnit()
		{
			m_cleaner.RemoveDeadUnit();
        }

		public virtual void OnDestroy()
		{
			m_redGroup.Clear();
			m_blueGroup.Clear();
            //PawnDataList.Clear();
			m_pawnEntityList.Clear();
			m_cleaner = null;
		}

		private class DataCleaner
		{
			enum Step
			{
				Red,
				Blue,
				All
			}

			private CGameState m_state;
			private Step m_status;

			public DataCleaner(CGameState state)
			{
				m_state = state;
				m_status = Step.Red;
			}

			public void RemoveDeadUnit()
			{
				switch (m_status) {
					case Step.Red:
						RemoveUnitOf(m_state.RedGroup);
						m_status = Step.Blue;
                    break;
					case Step.Blue:
						RemoveUnitOf(m_state.BlueGroup);
						m_status = Step.All;
						break;
					case Step.All:
						m_status = Step.Red;
						break;
				}
			}

			private void RemoveUnitOf(List<CAIController> list)
			{
				int max = list.Count;
				if (max <= 0) return;

				max -= 1;
				for (int i = max; i >= 0; i--) {
					var item = list[i];
					if (item == null) continue;
					if (item.Pawn.Dead) {
						list.RemoveAt(i);
						GameObject.Destroy(item.gameObject);
					}
				}
			}
		}
		//end privae class
	}

	//end class
}
