using System;
using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.UI;
using DarkRoom.Utility;
using PureMVC.Patterns;
using UnityEngine;

namespace Sword
{
	public class Procedure_DungeonBattle : CProcedureBase
	{
		public const string NAME = "DungeonBattle";

		public Procedure_DungeonBattle() : base(NAME)
		{
			m_targetSceneName = SwordConst.DUNGEON_BATTLE_SCENE;
		}

		protected override void PreEnter(CStateMachine sm)
		{
			base.PreEnter(sm);
			m_preCreatePrefabAddress.Add("UI_BattleMain");
		}

		protected override void StartLoadingPrefab()
		{
			foreach (var item in m_preCreatePrefabAddress)
			{
				CResourceManager.LoadPrefab(item, OnPrefabLoaded);
			}
		}

		protected override void OnEntireLevelComplete()
		{
			Facade.instance.SendNotification(NotiConst.Open_BattleMain);

			DungeonBattleScene scene = GameObject.FindObjectOfType<DungeonBattleScene>();
			scene.LevelId = 10001;
			if (scene != null) scene.Launch();
		}
	}
}