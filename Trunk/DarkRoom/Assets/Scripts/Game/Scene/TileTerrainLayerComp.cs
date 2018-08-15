using UnityEngine;
using System.Collections;
using DarkRoom.Game;
using DarkRoom.PCG;

namespace Sword {
    /// <summary>
    /// 挂在地形/装饰物等layer的父亲上
    /// 代表整个terrain go
    /// 
    /// 存储着地形数据/装饰物信息等无关逻辑的信息
    /// </summary>
	public class TileTerrainLayerComp : MonoBehaviour
    {
        private CAssetGrid m_assetGrid;

        private Transform m_tran;

		protected void Start()
		{
		    m_tran = transform;

            int desireLayer = LayerMask.NameToLayer(CWorldLayer.LAYER_NAME_TERRAIN);
			if (desireLayer == -1) {
				Debug.LogError("We Do Not Set Terrain Layer. Please Set One Terrain layer");
			}
		}

        public void SetAssetGrid(CAssetGrid grid)
        {
            m_assetGrid = grid;
        }

		public void Build() {
		    if (m_assetGrid == null)
		    {
		        Debug.LogError("AssetGrid Must Not Null");
                return;
		    }

		    StartCoroutine(CoroutineBuild());
		}

        IEnumerator CoroutineBuild()
        {
            for (int i = 0; i < m_assetGrid.NumCols; i++)
            {
                yield return new WaitForEndOfFrame();
                for (int j = 0; j < m_assetGrid.NumRows; j++)
                {
                    var tile = m_assetGrid.GetNodeSubType(i, j);
                    var prefab = GetPrefabBySubType(tile);

                    var pos = CMapUtil.GetTileCenterPosByColRow(i, j);
                    LoadAndCreateTile(prefab, pos);
                }
            }

            yield return new WaitForEndOfFrame();
        }

        private string GetPrefabBySubType(int subType){
            CForestTerrainSubType index = (CForestTerrainSubType)subType;
            string str = string.Empty;
            switch (index){
                case CForestTerrainSubType.Grass1:
                    str = "Tile_1_A";
                    break;
                case CForestTerrainSubType.Grass2:
                    str = "Tile_1_B";
                    break;
                case CForestTerrainSubType.Land1:
                    str = "Tile_1_C";
                    break;
                case CForestTerrainSubType.Land2:
                    str = "Tile_1_D";
                    break;
            }

            return str;
        }

        private void LoadAndCreateTile(string prefab, Vector3 pos){
            AssetManager.LoadTilePrefab("map_forest", prefab, m_tran, pos);
        }

		//end class
	}
}