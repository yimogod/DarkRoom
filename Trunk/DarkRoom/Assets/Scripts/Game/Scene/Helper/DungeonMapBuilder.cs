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
        private CTileMapGeneratorBase m_mapGen;

        public CAssetGrid AssetGrid => m_mapGen.Grid;

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
			gen.TerrainType = (int)GameConst.TileType.TERRIAN;
            gen.SeaType = (int) GameConst.TileType.LAKE;
            gen.SetAsset(0, "sea_01", false);
            gen.SetAsset(1, "sea_02", false);
            gen.SetAsset(2, "coast_01", false);
            gen.SetAsset(3, "coast_02", false);
            gen.SetAsset(4, "grass_01", true);
            gen.SetAsset(5, "grass_02", true);
            gen.SetAsset(6, "grass_03", true);
            gen.SetAsset(7, "land_01", true);
            gen.SetAsset(8, "land_02", true);
            gen.SetAsset(9, "stone_01", true);
            gen.SetAsset(10, "stone_02", true);
            gen.SetDefaultAsset("grass_01", true);
            gen.Generate();
			m_mapGen = gen;

            terrainLayer.SetAssetGrid(gen.Grid);
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
		    targetGrid.CopyUnWalkableFrom(m_mapGen.Grid);
		}
    }
}