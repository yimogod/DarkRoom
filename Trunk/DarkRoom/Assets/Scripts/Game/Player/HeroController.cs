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

		private void OnClickMap(CEvent e)
		{
			if (!gameState.InHeroRound) return;
		}
	}
}
