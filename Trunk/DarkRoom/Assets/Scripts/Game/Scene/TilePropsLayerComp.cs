using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;
using DarkRoom.PCG;

namespace Sword
{
    /// <summary>
    /// 装饰物层
    /// 由decal, block等组成
    /// </summary>
	public class TilePropsLayerComp : MonoBehaviour
    {
        private CAssetGrid m_assetGrid;

        private Transform m_parent;

        public void SetAssetGrid(CAssetGrid grid)
        {
            m_assetGrid = grid;
        }

        public void Build()
        {
            if (m_assetGrid == null)
            {
                Debug.LogError("AssetGrid Must Not Null");
                return;
            }

            m_parent = CWorld.Instance.Layer.TerrainLayer;

            StartCoroutine("CoroutineBuild");
        }

        private IEnumerator CoroutineBuild()
        {
            for (int col = 0; col < m_assetGrid.NumCols; col++)
            {
                yield return new WaitForEndOfFrame();
                for (int row = 0; row < m_assetGrid.NumRows; row++)
                {
                    var subType = m_assetGrid.GetNodeSubType(col, row);
                    var prefab = GetPrefabBySubType((CForestBlockSubType)subType);
                    if (string.IsNullOrEmpty(prefab)) continue;

                    var pos = CMapUtil.GetTileCenterPosByColRow(col, row);
                    pos.y = GameConst.DEFAULT_TERRAIN_HEIGHT;
                    LoadAndCreateTile(prefab, pos);

                    yield return new WaitForEndOfFrame();
                }
            }
        }

        private string GetPrefabBySubType(CForestBlockSubType subType)
        {
            string prefab = string.Empty;
            if (subType == CForestBlockSubType.None) return prefab;

            switch (subType)
            {
                case CForestBlockSubType.Tree:
                    prefab = "Tree_01";
                    break;
                case CForestBlockSubType.Rock1:
                    prefab = "Rock_01";
                    break;
                case CForestBlockSubType.Rock2:
                    prefab = "Rock_02";
                    break;
                case CForestBlockSubType.Plant1:
                    prefab = "Rock_01";
                    break;
                case CForestBlockSubType.Plant2:
                    prefab = "Rock_02";
                    break;
            }

            return prefab;
        }

        private void LoadAndCreateTile(string prefab, Vector3 pos)
        {
            AssetManager.LoadTilePrefab("map_forest", prefab, m_parent, pos);
        }
	}
}