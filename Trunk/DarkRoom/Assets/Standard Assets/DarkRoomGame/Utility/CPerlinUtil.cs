using System;
using System.Collections.Generic;

namespace DarkRoom.Game
{
	public class CPerlinUtil
	{
		private static Random s_perlinRandom = new Random();

		/// <summary>
		/// 产生一个根据柏林噪声生成的参数
		/// </summary>
		/// <returns>The perlin value noise.</returns>
		public static float NextPerlinValueNoise(int col, int row, float div)
		{
			int xf = s_perlinRandom.Next(0, 100);
			int yf = s_perlinRandom.Next(0, 100);
			float noise = UnityEngine.Mathf.PerlinNoise((col + xf) / div, (row + yf) / div);
			return noise;
		}
	}
}
