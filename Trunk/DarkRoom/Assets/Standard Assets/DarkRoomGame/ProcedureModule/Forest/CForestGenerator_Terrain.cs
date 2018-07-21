using System;
using DarkRoom.Core;
using DarkRoom.Game;
using Rayman;
using UnityEngine;

namespace DarkRoom.PCG{
	
    /// <summary>
    /// 产生terrain的数据, 包含路和池塘
    /// 
    /// 关于terrain的资源
    /// 0, 1两种草地
    /// 2, 3两种土路
    /// 4 凸起
    /// </summary>
	public class CForestGenerator_Terrain : CTileMapGeneratorBase
    {
	    /// <summary>
	    /// 四种地面的基础材质, 看情况如果太多,会适当减少种类
	    /// </summary>
	    public float GrassHeight = 0.24f;
	    public float GrassHeight2 = 0.35f;
	    public float LandHeight = 0.52f;
	    public float LandHeight2 = 0.8f;
	    /// <summary>
	    /// 指代地图凸起
	    /// </summary>
	    public float WallHeight = 1f;

	    /// <summary>
	    /// 从外面指定的, terrain平坦的类型的值
	    /// </summary>
	    public int TerrainType_Floor = 1;

	    /// <summary>
	    /// 从外面指定的, terrain凸起类型的值
	    /// </summary>
	    public int TerrainType_Wall = 2;

        /// <summary>
        /// pond对应的格子的类型
        /// </summary>
        public int TerrainType_Pond = 3;

        /// <summary>
        /// 创建池塘的个数
        /// </summary>
        public int MaxPondNum = 1;

        /// <summary>
        /// 柏林噪声产生地形数据
        /// </summary>
        public void Generate(int cols, int rows)
		{
		    m_maxAssetsNum = 5;
		    m_walkableList = new bool[m_maxAssetsNum];
		    m_assetList = new string[m_maxAssetsNum];

            m_grid.Init(cols, rows);

		    GenerateTerrain();
		    GeneratePond();
            GenerateRoad();
		}

        private void GenerateRoad()
        {

        }

        private void GeneratePond()
        {
            if(MaxPondNum <= 0)return;
            int num = CDarkRandom.Next(MaxPondNum + 1);
            if (num <= 0) return;

            for (int i = 0; i < num; i++)
            {
                PondGenerator p = new PondGenerator(m_numCols, m_numRows);
            }
        }

        private void GenerateTerrain()
        {
            var perlin = new CPerlinMap(m_numCols, m_numRows);
            perlin.Generate();

            for (int x = 0; x < m_numCols; x++)
            {
                for (int z = 0; z < m_numRows; z++)
                {
                    int index = GetTypeAtHeight(perlin[x, z]);
                    int type = GetTypeByIndex(index);

                    m_grid.FillData(x, z, type, GetAsset(index), GetAssetWalkable(index));
                }
            }
        }

	    /// <summary>
	    /// 根据高度, 从配置中读取相关的asset
	    /// </summary>
	    private int GetTypeAtHeight(float height)
	    {
	        //两种草
	        if (height <= GrassHeight)
	        {
	            if (CDarkRandom.SmallerThan(0.5f)) return 0;
	            return 0;
	        }

	        //另外一种草
	        if (height <= GrassHeight2) return 1;

	        //两种地面
	        if (height <= LandHeight) return 2;
	        if (height <= LandHeight2) return 3;

	        //墙壁
	        if (height <= WallHeight) return 4;

	        //默认的绿草地
	        return 0;
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
	                v = TerrainType_Floor;
	                break;
	            case 4:
	                v = TerrainType_Wall;
	                break;
	        }

	        return v;
	    }
    }
}