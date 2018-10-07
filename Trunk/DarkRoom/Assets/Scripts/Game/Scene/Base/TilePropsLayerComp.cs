using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;
using DarkRoom.PCG;
using DarkRoom.AI;

namespace Sword
{
	/// <summary>
	/// 装饰物层的基类, 又block和decal分别继承实现不同的映射
	/// </summary>
	public class TilePropsLayerComp : MonoBehaviour
	{
		protected CAssetGrid m_assetGrid;

		protected Transform m_parent;

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
			for (int row = 0; row < m_assetGrid.NumRows; row++)
			{
				yield return new WaitForEndOfFrame();
				for (int col = 0; col < m_assetGrid.NumCols; col++)
				{
					var subType = m_assetGrid.GetNodeSubType(col, row);
					var prefab = GetPrefabBySubType(subType);
					if (string.IsNullOrEmpty(prefab)) continue;

					var pos = CMapUtil.GetTileCenterPosByColRow(col, row);
					pos.y = GameConst.DEFAULT_TERRAIN_HEIGHT;
					LoadAndCreateTile(prefab, pos);
				}
			}
		}

		protected virtual string GetPrefabBySubType(int enumType)
		{
			return string.Empty;
		}

		protected void LoadAndCreateTile(string prefab, Vector3 pos)
		{
			AssetManager.LoadTilePrefab("map_forest", prefab, m_parent, pos);
		}

		protected string RandomSelectAddress(string prefix, int start, int end)
		{
			int index = Random.Range(start, end + 1);
			if (index < 10) return $"{prefix}0{index}";
			return $"{prefix}{index}";
		}
	}
}