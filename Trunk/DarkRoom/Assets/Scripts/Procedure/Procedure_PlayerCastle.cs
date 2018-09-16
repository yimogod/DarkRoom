using System;
using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.UI;
using UnityEngine;
using DarkRoom.Utility;
using PureMVC.Patterns;

namespace Sword
{
	public class Procedure_PlayerCastle : CProcedureBase
	{
		public const string NAME = "PlayerCastle";

		private SwordMetaParserManager m_parser = new SwordMetaParserManager();

		public Procedure_PlayerCastle() : base(NAME)
		{
			m_targetSceneName = SwordConst.PLAYER_CASTLE_SCENE;
		}

		public override void Exit(CStateMachine sm)
		{
			base.Exit(sm);
			m_parser.Dispose();
		}

		protected override void PreEnter(CStateMachine sm)
		{
			if (m_parser.HasInitMain)
			{
				base.PreEnter(sm);
			}
			else
			{
				m_parser.OnSingleParseComplete = OnMetaLoaded;
				m_parser.InitMain();
				m_enterSceneAssetMaxNum = m_parser.MainMetaNum;
			}
		}

		protected override void StartLoadingPrefab()
		{
			//如果没有解析过才解析
			m_parser.Execute();

			foreach (var item in m_preCreatePrefabAddress)
			{
				CResourceManager.LoadPrefab(item, OnPrefabLoaded);
			}
		}

		private void OnPrefabLoaded(GameObject go)
		{
			m_enterSceneAssetLoadedNum++;
			CheckAllAssetsLoadComplete();
		}

		private void OnMetaLoaded()
		{
			m_enterSceneAssetLoadedNum++;
			CheckAllAssetsLoadComplete();
		}

		protected override void OnEntireLevelComplete()
		{
		}
	}
}