﻿using System;
using System.Collections.Generic;
using DarkRoom.Core;
using PureMVC.Patterns;
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

        protected override void OnEntireLevelComplete()
        {
            Facade.instance.SendNotification(NotiConst.Open_CharacterCreate);
        }
    }
}
