using UnityEngine;
using System.Collections;
using DarkRoom.Game;

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

            int desireLayer = LayerMask.NameToLayer("Terrain");
			if (desireLayer == -1) {
				Debug.LogError("We Do Not Set Terrain Layer. Please Set One Terrain layer");
			}

			//设置terrain组件的所有孩子到terrain layer
			if (gameObject.layer != desireLayer) {
				gameObject.layer = desireLayer;

				Transform tran = transform;
				int count = tran.childCount;
				for (int i = 0; i < count; i++) {
					Transform child = tran.GetChild(i);
					if (child == null) continue;
					child.gameObject.layer = desireLayer;
				}
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
                    var tile = m_assetGrid.GetNodeAsset(i, j);
                    var pos = CMapUtil.GetTileCenterPosByColRow(i, j);
                    AssetManager.LoadTilePrefab("", tile, m_tran, pos);
                }
            }

            yield return new WaitForEndOfFrame();
        }

		//end class
	}
}