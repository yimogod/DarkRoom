using System.Collections.Generic;
using DarkRoom.Core;

namespace DarkRoom.AI {
	/// <summary>
	/// 图的节点. 可用于寻路(Dijkstra, 盲目深度/广度优先)
	/// </summary>
	public class CGraphNode : IPriority
	{
		/// <summary>
		/// 用来标记GraphNode的唯一性
		/// </summary>
		public int Id;

		/// <summary>
		/// 图节点的邻居
		/// </summary>
		public List<Neighbour> Neighbours = new List<Neighbour>();

		/// <summary>
		/// 形成寻路结果链表的数据
		/// </summary>
		public CGraphNode Parent;

		/// <summary>
		/// GraphNode 用组合方式携带的数据
		/// </summary>
		public System.Object Value;

		/// <summary>
		/// 类似于A*的g, 指的是从开始节点到本节点的代价
		/// </summary>
		public float G;

		public CGraphNode(int id) { this.Id = id; }

		/// <summary>
		/// 本node的链接node个数
		/// </summary>
		public int neighbourCount {
			get { return Neighbours.Count; }
		}

		public void FillValue(float g, CGraphNode parent) {
			this.G = g;
			this.Parent = parent;
		}

		/// <summary>
		/// 实现优先级队列的接口
		/// </summary>
		/// <returns></returns>
		public float GetPriority() { return G; }

		/// <summary>
		/// 添加邻接信息
		/// </summary>
		/// <param name="cGraphNode">邻居节点</param>
		/// <param name="cost">到邻居的代价</param>
		public void AddNeighbour(CGraphNode cGraphNode, float cost)
		{
			Neighbours.Add(new Neighbour(cGraphNode, cost));
			cGraphNode.Neighbours.Add(new Neighbour(this, cost));
		}

		/// <summary>
		/// 默认相邻点见成本为1的方法
		/// </summary>
		/// <param name="cGraphNode"></param>
		public void AddNeighbour(CGraphNode cGraphNode)
		{
			AddNeighbour(cGraphNode, 1f);
		}

		/// <summary>
		/// 用于存储graph node 周围的相邻的node
		/// </summary>
		public struct Neighbour
		{
			public CGraphNode node;
			public float cost;

			public Neighbour(CGraphNode cGraphNode, float cost)
			{
				node = cGraphNode;
				this.cost = cost;
			}

		}
	}
}