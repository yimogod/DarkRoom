﻿using UnityEngine;

namespace DarkRoom.PCG{
	/// 注意这里仅仅处理数据

	/* 用细胞自动机来产生是否可以通行.然后不可通行的都种上树
	 * 可以通行的试试其他地形
	 * 森林地形,
	 * 1.由不同深度颜色的绿地组成, 由细胞自动机生成基本地形
	 * 2.然后有池塘或者湖泊--根据配置和随机数来确定添加以及添加的个数
	 * 3.会有小木屋(仅作装饰)--会有道路连接到--传送点的木屋会由trigger system来生成
	 * 4.中间穿插道路
	 * 5.会有树丛
	 * 6.地上种些小花--附着物--附着物需要在所有的地形产生完毕后, 在空白的区域点缀些装饰
	 * 
	 * 这意味着, 我们会有1种基本地形(浅绿), 1中辅助地形(深绿), 1种特殊地形(湖泊)
	 * 多种小花--不同小花可能会对应不同的地形--有的在陆地, 有的在水里
	 * 
	 * 另外我们的地图现在都是3d的. 理论上都64x64起
	 * 为什么要强调64x64, 因为如果地图小于64的话, 细胞自动机随机的不够, 不能够产生合理数据
	 * 如果地图小于64, 需要在64上随机, 然后采样成小地图
	*/
	[RequireComponent(typeof(CPerlinNoise))]
	public class CPlainTerrainGenerator : MonoBehaviour {
		/// <summary>
		/// 地图宽度
		/// </summary>
		public int Width;

		/// <summary>
		/// 地图高度
		/// </summary>
		public int Height;

		private CPlain.PerlinMap m_map;

		//柏林模糊
		private CPerlinNoise m_perlin;

		private CPlainRoadGenerator m_road;

		/// <summary>
		/// 获取柏林模糊产生的地图
		/// </summary>
		public CPlain.PerlinMap Map { get { return m_map; } }

		void Start()
		{
			m_perlin = gameObject.GetComponent<CPerlinNoise>();
			m_road = new CPlainRoadGenerator();
        }

		/// <summary>
		/// 柏林噪声产生地形数据
		/// </summary>
		public void Generate()
		{
			Generate(Width, Height);
		}

		/// <summary>
		/// 柏林噪声产生地形数据
		/// </summary>
		public void Generate(int width, int height)
		{
			Width = width;
			Height = height;

			float[,] map = m_perlin.GetNoiseValues(Width, Height);
			m_map = new CPlain.PerlinMap(map);
		}
	}
}