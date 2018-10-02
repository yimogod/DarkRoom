using System;
using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace Sword
{
	public class HeroEntity : ActorEntity
	{
		private HeroMoveRange m_rangeTile;

		public Vector2Int TilePosition => new Vector2Int((int)LocalPosition.x, (int)LocalPosition.z);

		protected override void Start()
		{
			base.Start();

			var go = new GameObject("MoveRange");
			go.transform.SetParent(transform);
			go.transform.localPosition = Vector3.zero;
			m_rangeTile = go.AddComponent<HeroMoveRange>();
			ShowMoveRange(false);
			ShowMoveRange(true);
		}

		/// <summary>
		/// 隐藏/显示移动范围的格子
		/// </summary>
		public void ShowMoveRange(bool value)
		{
			if (value)
			{
				m_rangeTile.Show(MoveRange, TMap.Instance.WalkableGrid, LocalPosition);
			}
			else
			{
				m_rangeTile.Hide();
			}
		}

		public void OnClickMap()
		{
			m_anim.PlayerClick();
		}
	}
}