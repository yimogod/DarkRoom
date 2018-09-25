using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.Game;

namespace DarkRoom.AI {
	/// <summary>
	/// Dijkstra标准算法
	/// 寻路结果不包含传入的开始点
	/// </summary>
	public class CDijkstra {
		//开始和结束点
		private CGraphNode m_startNode;
		private CGraphNode m_endNode;

		//寻路计算需要的三个列表
		private CPriorityQueue<CGraphNode> m_open = new CPriorityQueue<CGraphNode>();
		private List<CGraphNode> m_closed = new List<CGraphNode>();
		private Stack<CGraphNode> m_path = new Stack<CGraphNode>();

		public CDijkstra() {}

		/// <summary>
		/// 获取寻路结果. 在调用FindPath方法后再使用此结果
		/// </summary>
		/// <value>The path.</value>
		public Stack<CGraphNode> Path {
			get { return m_path; }
		}

		/// <summary>
		/// Dijkstra寻路入口
		/// </summary>
		/// <param name="graph">整张图的数据</param>
		/// <param name="start">开始节点id</param>
		/// <param name="end">结束节点id</param>
		/// <returns></returns>
		public bool FindPath(List<CGraphNode> graph, int start, int end)
		{
			CGraphNode startNode = graph[start];
			CGraphNode endNode = graph[end];

			return FindPath(startNode, endNode);
		}

		/// <summary>
		/// 寻路start 和 end, 因为GraphNode本身包含图结构的数据
		/// 所以我们没必要传如所有的GraphNode数据了
		/// </summary>
		/// <param name="start">开始节点</param>
		/// <param name="end">结束节点</param>
		/// <returns></returns>
		public bool FindPath(CGraphNode start, CGraphNode end) {
			m_open.Clear();
			m_closed.Clear();

			m_startNode = start;
			m_endNode = end;

			m_startNode.G = 0;
			bool result = Search();
			return result;
		}

		//寻路的具体算法
		private bool Search() {
			CGraphNode node = m_startNode;

			while (node != m_endNode) {
				for (int i = 0; i < node.NeighbourCount; i++) {
					CGraphNode.Neighbour neighbour = node.Neighbours[i];
					CGraphNode test = neighbour.node;
                    if (test == node) continue;
					if (IsClosed(test)) continue;


					float g = node.G + neighbour.cost;
					if (IsOpen(test)) {
						if (test.G > g) test.FillValue(g, node);
					} else {
						test.FillValue(g, node);
						m_open.Push(test);
					}
				}
				m_closed.Add(node);

				if (m_open.Count == 0) return false;
				node = m_open.Pop();
			}
			BuildPath();
			return true;
		}

		private void BuildPath() {
			m_path.Clear();
			CGraphNode node = m_endNode;
			m_path.Push(node);

			while (node != m_startNode) {
				node = node.Parent;
				m_path.Push(node);
			}

			/* remove the first node which indeed is start node */
			if (m_path.Count > 0) m_path.Pop();
		}

		private bool IsOpen(CGraphNode node) {
			return m_open.Contains(node);
		}

		private bool IsClosed(CGraphNode node) {
			return m_closed.Contains(node);
		}
	}
}