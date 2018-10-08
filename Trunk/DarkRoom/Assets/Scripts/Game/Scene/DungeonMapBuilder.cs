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

		public CAssetGrid TerrainGrid => m_mapGen.TerrainGrid;
		public CAssetGrid BlockGrid => m_mapGen.BlockGrid;
		public CAssetGrid DecalGrid => m_mapGen.DecalGrid;
		public CAssetGrid ActorGrid => m_actorGen.Grid;

		public Vector2Int HeroBornPos => m_actorGen.HeroBornPos;

		public DungeonMapBuilder(MapMeta meta)
		{
			m_mapMeta = meta;
		}

		/// <summary>
		/// 随机生成地图及tile数据
		/// 并且把数据填充入TileTerrainComp
		/// </summary>
		public void CreateMap()
		{
			m_mapGen = new CForestGenerator();
			m_mapGen.Generate();
		}

		/// <summary>
		/// 根据地图配置, 随机放置角色
		/// </summary>
		public void CreateActor(CStarGrid walkableGrid)
		{
			m_actorGen = new ActorGenerator();
			m_actorGen.Generate(m_mapMeta, walkableGrid);
		}

		/// <summary>
		/// 讲生成器的通行数据拷贝到我们的tmap中
		/// </summary>
		public void CopyUnWalkableToGrid(CStarGrid targetGrid)
		{
			//不可通行的来源有很多地方
			//1 terrain的湖水
			targetGrid.CopyUnWalkableFrom(TerrainGrid);

			//2 单位层的房子

			//3 障碍物
			targetGrid.CopyUnWalkableFrom(BlockGrid);
		}

		/// <summary>
		/// 拷贝单位占领的不可同行区域
		/// </summary>
		public void CopyActorUnWalkableToGrid(CStarGrid targetGrid)
		{
			targetGrid.CopyUnWalkableFrom(ActorGrid);
		}
	}
}