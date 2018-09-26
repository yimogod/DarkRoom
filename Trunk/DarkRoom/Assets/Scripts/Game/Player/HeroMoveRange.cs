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

		void Awake()
		{
			m_tran = transform;
		}

		void Start()
		{
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
			int len = range * 2 + 1;
			int gap = m_maxView - range;
			for (int c = 0; c < len; c++)
			{
				for (int r = 0; r < len; r++)
				{
					var go = m_dict[(r + gap) * 1000 + (c + gap)];
					var colInWorld = (int)center.x + c - range;
					var rowInWorld = (int)center.z + r - range;
					var w = grid.IsWalkable(colInWorld, rowInWorld);
					go.SetActive(w);
				}
			}
		}
	}
}