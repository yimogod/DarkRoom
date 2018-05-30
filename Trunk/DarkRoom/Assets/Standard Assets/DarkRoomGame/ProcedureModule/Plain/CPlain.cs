using System;
using System.Collections.Generic;

namespace DarkRoom.PCG {
	public class CPlain {

		/// <summary>
		/// 柏林模糊生成的地图
		/// </summary>
		public class PerlinMap {
			//存储着柏林模糊生成的数据 [x, z]
			private float[,] m_map;
			private int m_width;
			private int m_height;

			public int Width { get { return m_width; } }
			public int Height { get { return m_height; } }

			public PerlinMap(float[,] map) {
				m_map = map;
				m_width = m_map.GetLength(0);
				m_height = m_map.GetLength(1);
			}

			/// <summary>
			/// 坐标是否在地图里面
			/// </summary>
			public bool InMapRange(int x, int z) {
				return x >= 0 && x < m_width && z >= 0 && z < m_height;
			}

			public float this[int x, int z] {
				get { return m_map[x, z]; }
				set { m_map[x, z] = value; }
			}
		}
	}
}
