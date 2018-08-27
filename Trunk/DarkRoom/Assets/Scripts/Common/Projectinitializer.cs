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
            mm.Initialize();
            mm.ExecuteLite();
            mm.ExecuteMain();
        }

        void Start()
        {
            (Facade.instance as ApplicationFacade).Startup();
			if (scene != null) scene.Launch();
        }
    }
}

