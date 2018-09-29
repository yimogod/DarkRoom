using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sword
{
	public class ActorAnimController : MonoBehaviour
	{
		public const string Idle_Key = "RandomIdle";
		public const string Move_Key = "ForwardSpeed";

		private Animator m_anim;
		private ActorEntity m_entity;

		void Start()
		{
			m_entity = GetComponent<ActorEntity>();
		}

		public void AttachAnimator(Animator anim)
		{
			m_anim = anim;
		}

		void Update()
		{
			if(m_anim==null)return;
			var speed = m_entity.Mover.Velocity.magnitude;
			m_anim.SetFloat(Move_Key, speed);
		}

		void LateUpdate()
		{
			m_anim.transform.localPosition = Vector3.zero;
		}
	}
}

