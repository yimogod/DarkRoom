using System.Collections;
using System.Collections.Generic;
using DarkRoom.Game;
using Sword;
using UnityEngine;

public class StartUp : MonoBehaviour {

	void Awake () {
	    Application.targetFrameRate = 30;
	    MetaParserManager mm = new MetaParserManager();
	    mm.Init();
	    mm.Execute();
    }

    void Start()
    {
        ApplicationFacade.instance.Startup();

        ApplicationManager.Instance.InitializeProcedure(
            new CharacterEntry_Procedure(),
            new CharacterCreate_Procedure(),
            new CharacterChoose_Procedure(),
            new PlayerCastle_Procedure(),
            new DungeonBattle_Procedure()
            );

        ApplicationManager.Instance.AppLaunch();
    }

    void Update () {
		
	}
}
