using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.Game{
	public class ResourceManager{
		private static AssetBundleLoader _loader;

		/*asset文件所在的bundle, key是asset路径, value是bundle name*/
		private static Dictionary<string, string> _assetMapBundleDict = new Dictionary<string, string>();

		public static string temporaryCachePath{
			get{ return Application.temporaryCachePath; }
		}

		public static string persistentDataPath{
			get{ return Application.persistentDataPath; }
		}

		public static string GetStreamAssetsPath(RuntimePlatform platform) {
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

		public static string streamAssetsPath{
			get{ return GetStreamAssetsPath(Application.platform); }
		}

		public static void Init(){
			_loader = GameObject.FindObjectOfType<AssetBundleLoader>();
		}

		public static void AddAssetMap(string asset, string bundle){
			_assetMapBundleDict.Add(asset, bundle);
		}

		public static void Clear(){
			_loader.Clear();
		}

		//path 应该是Assets/Prefabs/UI/xxx.prefab
		public static GameObject LoadAndCreatePrefab(string path){
			UnityEngine.Object asset = LoadAsset(path);
			if(asset == null)return null;
			if(asset is GameObject)return  GameObject.Instantiate(asset) as GameObject;

			Debug.LogError(string.Format("{0} is not <GameObject>", path));
			return null;
		}

		public static Sprite LoadSprite(string path){
			UnityEngine.Object asset = LoadAsset(path);
			if(asset == null)return null;
			if(asset is Sprite)return asset as Sprite;

			Debug.LogError(string.Format("{0} is not <Sprite>", path));
			return null;
		}

		//一般加载图片都是icon, 而我们明确知道icon的bundle name
		public static Texture2D LoadTexture2D(string path, string bundleName = null){
			UnityEngine.Object asset = LoadAsset(path, bundleName);
			if(asset == null)return null;
			if(asset is Texture2D)return asset as Texture2D;

			Debug.LogError(string.Format("{0} is not <Texture2D>", path));
			return null;
		}

		//如果不提供bundlename,我们就去表里面查询
		private static UnityEngine.Object LoadAsset(string path, string bundleName = null){
			if(!_assetMapBundleDict.ContainsKey(path)){
				Debug.LogError(string.Format("{0} do not in some bundle. Try to Run Create BundleName", path));
				return null;
			}

			if(string.IsNullOrEmpty(bundleName))bundleName = _assetMapBundleDict[path];
			if(string.IsNullOrEmpty(bundleName)) {
				Debug.LogError(string.Format("{0} bundle is not in map meta file. Try to Run Create BundleName", path));
			}

		    string fileName = "";//CDarkUtil.GetFileNameFromPath(path);
			Debug.Log(fileName);

			if (_loader == null) {
				Debug.LogError("Please add AssetBundleLoader to global GameObject || Run ResourceManager.Init()");
			}
			UnityEngine.Object asset = _loader.LoadFromBunlde(bundleName, fileName);


			return asset;
		}
	}
}
