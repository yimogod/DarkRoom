using UnityEngine;
using System.Collections;
namespace Sword {
	public class TerrainComp : MonoBehaviour {
		public virtual int numRows {
			get { return 0; }
		}

		public virtual int numCols {
			get { return 0; }
		}

		public virtual int numRowsOfHeight {
			get { return 0; }
		}

		public virtual int numColsOfHeight {
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
		public virtual void SetTileByName(int row, int col, int layer, float value) {
		}

		//row = z, col = x
		public virtual float GetWorldY(int x, int z) {
			return 0;
		}

		//end class
	}
}