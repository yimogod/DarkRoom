using System;
using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace Sword
{
	public class HeroMoveRange : MonoBehaviour
	{
		private Transform m_tran;

		void Awake()
		{
			m_tran = transform;
			CreateRangeTile();
		}

		private void CreateRangeTile()
		{
			for (int c = 0; c < 5; c++)
			{
				for (int r = 0; r < 5; r++)
				{
					var go = new GameObject($"{c}_{r}");
					var gt = go.transform;
					gt.parent = m_tran;
					gt.localPosition = new Vector3(c - 2f, 0, r - 2f);
					AssetManager.LoadTilePrefab("map_common", "TileFlag", gt, Vector3.zero);
				}
			}
		}
	}
}