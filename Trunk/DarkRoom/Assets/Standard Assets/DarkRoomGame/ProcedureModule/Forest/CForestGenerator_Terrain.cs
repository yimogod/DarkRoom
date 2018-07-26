using System;
using DarkRoom.Core;
using DarkRoom.Game;
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
        /// 创建池塘的个数
        /// </summary>
        public int MaxPondNum = 1;

        public CForestGenerator_Terrain(){
        }

        /// <summary>
        /// 创建基础地形和河流
        /// </summary>
        public void Generate(int cols, int rows)
		{
            m_grid.Init(cols, rows);

		    GenerateTerrain();
		    GeneratePond();
            GenerateRoad();
		}

        /// <summary>
        /// 创建道路, 需要在创建房屋之后再调用
        /// </summary>
        public void GenerateRoad()
        {
        }

        private void GeneratePond()
        {
            if(MaxPondNum <= 0)return;
            int num = CDarkRandom.Next(MaxPondNum + 1);
            if (num <= 0) return;

            //池塘的相关配置
            var subType = ForestTerrainSubType.Pond;
            var type = (int) CProcedureLayer.Terrain;
            var walkable = GetSubTypeWalkable(subType);

            CPondGenerator p = new CPondGenerator();
            int pondCols = 32;
            int pondRows = 32;
            int leftCols = m_numCols - pondCols;
            int leftRows = m_numRows - pondRows;
            Vector2Int size = new Vector2Int(pondCols, pondRows);

            for (int i = 0; i < num; i++)
            {
                int startX = CDarkRandom.Next(pondCols, leftCols);
                int startZ = CDarkRandom.Next(pondRows, leftRows);
                var ponds = p.Generate(size);

                for (int x = 0; x < pondCols; x++)
                {
                    for (int z = 0; z < pondRows; z++)
                    {
                        if (ponds[x, z] < 0)continue;
                        m_grid.FillData(startX + x, startZ + z, type, (int)subType, walkable);
                    }
                }
            }
        }

        /// <summary>
        /// 柏林噪声产生地形数据, 
        /// </summary>
        private void GenerateTerrain()
        {
            var perlin = new CPerlinMap(m_numCols, m_numRows);
            perlin.Generate();

            var type = (int)CProcedureLayer.Terrain;
            for (int x = 0; x < m_numCols; x++)
            {
                for (int z = 0; z < m_numRows; z++)
                {
                    ForestTerrainSubType subType = GetAssetIndexAtHeight(perlin[x, z]);
                    m_grid.FillData(x, z, type, (int)subType, GetSubTypeWalkable(subType));
                }
            }
        }

	    /// <summary>
	    /// 根据高度, 从配置中读取相关的asset
	    /// </summary>
	    private ForestTerrainSubType GetAssetIndexAtHeight(float height)
	    {
	        //两种草
	        if (height <= GrassHeight)
	        {
	            if (CDarkRandom.SmallerThan(0.5f)) return ForestTerrainSubType.Grass1;
	            return ForestTerrainSubType.Grass1;
	        }

	        //另外一种草
	        if (height <= GrassHeight2) return ForestTerrainSubType.Grass2;

	        //两种地面
	        if (height <= LandHeight) return ForestTerrainSubType.Land1;
	        if (height <= LandHeight2) return ForestTerrainSubType.Land2;

	        //墙壁
	        if (height <= WallHeight) return ForestTerrainSubType.Hill;

	        //默认的绿草地
	        return ForestTerrainSubType.Grass1;
	    }

        private bool GetSubTypeWalkable(ForestTerrainSubType subType)
        {
            bool v = false;
            switch (subType)
            {
                case ForestTerrainSubType.Grass1:
                case ForestTerrainSubType.Grass2:
                case ForestTerrainSubType.Land1:
                case ForestTerrainSubType.Land2:
                case ForestTerrainSubType.Floor:
                case ForestTerrainSubType.Road:
                    v = true;
                    break;
                case ForestTerrainSubType.Hill:
                case ForestTerrainSubType.Wall:
                case ForestTerrainSubType.Pond:
                    v = false;
                    break;
            }
            return v;
        }
    }
}