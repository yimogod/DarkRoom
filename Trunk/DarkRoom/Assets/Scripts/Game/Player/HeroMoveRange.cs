using System;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Game;
using UnityEngine;

namespace Sword
{
	public class HeroMoveRange : MonoBehaviour
	{
		private Transform m_tran;

		private Dictionary<int, GameObject> m_dict = new Dictionary<int, GameObject>();

		private int m_maxView = 6;

		private CAStar m_star = new CAStar(CAStar.Connection.Four);
		//m_grid的尺寸并不和m_maxView大小一样
		private CStarGrid m_grid = new CStarGrid();

		void Awake()
		{
			m_tran = transform;

			int len = m_maxView * 2 + 1;
			for (int c = 0; c < len; c++)
			{
				for (int r = 0; r < len; r++)
				{
					var go = new GameObject($"{c}_{r}");
					var gt = go.transform;
					gt.parent = m_tran;
					gt.localPosition = CMapUtil.GetTileLeftBottomPosByColRow(c - m_maxView, r - m_maxView);
					AssetManager.LoadTilePrefab("map_common", "TileFlag", gt, Vector3.zero);

					int id = r * 1000 + c;
					m_dict[id] = go;
				}
			}
		}

		/// <summary>
		/// 隐藏全部标记
		/// </summary>
		public void Hide()
		{
			foreach (var item in m_dict)
			{
				item.Value.SetActive(false);
			}
		}

		/// <summary>
		/// 根据可通行性显示格子
		/// 请自行保证之前是全部隐藏的
		/// </summary>
		public void Show(int range, CStarGrid grid, Vector3 center)
		{
			//如果放在hero下面, hero会旋转, 这样我们的格子也会被旋转.
			//所以需要设置格子的旋转.否则显示的和可通行数据对不上
			m_tran.rotation = Quaternion.identity;
			Vector2Int pos = new Vector2Int((int)center.x, (int)center.z);

			DectectGridRange(range);
			CopyWalkableData(range, grid, pos);

			int gap = m_maxView - range;
			m_grid.SetStartNode(range, range);
			for (int c = 0; c < m_grid.NumCols; c++)
			{
				for (int r = 0; r < m_grid.NumRows; r++)
				{
					int dist = Math.Abs(c - range) + Math.Abs(r - range);
					if (dist > range) continue;
					m_grid.SetEndNode(c, r);

					var go = m_dict[(r + gap) * 1000 + (c + gap)];
					bool w =m_star.FindPath(m_grid, true);
					//如果不可通行, 或者通行的路径大于range
					if (!w || m_star.Path.Count >= range){
						go.SetActive(false);
					}else{
						go.SetActive(true);
					}
					
				}
			}
		}

		private void DectectGridRange(int range)
		{
			int len = range * 2 + 1;
			if (m_grid.NumCols == len && m_grid.NumRows == len) return;
			m_grid.Init(len, len);
		}

		private void CopyWalkableData(int range, CStarGrid grid, Vector2Int center)
		{
			int startCol = center.x - range;
			int startRow = center.y - range;
			for (int c = 0; c < m_grid.NumCols; c++)
			{
				for (int r = 0; r < m_grid.NumRows; r++)
				{
					bool w = grid.IsWalkable(startCol + c, startRow + r);
					m_grid.SetWalkable(c, r, w);
				}
			}
		}

		//-------------- end of class
	}
}