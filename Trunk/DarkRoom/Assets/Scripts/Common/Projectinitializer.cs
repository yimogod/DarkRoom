using System;
using UnityEngine;

namespace Sword
{
    public class Projectinitializer : MonoBehaviour
    {
        void Awake()
        {
            Application.targetFrameRate = 60;
            MetaParserManager mm = new MetaParserManager();
            mm.Init();
            mm.Execute();
        }

        void Start()
        {
            ApplicationFacade.instance.Startup();
        }
    }
}

