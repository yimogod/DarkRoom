using System;
using System.Collections.Generic;
using DarkRoom.Game;
using DarkRoom.Utility;
using UnityEngine;

namespace Sword
{
	public class HeroController : CPlayerController
	{
		private SwordGameState gameState =>
			CWorld.Instance.GetGameState<SwordGameState>();
		protected override void SetupInputComponent()
		{
			if(!gameState.InHeroRound)return;

			if (CMouseInput.Instance.HasClicked)
			{

			}
		}

		protected override void Update()
		{
			base.Update();
			
		}
	}
}
