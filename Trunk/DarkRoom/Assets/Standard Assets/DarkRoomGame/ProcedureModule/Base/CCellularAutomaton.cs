using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.PCG {
	public class CCellularAutomaton
	{
		/// <summary>
		/// 随机数种子
		/// </summary>
		public int Seed;

		/// <summary>
		/// 是否随机种子
		/// </summary>
		public bool RandomSeed;

		/// <summary>
		/// 在初始化时, 小于这个值的格子初始位为活着, 即格子的值是1
		/// </summary>
		[Range(0.43f, 0.48f)]
		public float Threshold = 0.44f;

		/// <summary>
		/// 整个地图的边缘需要是活着的
		/// 这个是算法的本身造成的, youtube的例子也是这样
		/// </summary>
		private bool m_needAliveEdage = true;

		private int m_iterations = 5;

        private bool m_edgeAlive = true;

        //x, y
        private int[,] m_values;
        private int m_numCols;
		private int m_numRows;

        /// <summary>
        /// 细胞自动机, 0是死亡 1是活着
        /// </summary>
        public CCellularAutomaton()
        {
        }

        /// <summary>
        /// 处于边缘的是否活着
        /// </summary>
        public CCellularAutomaton(bool edgeAlive)
        {
            m_edgeAlive = edgeAlive;
        }
        
        /// <summary>
        /// 对传入的数组做一次平滑处理
        /// </summary>
	    public void SmoothOnce(int[,] values)
	    {
	        m_values = values;
	        m_numCols = m_values.GetLength(0);
	        m_numRows = m_values.GetLength(1);
	        MoreIsBetterThanLess();
        }

        /// <summary>
        /// 返回二维数组[y,x], 0代表死亡
        /// </summary>
        public int[,] Generate(int cols, int rows)
		{
			m_numCols = cols;
			m_numRows = rows;
			m_values = new int[m_numCols, m_numRows];

			if (RandomSeed)Seed = Random.Range(-10000, 1000);
			CDarkRandom.SetSeed(Seed);

			RandomFillMap();

			for (int i = 0; i < m_iterations; ++i){
				MoreIsBetterThanLess();
			}

			return m_values;
		}

		/*少数服从多数*/
		private void MoreIsBetterThanLess() {
            //平滑池塘
            for (int y = 0; y < m_numRows; y++)
            {
                for (int x = 0; x < m_numCols; x++)
                {
                    int cell = 0;
                    cell += HasPondTileConnected(x + 1, y);
                    cell += HasPondTileConnected(x + 1, y + 1);
                    cell += HasPondTileConnected(x, y + 1);
                    cell += HasPondTileConnected(x - 1, y + 1);
                    cell += HasPondTileConnected(x - 1, y);
                    cell += HasPondTileConnected(x - 1, y - 1);
                    cell += HasPondTileConnected(x, y - 1);
                    cell += HasPondTileConnected(x + 1, y - 1);

                    if (cell < 4){
                        m_values[x, y] = 0;
                    }else if (cell > 4){
                        m_values[x, y] = 1;
                    }
                }
            }


		}


        //四周是不是有邻居
        private int HasPondTileConnected(int x, int y)
        {
            int edgeValue = m_edgeAlive ? 1 : 0;
            if (x <= 0 || x >= (m_numCols - 1)) return edgeValue;
            if (y <= 0 || y >= (m_numRows - 1)) return edgeValue;

            float v = m_values[x, y];
            return v > 0 ? 1 : 0;
        }


        //随机填充地图
	    private void RandomFillMap() {
			for (int x = 0; x < m_numCols; x++) {
				for (int y = 0; y < m_numRows; y++) {
					bool b = CDarkRandom.SmallerThan(Threshold);
					m_values[x, y] = b ? 1 : 0;
				}
			}

			if (m_needAliveEdage)
			{
				//如果是整地图, 则我们需要在四周筑上围墙
				//随机填充满所有的地图
				int maxIndex_Y = m_numRows - 1;
				int maxIndex_X = m_numCols - 1;

				for (int x = 0; x < m_numCols; x++)
				{
					m_values[x, 0] = 1;
					m_values[x, maxIndex_Y] = 1;
				}

				for (int y = 0; y < m_numRows; y++)
				{
					m_values[0, y] = 1;
					m_values[maxIndex_X, y] = 1;
				}
			}
		}

		//---finished
	}
}