using System;
using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.UI;
using DarkRoom.Utility;
using PureMVC.Patterns;

namespace Sword
{
	public class Procedure_PlayerCastle : CProcedureBase
	{
		public const string NAME = "PlayerCastle";

		public Procedure_PlayerCastle() : base(NAME)
		{
			m_targetSceneName = SwordConst.PLAYER_CASTLE_SCENE;
		}

		protected override void PreEnter(CStateMachine sm)
		{
			base.PreEnter(sm);
			//m_preCreatePrefabAddress.Add("UI_CharacterCreate");
		}

		protected override void StartLoadingPrefab()
		{
			CheckAllAssetsLoadComplete();
		}

		protected override void OnEntireLevelComplete()
		{
		}
	}
}