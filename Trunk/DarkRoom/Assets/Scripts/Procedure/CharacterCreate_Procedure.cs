using System;
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
        }

        public override void Enter(CStateMachine sm)
        {
            base.Enter(sm);
        }

        protected override void OnPostEnterLevel()
        {
            Facade.instance.SendNotification(NotiConst.Open_CharacterCreate);
        }
    }
}
