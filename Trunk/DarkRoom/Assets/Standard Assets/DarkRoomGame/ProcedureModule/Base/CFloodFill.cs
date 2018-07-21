using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.PCG {
	/// <summary>
	/// 冲击洪水的方法填充封闭区域
	/// </summary>
	public class CFloodFill
	{
		//洪水要冲击的地图
		private CCellularGrid m_map;

		//所有的连续区域的列表. 区域的格子的值要等于 tileType
		//List<Cave.Coord>代表一个封闭区域
		public List<List<Vector2Int>> GetRegions(CCellularGrid map, int tileType)
		{
			m_map = map;

			List<List<Vector2Int>> regions = new List<List<Vector2Int>>();
			int[,] mapFlags = new int[m_map.NumCols, m_map.NumRows];

			for (int x = 0; x < m_map.NumCols; x++) {
				for (int y = 0; y < m_map.NumRows; y++) {
					//本格子已经处理过了
					if (mapFlags[x, y] != 0)continue;
					//本格子不等于要处理的类型
					if (m_map[x, y] != tileType)continue;

					//获取本格子所在的封闭区域
					List<Vector2Int> newRegion = GetRegionTiles(x, y);
					regions.Add(newRegion);

					//标记已经处理过的格子
					foreach (Vector2Int tile in newRegion) {
						mapFlags[tile.x, tile.y] = 1;
					}
				}
			}

			return regions;
		}

		//获取周围和startX, startY值一样的所有的tile
		//floodfill算法
		private List<Vector2Int> GetRegionTiles(int startX, int startY) {
			List<Vector2Int> tiles = new List<Vector2Int>();
			int[,] mapFlags = new int[m_map.NumCols, m_map.NumRows];
			int tileType = m_map[startX, startY];

			Queue<Vector2Int> queue = new Queue<Vector2Int>();
			queue.Enqueue(new Vector2Int(startX, startY));
			//传入的格子处理过了
			mapFlags[startX, startY] = 1;

			while (queue.Count > 0) {
			    Vector2Int tile = queue.Dequeue();
				tiles.Add(tile);

				//当前格子的上下左右, 开始漫延
				for (int x = tile.x - 1; x <= tile.x + 1; x++) {
					for (int y = tile.y - 1; y <= tile.y + 1; y++) {
						if (!m_map.InMapRange(x, y))continue;

						//仅在正方向上判断
						if (y == tile.y || x == tile.x) {
							//已经处理过了
							if (mapFlags[x, y] != 0)continue;
							if (m_map[x, y] != tileType)continue;
							
							queue.Enqueue(new Vector2Int(x, y));
							mapFlags[x, y] = 1;
						}
					}
				}
			}
			return tiles;
		}

		
	}
}
