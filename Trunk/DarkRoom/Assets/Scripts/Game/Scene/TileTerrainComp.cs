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
	public class TileTerrainComp : MonoBehaviour
    {
        public CAssetGrid AssetGrid;


		public int NumRows {
			get { return 0; }
		}

		public int NumCols {
			get { return 0; }
		}

		public int NumRowsOfHeight {
			get { return 0; }
		}

		public int NumColsOfHeight {
			get { return 0; }
		}

		protected virtual void Start() {
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

		public virtual void ForceBuild() {
		}

		//用value来填充整个tile, 覆盖其他layer
		public void SetTileByName(int row, int col, int layer, float value) {
		}

		//row = z, col = x
		public float GetWorldY(int x, int z) {
			return 0;
		}

		//end class
	}
}