using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace Sword
{
    public class CharacterEntry_Procedure : CProcedureBase
    {
        public const string NAME = "CharacterEntry";

        private SwordMetaParserManager m_parser = new SwordMetaParserManager();

        private bool m_parseComplete = false;

        public CharacterEntry_Procedure() : base(NAME)
        {
            m_targetSceneName = SwordConst.CHARACTER_ENTRY_SCENE;
        }

        //并没有调用父类的enter方法
        public override void Enter(CStateMachine sm)
        {
            //第一次进入 entry
            if (sm.LastState == null)
            {
                Debug.Log("Enter Character Entry First Time");

                m_parser.Initialize();
                m_parser.ExecuteLite();
            }
            else
            {
                Debug.Log("Enter Character Entry = Come Back");
            }
        }

        public override void Execute(CStateMachine sm)
        {
            base.Execute(sm);

            if (sm.LastState == null && !m_parseComplete)
            {
                m_parseComplete = m_parser.ExcuteNextMain();
                if (m_parseComplete)StartLoading();
            }
        }

        protected override void OnPostEnterSceneComplete()
        {
            m_parser.Dispose();

            ApplicationFacade.instance.SendNotification(NotiConst.Open_CharacterEntry);
        }
    }
}
