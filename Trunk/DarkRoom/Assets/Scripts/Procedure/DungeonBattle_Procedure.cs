using System;
using System.Collections.Generic;
using DarkRoom.Core;

namespace Sword
{
    public class DungeonBattle_Procedure : CProcedureBase
    {
        public const string NAME = "DungeonBattle";

        public DungeonBattle_Procedure() : base(NAME)
        {
            m_targetSceneName = SwordConst.DUNGEON_BATTLE_SCENE;
        }

        protected override void OnPostEnterLevel()
        {
            //Facade.instance.SendNotification(NotiConst.Open_CharacterEntry);
        }
    }
}
