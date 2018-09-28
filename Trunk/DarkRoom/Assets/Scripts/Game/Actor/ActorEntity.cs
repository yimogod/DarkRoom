using System;
using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace Sword
{
	[RequireComponent(typeof(ActorAnimController))]
	public class ActorEntity : CPawnEntity
	{
		/// <summary>
		/// 配表id
		/// </summary>
		public int MetaId;

		/// <summary>
		/// 资源地址, 生成对象的时候赋值
		/// </summary>
		public string Address;

		/// <summary>
		/// 移动力
		/// </summary>
		public int MoveRange = 3;

		/// <summary>
		/// 名称
		/// </summary>
		//public string Name;

		/// <summary>
		/// actor的属性,包含蓝,红,攻,防
		/// </summary>
		public SwordAttributeSet AttributeSet = new SwordAttributeSet();

		protected CWorld m_world => CWorld.Instance;

		protected SwordGameMode m_gameMode => m_world.GetGameMode<SwordGameMode>();

		protected ActorAnimController m_anim;

		protected override void Start()
		{
			base.Start();
			LoadModelAsyn();
		}

		protected override void RegisterAllComponents()
		{
			base.RegisterAllComponents();
			m_anim = gameObject.GetComponent<ActorAnimController>();
		}

		protected void LoadModelAsyn()
		{
			AssetManager.LoadActorPrefab(Address, m_tran, OnModelLoaded);
		}

		protected virtual void OnModelLoaded(GameObject loadedObject)
		{
			m_anim.AttachAnimator(loadedObject.GetComponent<Animator>());
		}
	}
}