using System;
using System.Collections.Generic;
using DarkRoom.Core;
using PureMVC.Patterns;
using UnityEngine;

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
        }

        //并没有调用父类的enter方法
        public override void Enter(CStateMachine sm)
        {
            m_firstEnter = sm.LastState == null;
            //第一次进入 entry
            if (m_firstEnter)
            {
                Debug.Log("Enter Character Entry First Time");

                m_parser.Initialize();
                m_parser.ExecuteLite();
            }
            else
            {
                Debug.Log("Enter Character Entry = Come Back");
                base.Enter(sm);
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

        protected override void OnPostEnterLevel()
        {
            if (m_firstEnter)
            {
                m_parser.Dispose();

                var user = UserVO.LoadOrCreate();
                if (user.HasCurrentCharacter)
                {
                    var character = UserProxy.Load(user.CurrentCharacterName);
                    ProxyPool.UserProxy.Character = character;

                    var hero = HeroProxy.Load(user.CurrentCharacterName);
                    ProxyPool.HeroProxy.Hero = hero;
                }

                ProxyPool.UserProxy.User = user;
            }
            
            Facade.instance.SendNotification(NotiConst.Open_CharacterEntry);
        }
    }
}
