using System;
using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace Sword
{
	public class HeroEntity : ActorEntity
	{
		private HeroMoveRange m_rangeTile;

		protected override void Start()
		{
			base.Start();

			var go = new GameObject("MoveRange");
			go.transform.SetParent(transform);
			go.transform.localPosition = Vector3.zero;
			m_rangeTile = go.AddComponent<HeroMoveRange>();
		}

		/// <summary>
		/// 隐藏/显示移动范围的格子
		/// </summary>
		public void ShowMoveRange(bool value)
		{
			if (value)
			{
				m_rangeTile.Show(5, TMap.Instance.WalkableGrid, LocalPosition);
			}
			else
			{
				m_rangeTile.Hide();
			}
		}


	}
}