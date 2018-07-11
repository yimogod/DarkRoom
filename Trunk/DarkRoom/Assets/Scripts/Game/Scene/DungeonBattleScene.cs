using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sword
{
    public class DungeonBattleScene : MonoBehaviour
    {
        public GameObject rootLayer;

        private MapMeta _mapMeta;

        private DungeonMapBuilder _builder;
        private Terrain3DComp _terrain3D = null;
        private DungeonUnit3DPlant _unit3DCreator = null;

        void Start()
        {
            InitMapThing();
            LoadAndCreateMapThing();


            //CreateWarFrog();

            GameUtil.CameraFocusHero();
            AssetManager.LoadUIPrefab("UI_Battle_HUD_Preb");

            GameEngine.Instance.Init();
            GameEngine.Instance.Start();

            //DebugMap dm = GameObject.FindObjectOfType<DebugMap>();
            //dm.CreateBlockMap(TMap.ins.walkGrid);

            _builder.Clear();
        }

        private void InitMapThing()
        {
           // rootLayer.AddComponent<WorldContainer>();

            _mapMeta = MapMetaManager.GetMeta("1");
            TMap.Instance.Init(_mapMeta);
            _builder = new DungeonMapBuilder(TMap.Instance);

            //读取加载地形数据, 并组装terrain3d comp
            GameObject terrainGO = AssetManager.LoadMapPrefab("Forest", "Forest_01");
            Terrain terrain = terrainGO.GetComponent<Terrain>();
            _terrain3D = gameObject.AddComponent<Terrain3DComp>();
            _terrain3D.terrain = terrain;

            TMap.Instance.terrain = _terrain3D;
            _unit3DCreator = gameObject.AddComponent<DungeonUnit3DPlant>();
        }

        private void LoadAndCreateMapThing()
        {
            _builder.CreateOther();
            _builder.CopyData();

            _unit3DCreator.Init(_terrain3D);
            _unit3DCreator.InstantiateUnit(_mapMeta, _builder.typeGrid, _builder.assetGrid);
        }

        void Update()
        {
            //GameEngine.Instance.Update();
        }

    }
}