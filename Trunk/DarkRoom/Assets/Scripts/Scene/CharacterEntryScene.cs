using System.Collections;
using System.Collections.Generic;
using DarkRoom.Utility;
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

		public void LoadHero(HeroVO hero)
		{
			if(hero == null)return;
			CResourceManager.InstantiatePrefab(hero.Address, HeroRoot);
		}
	}
}