using System;
using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.Game;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using UnityEngine;

namespace Sword
{
	public class HeroProxy : Proxy, IProxy
	{
		public const string NAME = "HeroProxy";

		public static string GetSaveSlot(string name)
		{
			return $"hero_{name}";
		}

		/// <summary>
		/// 玩家当前的英雄
		/// </summary>
		public HeroVO Hero = null;

		public HeroProxy() : base(NAME)
		{
		}

		/// <summary>
		/// 保存英雄到数据库
		/// </summary>
		public void SaveHero()
		{
			var slot = GetSaveSlot(Hero.Name);
			ES3.Save<HeroVO>(slot, Hero);
		}

		public static HeroVO Load(string name)
		{
			var slot = GetSaveSlot(name);
			HeroVO vo = null;

			if (ES3.KeyExists(slot))
			{
				vo = ES3.Load<HeroVO>(slot);
			}

			return vo;
		}

		public void CreateHero(string name, int actorClass, int actorRace)
		{
			HeroVO vo = new HeroVO
			{
				Name = name,
				Class = actorClass,
				Race = actorRace,
				Level = 1,
				AttributePoint = 5,
				SkillPoint = 0,
			};
			vo.Address = AssetManager.GetHeroModelAddress(vo.RaceEnum, vo.ClassEnum);
			Hero = vo;
			SaveHero();
		}

		/// <summary>
		/// 创建英雄的GO
		/// </summary>
		public HeroEntity CreateHeroEntity(Vector3 localPosition)
		{
			var entity = CWorld.Instance.SpawnUnit<HeroEntity>("Hero_" + Hero.Name, localPosition);
			entity.Address = Hero.Address;
			entity.Team = CUnitEntity.TeamSide.Red;

			var go = entity.gameObject;
			var ctrl = go.AddComponent<HeroController>();

			var attr = entity.AttributeSet;
			attr.InitAttr(Hero.ClassEnum, Hero.RaceEnum, 1f);
			attr.SetPrimaryAttrPersistentValue(Hero.Strength, Hero.Dexterity, Hero.Constitution,
				Hero.Magic, Hero.Willpower, Hero.Luck);
			attr.InitSubAttr();
			attr.InitLevel(Hero.Level);

			//目前我们仅仅英雄会有不同的武器
			//gameObject.AddComponent<HeroFSMComp>();
			//gameObject.AddComponent<HeroControlKeyboard>();
			Hero.Entity = entity;
			return entity;
		}
	}
}