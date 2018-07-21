using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.PCG {
	[RequireComponent(typeof(CForestGenerator_Terrain))]
	public class CLandScapeGenerator : CTileMapGeneratorBase {
		public float SeaLevel = 0.2f;
		public float BeachHeight = 0.22f;
		public float GrassHeight = 0.3f;
		public float GrassHeight2 = 0.35f;
		public float LandHeight = 0.52f;
		public float LandHeight2 = 0.8f;
		public float StoneHeight = 1f;

        /// <summary>
        /// 从外面指定的, terrain类型的值
        /// </summary>
	    public int TerrainType = 1;

	    /// <summary>
	    /// 从外面指定的, 水域类型的值
	    /// </summary>
	    public int SeaType = 2;

        private CForestGenerator_Terrain m_terrain;

		void Awake()
		{
		    m_maxAssetsNum = 11;
		    m_walkableList = new bool[m_maxAssetsNum];
            m_assetList = new string[m_maxAssetsNum];

		    m_terrain = new CForestGenerator_Terrain();
		}

        /// <summary>
        /// 根据高度, 从配置中读取相关的asset
        /// </summary>
		private int GetTypeAtHeight(float height) {
            //两种海洋
			if (height < SeaLevel){
				if(CDarkRandom.SmallerThan(0.5f))return 0;
				return 1;
			}

            //两种海岸线
            if (height <= BeachHeight){
				if(CDarkRandom.SmallerThan(0.5f))return 2;
				return 3;
			}

            //两种草
			if (height <= GrassHeight){
				if(CDarkRandom.SmallerThan(0.5f))return 4;
				return 5;
			}

            //另外一种草
			if (height <= GrassHeight2)return 6;

            //两种地面
			if (height <= LandHeight)return 7;
			if (height <= LandHeight2)return 8;

            //两种石头
			if (height <= StoneHeight){
				if(CDarkRandom.SmallerThan(0.5f))return 9;
				return 10;
			}

            //默认的绿草地
			return 4;
		}

	    private int GetTypeByIndex(int index)
	    {
	        int v = -1;
	        switch (index)
	        {
                case 0:
	            case 1:
	            case 2:
	            case 3:
                    v = SeaType;
	                break;
	            case 4:
	            case 5:
	            case 6:
	            case 7:
	            case 8:
	            case 9:
	            case 10:
	                v = TerrainType;
	                break;
            }

	        return v;
	    }

		public override void Generate()
		{
			base.Generate();
			m_terrain.Generate(m_numCols, m_numRows);

			/*CPerlinMap perlin = m_terrain.PerlinMap;
			for (int x = 0; x < m_numCols; x++) {
				for (int z = 0; z < m_numRows; z++) {
					int index = GetTypeAtHeight(perlin[x, z]);
				    int type = GetTypeByIndex(index);
                    m_grid.FillData(x, z, type, GetAsset(index), GetAssetWalkable(index));
                }
			}*/
		}
	}
}
