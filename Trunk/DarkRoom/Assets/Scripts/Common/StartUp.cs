using DarkRoom.Game;
using Sword;
using UnityEngine;
using PureMVC.Patterns;

public class StartUp : MonoBehaviour {

	void Awake () {
	    Application.targetFrameRate = 30;

	    CApplicationManager.Instance.InitializeProcedure(
	        new Procedure_CharacterEntry(),
	        new Procedure_CharacterCreate(),
	        new Procedure_CharacterChoose(),
	        new Procedure_PlayerCastle(),
	        new Procedure_DungeonBattle()
	    );
    }

    void Start()
    {
        CApplicationManager.Instance.AppLaunch();
        Facade.instance.RegisterCommand(StartupCommand.NAME, typeof(StartupCommand));
        Facade.instance.SendNotification(StartupCommand.NAME);

        CApplicationManager.Instance.ChangeProcedure(Procedure_CharacterEntry.NAME);
    }
}
