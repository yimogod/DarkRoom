using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;
using DarkRoom.AI;
using DarkRoom.Core;

namespace Sword
{
    public class TileUnitLayerComp : MonoBehaviour
    {
        protected DungeonMapBuilder _builder;
        protected MapMeta _mapMeta;

        protected TileTerrainLayerComp _terrain = null;
        protected CAssetGrid _assetGrid;

        public void Init(TileTerrainLayerComp terrain)
        {
            _terrain = terrain;
        }

        public void Build()
        {
            if (_assetGrid == null)
            {
                Debug.LogError("AssetGrid Must Not Null");
                return;
            }

        }

        public virtual void InstantiateUnit(MapMeta mapMeta, CAssetGrid assetGrid)
        {
            _mapMeta = mapMeta;
            _assetGrid = assetGrid;
        }

        private IEnumerator CreateUnitOfTileMap()
        {
            for (int row = 0; row < _mapMeta.Rows; row++)
            {
                for (int col = 0; col < _mapMeta.Cols; col++)
                {
                    //string pname = _assetGrid.GetNode(row, col).name;
                    string pname = "";
                    if (string.IsNullOrEmpty(pname)) continue;

                    int type = _assetGrid.GetNodeType(row, col);
                    switch (type)
                    {
                        case (int)GameConst.TileType.TREE:
                            PlaceCenterAndAddTreeComp(pname, col, row, 3);
                            break;
                        case (int)GameConst.TileType.ROOM:
                            PlaceUnitByLeftBottom(pname, col, row);
                            break;
                        case (int)GameConst.TileType.DECAL:
                            PlaceUnitByCenter(pname, col, row);
                            break;
                        case (int)GameConst.TileType.DECO_BLOCK:
                            PlaceUnitByCenter(pname, col, row);
                            break;
                        case (int)GameConst.TileType.DECO_DESTROY:
                            PlaceCenterAndAddTreeComp(pname, col, row, 1);
                            break;
                    }

                    yield return new WaitForEndOfFrame();
                }
            }
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
            //AssetManager.LoadMapPrefab(_mapMeta.MapRoot, unitName);
            //if (go == null) return null;

            //树以中心为原点, 且占用一格
            Vector3 pos = CMapUtil.GetTileCenterPosByColRow(col, row);
            //CDarkUtil.AddChildWorld(WorldContainer.Instance.unitLayer, go.transform, pos);
            return null;
        }

        protected GameObject PlaceUnitByLeftBottom(string unitName, int col, int row)
        {
            //AssetManager.LoadMapPrefab(_mapMeta.MapRoot, unitName);

            //房子左下角对齐, 获取的应该是方块的左下角坐标
            Vector3 pos = CMapUtil.GetTileLeftBottomPosByColRow(col, row);
            //CDarkUtil.AddChildWorld(WorldContainer.Instance.unitLayer, go.transform, pos);

            return null;
        }
    }
}