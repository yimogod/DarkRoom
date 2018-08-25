using System.Collections;
using System.Collections.Generic;
using DarkRoom.Game;
using Sword;
using UnityEngine;

public class StartUp : MonoBehaviour {

	void Awake () {
	    Application.targetFrameRate = 30;

	    CApplicationManager.Instance.InitializeProcedure(
	        new CharacterEntry_Procedure(),
	        new CharacterCreate_Procedure(),
	        new CharacterChoose_Procedure(),
	        new PlayerCastle_Procedure(),
	        new DungeonBattle_Procedure()
	    );
    }

    void Start()
    {
        CApplicationManager.Instance.AppLaunch();
        ApplicationFacade.instance.Startup();

        CApplicationManager.Instance.ChangeProcedure(CharacterEntry_Procedure.NAME);
    }
}
