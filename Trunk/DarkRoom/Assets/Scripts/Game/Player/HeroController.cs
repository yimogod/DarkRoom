using System;
using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.Game;
using DarkRoom.Utility;
using UnityEngine;

namespace Sword
{
	[RequireComponent(typeof(HeroEntity))]
	public class HeroController : CPlayerController
	{
		private SwordGameState gameState =>
			CWorld.Instance.GetGameState<SwordGameState>();

		private HeroEntity m_entity => m_pawn as HeroEntity;

		protected override void Start()
		{
			base.Start();
			m_entity.Mover.MaxSpeed = 2f;
		}

		protected override void SetupInputComponent()
		{
			base.SetupInputComponent();
			CMouseInput.Instance.AddClickListener(OnClickMap);
		}

		protected override void Update()
		{
			base.Update();
		}

		private void OnClickMap(CEvent evt)
		{
			if (!gameState.InHeroRound)
			{
				Debug.Log("<OnClickMap> Not In Hero Round");
				return;
			}
			if (m_entity.IsFollowingPath)
			{
				Debug.Log("<OnClickMap> Hero IsFollowingPath");
				return;
			}

			CMouseEvent e = evt as CMouseEvent;
			var tile = CMapUtil.GetTileByPos(e.WorldPosition);
			if (!TMap.Instance.Walkable(tile.x, tile.y))
			{
				Debug.Log("<OnClickMap> Click UnWalkable Tile");
				return;
			}

			m_entity.ShowMoveRange(false);
			var target = CMapUtil.GetTileCenterPosByColRow(e.WorldPosition);
			MoveToLocation(target, result => m_entity.ShowMoveRange(true));
		}
	}
}
