using System;
using System.Collections.Generic;
using DarkRoom.UI;
using PureMVC.Patterns;

namespace Sword
{
    public class PlayerCastle_Procedure : CProcedureBase
    {
        public const string NAME = "PlayerCastle";

        public PlayerCastle_Procedure() : base(NAME)
        {
            m_targetSceneName = SwordConst.PLAYER_CASTLE_SCENE;
        }

        protected override void OnEntireLevelComplete()
        {
            //Facade.instance.SendNotification(NotiConst.Open_CharacterEntry);
        }
    }
}
