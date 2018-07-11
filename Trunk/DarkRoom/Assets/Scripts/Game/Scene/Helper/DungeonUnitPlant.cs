using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;
using DarkRoom.AI;
using DarkRoom.Core;

namespace Sword
{
    public class DungeonUnitPlant : MonoBehaviour
    {
        protected DungeonMapBuilder _builder;
        protected MapMeta _mapMeta;

        protected TerrainComp _terrain = null;
        protected CAssetGrid _assetGrid;
        protected CAssetGrid _typeGrid;

        public void Init(TerrainComp terrain)
        {
            _terrain = terrain;
        }

        public virtual void InstantiateUnit(MapMeta mapMeta, CAssetGrid typeGrid, CAssetGrid assetGrid)
        {
            _mapMeta = mapMeta;
            _typeGrid = typeGrid;
            _assetGrid = assetGrid;
        }

        protected void PlaceCenterAndAddTreeComp(string unitName, int col, int row, int life)
        {
            GameObject go = PlaceUnitByCenter(unitName, col, row);
            if (go == null) return;

            float scale = CDarkRandom.Next(90, 110) * 0.01f;
            go.transform.localScale = new Vector3(scale, scale, scale);
        }

        protected GameObject PlaceUnitByCenter(string unitName, int col, int row)
        {
            GameObject go = AssetManager.LoadMapPrefab(_mapMeta.mapRoot, unitName);
            if (go == null) return null;

            //树以中心为原点, 且占用一格
            Vector3 pos = CMapUtil.GetTileCenterPosByColRow(col, row);
            pos.y = _terrain.GetWorldY(col, row);
            //CDarkUtil.AddChildWorld(WorldContainer.Instance.unitLayer, go.transform, pos);

            return go;
        }

        protected GameObject PlaceUnitByLeftBottom(string unitName, int col, int row)
        {
            GameObject go = AssetManager.LoadMapPrefab(_mapMeta.mapRoot, unitName);
            if (go == null) return null;

            //房子左下角对齐, 获取的应该是方块的左下角坐标
            Vector3 pos = CMapUtil.GetTileLeftBottomPosByColRow(col, row);
            pos.y = _terrain.GetWorldY(col, row);
            //CDarkUtil.AddChildWorld(WorldContainer.Instance.unitLayer, go.transform, pos);

            return go;
        }
    }
}