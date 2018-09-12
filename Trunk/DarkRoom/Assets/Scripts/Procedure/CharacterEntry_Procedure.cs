using System;
using System.Collections.Generic;
using DarkRoom.Core;
using PureMVC.Patterns;
using UnityEngine;
using DarkRoom.Game;
using DarkRoom.UI;

namespace Sword
{
	public class CharacterEntry_Procedure : CProcedureBase
	{
		public const string NAME = "CharacterEntry";

		private SwordMetaParserManager m_parser = new SwordMetaParserManager();

		private bool m_parseComplete = false;

		//第一次进入
		private bool m_firstEnter = true;

		public CharacterEntry_Procedure() : base(NAME)
		{
			m_targetSceneName = SwordConst.CHARACTER_ENTRY_SCENE;

			m_parser.OnSingleParseComplete = OnMetaLoaded;
			m_preCreatePrefabAddress.Add("UI_CharacterEntry");
		}

		public override void Enter(CStateMachine sm)
		{
			m_firstEnter = sm.LastState == null;
			base.Enter(sm);
		}

		protected override void PreEnter(CStateMachine sm)
		{
			if (m_firstEnter)
			{
				Debug.Log("Enter Character Entry First Time");
				m_parser.InitLite();
				m_enterSceneAssetMaxNum = m_parser.MainMetaNum;
			}
			else
			{
				base.PreEnter(sm);
			}
		}

		protected override void StartLoadingPrefab()
		{
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
			if (m_firstEnter)
			{
				m_parser.Dispose();

				var user = UserVO.LoadOrCreate();
                ProxyPool.UserProxy.User = user;

                if (user.HasAnyCharacter)
				{
					var character = UserProxy.Load(user.CurrentCharacterName);
					ProxyPool.UserProxy.Character = character;

					var hero = HeroProxy.Load(user.CurrentCharacterName);
					hero.Class = character.Class;
					hero.Race = character.Race;
					hero.Level = character.Level;
					ProxyPool.HeroProxy.Hero = hero;
                }
			}

			Facade.instance.SendNotification(NotiConst.Open_CharacterEntry);

			CharacterEntryScene scene = GameObject.FindObjectOfType<CharacterEntryScene>();
			if (scene == null)
			{
				Debug.LogError("Please add CharacterEntryScene on Scene Object In Unity");
				return;
			}
			scene.LoadHero(ProxyPool.HeroProxy.Hero);
		}
	}
}