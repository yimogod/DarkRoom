using System;
using PureMVC.Patterns;
using UnityEngine;

namespace Sword
{
    public class Projectinitializer : MonoBehaviour
    {
        /* for test */
        public DungeonBattleScene scene;

        void Awake()
        {
            Application.targetFrameRate = 30;
            SwordMetaParserManager mm = new SwordMetaParserManager();
            mm.InitLite();
            mm.Execute();
        }

        void Start()
        {
            Facade.instance.RegisterCommand(StartupCommand.NAME, typeof(StartupCommand));
            Facade.instance.SendNotification(StartupCommand.NAME);
            if (scene != null) scene.Launch();
        }
    }
}

