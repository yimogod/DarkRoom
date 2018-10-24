using System;
using PureMVC.Patterns;
using UnityEngine;
using DarkRoom.Core;
using DarkRoom.UI;
using DarkRoom.Utility;

namespace Sword
{
	public class Procedure_CharacterEntry : CProcedureBase
	{
		public const string NAME = "CharacterEntry";

		private SwordMetaParserManager m_parser = new SwordMetaParserManager();

		private bool m_parseComplete = false;

		//第一次进入
		private bool m_firstEnter = true;

		public Procedure_CharacterEntry() : base(NAME)
		{
			m_targetSceneName = SwordConst.CHARACTER_ENTRY_SCENE;
		}

		public override void Enter(CStateMachine sm)
		{
			m_firstEnter = sm.LastState == null;
			base.Enter(sm);
		}

		public override void Exit(CStateMachine sm)
		{
			base.Exit(sm);
			m_parser.Dispose();
		}

		protected override void PreEnter(CStateMachine sm)
		{
			m_preCreatePrefabAddress.Add("UI_CharacterEntry");

			if (m_firstEnter)
			{
				m_parser.OnSingleParseComplete = OnMetaLoaded;
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
			//如果没有解析过才解析
			m_parser.Execute();

			foreach (var item in m_preCreatePrefabAddress)
			{
				CResourceManager.LoadPrefab(item, OnPrefabLoaded);
			}
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
				var u = UserVO.LoadOrCreate();
				ProxyPool.UserProxy.User = u;
			}

			UserVO user = ProxyPool.UserProxy.User;
			user.FindCurrentCharacter();

			//设置当前角色的数据
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