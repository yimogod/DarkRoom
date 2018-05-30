using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.PCG
{
	/// <summary>
	/// 平原产生河流和道路的类
	/// </summary>
	public class CPlainRoadGenerator {
		/// <summary>
		/// 最简单的产生路径的方法. 不考虑地图的实际情况
		/// 请保证xy1 和 xy2 的距离足够远. 至少得有6 * 5个格子以上吧
		/// 返回的列表是经过排序的
		/// </summary>
		public List<Vector2> GetWayPoints(int width, int height, int x1, int y1, int x2, int y2)
		{
			CIntLine line = new CIntLine(new Vector2(x1, y1), new Vector2(x2, y2));

			int step = 6;
			int maxLimit = 10;
			List<Vector2> list = new List<Vector2>();
			
            int dx = Mathf.Abs(x1 - x2);
			int dy = Mathf.Abs(y1 - y2);

			int x, y, delta, intX, intY, min, max;
			Vector2 start, end;

			if (dx > dy) {
				if (x1 < x2) {
					start = new Vector2(x1, y1);
					end = new Vector2(x2, y2);
					min = x1;
					max = x2;
				} else {
					end = new Vector2(x1, y1);
					start = new Vector2(x2, y2);
					max = x1;
					min = x2;
				}

				list.Add(start);
				for (x = min; x < max; x += step) {
					y = Mathf.RoundToInt(height * CDarkRandom.NextPerlinValueNoise(width, height, 10.0f));
					intY = line.GetY(x);
					delta = intY - y;
					if (delta > maxLimit) {
						y = intY + maxLimit;
					} else if (delta < -maxLimit) {
						y = intY - maxLimit;
					}

					Vector2 v = new Vector2(x, y);
					MakeTileInMap(width, height, ref v);
					list.Add(v);
				}
				list.Add(end);

			} else {
				if (y1 < y2) {
					start = new Vector2(x1, y1);
					end = new Vector2(x2, y2);
					min = y1;
					max = y2;
				} else {
					end = new Vector2(x1, y1);
					start = new Vector2(x2, y2);
					max = y1;
					min = y2;
				}
				list.Add(start);

				for (y = min; y < max; y += step) {
					x = Mathf.RoundToInt(width * CDarkRandom.NextPerlinValueNoise(width, height, 10.0f));
					intX = line.GetX(y);
					delta = intX - x;
					if (delta > maxLimit) {
						x = intX + maxLimit;
					} else if (delta < -maxLimit) {
						x = intX - maxLimit;
					}

					Vector2 v = new Vector2(x, y);
					MakeTileInMap(width, height, ref v);
					list.Add(v);
				}

				list.Add(end);
			}

			return list;
		}

		private void MakeTileInMap(int width, int height, ref Vector2 v)
		{
			if (v.x < 0) v.x = 0;
			if (v.y < 0) v.y = 0;
			if (v.x > (width - 1)) v.x = width - 1;
			if (v.y > (height - 1)) v.y = height - 1;
		}
	}
}