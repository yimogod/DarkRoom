using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.Game;

namespace Sword
{
	public class DungeonBattleScene : MonoBehaviour
	{
		/// <summary>
		/// 选择关卡的meta id
		/// </summary>
		public int LevelId;

		private DungeonMapBuilder m_builder = null;
		private TileTerrainLayerComp m_terrainLayer = null;
		private TileBlockLayerComp m_blockLayer = null;
		private TileDecalLayerComp m_decalLayer = null;

		private TileActorLayerComp m_actorLayer = null;

		private TileDebugLayerComp m_debugLayer = null;

		public void Launch()
		{
			CreateMapData();
			CreateMapThing();

			//GameUtil.CameraFocusHero();

			GameEngine.Instance.Init();
			GameEngine.Instance.Start();
		}

		private void CreateMapData()
		{
			MapMeta m_mapMeta = MapMetaManager.GetMeta(LevelId);
			if (m_mapMeta == null)
			{
				Debug.LogError("Invalid Level id .. " + LevelId);
				return;
			}

			TMap.Instance.Init(m_mapMeta);
			m_builder = new DungeonMapBuilder(m_mapMeta);
			m_builder.CreateMap();
			m_builder.CreateActor();

			//读取加载地形数据, 并组装terrain3d comp
			m_terrainLayer = gameObject.GetOrCreateComponent<TileTerrainLayerComp>();
			m_terrainLayer.SetAssetGrid(m_builder.TerrainGrid);

			//读取加载没有逻辑的障碍
			m_blockLayer = gameObject.GetOrCreateComponent<TileBlockLayerComp>();
			m_blockLayer.SetAssetGrid(m_builder.BlockGrid);

			//加载没有逻辑的贴花
			m_decalLayer = gameObject.GetOrCreateComponent<TileDecalLayerComp>();
			m_decalLayer.SetAssetGrid(m_builder.DecalGrid);

			//拷贝不可通行数据
			m_builder.CopyUnWalkableToGrid(TMap.Instance.WalkableGrid);

			//加载角色数据
			m_actorLayer = gameObject.GetOrCreateComponent<TileActorLayerComp>();
			m_actorLayer.SetAssetGrid(null);

			//创建可通行debug层
			m_debugLayer = gameObject.GetOrCreateComponent<TileDebugLayerComp>();
			m_debugLayer.SetStarGrid(TMap.Instance.WalkableGrid);
		}

		private void CreateMapThing()
		{
			m_terrainLayer.Build();
			m_blockLayer.Build();
			m_decalLayer.Build();
			m_actorLayer.Build();

			m_debugLayer.Draw();
		}

		void Update()
		{
			//GameEngine.Instance.Update();
		}
	}
}