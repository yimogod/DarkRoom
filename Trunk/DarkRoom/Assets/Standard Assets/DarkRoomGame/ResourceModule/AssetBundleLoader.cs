using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DarkRoom.Game {

	public class AssetBundleLoader : MonoBehaviour {
		//异步加载当前的bundle的信息
		private string _bundlePath;
		private string _assetName;
		private Action<UnityEngine.Object> _onComplete;

		//已经加载过的bundle, 会在切场景去除
		private Dictionary<string, AssetBundle> _bundleDict = new Dictionary<string, AssetBundle>();

		public int version = 1;

		public void Clear() {
			foreach (KeyValuePair<string, AssetBundle> item in _bundleDict) {
				item.Value.Unload(false);
			}
			_bundleDict.Clear();
		}


#if UNITY_EDITOR
	//assets name 没有后缀, bundlePath应该是ui/xxx.u3d
	public UnityEngine.Object LoadInEditor(string bundlePath, string assetName){
		string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(bundlePath, assetName);
		if (assetPaths.Length == 0) {
			return null;
		}

		//path should be Assets/Textures/texture.png
		UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
		return asset;
	}
#endif

		//assets name 没有后缀
		public UnityEngine.Object LoadFromBunlde(string bundlePath, string assetName) {
			AssetBundle bundle = null;
			UnityEngine.Object asset = null;
			if (_bundleDict.ContainsKey(bundlePath)) {
				bundle = _bundleDict[bundlePath];
				asset = bundle.LoadAsset(assetName);
				return asset;
			}

			string outputPath = Rayman.PlatformKit.GetCurrentPlatformBundleRoot();
			outputPath = string.Format("{0}{1}", outputPath, bundlePath);
			bundle = AssetBundle.LoadFromFile(outputPath);
			if (bundle == null) {
				Debug.Log("Failed to load AssetBundle!---" + bundlePath);
				return null;
			}

			_bundleDict[bundlePath] = bundle;
			asset = bundle.LoadAsset(assetName);
			return asset;
		}

		public void LoadFromBundleAsync(string bundlePath, string assetName, Action<UnityEngine.Object> onComplete) {
			if (_bundleDict.ContainsKey(bundlePath)) {
				AssetBundle bundle = _bundleDict[bundlePath];
				UnityEngine.Object asset = bundle.LoadAsset(assetName);
				onComplete(asset);
				return;
			}


			int idx = assetName.LastIndexOf(".");
			if (idx > 0) assetName = assetName.Substring(0, idx);

			_bundlePath = bundlePath;
			_assetName = assetName;
			_onComplete = onComplete;

			StartCoroutine(DownloadAndCache());
		}

		IEnumerator DownloadAndCache() {
			// Wait for the Caching system to be ready
			while (!Caching.ready) yield return null;

			//bunlde path shoule be http://myserver.com/myassetBundle.unity3d
			// Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
			using (WWW www = WWW.LoadFromCacheOrDownload(_bundlePath, version)) {
				yield return www;
				if (www.error != null)
					throw new Exception("WWW download had an error:" + www.error);
				AssetBundle bundle = www.assetBundle;
				if (_assetName == "") {
					_onComplete(bundle.mainAsset);
				} else {
					_onComplete(bundle.LoadAsset(_assetName));
				}

				// Unload the AssetBundles compressed contents to conserve memory
				bundle.Unload(false);
			}
		}
	}
}