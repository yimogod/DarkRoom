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
			if (!gameState.InHeroRound) return;

			CMouseEvent e = evt as CMouseEvent;
			Debug.Log("click " + e.WorldPosition.x + " " + e.WorldPosition.z);
			var target = CMapUtil.GetTileCenterPosByColRow(e.WorldPosition);
			m_entity.ShowMoveRange(false);
			MoveToLocation(target, result => m_entity.ShowMoveRange(true));
		}
	}
}
