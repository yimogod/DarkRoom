using System;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 行为, 根据SC2, 典型的一个behavior就是添加buff
	/// 
	/// 在UE4中, effect用来做buff了
	/// </summary>
	[RequireComponent(typeof(CAIController))]
	public class CBehavior : MonoBehaviour {
		/// <summary>
		/// buff 名称, 要跟meta名称对应
		/// </summary>
		public string BehaviorName;

		public CBehaviorMeta.BehaviorType Type = CBehaviorMeta.BehaviorType.ApplyBuff;

		//buff在我的身上
		protected CAIController m_owner;

		//谁引起的buff
		protected CAIController m_from;
		protected CAIController m_to;
		protected Vector3 m_toPos;

		/// <summary>
		/// 效果配置信息
		/// </summary>
		public CBehaviorMeta MetaBase {
			get {
				if (string.IsNullOrEmpty(BehaviorName)) {
					Debug.LogError("Notice BehaviorName is null");
					return null;
				}
				return CBehaviorMetaManager.GetMeta(BehaviorName);
			}
		}

		protected virtual void Awake(){
			m_owner = gameObject.GetComponent<CAIController>();
		}

		protected virtual void Start() {

		}

		protected virtual void Update()
		{
			
		}

		/// <summary>
		/// from是buff来源于谁--技能释放者或者飞弹之类的
		/// to是buff挂在谁身上
		/// </summary>
		public virtual void Apply(CAIController from, CAIController to)
		{
			m_from = from;
			m_to = to;

			//理论上我们调用这个方法的时候, behavior已经挂在to的身上了
			if (m_owner != to) {
				Debug.LogError("buff attach to wrong target");
				return;
			}

			if (from == null) {
				Debug.LogError("buff has not instigator");
				return;
			}


		}

		protected virtual void OnDestroy(){
			m_owner = null;
		}


		/// <summary>
		/// 根据配表创建behavior
		/// </summary>
		public static CBehavior Create(string meta, GameObject go) {
			CBehavior behavior = null;

			CBehaviorMeta emeta = CBehaviorMetaManager.GetMeta(meta);
			switch (emeta.Type) {
				case CBehaviorMeta.BehaviorType.ApplyBuff:
					behavior = go.AddComponent<CBehaviorBuff>();
					behavior.BehaviorName = meta;
					break;
			}

			return behavior;
		}
	}
}