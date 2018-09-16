using DarkRoom.Core;
using DarkRoom.Game;
using UnityEngine;

using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class SystemInitCommand : SimpleCommand
    {
        public override void Execute(INotification note)
        {
            GameObject globalExist = GameObject.Find("GlobalExistComp");
            if (globalExist == null)
            {
                globalExist = new GameObject("GlobalExistComp");
            }

            CGlobalExistComp existComp = globalExist.GetComponent<CGlobalExistComp>();
            if (existComp == null)
            {
                globalExist.AddComponent<CGlobalExistComp>();
            }

            CShowFPS fpsComp = globalExist.GetComponent<CShowFPS>();
            if (fpsComp == null)
            {
                globalExist.AddComponent<CShowFPS>();
            }

            CWorld.Instance.InitializeGameMode<SwordGameMode>();
            CWorld.Instance.InitializeGameState<SwordGameState>();
        }
    }
}


