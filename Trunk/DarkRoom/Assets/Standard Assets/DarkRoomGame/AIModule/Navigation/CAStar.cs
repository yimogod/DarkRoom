using System;
using System.Collections.Generic;
using DarkRoom.Core;

namespace DarkRoom.AI {
	/// <summary>
	/// A star标准算法. 提供了4和8方向寻路.
	/// 寻路结果不包含传入的开始点,至于是否包含终点由传入参数决定
	/// 2017/03/30/v1 标准寻路算法的实现
	/// Author. liuzhibin
	/// </summary>
	public class CAStar
	{
		/// <summary>
		/// Connection.4向寻路还是8向寻路
		/// </summary>
		public enum Connection{
			Four,
			Eight
		}

		//4,8方向的寻路
		private readonly Connection m_conn;

		//AStar要寻路的数据
		private CAStarGrid m_grid;
		//开始和结束点
		private AStarNode m_startNode;
		private AStarNode m_endNode;

		//直线通过的成本
		private float m_straightCost = 1.0f;
		//斜线通过的成本
		private float m_diagCost = 1.4f;

		//AStar计算需要的三个列表
		private CPriorityQueue<AStarNode> m_open = new CPriorityQueue<AStarNode>();
		private List<AStarNode> m_closed = new List<AStarNode>();
		private Stack<AStarNode> m_path = new Stack<AStarNode>();

		//是否寻找到最近的点. 还是end node上面
		private bool m_findClosestNode = false;

		/// <param name="conn">决定是4方向还是8方向寻路</param>
		public CAStar(Connection conn){
			m_conn = conn;
		}

		/// <summary>
		/// 获取寻路结果. 在调用FindPath方法后再使用此结果
		/// </summary>
		/// <value>The path.</value>
		public Stack<AStarNode> path{
			get { return m_path; }
		}

		/// <summary>
		/// Finds the path.
		/// result do not has the start node itself 
		/// </summary>
		/// <returns><c>true</c>, if path was found, <c>false</c> otherwise.</returns>
		/// <param name="grid">传入需要进行寻路的地图</param>
		/// <param name="closeToDestination">If set to <c>false</c>, 寻路结果不包含终点</param>
		public bool FindPath(CAStarGrid grid, bool closeToDestination = false){
			m_grid = grid;
			m_findClosestNode = closeToDestination;
			m_open.Clear();
			m_closed.Clear();

			m_startNode = m_grid.StartNode;
			m_endNode = m_grid.EndNode;
			//终点不可通行, 我就不寻路了
			if (!m_endNode.Walkable)return false;

			m_startNode.g = 0;
			m_startNode.h = Heuristic(m_startNode);
			m_startNode.f = m_startNode.g + m_startNode.h;
			bool result = Search();
			return result;
		}

		//寻路的具体算法
		private bool Search(){
			AStarNode node = m_startNode;

			while(node != m_endNode){
				int startCol = Math.Max(0, node.Col - 1);
				int endCol = Math.Min(m_grid.NumCols - 1, node.Col + 1);
				int startRow = Math.Max(0, node.Row - 1);
				int endRow = Math.Min(m_grid.NumRows - 1, node.Row + 1);

				//默认我们是寻找九宫格的可通行性
				//但对于2d游戏, 我们想仅仅寻找左右上下
				for(int i = startRow; i <= endRow; i++){
					for(int j = startCol; j <= endCol; j++){
						AStarNode test = m_grid.GetNode(j, i);
						if(test == node)continue;
						if(!test.Walkable)continue;
						//这里之前是在后面也做f更新了. 但考虑提前到这里会提高效率
						//如果寻路结果错误, 要移回去
						if (IsClosed(test))continue;

						//如果仅计算上下左右, 就4个斜角的就不计算了
						if(m_conn == Connection.Four && node.Col != test.Col && node.Row != test.Row)
							continue;

						//设置相邻两个格子的通行代价
						float cost = m_diagCost;
						if(node.Col == test.Col || node.Row == test.Row){
							cost = m_straightCost;
						}

						float g = node.g + cost * test.CostMultiplier;
						float h = Heuristic(test);
						float f = g + h;
						if(IsOpen(test)){
							if(test.f > f)test.FillValue(f, g, h, node);
						} else {
							test.FillValue(f, g, h, node);
							m_open.Push(test);
						}
					}
				}
				m_closed.Add(node);

				if(m_open.Count == 0)return false;
				node = m_open.Pop();
			}
			BuildPath();
			return true;
		}

		private void BuildPath(){
			m_path.Clear();
			AStarNode node = m_endNode;
			if (!m_findClosestNode)m_path.Push(node);

			while(node != m_startNode){
				node = node.Parent;
				m_path.Push(node);
			}

			/* remove the first node which indeed is start node */
			if(m_path.Count > 0)m_path.Pop();
		}

		//寻路评估函数
		private float Heuristic(AStarNode node){
			int dcol = m_endNode.Col - node.Col;
			int drow = m_endNode.Row - node.Row;
			return Math.Abs (dcol) * m_straightCost + Math.Abs(drow) * m_straightCost;
		}

		private bool IsOpen(AStarNode node){
			return m_open.Contains(node);
		}

		private bool IsClosed(AStarNode node){
			return m_closed.Contains(node);
		}
	}
}