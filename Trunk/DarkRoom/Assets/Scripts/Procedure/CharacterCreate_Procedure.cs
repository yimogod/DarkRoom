using System;
using System.Collections.Generic;
using PureMVC.Patterns;
using DarkRoom.UI;
using DarkRoom.Utility;
using UnityEngine;

namespace Sword
{
    public class CharacterCreate_Procedure : CProcedureBase
    {
        public const string NAME = "CharacterCreate";

        public CharacterCreate_Procedure() : base(NAME)
        {
            m_targetSceneName = SwordConst.CHARACTER_CREATE_SCENE;
            m_preCreatePrefabAddress.Add("UI_CharacterCreate");
        }

        protected override void StartLoadingPrefab()
        {
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

        protected override void OnEntireLevelComplete()
        {
            Facade.instance.SendNotification(NotiConst.Open_CharacterCreate);
        }
    }
}
