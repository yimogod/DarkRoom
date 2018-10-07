using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;
using DarkRoom.PCG;
using DarkRoom.AI;

namespace Sword
{
	/// <summary>
	/// 用于显示不可通行区域
	/// </summary>
	public class TileDebugLayerComp : MonoBehaviour
	{
		private CStarGrid m_grid;
		private Transform m_parent;

		public void SetStarGrid(CStarGrid grid)
		{
			m_grid = grid;
		}

		public void Draw()
		{
			if (m_grid == null)
			{
				Debug.LogError("AssetGrid Must Not Null");
				return;
			}

			m_parent = CWorld.Instance.Layer.PoolLayer;
			CoroutineBuild();
		}

		private void CoroutineBuild()
		{
			for (int row = 0; row < m_grid.NumRows; row++)
			{
				for (int col = 0; col < m_grid.NumCols; col++)
				{
					bool walkable = m_grid.IsWalkable(col, row);
					if (!walkable) continue;

					var pos = CMapUtil.GetTileCenterPosByColRow(col, row);
					pos.y = GameConst.DEFAULT_TERRAIN_HEIGHT + 0.05f;
					LoadAndCreateTile(pos);
				}
			}
		}


		protected void LoadAndCreateTile(Vector3 pos)
		{
			AssetManager.LoadTilePrefab("map_common", "TileFlag", m_parent, pos);
		}
	}
}
