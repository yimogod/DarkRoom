using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace Sword
{
	public class SwordGameState : CGameState
	{
		/// <summary>
		/// 当前可以行动的单位
		/// </summary>
		public CUnitEntity.TeamSide ActionTeam = CUnitEntity.TeamSide.Red;

		public bool InHeroRound => ActionTeam == CUnitEntity.TeamSide.Red;
		public bool InEnemyRound => ActionTeam == CUnitEntity.TeamSide.Blue;
	}
}