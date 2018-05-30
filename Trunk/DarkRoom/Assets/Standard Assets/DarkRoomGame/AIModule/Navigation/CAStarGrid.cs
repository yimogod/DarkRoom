using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.AI {
	/// <summary>
	/// 二维表格数据. 用于AStar和直线探测寻路
	///2017/03/30/v1
	/// Author. liuzhibin
	/// </summary>
	public class CAStarGrid : IWalkable
	{
		//地图所有的node数据.
		//[col][row]
		private AStarNode[,] m_nodes;
		private int m_numRows;
		private int m_numCols;

		//本次需要寻路的开始和终点
		private AStarNode m_startNode;
		private AStarNode m_endNode;

		/// <summary>
		/// 默认构造函数. 啥都没干
		/// </summary>
		public CAStarGrid(){}

		/// <summary>
		/// 返回寻路起点
		/// </summary>
		/// <value>The start node.</value>
		public AStarNode StartNode{ get { return m_startNode; } }

		/// <summary>
		/// 返回寻路终点
		/// </summary>
		/// <value>The end node.</value>
		public AStarNode EndNode{ get { return m_endNode; } }

		/// <summary>
		/// 返回网格列数
		/// </summary>
		/// <value>The number cols.</value>
		public int NumCols{ get { return m_numCols; } }

		/// <summary>
		/// 返回网格行数
		/// </summary>
		/// <value>The number rows.</value>
		public int NumRows{ get { return m_numRows; } }

		/// <summary>
		/// 初始化二维网格地图. 并指定所有的格子的可通行性为true
		/// </summary>
		/// <param name="numCols">Number cols.</param>
		/// <param name="numRows">Number rows.</param>
		public void Init(int numCols, int numRows){
			Init(numCols, numRows, true);
		}

		/// <summary>
		/// 初始化二维网格地图. 并指定所有的格子的可通行性的初始化值
		/// </summary>
		/// <param name="numCols">Number cols.</param>
		/// <param name="numRows">Number rows.</param>
		/// <param name="walkable">If set to <c>true</c> walkable.此地图所有的格子可通行则为true</param>
		public void Init(int numCols, int numRows, bool walkable){
			m_numRows = numRows;
			m_numCols = numCols;

			m_nodes = new AStarNode[m_numCols, m_numRows];
			for(int row = 0; row < m_numRows; row++){
				for(int col = 0; col < m_numCols; col++){
					m_nodes[col, row] = new AStarNode(col, row, walkable);
				}
			}
		}

		

		/// <summary>
		/// 设置地图所有的格子的walkable 为 value
		/// </summary>
		public void SetAllMapWalkable(bool value){
			for(int row = 0; row < NumRows; row++){
				for(int col = 0; col < NumCols; col++){
					m_nodes[col, row].Walkable = value;
				}
			}
		}

		/// <summary>
		/// 根据坐标获取格子
		/// 如果格子非法, 就返回 null
		/// </summary>
		/// <returns>The node.</returns>
		/// <param name="row">Row.</param>
		/// <param name="col">Col.</param>
		public AStarNode GetNode(int col, int row){
			if (row < 0 || col < 0)return null;
			if (row >= m_numRows)return null;
			if (col >= m_numCols)return null;
			return m_nodes[col, row];
		}

		/// <summary>
		/// 通过坐标本格子的可通行性
		/// 如果格子非法, 则不可通行
		/// </summary>
		/// <returns>本格子的可通行性</returns>
		/// <param name="row">Row. Base On 0</param>
		/// <param name="col">Col. Base On 0</param>
		public bool IsWalkable(int col, int row){
			AStarNode node = GetNode(col, row);
			if(node == null)return false;
			return node.Walkable;
		}

		/// <summary>
		/// 设置合法的格子的可通行性
		/// </summary>
		/// <param name="row">Row.</param>
		/// <param name="col">Col.</param>
		/// <param name="value">格子的可通行性</param>
		public void SetWalkable(int col, int row, bool value){
			AStarNode node = GetNode(col, row);
			if (node == null){
				Debug.LogError("SetWalkable Error");
				return;
			}

			node.Walkable = value;
		}

		/// <summary>
		/// 设置寻路的起点
		/// </summary>
		/// <param name="row">Row.</param>
		/// <param name="col">Col.</param>
		public void SetStartNode(int col, int row){
			AStarNode node = GetNode(col, row);
			if (node == null){
				Debug.LogError("SetStartNode Error");
				return;
			}
			m_startNode = m_nodes[col, row];
		}

		/// <summary>
		/// 设置寻路的终点
		/// </summary>
		/// <param name="row">Row.</param>
		/// <param name="col">Col.</param>
		public void SetEndNode(int col, int row){
			AStarNode node = GetNode(col, row);
			if (node == null){
				Debug.LogError("SetEndNode Error");
				return;
			}
			m_endNode = m_nodes[col, row];
		}

		/// <summary>
		/// 清理本实例持有的一些对象引用
		/// </summary>
		public void Dispose(){
			m_nodes = null;
			m_startNode = null;
			m_endNode = null;
		}
	}

	/// <summary>
	/// AStarGrid的单位数据.记录了AStar寻路需要的一些数据
	/// 2017/03/30/v1
	/// Author. liuzhibin
	/// </summary>
	public class AStarNode : IPriority
	{
		/// <summary>
		/// The row.
		/// </summary>
		public int Row;

		/// <summary>
		/// The col.
		/// </summary>
		public int Col;

		/// <summary>
		/// The walkable.
		/// </summary>
		public bool Walkable = true;

		/// <summary>
		/// 经过此点寻路的总代价. f代价
		/// </summary>
		public float f;

		/// <summary>
		/// 原点到此点的移动代价. g代价
		/// </summary>
		public float g;

		/// <summary>
		/// 此点到目的点的代价, h代价
		/// </summary>
		public float h;

		/// <summary>
		/// 形成寻路结果链表的数据
		/// </summary>
		public AStarNode Parent;

		/// <summary>
		/// 本地代价修正,比如雪地的代价就是2之类的
		/// </summary>
		public float CostMultiplier = 1.0f;

		/// <summary>
		/// Initializes a new instance of the <see cref="AStarNode"/> class.
		/// </summary>
		/// <param name="row">Row.</param>
		/// <param name="col">Col.</param>
		/// <param name="walkable">本单位格子是否可以通行</param>
		public AStarNode(int col, int row, bool walkable) {
			Col = col;
			Row = row;
			Walkable = walkable;
		}

		//实现优先级队列的接口
		public float GetPriority() { return f; }

		/// <summary>
		/// 填充本node的值
		/// </summary>
		/// <param name="f">F.</param>
		/// <param name="g">G.</param>
		/// <param name="h">H.</param>
		/// <param name="parent">Parent.</param>
		public void FillValue(float f, float g, float h, AStarNode parent) {
			this.f = f;
			this.g = g;
			this.h = h;
			this.Parent = parent;
		}
    }
}
