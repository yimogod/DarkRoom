using System;
using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace Sword
{
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
		/// 名称
		/// </summary>
		//public string Name;

		/// <summary>
		/// actor的属性,包含蓝,红,攻,防
		/// </summary>
		public SwordAttributeSet AttributeSet = new SwordAttributeSet();

		protected CWorld m_world => CWorld.Instance;

		protected SwordGameMode m_gameMode => m_world.GetGameMode<SwordGameMode>();

		protected override void Start()
		{
			base.Start();
			LoadModelAsyn();
		}

		protected void LoadModelAsyn()
		{
			AssetManager.LoadActorPrefab(Address, m_tran, Vector3.zero);
		}
	}
}