﻿using System;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.Game {
	/// <summary>
	/// 二维表格数据. 用于AStar和直线探测寻路
	/// </summary>
	public class CMapGrid<T> : IWalkableGrid where T : IWalkableNode
	{
		//地图所有的node数据.
		//[col][row]
		protected T[,] m_nodes;
        protected int m_numRows;
        protected int m_numCols;

        //本次需要寻路的开始和终点
        protected T m_startNode;
        protected T m_endNode;

		/// <summary>
		/// 默认构造函数. 啥都没干
		/// </summary>
		public CMapGrid(){}

		/// <summary>
		/// 返回寻路起点
		/// </summary>
		/// <value>The start node.</value>
		public T StartNode{ get { return m_startNode; } }

		/// <summary>
		/// 返回寻路终点
		/// </summary>
		/// <value>The end node.</value>
		public T EndNode{ get { return m_endNode; } }

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

			m_nodes = new T[m_numCols, m_numRows];
			for(int row = 0; row < m_numRows; row++){
				for(int col = 0; col < m_numCols; col++){
                    T t  = default(T);
                    t.Col = col;
                    t.Row = row;
                    m_nodes[col, row] = t;
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
		public T GetNode(int col, int row){
			if (row < 0 || col < 0)return default(T);
			if (row >= m_numRows)return default(T);
			if (col >= m_numCols)return default(T);
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
			T node = GetNode(col, row);
			if(node.Invalid)return false;
			return node.Walkable;
		}

		/// <summary>
		/// 设置合法的格子的可通行性
		/// </summary>
		/// <param name="row">Row.</param>
		/// <param name="col">Col.</param>
		/// <param name="value">格子的可通行性</param>
		public void SetWalkable(int col, int row, bool value){
			T node = GetNode(col, row);
			if (node.Invalid){
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
			T node = GetNode(col, row);
			if (node.Invalid){
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
			T node = GetNode(col, row);
			if (node.Invalid){
				Debug.LogError("SetEndNode Error");
				return;
			}
			m_endNode = m_nodes[col, row];
		}

	    public void CopyUnWalkableFrom(IWalkableGrid grid, int startCol, int startRow, int endCol, int endRow)
	    {
	        for (int r = startRow; r < endRow; r++)
	        {
	            for (int c = startCol; c < endCol; c++)
	            {
	                if (grid.IsWalkable(c, r)) continue;
	                SetWalkable(c, r, false);
                }
	        }
	    }

	    public void CopyWalkableFrom(IWalkableGrid grid, int startCol, int startRow, int endCol, int endRow)
	    {
	        for (int r = startRow; r < endRow; r++)
	        {
	            for (int c = startCol; c < endCol; c++)
	            {
	                bool inWalkable = grid.IsWalkable(c, r);
	                if (inWalkable)SetWalkable(c, r, true);
	            }
	        }
	    }

	    /*获取所在位置最近的可通行图*/
	    public Vector3 FindNearestWalkablePos(Vector3 pos)
	    {
	        int row = (int)pos.z;
	        int col = (int)pos.x;
	        T node = GetNode(row, col);
	        if (node != null && node.Walkable) return pos;


	        int gap = 1;
	        int maxGap = Math.Max(m_numCols, m_numRows);
	        while (gap < maxGap)
	        {
	            int minCol = col - gap;
	            int maxCol = col + gap;
	            int minRow = row - gap;
	            int maxRow = row + gap;

	            //1. two rows line
	            for (int i = minCol; i <= maxCol; i++)
	            {
	                node = GetNode(maxRow, i);
	                //if (node != null && node.Walkable) return node.vector;

	                node = GetNode(minRow, i);
	                //if (node != null && node.Walkable) return node.vector;
	            }

	            //2. two cols line
	            for (int i = minRow + 1; i < maxRow; i++)
	            {
	                node = GetNode(i, minCol);
	                //if (node != null && node.Walkable) return node.vector;

	                node = GetNode(i, maxCol);
	               // if (node != null && node.Walkable) return node.vector;
	            }

	            gap++;
	        }

	        return CDarkConst.INVALID_VEC3;
	    }

        /// <summary>
        /// 清理本实例持有的一些对象引用
        /// </summary>
        public void Dispose(){
			m_nodes = null;
		}
	}
}