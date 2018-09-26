using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;

namespace DarkRoom.Utility
{
	public struct CResourceLoader
	{
		public string Address;

		public void LoadObject<T>(string address, Action<T> onComplete = null) where T : class
		{
			Address = address;
			var result = Addressables.LoadAsset<T>(address);
			InternalOnComplete<T>(result, onComplete);
		}

		/// <summary>
		/// 加载go, 但不初始化
		/// </summary>
		/// <param name="address">Address.</param>
		public void LoadGameObject(string address, Action<GameObject> onComplete)
		{
			Address = address;
			LoadObject<GameObject>(address, onComplete);
		}

		/// <summary>
		/// 创建go并放在parent下面, 且指定坐标
		/// </summary>
		public void InstantiateGameObject(string address, Transform parent, Vector3 localPosition,
			Action<GameObject> onComplete = null)
		{
			Address = address;

			//特别注意, Addressables 需要的地址是Position, 不是localPosition
			var pos = parent.position + localPosition;
			var result = Addressables.Instantiate<GameObject>(address, pos, Quaternion.identity, parent);
			InternalOnComplete<GameObject>(result, onComplete);
		}

		private void InternalOnComplete<T>(IAsyncOperation<T> result, Action<T> onComplete)
		{
			string s = Address;
			result.Completed += operation =>
			{
				if (operation.Status == AsyncOperationStatus.Failed)
				{
					Debug.LogWarningFormat("Load {0} Failed", s);
				}
			};


			if (onComplete == null) return;
			result.Completed += operation => { onComplete(operation.Result); };
		}
	}
}