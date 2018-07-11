using UnityEngine;

namespace DarkRoom.PCG
{
	public class CPerlinNoise
	{
		/// <summary>
		/// 种子
		/// </summary>
		public int Seed;

		/// <summary>
		/// 是否随机种子
		/// </summary>
		public bool RandomSeed = true;

		/// <summary>
		/// 采样次数, 一般4~8
		/// </summary>
		[Range(4, 8)]
		public int Octaves = 4;

		/// <summary>
		/// 频率, 大于10就能产生好看的地图
		/// </summary>
		[Range(10f, 99f)]
		public float Frequency = 12f;

		/// <summary>
		/// 波幅, 测试从2~9999都没问题
		/// </summary>
		[Range(2f, 99f)]
		public float Amptitude = 4f;

		/// <summary>
		/// 缺项(0~1), 经过测试, 这个值取0.01~0.99都会产生很漂亮的地图
		/// </summary>
		[Range(0f, 1f)]
		public float Lacunarity = 0.21f;

		/// <summary>
		/// 持久度 (0~1), 经过测试, 这个值取0.01~0.99都会产生很漂亮的地图
		/// </summary>
		[Range(0f, 1f)]
		public float Persistance = 0.69f;

		/// <summary>
		/// 获取柏林模糊的二维地图, 格子数据归一化
		/// </summary>
		public float[,] GetNoiseValues(int cols, int rows)
		{
			if (RandomSeed)
				Seed = Random.Range(-10000, 10000);

			float w = (float) cols;
			float h = (float) rows;

			float[,] values = new float[cols, rows];

			float min = float.MaxValue;
			float max = float.MinValue;
			for (int i = 0; i < cols; i++) {
				for (int j = 0; j < rows; j++) {
					float r = 0f;
					float ta = Amptitude;
					float tf = Frequency;

					for (int k = 0; k < Octaves; k++) {
						float x = (i + Seed) / w * tf;
						float y = j / h * tf;
						r += Mathf.PerlinNoise(x, y) * ta;

						ta *= Lacunarity;
						tf *= Persistance;
					}

					values[i, j] = r;
					if (min > r) min = r;
					if (max < r) max = r;
				}
			}

			//归一化
			for (int i = 0; i < cols; i++) {
				for (int j = 0; j < rows; j++) {
					values[i, j] = Mathf.InverseLerp(min, max, values[i, j]);
				}
			}

			return values;
		}
	}
}