﻿using System;
using System.Collections.Generic;

namespace DarkRoom.PCG {
	/// <summary>
	/// 冲击洪水的方法填充封闭区域
	/// </summary>
	public class CFloodFill
	{
		//洪水要冲击的地图
		private CCave.CellularMap m_map;

		//所有的连续区域的列表. 区域的格子的值要等于 tileType
		//List<Cave.Coord>代表一个封闭区域
		public List<List<CCave.Coord>> GetRegions(CCave.CellularMap map, int tileType)
		{
			m_map = map;

			List<List<CCave.Coord>> regions = new List<List<CCave.Coord>>();
			int[,] mapFlags = new int[m_map.Width, m_map.Height];

			for (int x = 0; x < m_map.Width; x++) {
				for (int y = 0; y < m_map.Height; y++) {
					//本格子已经处理过了
					if (mapFlags[x, y] != 0)continue;
					//本格子不等于要处理的类型
					if (m_map[x, y] != tileType)continue;

					//获取本格子所在的封闭区域
					List<CCave.Coord> newRegion = GetRegionTiles(x, y);
					regions.Add(newRegion);

					//标记已经处理过的格子
					foreach (CCave.Coord tile in newRegion) {
						mapFlags[tile.tileX, tile.tileZ] = 1;
					}
				}
			}

			return regions;
		}

		//获取周围和startX, startY值一样的所有的tile
		//floodfill算法
		private List<CCave.Coord> GetRegionTiles(int startX, int startY) {
			List<CCave.Coord> tiles = new List<CCave.Coord>();
			int[,] mapFlags = new int[m_map.Width, m_map.Height];
			int tileType = m_map[startX, startY];

			Queue<CCave.Coord> queue = new Queue<CCave.Coord>();
			queue.Enqueue(new CCave.Coord(startX, startY));
			//传入的格子处理过了
			mapFlags[startX, startY] = 1;

			while (queue.Count > 0) {
			    CCave.Coord tile = queue.Dequeue();
				tiles.Add(tile);

				//当前格子的上下左右, 开始漫延
				for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
					for (int y = tile.tileZ - 1; y <= tile.tileZ + 1; y++) {
						if (!m_map.InMapRange(x, y))continue;

						//仅在正方向上判断
						if (y == tile.tileZ || x == tile.tileX) {
							//已经处理过了
							if (mapFlags[x, y] != 0)continue;
							if (m_map[x, y] != tileType)continue;
							
							queue.Enqueue(new CCave.Coord(x, y));
							mapFlags[x, y] = 1;
						}
					}
				}
			}
			return tiles;
		}

		
	}
}
