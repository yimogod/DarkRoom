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

		//path 应该是Assets/Prefabs/UI/xxx.prefab
		public static void InstantiatePrefab(string address, Transform parent)
		{
            CResourceLoader loader = new CResourceLoader();
            loader.InstantiateGameObject(address, parent);
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
	}
}
