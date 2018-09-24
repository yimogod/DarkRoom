using System;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.Game
{
	/// <summary>
	/// 二维表格数据. 用于AStar和直线探测寻路
	/// </summary>
	public class CMapGrid<T> : IWalkableGrid where T : IWalkableNode, new()
	{
		//地图所有的node数据.
		//[col][row]
		protected T[,] m_nodes;
		protected Vector2Int m_size;

		/// <summary>
		/// 返回网格列数
		/// </summary>
		/// <value>The number cols.</value>
		public int NumCols => m_size.x;

		/// <summary>
		/// 返回网格行数
		/// </summary>
		/// <value>The number rows.</value>
		public int NumRows => m_size.y;

		public T[,] RawData => m_nodes;

		/// <summary>
		/// 初始化二维网格地图. 并指定所有的格子的可通行性为true
		/// </summary>
		/// <param name="numCols">Number cols.</param>
		/// <param name="numRows">Number rows.</param>
		public void Init(int numCols, int numRows)
		{
			Init(numCols, numRows, true);
		}

		/// <summary>
		/// 初始化二维网格地图. 并指定所有的格子的可通行性的初始化值
		/// </summary>
		/// <param name="numCols">Number cols.</param>
		/// <param name="numRows">Number rows.</param>
		/// <param name="walkable">If set to <c>true</c> walkable.此地图所有的格子可通行则为true</param>
		public void Init(int numCols, int numRows, bool walkable)
		{
			m_size = new Vector2Int(numCols, numRows);

			m_nodes = new T[m_size.x, m_size.y];
			for (int row = 0; row < m_size.y; row++)
			{
				for (int col = 0; col < m_size.x; col++)
				{
					T t = new T();
					t.Col = col;
					t.Row = row;
					t.Walkable = walkable;
					m_nodes[col, row] = t;
				}
			}
		}

		/// <summary>
		/// 设置地图所有的格子的walkable 为 value
		/// </summary>
		public void SetAllMapWalkable(bool value)
		{
			for (int row = 0; row < NumRows; row++)
			{
				for (int col = 0; col < NumCols; col++)
				{
					m_nodes[col, row].Walkable = value;
				}
			}
		}

		public T GetNode(Vector2Int pos)
		{
			return GetNode(pos.x, pos.y);
		}

		/// <summary>
		/// 根据坐标获取格子
		/// 如果格子非法, 就返回 null
		/// </summary>
		public T GetNode(int col, int row)
		{
			if (row < 0 || col < 0) return new T();
			if (row >= m_size.y) return new T();
			if (col >= m_size.x) return new T();
			return m_nodes[col, row];
		}

		/// <summary>
		/// 通过坐标本格子的可通行性
		/// 如果格子非法, 则不可通行
		/// </summary>
		/// <returns>本格子的可通行性</returns>
		/// <param name="row">Row. Base On 0</param>
		/// <param name="col">Col. Base On 0</param>
		public bool IsWalkable(int col, int row)
		{
			T node = GetNode(col, row);
			if (node.Invalid) return false;
			return node.Walkable;
		}

		/// <summary>
		/// 设置合法的格子的可通行性
		/// </summary>
		public void SetWalkable(Vector2Int pos, bool value)
		{
			SetWalkable(pos.x, pos.y, value);
		}

		/// <summary>
		/// 设置合法的格子的可通行性
		/// </summary>
		public void SetWalkable(int col, int row, bool value)
		{
			T node = GetNode(col, row);
			if (node.Invalid)
			{
				Debug.LogError("SetWalkable Error");
				return;
			}

			node.Walkable = value;
		}

		public void CopyUnWalkableFrom(IWalkableGrid grid)
		{
			CopyUnWalkableFrom(grid, 0, 0, m_size.x, m_size.y);
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

		public void CopyWalkableFrom(IWalkableGrid grid)
		{
			CopyWalkableFrom(grid, 0, 0, m_size.x, m_size.y);
		}

		public void CopyWalkableFrom(IWalkableGrid grid, int startCol, int startRow, int endCol, int endRow)
		{
			for (int r = startRow; r < endRow; r++)
			{
				for (int c = startCol; c < endCol; c++)
				{
					bool inWalkable = grid.IsWalkable(c, r);
					if (inWalkable) SetWalkable(c, r, true);
				}
			}
		}

		/*获取所在位置最近的可通行图*/
		public Vector2Int FindNearestWalkablePos(Vector2Int pos)
		{
			T node = GetNode(pos);
			if (node != null && node.Walkable) return pos;


			int gap = 1;
			int maxGap = Math.Max(m_size.x, m_size.y);
			while (gap < maxGap)
			{
				int minCol = pos.x - gap;
				int maxCol = pos.x + gap;
				int minRow = pos.y - gap;
				int maxRow = pos.y + gap;

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

			return CDarkConst.INVALID_VEC2INT;
		}

		/// <summary>
		/// 清理本实例持有的一些对象引用
		/// </summary>
		public void Dispose()
		{
			m_nodes = null;
		}
	}
}