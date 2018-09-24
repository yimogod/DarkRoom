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

		private DungeonMapBuilder m_builder;
		private TileTerrainLayerComp m_terrainLayer = null;
        private TileBlockLayerComp m_blockLayer = null;

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

            //读取加载地形数据, 并组装terrain3d comp
            m_terrainLayer = gameObject.GetOrCreateComponent<TileTerrainLayerComp>();
            m_terrainLayer.SetAssetGrid(m_builder.TerrainGrid);

            //读取加载没有逻辑的装饰物
            m_blockLayer = gameObject.GetOrCreateComponent<TileBlockLayerComp>();
            m_blockLayer.SetAssetGrid(m_builder.BlockGrid);

            //创建并读取unit数据. 并组装unit layer comp
            //var unitTran = CWorld.Instance.Layer.UnitLayer;
            //m_unitLayer = unitTran.gameObject.GetOrCreateComponent<TileUnitLayerComp>();
            //m_builder.CreateActor(m_unitLayer);
        }

		private void CreateMapThing()
		{
			m_terrainLayer.Build();
            m_blockLayer.Build();
			//m_unitLayer.Build();

			//m_builder.CopyData(TMap.Instance.WalkableGrid);
		}

		void Update()
		{
			//GameEngine.Instance.Update();
		}
	}
}