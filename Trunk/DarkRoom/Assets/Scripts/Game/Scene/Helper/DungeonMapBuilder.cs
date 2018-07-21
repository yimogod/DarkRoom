using System;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Core;
using DarkRoom.Game;
using DarkRoom.PCG;
using UnityEngine;

namespace Sword
{
    /// <summary>
    /// 调用对应的地形/单位生成器, 生成地图
    /// </summary>
    public class DungeonMapBuilder
    {
        private MapMeta m_mapMeta;

        private ActorGenerator m_actorGen;
        private CForestGenerator m_mapGen;

        public CAssetGrid AssetGrid => m_mapGen.TerrainGrid;

        public DungeonMapBuilder(MapMeta meta)
        {
            m_mapMeta = meta;
		}

        /// <summary>
        /// 随机生成地图及tile数据
        /// 并且把数据填充入TileTerrainComp
        /// </summary>
        public void CreateMap(TileTerrainLayerComp terrainLayer)
        {
            //地图生成器生成地图
            CForestGenerator gen = terrainLayer.GetOrCreateComponentOnGameObject<CForestGenerator>();
            gen.SetTerrainAsset(ForestTerrainAssetIndex.Grass1, "grass_01", true);
            gen.SetTerrainAsset(ForestTerrainAssetIndex.Grass2, "grass_02", true);
            gen.SetTerrainAsset(ForestTerrainAssetIndex.Land1, "land_01", true);
            gen.SetTerrainAsset(ForestTerrainAssetIndex.Land2, "land_02", true);
            gen.SetTerrainAsset(ForestTerrainAssetIndex.Wall, "stone_01", false);
            gen.SetTerrainAsset(ForestTerrainAssetIndex.Pond, "pond_01", false);
            gen.SetTerrainAsset(ForestTerrainAssetIndex.Road, "road_01", true);
            gen.SetTerrainDefaultAsset("grass_01", true);
            gen.Generate();
			m_mapGen = gen;

            terrainLayer.SetAssetGrid(gen.TerrainGrid);
        }

        public void CreateActor(TileUnitLayerComp unitLayer)
        {
            m_actorGen = unitLayer.GetOrCreateComponentOnGameObject<ActorGenerator>();
            m_actorGen.Generate(m_mapMeta, AssetGrid);
        }

        /// <summary>
        /// 讲生成器的通行数据拷贝到我们的tmap中
        /// </summary>
        public void CopyData(CMapGrid<CStarNode> targetGrid)
		{
		    targetGrid.Init(m_mapMeta.Cols, m_mapMeta.Rows, true);

            //不可通行的来源有很多地方
            //1 terrain的湖水
		    targetGrid.CopyUnWalkableFrom(m_mapGen.TerrainGrid);
            //2 单位层的房子

		}
    }
}