using UnityEngine;
using System.Collections;

namespace DarkRoom.Utility
{
	//自动回到池子的visual effect
	public class CAutoBackToPoolVFX : MonoBehaviour{
		[HideInInspector]
		public Transform poolLayer = null;
		[HideInInspector]
		public Transform attachTran = null;

		private Transform _self;
		private bool _enabled = false;
		//我初始的时候是否有attach
		//如果有我就跟着走, 没有我就停留原地
		//但如果先有, 后来没有了, 那我就回收
		private bool _followAttach = false;
		private bool _dieWithAttachDie = false;

		void Start(){
			_self = transform;
		}

		public void FollowAttach(){
			_followAttach = true;
		}

		public void DieWithAttachDie(){
			_dieWithAttachDie = true;
		}

		public void DelayBackToPool(float delay){
			_enabled = true;

			if(!gameObject.activeSelf)gameObject.SetActive(true);
			if(delay > 0)Invoke("BackToPool", delay);
		}

		public void CancelBackToPool(){
			Reset();
		}

		private void BackToPool(){
			transform.SetParent(poolLayer);
			Reset();
		}

		private void Reset(){
			poolLayer = null;
			attachTran = null;
			_enabled = false;
			_followAttach = false;
			_dieWithAttachDie = false;
			CancelInvoke();

			if(gameObject.activeSelf)gameObject.SetActive(false);
		}

		void Update(){
			if (!_enabled)return;

			if (_followAttach && attachTran != null){
				_self.position = attachTran.position;
			}

			if (_dieWithAttachDie && attachTran == null){
				BackToPool();
			}
		}
	}
}