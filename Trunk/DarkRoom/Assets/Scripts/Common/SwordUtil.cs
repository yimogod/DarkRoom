using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace Sword
{
	public class SwordUtil
	{
		/// <summary>
		/// 获取父class类
		/// </summary>
		public static MetaClass GetMetaClass(ActorClass actorClass)
		{
			MetaClass v = MetaClass.Warrier;
			switch (actorClass)
			{
				case ActorClass.Berserker:
					v = MetaClass.Warrier;
					break;
				case ActorClass.Alchemist:
					v = MetaClass.Mage;
					break;
			}
			return v;
		}

		/// <summary>
		/// 获取父类种族
		/// </summary>
		public static MetaRace GetMetaRace(ActorRace actorRace)
		{
			MetaRace v = MetaRace.Human;
			switch (actorRace)
			{
				case ActorRace.Cornac:
				case ActorRace.Higher:
					v = MetaRace.Human;
					break;
				case ActorRace.Shalore:
				case ActorRace.Thalore:
					v = MetaRace.Elf;
					break;
			}
			return v;
		}

		/// <summary>
		/// 获取职业的string
		/// </summary>
		public static string GetClassName(ActorClass actorClass)
		{
			return actorClass.ToString();
		}

		/// <summary>
		/// 根据名称获取枚举
		/// </summary>
		public static ActorClass GetClassEnum(string name)
		{
			return (ActorClass)CStringEnum.Parse(typeof(ActorClass), name, true);
		}

		/// <summary>
		/// 获取职业的string
		/// </summary>
		public static string GetRaceName(ActorRace race)
		{
			return race.ToString();
		}

		/// <summary>
		/// 根据名称获取枚举
		/// </summary>
		public static ActorRace GetRaceEnum(string name)
		{
			return (ActorRace)CStringEnum.Parse(typeof(ActorRace), name, true);
		}
	}
}