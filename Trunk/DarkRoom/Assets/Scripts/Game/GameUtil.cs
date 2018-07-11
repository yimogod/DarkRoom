using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace Sword
{

    public class GameUtil
    {
        //相机盯住英雄
        public static void CameraFocusHero()
        {
            //CUnitEntity hero = TMap.Instance.hero;
           // Camera cam = Camera.main;
           // if (cam == null) return;

            //FollowObjectBehaviour fo = cam.GetComponent<FollowObjectBehaviour>();
            //if (fo == null) return;

            //fo.Target = hero.transform;
        }

        //在一个方向上复制多个basego
        public static void ExtendTileSprite(GameObject baseGo, int times, Vector3 offset)
        {
            Transform parent = baseGo.transform.parent;
            Vector3 pos = baseGo.transform.localPosition;

            for (int i = 0; i < times; ++i)
            {
                Vector3 gpos = pos + offset * i;
                GameObject go = GameObject.Instantiate(baseGo);
                CDarkUtil.AddChild(parent, go.transform, gpos);
            }
        }
    }

}