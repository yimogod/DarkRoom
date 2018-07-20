using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.Game;

namespace Sword
{
    public class DungeonBattleScene : MonoBehaviour
    {
        private MapMeta m_mapMeta;

        private DungeonMapBuilder m_builder;
        private TileTerrainLayerComp m_terrainLayer = null;
        private TileUnitLayerComp m_unitLayer = null;

        public void Launch()
        {
            InitMapThing();
            CreateMapThing();

            //GameUtil.CameraFocusHero();
            //AssetManager.LoadUIPrefab("UI_Battle_HUD_Preb");

            GameEngine.Instance.Init();
            GameEngine.Instance.Start();
        }

        private void InitMapThing()
        {
            m_mapMeta = MapMetaManager.GetMeta("1");
            TMap.Instance.Init(m_mapMeta);
            m_builder = new DungeonMapBuilder(m_mapMeta);

            //读取加载地形数据, 并组装terrain3d comp
            var terrainTran = CWorld.Instance.Layer.TerrainLayer;
            m_terrainLayer = terrainTran.gameObject.GetOrCreateComponent<TileTerrainLayerComp>();
            m_builder.CreateMap(m_terrainLayer);

            //创建并读取unit数据. 并组装unit layer comp
            var unitTran = CWorld.Instance.Layer.UnitLayer;
            m_unitLayer = unitTran.gameObject.GetOrCreateComponent<TileUnitLayerComp>();
            m_builder.CreateActor(m_unitLayer);
        }

        private void CreateMapThing()
        {
            m_terrainLayer.Build();
            m_unitLayer.Build();

            m_builder.CopyData(TMap.Instance.WalkableGrid);
        }

        void Update()
        {
            //GameEngine.Instance.Update();
        }

    }
}