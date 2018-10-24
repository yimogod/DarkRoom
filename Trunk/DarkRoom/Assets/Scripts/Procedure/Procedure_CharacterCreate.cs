using System;
using System.Collections.Generic;
using DarkRoom.Core;
using PureMVC.Patterns;
using DarkRoom.UI;
using DarkRoom.Utility;
using UnityEngine;

namespace Sword
{
	public class Procedure_CharacterCreate : CProcedureBase
	{
		public const string NAME = "CharacterCreate";

		public Procedure_CharacterCreate() : base(NAME)
		{
			m_targetSceneName = SwordConst.CHARACTER_CREATE_SCENE;
		}

		protected override void PreEnter(CStateMachine sm)
		{
			base.PreEnter(sm);
			m_preCreatePrefabAddress.Add("UI_CharacterCreate");
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
			Facade.instance.SendNotification(NotiConst.Open_CharacterCreate);
		}
	}
}