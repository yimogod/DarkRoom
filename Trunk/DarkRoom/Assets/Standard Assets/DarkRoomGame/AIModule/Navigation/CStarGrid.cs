using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.AI {
	/// <summary>
	/// 用于过程生成地图的数据结构
	/// </summary>
	public class CStarGrid : CMapGrid<CStarNode> {
	    //本次需要寻路的开始和终点
	    protected CStarNode m_startNode;
	    protected CStarNode m_endNode;

	    /// <summary>
	    /// 返回寻路起点
	    /// </summary>
	    /// <value>The start node.</value>
	    public CStarNode StartNode => m_startNode;

	    /// <summary>
	    /// 返回寻路终点
	    /// </summary>
	    /// <value>The end node.</value>
	    public CStarNode EndNode => m_endNode;

	    /// <summary>
	    /// 设置寻路的起点
	    /// </summary>
	    public void SetStartNode(Vector2Int pos)
	    {
	        SetStartNode(pos.x, pos.y);
	    }

        /// <summary>
        /// 设置寻路的起点
        /// </summary>
        public void SetStartNode(int col, int row)
	    {
	        var node = GetNode(col, row);
	        if (node.Invalid)
	        {
	            Debug.LogError("SetStartNode Error");
	            return;
	        }
	        m_startNode = m_nodes[col, row];
	    }

	    /// <summary>
	    /// 设置寻路的终点
	    /// </summary>
	    public void SetEndNode(Vector2Int pos)
	    {
	        SetEndNode(pos.x, pos.y);
	    }

        /// <summary>
        /// 设置寻路的终点
        /// </summary>
        public void SetEndNode(int col, int row)
	    {
	        var node = GetNode(col, row);
	        if (node.Invalid)
	        {
	            Debug.LogError("SetEndNode Error");
	            return;
	        }
	        m_endNode = m_nodes[col, row];
	    }
    }

    /// <summary>
    /// AStarGrid的单位数据.记录了AStar寻路需要的一些数据
    /// </summary>
    public class CStarNode : IPriority, IWalkableNode
    {
        /// <summary>
        /// The row.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// The col.
        /// </summary>
        public int Col { get; set; }

        /// <summary>
        /// The walkable.
        /// </summary>
        public bool Walkable { get; set; }

        public bool Invalid
        {
            get { return Col < 0 || Row < 0; }
        }

        /// <summary>
        /// 经过此点寻路的总代价. f代价
        /// </summary>
        public float F;

        /// <summary>
        /// 原点到此点的移动代价. g代价
        /// </summary>
        public float G;

        /// <summary>
        /// 此点到目的点的代价, h代价
        /// </summary>
        public float H;

        /// <summary>
        /// 形成寻路结果链表的数据
        /// </summary>
        public CStarNode Parent;

        /// <summary>
        /// 本地代价修正,比如雪地的代价就是2之类的
        /// </summary>
        public float CostMultiplier = 1.0f;

        public CStarNode()
        {
            Col = -1;
            Row = -1;
            Walkable = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CStarNode"/> class.
        /// </summary>
        /// <param name="row">Row.</param>
        /// <param name="col">Col.</param>
        /// <param name="walkable">本单位格子是否可以通行</param>
        public CStarNode(int col, int row, bool walkable)
        {
            Col = col;
            Row = row;
            Walkable = walkable;
        }

        //实现优先级队列的接口
        public float GetPriority() { return F; }

        /// <summary>
        /// 填充本node的值
        /// </summary>
        /// <param name="f">F.</param>
        /// <param name="g">G.</param>
        /// <param name="h">H.</param>
        /// <param name="parent">Parent.</param>
        public void FillValue(float f, float g, float h, CStarNode parent)
        {
            F = f;
            G = g;
            H = h;
            Parent = parent;
        }

		public override string ToString()
		{
			return Walkable ? "1" : "0";
		}
	}
}
