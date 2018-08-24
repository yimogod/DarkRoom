using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;

namespace DarkRoom.Game {
    public struct CResourceLoader
    {
        public void LoadObject<T>(string address, Action<T> onComplete = null) where T : class
        {
            var result = Addressables.LoadAsset<T>(address);
            InternalOnComplete<T>(result, onComplete);
        }

        /// <summary>
        /// 创建go并放在parent下面, 且指定坐标
        /// </summary>
        public void InstantiateGameObject(string address, Transform parent, Vector3 localPosition, Action<GameObject> onComplete = null)
        {
            var result = Addressables.Instantiate<GameObject>(address, localPosition, Quaternion.identity, parent);
            InternalOnComplete<GameObject>(result, onComplete);
        }

        private void InternalOnComplete<T>(IAsyncOperation<T> result, Action<T> onComplete)
        {
            if (onComplete == null)return;
            result.Completed += operation => onComplete(operation.Result);
        }
    }
}