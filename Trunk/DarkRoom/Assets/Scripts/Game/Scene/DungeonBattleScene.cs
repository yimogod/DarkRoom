using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sword
{
    public class DungeonBattleScene : MonoBehaviour
    {
        private MapMeta m_mapMeta;

        private DungeonMapBuilder m_builder;
        private TileTerrainComp m_terrain3D = null;
        private DungeonUnit3DPlant m_unit3DCreator = null;

        void Start()
        {
            InitMapThing();
            LoadAndCreateMapThing();

            //GameUtil.CameraFocusHero();
            //AssetManager.LoadUIPrefab("UI_Battle_HUD_Preb");

            GameEngine.Instance.Init();
            GameEngine.Instance.Start();

            m_builder.Clear();
        }

        private void InitMapThing()
        {
            m_mapMeta = MapMetaManager.GetMeta("1");
            TMap.Instance.Init(m_mapMeta);
            m_builder = new DungeonMapBuilder(TMap.Instance);

            //读取加载地形数据, 并组装terrain3d comp
            GameObject terrainGO = AssetManager.LoadMapPrefab("Forest", "Forest_01");
            m_terrain3D = gameObject.AddComponent<TileTerrainComp>();

            TMap.Instance.terrain = m_terrain3D;
            m_unit3DCreator = gameObject.AddComponent<DungeonUnit3DPlant>();
        }

        private void LoadAndCreateMapThing()
        {
            m_builder.CreateOther();
            m_builder.CopyData();

            m_unit3DCreator.Init(m_terrain3D);
            m_unit3DCreator.InstantiateUnit(m_mapMeta, m_builder.typeGrid, m_builder.assetGrid);
        }

        void Update()
        {
            //GameEngine.Instance.Update();
        }

    }
}