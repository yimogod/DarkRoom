using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.PCG {
	public class CCellularAutomaton : MonoBehaviour
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
		[Range(0.25f, 0.48f)]
		public float Threshold = 0.44f;

		/// <summary>
		/// 整个地图的边缘需要是活着的
		/// 这个是算法的本身造成的, youtube的例子也是这样
		/// </summary>
		private bool m_needAliveEdage = true;

		private int m_iterations = 5;

		//x, y
		private int[,] m_values;
        private int m_width;
		private int m_height;

		/// <summary>
		/// 返回二维数组[y,x], 0代表死亡
		/// </summary>
		public int[,] GenerateTerrianWithCellular(int width, int height)
		{
			m_width = width;
			m_height = height;
			m_values = new int[m_width, m_height];

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
			for(int y = 0; y < m_height; y++){
				for(int x = 0; x < m_width; x++){
					int num = FindSurroundAliveNum(x, y);
					if(num < 4) {
						m_values[x, y] = 0;
					}else if(num > 4){
						m_values[x, y] = 1;
					}
				}
			}
		}

		/*寻找9宫格或者的个数*/
		private int FindSurroundAliveNum(int tileX, int tileY){
			int leftX = tileX - 1;
			int rightX = tileX + 1;
			int downY = tileY - 1;
			int upY = tileY + 1;

			int num = 0;
			for (int x = leftX; x <= rightX; x ++){
				for (int y = downY; y <= upY; y ++) {
					//格子不合法就下一个
					bool b = IsCoordValid(x, y);
					//如果在边缘, 则算它生存条件良好
					if (!b) {
						num += 1;
                        continue;
					}
					
					if(m_values[x, y] == 1) {
						num += 1;
					}
				}
			}

			return num;
		}

		//坐标是否合法
		private bool IsCoordValid(int x, int y) {
			return x >= 0 && x < m_width && y >= 0 && y < m_height;
		}

		//随机填充地图
		void RandomFillMap() {
			for (int x = 0; x < m_width; x++) {
				for (int y = 0; y < m_height; y++) {
					bool b = CDarkRandom.SmallerThan(Threshold);
					m_values[x, y] = b ? 1 : 0;
				}
			}

			if (m_needAliveEdage)
			{
				//如果是整地图, 则我们需要在四周筑上围墙
				//随机填充满所有的地图
				int maxIndex_Y = m_height - 1;
				int maxIndex_X = m_width - 1;

				for (int x = 0; x < m_width; x++)
				{
					m_values[x, 0] = 1;
					m_values[x, maxIndex_Y] = 1;
				}

				for (int y = 0; y < m_height; y++)
				{
					m_values[0, y] = 1;
					m_values[maxIndex_X, y] = 1;
				}
			}
		}

		//---finished
	}
}