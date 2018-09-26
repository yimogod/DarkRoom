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


		}

		protected override void SetupInputComponent()
		{
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
			MoveToLocation(e.WorldPosition);
		}
	}
}
