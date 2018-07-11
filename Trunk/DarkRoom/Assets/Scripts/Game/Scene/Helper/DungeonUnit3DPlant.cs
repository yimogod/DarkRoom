using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DarkRoom.Game;

namespace Sword
{
    public class DungeonUnit3DPlant : DungeonUnitPlant
    {
        public override void InstantiateUnit(MapMeta mapMeta, CAssetGrid typeGrid, CAssetGrid assetGrid)
        {
            base.InstantiateUnit(mapMeta, typeGrid, assetGrid);
            StartCoroutine(CreateUnitOfTileMap());
        }

        private IEnumerator CreateUnitOfTileMap()
        {
            for (int row = 0; row < _mapMeta.rows; row++)
            {
                for (int col = 0; col < _mapMeta.cols; col++)
                {
                    //string pname = _assetGrid.GetNode(row, col).name;
                    string pname = "";
                    if (string.IsNullOrEmpty(pname)) continue;

                    int type = _typeGrid.GetValue(row, col);
                    switch (type)
                    {
                        case (int) GameConst.TileType.TREE:
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
    }
}