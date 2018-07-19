using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DarkRoom.Game {

    public struct CResourceLoader
    {
        public void LoadGameObject(string address)
        {
            Addressables.LoadAsset<GameObject>(address);
        }

        /// <summary>
        /// 创建go并放在parent下面
        /// </summary>
        public void InstantiateGameObject(string address, Transform parent)
        {
            Addressables.Instantiate<GameObject>(address, parent);
        }
    }
}