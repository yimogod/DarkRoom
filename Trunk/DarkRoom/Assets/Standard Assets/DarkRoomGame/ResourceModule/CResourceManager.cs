using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DarkRoom.Game{
	public class CResourceManager{

		public static string GetStreamAssetsPath(RuntimePlatform platform)
		{
		    var result = Addressables.LoadAsset<GameObject>("path");

			string path = string.Empty;

			switch (platform) {
				case RuntimePlatform.Android:
					path = string.Format("jar:file://{0}!/assets/", Application.dataPath);
					break;

				case RuntimePlatform.IPhonePlayer:
					path = string.Format("{{0}/Raw/}", Application.dataPath);
					break;

				case RuntimePlatform.WindowsPlayer:
				case RuntimePlatform.WindowsEditor:
					path = string.Format("file://{0}/StreamingAssets/", Application.dataPath);
					break;
			}

			return path;
        }

        /// <summary>
        /// 实例化address prefab, 且放在parent下面
        /// </summary>
		public static void InstantiatePrefab(string address, Transform parent, Vector3 localPosition, Action<GameObject> onComplete = null)
		{
            CResourceLoader loader = new CResourceLoader();
            loader.InstantiateGameObject(address, parent, localPosition, onComplete);
		}

	    public static void InstantiatePrefab(string address, Transform parent, Action<GameObject> onComplete = null)
	    {
	        CResourceLoader loader = new CResourceLoader();
	        loader.InstantiateGameObject(address, parent, Vector3.zero, onComplete);
	    }

        /// <summary>
        /// Loads the prefab.
        /// </summary>
        public static void LoadPrefab(string address, Action<GameObject> onComplete = null){
            CResourceLoader loader = new CResourceLoader();
            loader.LoadGameObject(address, onComplete);
        }

        public static void InstantiateSprite(string address)
        {
			
		}

		/// <summary>
        /// 加载icon之类, 我们会放入image控件
        /// </summary>
        /// <param name="address"></param>
		public static void LoadTexture2D(string address)
		{
		}

        /// <summary>
        /// 加载文本
        /// </summary>
        public static void LoadText(string address, Action<string> onComplete)
	    {
            CResourceLoader loader = new CResourceLoader();
            loader.LoadObject<UnityEngine.Object>(address, o => {
                onComplete(o.ToString());
            });
        }
	}
}
