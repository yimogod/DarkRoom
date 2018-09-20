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
		private TileUnitLayerComp m_unitLayer = null;

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

			//读取加载地形数据, 并组装terrain3d comp
			var terrainTran = CWorld.Instance.Layer.TerrainLayer;
			m_terrainLayer = terrainTran.gameObject.GetOrCreateComponent<TileTerrainLayerComp>();
			m_builder.CreateMap(m_terrainLayer);

			//创建并读取unit数据. 并组装unit layer comp
			//var unitTran = CWorld.Instance.Layer.UnitLayer;
			//m_unitLayer = unitTran.gameObject.GetOrCreateComponent<TileUnitLayerComp>();
			//m_builder.CreateActor(m_unitLayer);
		}

		private void CreateMapThing()
		{
			m_terrainLayer.Build();
			//m_unitLayer.Build();

			//m_builder.CopyData(TMap.Instance.WalkableGrid);
		}

		void Update()
		{
			//GameEngine.Instance.Update();
		}
	}
}