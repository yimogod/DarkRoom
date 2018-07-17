using System;
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
            MetaParserManager mm = new MetaParserManager();
            mm.Init();
            mm.Execute();
        }

        void Start()
        {
            ApplicationFacade.instance.Startup();

			if (scene != null) scene.Launch();
        }
    }
}

