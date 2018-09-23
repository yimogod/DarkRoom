using System;
using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.PCG
{
	/* 森林地形 注意这里仅仅处理数据
	   * 1.由不同深度颜色的绿地组成, 柏林模糊生成基本地形
	   * 2.然后有池塘或者湖泊--根据配置和随机数来确定添加以及添加的个数
	   * 3.会有小木屋(仅作装饰)--会有道路连接到--传送点的木屋会由trigger system来生成
	   * 4.中间穿插道路
	   * 5.会有树丛
	   * 6.地上种些小花--附着物--附着物需要在所有的地形产生完毕后, 在空白的区域点缀些装饰
	   * 
	   * 这意味着, 我们会有1种基本地形(浅绿), 1中辅助地形(深绿), 1种特殊地形(湖泊)
	   * 多种小花--不同小花可能会对应不同的地形--有的在陆地, 有的在水里
	   *
	 * 下面其实的没用. 因为我们用柏林模糊代替了细胞自动机
	   * 另外我们的地图现在都是3d的. 理论上都64x64起
	   * 为什么要强调64x64, 因为如果地图小于64的话, 细胞自动机随机的不够, 不能够产生合理数据
	   * 如果地图小于64, 需要在64上随机, 然后采样成小地图
	*/
	public class CForestGenerator
	{
		/// <summary>
		/// 基础地形的数据
		/// </summary>
		public CAssetGrid TerrainGrid => m_terrain.Grid;

        /// <summary>
        /// 障碍物数据
        /// </summary>
        public CAssetGrid TreeGrid => m_tree.Grid;

		private CForestGenerator_Terrain m_terrain;
        private CForestGenerator_Tree m_tree;
        private CForestGenerator_Room m_room;

		public CForestGenerator()
		{
			m_terrain = new CForestGenerator_Terrain();
			m_room = new CForestGenerator_Room();
            m_tree = new CForestGenerator_Tree();
        }

		public void Generate()
		{
			m_terrain.Generate(64, 64);
			m_room.Generate(64, 64);
            m_tree.Generate(64, 64);

            m_terrain.GenerateRoad();

            m_tree.DeleteTreeAtIllegalPostion(m_terrain.Grid);
        }
	}
}