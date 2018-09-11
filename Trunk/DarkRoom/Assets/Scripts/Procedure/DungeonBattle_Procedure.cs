using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace Sword
{
    public class DungeonBattle_Procedure : CProcedureBase
    {
        public const string NAME = "DungeonBattle";

        public DungeonBattle_Procedure() : base(NAME)
        {
            m_targetSceneName = SwordConst.DUNGEON_BATTLE_SCENE;
        }

        protected override void OnEntireLevelComplete()
        {
            //Facade.instance.SendNotification(NotiConst.Open_CharacterEntry);

            DungeonBattleScene scene = GameObject.FindObjectOfType<DungeonBattleScene>();
            if(scene != null)scene.Launch();
        }
    }
}
