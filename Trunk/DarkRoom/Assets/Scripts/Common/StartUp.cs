﻿using DarkRoom.Game;
using Sword;
using UnityEngine;
using PureMVC.Patterns;

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
        Facade.instance.RegisterCommand(StartupCommand.NAME, typeof(StartupCommand));
        Facade.instance.SendNotification(StartupCommand.NAME);

        CApplicationManager.Instance.ChangeProcedure(CharacterEntry_Procedure.NAME);
    }
}
