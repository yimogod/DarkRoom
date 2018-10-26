using System;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Core;
using DarkRoom.Game;
using DarkRoom.Utility;
using UnityEngine;

namespace Sword
{
	[RequireComponent(typeof(HeroEntity))]
	public class HeroController : ActorController
	{
		private HeroEntity m_entity => m_pawn as HeroEntity;

		protected override void Start()
		{
			base.Start();
			m_entity.Mover.MaxSpeed = 5f;
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

			var pos = m_entity.TilePosition;
			var dist = tile.ManhattanMagnitude(pos);
			if (dist > m_entity.AttributeSet.MoveRange)
			{
				Debug.Log("<OnClickMap> Click Tile Out Of Range");
				return;
			}

			var target = CMapUtil.GetTileByPos(e.WorldPosition);
			var waypoints = CTileNavigationSystem.Instance.GetWayPointBetween(pos, target);
			if (dist > waypoints.Count)
			{
				Debug.Log("<OnClickMap> Click Tile Out Of Range");
				return;
			}

			m_entity.OnClickMap();
			m_entity.ShowMoveRange(false);
			MoveToLocation(waypoints, result => m_entity.ShowMoveRange(true));
		}
	}
}
