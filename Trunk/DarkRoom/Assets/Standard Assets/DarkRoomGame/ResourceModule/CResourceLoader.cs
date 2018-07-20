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
        /// 创建go并放在parent下面, 且指定坐标
        /// </summary>
        public void InstantiateGameObject(string address, Transform parent, Vector3 localPosition)
        {
            Addressables.Instantiate<GameObject>(address, localPosition, Quaternion.identity, parent);
        }
    }
}