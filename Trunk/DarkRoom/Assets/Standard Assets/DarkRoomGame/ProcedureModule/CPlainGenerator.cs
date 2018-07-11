using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.PCG {
	[RequireComponent(typeof(CPlainTerrainGenerator))]
	public class CPlainGenerator : CTileMapGeneratorBase {
		public float SeaLevel = 0.2f;
		public float BeachHeight = 0.22f;
		public float GrassHeight = 0.3f;
		public float GrassHeight2 = 0.35f;
		public float LandHeight = 0.52f;
		public float LandHeight2 = 0.8f;
		public float StoneHeight = 1f;

        /// <summary>
        /// 存储的asset列表
        /// 默认情况下
        /// 0, 1代表两种海的颜色
        /// 2, 3代表海岸线
        /// 4, 5, 6代表草地, 其中4代表默认的绿地
        /// 7, 8 两种地面
        /// 9, 10两种石头地面
        /// </summary>
		public List<string> AssetList;

        /// <summary>
        /// 从外面指定的, terrain类型的值
        /// </summary>
	    public int TerrainType = 1;

		private CPlainTerrainGenerator m_terrain;

		void Awake()
		{
			m_terrain = gameObject.GetComponent<CPlainTerrainGenerator>();
		}

        /// <summary>
        /// 根据高度, 从配置中读取相关的asset
        /// </summary>
		private string GetAssetAtHeight(float height) {
            //两种海洋
			if (height < SeaLevel){
				if(CDarkRandom.SmallerThan(0.5f))
					return AssetList[0];
				return AssetList[1];
			}

            //两种海岸线
            if (height <= BeachHeight){
				if(CDarkRandom.SmallerThan(0.5f))
					return AssetList[2];
				return AssetList[3];
			}

            //两种草
			if (height <= GrassHeight){
				if(CDarkRandom.SmallerThan(0.5f))
					return AssetList[4];
				return AssetList[5];
			}

            //另外一种草
			if (height <= GrassHeight2)
				return AssetList[6];

            //两种地面
			if (height <= LandHeight)
				return AssetList[7];
			if (height <= LandHeight2)
				return AssetList[8];

            //两种石头
			if (height <= StoneHeight){
				if(CDarkRandom.SmallerThan(0.5f))
					return AssetList[9];
				return AssetList[10];
			}

            //默认的绿草地
			return AssetList[4];
		}

		public override void Generate()
		{
			base.Generate();
			m_terrain.Generate(m_numCols, m_numRows);

			CPlain.PerlinMap perlin = m_terrain.Map;
			for (int x = 0; x < m_numCols; x++) {
				for (int z = 0; z < m_numRows; z++) {
					string asset = GetAssetAtHeight(perlin[x, z]);
					m_grid.SetTypeAndAsset(x, z, TerrainType, asset);
                }
			}
		}
	}
}
