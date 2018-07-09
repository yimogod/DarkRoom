using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.Utility
{
	public class CVisualEffectLauncher{
		/// <summary>
		/// 播放地面点击特效
		/// </summary>
		public static void PlayClickEffectOnGround(GameObject effect, Vector3 worldPosition) {
			LaunchSingletonEffect_Static(effect, worldPosition);
		}

		/// <summary>
		/// 播放角色选中特效
		/// </summary>
		public static void PlayActorSelectedEffectOnGround(GameObject effect, Transform actor) {
			LaunchSingletonEffect_FollowUnit(effect, actor);
		}

		/// <summary>
		/// 播放鼠标点击特效
		/// </summary>
		public static GameObject LaunchSingletonEffect_Static(GameObject effectBase, Vector3 worldPosition){
			Transform parent = CWorld.Instance.Layer.UnitLayer;
			Vector3 localPosition =  parent.worldToLocalMatrix.MultiplyPoint3x4(worldPosition);

			return LaunchSingletonEffect(effectBase, localPosition, Quaternion.identity, 1f, null, false, false);
		}

		//鼠标选中人的特效. 人死亡, 特效消失.
		public static GameObject LaunchSingletonEffect_FollowUnit(GameObject effectBase, Transform attachObj){
			return LaunchSingletonEffect(effectBase, Vector3.zero, Quaternion.identity, 1f, attachObj, true, true);
		}

		//鼠标点击这种的只能有一个实例. 如果前面的正在播放. 那就重新播放
		public static GameObject LaunchSingletonEffect(GameObject effectBase, Vector3 localPosition,
			Quaternion orientation, float scale, Transform attachObj, bool dieWithAttach, bool followAttach){
			if (effectBase == null){
				Debug.LogError("Effect base is null when u want to launch it");
				return null;
			}

			/*GameObject go = CWorld.Instance.FindEffectInPool(effectBase.name);
			if (go == null){
				go = CWorld.Instance.FindEffectInLayer(effectBase.name);
			}

			if (go == null){
				go = GameObject.Instantiate<GameObject>(effectBase);
				go.name = effectBase.name;
			}

			go.SetActive(false);
			go.SetActive(true);

			//如果正在播放, 就取消回池子的调用
			CAutoBackToPoolVFX pool = go.GetComponent<CAutoBackToPoolVFX>();
			if (pool == null){
				pool = go.AddComponent<CAutoBackToPoolVFX>();
			}
			pool.CancelBackToPool();
			pool.poolLayer = CWorld.Instance.PoolLayer;
			pool.attachTran = attachObj;
			if (dieWithAttach)
				pool.DieWithAttachDie();
			if (followAttach)
				pool.FollowAttach();

			float minValue = GetMinTimeWithGoEffectNeed(go, localPosition, orientation, scale);
			pool.DelayBackToPool(minValue);*/

			//return go;
		    return null;
		}

		public static GameObject LaunchTweenEffect(GameObject effectBase, float tweenDuation){
			if (effectBase == null){
				Debug.LogError("Effect base is null when u want to launch it");
				return null;
			}

			/*GameObject go = CWorld.Instance.FindEffectInPool(effectBase.name);
			if (go == null){
				go = GameObject.Instantiate<GameObject>(effectBase);
				go.name = effectBase.name;
			}
			go.SetActive(true);
			CWorld.Instance.PutBackToEffectPool(go, tweenDuation);*/

			//return go;
		    return null;
		}

		public static GameObject LaunchEffect(GameObject effectBase, Vector3 localPosition,
			Quaternion orientation, float scale, Transform attachObj, bool destroy){
			if (effectBase == null){
				Debug.LogError("Effect base is null when u want to launch it");
				return null;
			}

			/*GameObject go = CWorld.Instance.FindEffectInPool(effectBase.name);
			if (go == null){
				go = GameObject.Instantiate<GameObject>(effectBase);
				go.name = effectBase.name;
			}
			go.SetActive(true);

			float minValue = GetMinTimeWithGoEffectNeed(go, localPosition, orientation, scale);
			if (minValue > 0){
				if (destroy){
					GameObject.Destroy(go, minValue);
				} else{
					CWorld.Instance.PutBackToEffectPool(go, minValue);
				}
			}
			return go;*/

		    return null;
        }

		private static float GetMinTimeWithGoEffectNeed(GameObject go, Vector3 localPosition, Quaternion orientation, float scale){
			Transform transform = go.transform;
			transform.SetParent(CWorld.Instance.Layer.UnitLayer);
			transform.localPosition = localPosition;
			transform.localScale = Vector3.one * scale;
			transform.rotation = orientation;

			float minValue = float.MinValue;
			foreach (ParticleSystem system in go.GetComponentsInChildren<ParticleSystem>()){
				system.enableEmission = true;
				float duration = system.duration + system.startDelay;
				if (duration > minValue)minValue = duration;
			}

			foreach (AudioSource source in go.GetComponentsInChildren<AudioSource>()){
				if (source.clip == null)continue;
				if (source.isPlaying)continue;
				if (source.playOnAwake)continue;
				if (source.clip.length > minValue)minValue = source.clip.length;
			}

			return minValue;
		}

		//end of class
	}
}