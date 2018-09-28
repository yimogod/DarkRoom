using System;
using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.AI
{
	/// <summary>
	/// 导航系统, 所有单位行走的总接口
	/// </summary>
	public class CTileNavigationSystem
	{
		private static CTileNavigationSystem s_instance = null;

		//地图可通行数据
		private CStarGrid m_mapWalkableData = null;

		//同步寻路对象
		private CTilePathPlanner m_pathPlanner = null;

		//是否是空旷的空间
		private bool m_space = false;

		/// <summary>
		/// 导航模块的可通行数据
		/// </summary>
		public CStarGrid WalkableGrid => m_mapWalkableData;

		//私有的默认构造函数
		private CTileNavigationSystem()
		{
		}

		/// <summary>
		/// 获取 CNavigationSystem 的单例接口
		/// </summary>
		public static CTileNavigationSystem Instance
		{
			get
			{
				if (s_instance == null)
				{
					s_instance = new CTileNavigationSystem();
				}

				return s_instance;
			}
		}

		/// <summary>
		/// 导航系统存储的grid地图的row
		/// </summary>
		public int MapNumRows
		{
			get { return m_mapWalkableData.NumRows; }
		}

		/// <summary>
		/// 导航系统存储的grid地图的col
		/// </summary>
		public int MapNumCols
		{
			get { return m_mapWalkableData.NumCols; }
		}

		/// <summary>
		/// 初始化整个系统
		/// </summary>
		public void Initialize(int mapNumCols, int mapNumRows)
		{
			m_mapWalkableData = new CStarGrid();
			m_mapWalkableData.Init(mapNumCols, mapNumRows);
			m_pathPlanner = new CTilePathPlanner(m_mapWalkableData);
		}

		/// <summary>
		/// 作为绝对三维空旷的空间初始化导航数据
		/// 所有的寻路都是返回终点
		/// </summary>
		public void InitializeAsSpace()
		{
			m_space = true;
		}

		/// <summary>
		/// 设置地图所有的格子的walkable 为 value
		/// </summary>
		public void SetAllMapWalkable(bool value)
		{
			if (m_space) return;
			m_mapWalkableData.SetAllMapWalkable(value);
		}

		/// <summary>
		/// 设置某个格子的可通行性
		/// </summary>
		/// <param name="row">格子坐标</param>
		/// <param name="col">格子坐标</param>
		/// <param name="walkable">可通行性</param>
		public void SetWalkable(int col, int row, bool walkable)
		{
			if (m_space) return;
			m_mapWalkableData.SetWalkable(col, row, walkable);
			m_pathPlanner.SetWalkable(col, row, walkable);
		}

		public void CopyUnWalkableFrom(IWalkableGrid grid)
		{
			m_mapWalkableData.CopyUnWalkableFrom(grid);
		}

		/// <summary>
		/// 获取格子的可通行性
		/// </summary>
		/// <param name="row">格子坐标</param>
		/// <param name="col">格子坐标</param>
		/// <returns>是否可通行</returns>
		public bool IsWalkable(int col, int row)
		{
			if (m_space) return true;
			return m_mapWalkableData.IsWalkable(col, row);
		}

		/// <summary>
		/// 传入float字段获取格子的可通行性
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		public bool IsWalkable(float col, float row)
		{
			if (m_space) return true;
			return IsWalkable((int) col, (int) row);
		}

		/// <summary>
		/// 寻找一个可同行的地块.
		/// </summary>
		/// <returns>The walkable tile.</returns>
		public Vector3 FindWalkableTile()
		{
			int times = 10;
			while (times > 0)
			{
				int col = CDarkRandom.Next(m_mapWalkableData.NumCols);
				int row = CDarkRandom.Next(m_mapWalkableData.NumRows);
				if (IsWalkable(col, row))
					return new Vector3(col, 0, row);
				times--;
			}

			for (int c = 0; c < m_mapWalkableData.NumCols; c++)
			{
				for (int r = 0; r < m_mapWalkableData.NumRows; r++)
				{
					if (IsWalkable(c, r))
						return new Vector3(c, 0, r);
				}
			}

			return Vector3.zero;
		}

		/// <summary>
		/// 简单的移动到位置
		/// </summary>
		/// <param name="me">要移动的角色</param>
		/// <param name="goal">移动到的目的地</param>
		public void SimpleMoveToLocation(CController me, Vector3 goal, CPawnPathFollowingComp.OnPathFinished onComplete = null)
		{
			if (me.Pawn.IsFollowingPath)
			{
				me.Pawn.StopMovement();
			}

			CPawnPathFollowingComp follower = me.Pawn.Follower;
			//如果我已经到了目的地, 那么就直接达到
			bool alreadyAtGoal = follower.HasReached(goal, 0.5f);
			if (alreadyAtGoal)
			{
				follower.RequestMoveWithImmediateFinish(FinishPathResultType.Success);
				onComplete?.Invoke(FinishPathResultType.Success);
				return;
			}

			//或者我们走过去
			CTilePathResult data = GetWayPointBetween(me.LocalPosition.GetVector2Int(), goal.GetVector2Int());
			follower.RequestMove(data, onComplete);
		}

		/// <summary>
		/// 获取起点和终点寻路后的路径点
		/// 记住, 如果寻路采用多线程, 则本方法返回为空列表
		/// </summary>
		private List<Vector2Int> m_wayPoints = new List<Vector2Int>();

		private List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
		{
			m_wayPoints.Clear();
			if (m_space)
			{
				m_wayPoints.Add(goal);
			}
			else
			{
				var result = m_pathPlanner.FindPath(start, goal);
				if(result.Count > 0)m_wayPoints.AddRange(result);
			}

			return m_wayPoints;
		}


		/// <summary>
		/// 同步获取起点和终点寻路后的路径点
		/// </summary>
		public CTilePathResult GetWayPointBetween(Vector2Int startTile, Vector2Int goalTile)
		{
			//Debug.Log($"get way point between {start} and {goal}");
			var wayPoints = FindPath(startTile, goalTile);
			CTilePathResult data = new CTilePathResult();
			data.StartPos = startTile;
			data.EndPos = goalTile;
			data.SetWayPoints(wayPoints);
			return data;
		}


		public void PrintGrid()
		{
			CMapUtil.PrintGird(m_mapWalkableData);
		}

		public void DrawGrid()
		{
			CMapUtil.DrawGrid(m_mapWalkableData);
		}
	}
}