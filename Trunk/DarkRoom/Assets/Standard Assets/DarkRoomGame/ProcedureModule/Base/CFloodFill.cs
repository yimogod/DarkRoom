using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.PCG {
	/// <summary>
	/// 冲击洪水的方法填充封闭区域
	/// </summary>
	public class CFloodFill<T>
	{
		//洪水要冲击的地图
		private T[,] m_map;
        private int m_cols;
        private int m_rows;

        /// <summary>
        /// 关闭太小的格子
        /// 返回大区域的列表
        /// </summary>
        /// <param name="map">Map.</param>
        /// <param name="threshold">大小的阀值</param>
        /// <param name="little">小区域的格子的值</param>
        /// <param name="large">抹去的小区域的赋值</param>
        public List<List<Vector2Int>> Process(T[,] map, int threshold, T little, T large)
        {
            m_map = map;
            m_cols = map.GetLength(0);
            m_rows = map.GetLength(1);

            List<List<Vector2Int>> result = new List<List<Vector2Int>>();
            List<List<Vector2Int>> roomRegions = GetRegions(little);
            foreach (List<Vector2Int> roomRegion in roomRegions)
            {
                //如果区域太小, 就抹去
                if (roomRegion.Count < threshold)
                {
                    //Debug.LogWarning("------ have little roorm " + large.ToString());
                    foreach (Vector2Int tile in roomRegion)
                    {
                        //Debug.LogWarning("set value " + tile.ToString());
                        m_map[tile.x, tile.y] = large;
                    }
                    continue;
                }
                result.Add(roomRegion);
            }

            return result;
        }

        //所有的连续区域的列表. 区域的格子的值要等于 tileType
        private List<List<Vector2Int>> GetRegions(T tileType)
		{

			List<List<Vector2Int>> regions = new List<List<Vector2Int>>();
            int[,] mapFlags = new int[m_cols, m_rows];

			for (int x = 0; x < m_cols; x++) {
				for (int y = 0; y < m_rows; y++) {
					//本格子已经处理过了
					if (mapFlags[x, y] != 0)continue;
					//本格子不等于要处理的类型
                    if (!m_map[x, y].Equals(tileType))continue;

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
            int[,] mapFlags = new int[m_cols, m_rows];
            T tileType = m_map[startX, startY];

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
						if (!TileInMapRange(x, y))continue;

						//仅在正方向上判断
						if (y == tile.y || x == tile.x) {
							//已经处理过了
							if (mapFlags[x, y] != 0)continue;
                            if (!m_map[x, y].Equals(tileType))continue;
							
							queue.Enqueue(new Vector2Int(x, y));
							mapFlags[x, y] = 1;
						}
					}
				}
			}
			return tiles;
		}

        private bool TileInMapRange(int col , int row){
            if (col < 0 || row < 0) return false;
            if (col >= m_cols) return false;
            if (row >= m_rows) return false;

            return true;
        }
		
	}
}
