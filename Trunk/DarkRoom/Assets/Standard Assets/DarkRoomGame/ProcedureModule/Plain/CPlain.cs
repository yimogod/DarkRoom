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
			private int m_numCols;
			private int m_numRows;

			//public int NumCols { get { return m_numCols; } }
			//public int NumRows { get { return m_numRows; } }

			public PerlinMap(float[,] map) {
				m_map = map;
				m_numCols = m_map.GetLength(0);
				m_numRows = m_map.GetLength(1);
			}

			/// <summary>
			/// 坐标是否在地图里面
			/// </summary>
			public bool InMapRange(int x, int z) {
				return x >= 0 && x < m_numCols && z >= 0 && z < m_numRows;
			}

			public float this[int x, int z] {
				get { return m_map[x, z]; }
				set { m_map[x, z] = value; }
			}
		}
	}
}
