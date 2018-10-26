using System;
namespace DarkRoom.Core
{
	/// <summary>
	/// 自定义的全局random,用于框架内部, GamePlay尽量不用
	/// </summary>
	public class CDarkRandom {
		private static Random m_rand = new Random();

		/// <summary>
		/// 重新设置种子
		/// </summary>
		/// <param name="seed"></param>
		public static void SetSeed(int seed)
		{
			m_rand = new Random(seed);
        }

		/// <summary>
		/// 下一个在min, max中间的随机数
		/// </summary>
		public static int Next(int min, int max) {
			return m_rand.Next(min, max);
		}

		/// <summary>
		/// 获取在min, max中间的float随机数
		/// </summary>
		public static float Next(float min, float max)
		{
			int n = (int) (min * 1000f);
			int x = (int)(max * 1000f);
			int r = Next(n, x);
			return r * 0.001f;
		}

		/// <summary>
		/// 下一个在0和max之间的随机数
		/// </summary>
		/// <param name="max">Max.</param>
		public static int Next(int max) {
			return m_rand.Next(max);
		}

		/// <summary>
		/// value between 0~1
		/// </summary>
		public static float Next() {
			int f = Next(0, 100);
			return 1.0f * f / 100.0f;
		}

		/// <summary>
		///  是否比value 小
		/// </summary>
		/// <param name="value">value between 0~1</param>
		public static bool SmallerThan(float value) {
			float noise = Next();
			return noise < value;
		}
	}
}