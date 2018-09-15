using System;
using System.Collections.Generic;
using DarkRoom.UI;
using UnityEngine;

namespace Sword
{
    public class Procedure_DungeonBattle : CProcedureBase
    {
        public const string NAME = "DungeonBattle";

        public Procedure_DungeonBattle() : base(NAME)
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
