using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace Sword
{
    public class CharacterEntry_Procedure : CProcedureBase
    {
        public const string NAME = "CharacterEntry";

        protected override string m_targetSceneName => "";

        public CharacterEntry_Procedure() : base(NAME)
        {
        }

        public override void Enter(CStateMachine sm)
        {
            base.Enter(sm);

            //第一次进入 entry
            if (sm.LastState == null)
            {
                Debug.Log("Enter Character Entry First Time");

                MetaParserManager mm = new MetaParserManager();
                mm.Init();
                mm.Execute();
            }
            else
            {
                Debug.Log("Enter Character Entry = Come Back");
            }
        }

        public override void Exit(CStateMachine sm)
        {

        }
    }
}
