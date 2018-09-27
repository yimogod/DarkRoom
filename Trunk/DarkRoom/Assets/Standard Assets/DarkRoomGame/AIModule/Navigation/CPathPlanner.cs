using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.AI
{
	/// <summary>
	/// 寻路的总入口
	/// 内部用直线探测, AStar, Nav Mesh的具体算法获取可通行值. 最终返回一个Int3的列表
	/// 2017/03/30/v1 实现chunk内的局部寻路
	/// Author. liuzhibin
	/// </summary>
	public class CPathPlanner
	{
		private CAStar m_astar = new CAStar(CAStar.Connection.Four);
		private CStarGrid m_starGrid = null;

		//存储的路径列表. 外部只读. 不能修改
		private List<Vector2Int> m_wayPoints = new List<Vector2Int>();

		/// <summary>
		/// 传入地图可通行数据构建寻路对象
		/// </summary>
		public CPathPlanner(CStarGrid map)
		{
			m_starGrid = map;
		}

		/// <summary>
		/// 寻路, 大地图只有一个chunk. 直接进行内部寻路即可
		/// 比如说战斗地图
		/// 这里的坐标是全局坐标, 会根据实际坐标和通行地图的偏移做处理
		/// 如果找不到结果就返回 null
		/// </summary>
		public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
		{
			return FindPath(start.x, start.y, end.x, end.y);
		}

		public void FindPathAsyn(CPathResult result)
		{
			Debug.LogError("CPathPlanner do not have FindPathAsyn");
		}

		/// <summary>
		/// 寻路, 大地图只有一个chunk. 直接进行内部寻路即可
		/// 比如说战斗地图
		/// 这里的坐标是全局坐标, 会根据实际坐标和通行地图的偏移做处理
		/// 如果找不到结果就返回 null
		/// </summary>
		private List<Vector2Int> FindPath(int startCol, int startRow, int endCol, int endRow)
		{
			//Debug.Log($"{startCol}_{startRow}");
			//Debug.Log($"{endCol}_{endRow}");
			m_wayPoints.Clear();
			m_starGrid.SetStartNode(startCol, startRow);
			m_starGrid.SetEndNode(endCol, endRow);

			//这里判断尺寸要稍小一些. 是放置在数据边缘因为小数取证造成的边界不确定
			//最后造成寻路失败.
			int dr = System.Math.Abs(endRow - startRow);
			if (dr > (m_starGrid.NumRows - 2))
			{
				Debug.LogError("PathPlanner.Find Error -- Local Pos Delta Is Bigger Then A Star Grid");
				return m_wayPoints;
			}

			int dc = System.Math.Abs(endCol - startCol);
			if (dc > (m_starGrid.NumCols - 2))
			{
				Debug.LogError("PathPlanner.Find Error -- Local Pos Delta Is Bigger Then A Star Grid");
				return m_wayPoints;
			}

			bool result = m_astar.FindPath(m_starGrid);
			//Debug.Log($"search result is {result}");
			if (result)
			{
				var astarPath = m_astar.Path;
				while (astarPath.Count > 0)
				{
					var node = astarPath.Pop();
					m_wayPoints.Add(new Vector2Int(node.Col, node.Row));
				}
			}

			return m_wayPoints;
		}

		public void SetWalkable(int col, int row, bool walkable)
		{
			m_starGrid.SetWalkable(col, row, walkable);
		}
	}
}