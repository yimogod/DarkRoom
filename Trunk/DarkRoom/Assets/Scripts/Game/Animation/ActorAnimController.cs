using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sword
{
	public class ActorAnimController : MonoBehaviour
	{
		public const string Idle_Key = "RandomIdle";
		public const string Move_Key = "ForwardSpeed";
		public const string Input_Key = "InputDetected";

		[NonSerialized]
		public Animator Anim;
		private ActorEntity m_entity;
		private Transform m_tran;


		void Start()
		{
			m_entity = GetComponent<ActorEntity>();
		}

		public void AttachAnimator(Animator anim)
		{
			if(anim == null)return;
			Anim = anim;
			m_tran = anim.transform;
		}

		public void PlayerClick()
		{
			if (Anim == null) return;

			Anim.SetTrigger(Input_Key);
		}

		void Update()
		{
			if(Anim == null)return;
			var speed = m_entity.Mover.Velocity.magnitude;
			Anim.SetFloat(Move_Key, speed);
		}

		void LateUpdate()
		{
			if (m_tran == null) return;
			m_tran.localPosition = Vector3.zero;
		}
	}
}

