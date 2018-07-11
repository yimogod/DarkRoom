using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.AI {
	/// <summary>
	/// 寻路的总入口
	/// 内部用直线探测, AStar, Nav Mesh的具体算法获取可通行值. 最终返回一个Int3的列表
	/// 2017/03/30/v1 实现chunk内的局部寻路
	/// Author. liuzhibin
	/// </summary>
	public class CPathPlanner : IPathPlanner {
		//存储的所有的数据
		private CMapGrid<CStarNode> m_globalMapData;

		//用于局部寻路的类
		private CALine m_aline = new CALine();
		//我们这里默认a star只能用于32*32内部的寻路.
		//如果超了, 我们就navmesh到附近然后再寻路
		private CAStar m_astar = new CAStar(CAStar.Connection.Eight);
		//用于局部寻路的数据
		private CMapGrid<CStarNode> m_starGrid = new CMapGrid<CStarNode>();

		//存储的路径列表. 外部只读. 不能修改
		private List<Vector3> m_wayPoints = new List<Vector3>();

		/// <summary>
		/// 传入地图可通行数据构建寻路对象
		/// </summary>
		public CPathPlanner(CMapGrid<CStarNode> map){
			m_globalMapData = map;
			m_starGrid.Init(map.NumCols, map.NumRows, false);
        }

		/// <summary>
		/// 寻路, 大地图只有一个chunk. 直接进行内部寻路即可
		/// 比如说战斗地图
		/// 这里的坐标是全局坐标, 会根据实际坐标和通行地图的偏移做处理
		/// 如果找不到结果就返回 null
		/// </summary>
		public List<Vector3> FindPath(Vector3 start, Vector3 end)
		{
			return FindPath((int)start.x, (int)start.z, (int)end.x, (int)end.z);
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
		private List<Vector3> FindPath(int startCol, int startRow, int endCol, int endRow) {

			m_wayPoints.Clear();
			m_starGrid.SetStartNode(startCol, startRow);
			m_starGrid.SetEndNode(endCol, endRow);

			//先在超大地图中做直线查探
			bool result = m_aline.FindPath(m_starGrid);
			if (result) {
				//这里path为空, 但其实直线寻路成功.
				//为了和AStar寻路结果保持一直,手动添加end node
				m_wayPoints.Add(new Vector3(endCol, 0, endRow));
				return m_wayPoints;
			}

			//这里判断尺寸要稍小一些. 是放置在数据边缘因为小数取证造成的边界不确定
			//最后造成寻路失败.
			int dr = System.Math.Abs(endRow - startRow);
			if (dr > (m_starGrid.NumRows - 2)) {
				Debug.LogError("PathPlanner.Find Error -- Local Pos Delta Is Bigger Then A Star Grid");
				return m_wayPoints;
			}

			int dc = System.Math.Abs(endCol - startCol);
			if (dc > (m_starGrid.NumCols - 2)) {
				Debug.LogError("PathPlanner.Find Error -- Local Pos Delta Is Bigger Then A Star Grid");
				return m_wayPoints;
			}

			result = m_astar.FindPath(m_starGrid);
			if (result) {
				var astarPath = m_astar.path;
				while (astarPath.Count > 0) {
				    var node = astarPath.Pop();
					m_wayPoints.Add(new Vector3(node.Col, 0, node.Row));
				}
			}

			return m_wayPoints;
		}

		public void SetWalkable(int col, int row, bool walkable) {
			m_starGrid.SetWalkable(col, row, walkable);
		}

	}
}