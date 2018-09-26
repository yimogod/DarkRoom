using UnityEngine;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Core;
using DarkRoom.Game;

namespace Sword
{
	/// <summary>
	/// 存储着地图中哪些格子上有单位(monster, trigger), 单位的势力范围等
	/// </summary>
	public class ActorGeneraterPlaceholder
	{
		/* 存储怪 触发器和不可通行的位置, 以及怪的影响范围
		 如果添加一个怪, 则周围的2层都会被占用, 用于随机创建一个怪时避免重叠, key = row * 10000 + col;
		 */
		private Dictionary<int, bool> m_unitRangeDict = new Dictionary<int, bool>();

		/*存储怪 触发器 和不可通行的位置, 用于创建trigger时避开, 但可以在怪的影响力范围, key = row * 10000 + col;*/
		private Dictionary<int, bool> m_unitPosDict = new Dictionary<int, bool>();

		private int m_numRows;
		private int m_numCols;

		public ActorGeneraterPlaceholder(CStarGrid walkGrid)
		{
			m_numRows = walkGrid.NumRows;
			m_numCols = walkGrid.NumCols;

			for (int row = 0; row < m_numRows; ++row)
			{
				for (int col = 0; col < m_numCols; ++col)
				{
					bool w = walkGrid.IsWalkable(row, col);
					if (w) continue;

					int key = row * 10000 + col;
					m_unitRangeDict[key] = true;
					m_unitPosDict[key] = true;
				}
			}
		}

		/// <summary>
		/// 添加随机的位置到占位地图
		/// 如果位置不合理, 会重新寻找一个. 返回位置
		/// </summary>
		public Vector2Int AddUnitToDict(Vector2Int pos, int range = 2)
		{
			int key = 10000 * pos.y + pos.x;
			//如果位置已经被占用, 寻找一个最近的空位
			if (m_unitRangeDict.ContainsKey(key))
			{
				pos = FindFreeTileNear(pos);
				//如果找不到合适的位置就不添加了
				if (CDarkUtil.IsInvalidVec2Int(pos))return pos;
				key = 10000 * pos.y + pos.x;
			}

			m_unitPosDict[key] = true;
			if (range <= 0) return pos;

			int minCol = pos.x - range;
			int maxCol = minCol + range * 2;
			int minRow = pos.y - range;
			int maxRow = minRow + range * 2;

			for (int row = minRow; row <= maxRow; row++)
			{
				for (int col = minCol; col <= maxCol; col++)
				{
					key = row * 10000 + col;
					m_unitRangeDict[key] = true;
				}
			}

			return pos;
		}

		/// <summary>
		/// 寻找一个可以放置trigger的位置
		/// </summary>
		public Vector2Int FindFreeTileForTrigger()
		{
			return FindFreeTileNotInDict(m_unitPosDict);
		}

		/// <summary>
		/// 寻找一个可以放置monster的位置
		/// </summary>
		public Vector2Int FindFreeTileForMonster()
		{
			return FindFreeTileNotInDict(m_unitRangeDict);
		}

		//寻找tile点附近的可以放置怪物的坐标
		private Vector2Int FindFreeTileNear(Vector2Int pos)
		{
			int col, row, key;
			bool block;
			for (int i = 1; i < 4; ++i)
			{
				row = pos.y;

				col = pos.x - i;
				key = 10000 * row + col;
				block = m_unitPosDict.ContainsKey(key);
				if (!block) return new Vector2Int(col, row);

				col = pos.x + i;
				key = 10000 * row + col;
				block = m_unitPosDict.ContainsKey(key);
				if (!block) return new Vector2Int(col, row);


				col = pos.x;

				row = pos.y - i;
				key = 10000 * row + col;
				block = m_unitPosDict.ContainsKey(key);
				if (!block) return new Vector2Int(col, row);


				row = pos.x + i;
				key = 10000 * row + col;
				block = m_unitPosDict.ContainsKey(key);
				if (!block) return new Vector2Int(col, row);
			}


			return FindFreeTileForTrigger();
		}

		//创建, 地图中可以空置的位置
		private Vector2Int FindFreeTileNotInDict(Dictionary<int, bool> dict)
		{
			Vector2Int pos = CMapUtil.FindRandomNodeLocation(m_numRows, m_numCols);
			int key = 10000 * pos.y + pos.x;

			bool block = dict.ContainsKey(key);
			int maxTryTimes = 100;
			int tryTimes = 0;
			while (block && tryTimes < maxTryTimes)
			{
				pos = CMapUtil.FindRandomNodeLocation(m_numRows, m_numCols);
				key = 10000 * pos.y + pos.x;
				block = dict.ContainsKey(key);
				tryTimes++;
			}

			if (tryTimes == maxTryTimes)
			{
				Debug.LogError("check forest num, we can not find free tile in walkable grid!");
				return CDarkConst.INVALID_VEC2INT;
			}

			return pos;
		}

		/*清理辅助数据, 只在创建时做辅助用*/
		public void Clear()
		{
			m_unitRangeDict.Clear();
			m_unitPosDict.Clear();

			m_unitRangeDict = null;
			m_unitPosDict = null;
		}
	}
}