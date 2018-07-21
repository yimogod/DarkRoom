using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.Core
{ 
	/// <summary>
	/// 1为单位的直线
	/// </summary>
    public class CIntLine
	{
		//线段的两个端点
		private Vector2Int m_from, m_to;
		List<Vector2Int> m_line = new List<Vector2Int>();


		public CIntLine(Vector2Int from, Vector2Int to)
		{
			m_from = from;
			m_to = to;
		}

		/// <summary>
		/// 获取在线段上的所有tile
		/// </summary>
		public List<Vector2Int> GetLine() {
			if (m_line.Count > 0) {
				return m_line;
			}

			int x = (int)m_from.x;
			int y = (int)m_from.y;

			int dx = (int)(m_to.x - m_from.x);
			int dy = (int)(m_to.y - m_from.y);

			bool inverted = false;
			int step = Math.Sign(dx);
			int gradientStep = Math.Sign(dy);

			int longest = Mathf.Abs(dx);
			int shortest = Mathf.Abs(dy);

			if (longest < shortest) {
				inverted = true;
				longest = Mathf.Abs(dy);
				shortest = Mathf.Abs(dx);

				step = Math.Sign(dy);
				gradientStep = Math.Sign(dx);
			}

			int gradientAccumulation = longest / 2;
			for (int i = 0; i < longest; i++) {
				m_line.Add(new Vector2Int(x, y));

				if (inverted) {
					y += step;
				} else {
					x += step;
				}

				gradientAccumulation += shortest;
				if (gradientAccumulation >= longest) {
					if (inverted) {
						x += gradientStep;
					} else {
						y += gradientStep;
					}
					gradientAccumulation -= longest;
				}
			}

			return m_line;
		}

		/// <summary>
		/// 传入y获取x, 如果不在线段中则返回int.MinValue;
		/// </summary>
		public int GetX(int y)
		{
			GetLine();
            foreach (Vector2 v in m_line) {
				if (y == (int) v.y) return (int) v.x;
			}

			return int.MinValue;
		}

		/// <summary>
		/// 传入y获取x, 如果不在线段中则返回int.MinValue;
		/// </summary>
		public int GetY(int x) {
			GetLine();
			foreach (Vector2 v in m_line) {
				if (x == (int)v.x) return (int)v.y;
			}

			return int.MinValue;
		}
	}
}
