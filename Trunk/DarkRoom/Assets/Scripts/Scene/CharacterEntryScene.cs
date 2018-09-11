using System.Collections;
using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace Sword
{
	public class CharacterEntryScene : MonoBehaviour
	{
		/// <summary>
		/// 加载英雄模型的
		/// </summary>
		public Transform HeroRoot;

		void Awake()
		{
			if (HeroRoot == null)Debug.LogError("HeroRoot Must Not be Null");
		}

		public void LoadHero(ActorBornAttributeMeta meta)
		{
			CResourceManager.InstantiatePrefab(meta.Address, HeroRoot);
		}
	}
}