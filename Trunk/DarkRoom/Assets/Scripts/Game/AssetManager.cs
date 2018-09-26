using System;
using UnityEngine;
using DarkRoom.Utility;

namespace Sword
{
	public class AssetManager
	{
		//这些配置用在不同的项目中可以进行修改
		public static string FOLDER_ROOT_ITEM_ICON = "Prefabs/Icon/item/";
		public static string FOLDER_ROOT_HEAD_ICON = "Prefabs/Icon/head/";
		public static string FOLDER_ROOT_BODY_ICON = "Prefabs/Icon/body/";
		public static string FOLDER_ROOT_EQUIP_ICON = "Prefabs/Icon/equip/";

		public static string FOLDER_ROOT_MAP = "Prefabs/Map/{0}/{1}";

		public static string FOLDER_ROOT_SKILL = "Prefabs/Skill/";

		/// <summary>
		/// 通过种族职业获取英雄图标的地址
		/// </summary>
		public static string GetHeroIconAddress(string actorRace, string actorClass)
		{
			return $"Icon_{actorRace}_{actorClass}";
		}

		/// <summary>
		/// 通过职业和种族, 获取英雄模型地址
		/// </summary>
		public static string GetHeroModelAddress(ActorRace actorRace, ActorClass actorClass)
		{
			string rn = SwordUtil.GetRaceName(actorRace);
			string cn = SwordUtil.GetClassName(actorClass);
			return GetHeroModelAddress(rn, cn);
		}

		/// <summary>
		/// 获取英雄模型地址
		/// </summary>
		public static string GetHeroModelAddress(string actorRace, string actorClass)
		{
			return $"Hero_{actorRace}_{actorClass}";
		}

		/// <summary>
		/// 加载actor的模型, 我们的模型都是独立的. 且都是simply化的字符串
		/// 注意, hero的address是不需要prefab后缀的, 
		/// 因为在address工具里面, 如果使用了simple功能产生的名字是没用prefab后缀
		/// </summary>
		public static void LoadActorPrefab(string address, Transform parent, Vector3 localPosition)
		{
			CResourceManager.InstantiatePrefab(address, parent, localPosition);
		}

		public static void LoadItemIcon(string name)
		{
			//string path = string.Format("{0}{1}", FOLDER_ROOT_ITEM_ICON, name);
			//Texture2D texture = CResourceManager.LoadTexture2D(path);
			//Rect rect = new Rect(0, 0, texture.width, texture.height);
			//Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);
		}

		public static void LoadHeadIcon(string name)
		{
			//string path = string.Format("{0}{1}", FOLDER_ROOT_HEAD_ICON, name);
			//Texture2D texture = CResourceManager.LoadTexture2D(path);
			//Rect rect = new Rect(0, 0, texture.width, texture.height);
			//Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

			//return sprite;
		}

		public static void LoadBodyIcon(string name)
		{
			//string path = string.Format("{0}{1}", FOLDER_ROOT_BODY_ICON, name);
			//Texture2D texture = CResourceManager.LoadTexture2D(path);
			//Rect rect = new Rect(0, 0, texture.width, texture.height);
			//Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

			//return sprite;
		}

		public static void LoadEquipIcon(string name)
		{
			//string path = string.Format("{0}{1}", FOLDER_ROOT_EQUIP_ICON, name);
			//Texture2D texture = CResourceManager.LoadTexture2D(path);
			//Rect rect = new Rect(0, 0, texture.width, texture.height);
			//Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

			//return sprite;
		}

		public static void LoadMapPrefab(string mapFolder, string name)
		{
			//string path = string.Format(FOLDER_ROOT_MAP, mapFolder, name);
			//GameObject go = CResourceManager.LoadAndCreatePrefab(path);
			//return go;
		}

		/// <summary>
		/// 加载地图tile的资源
		/// </summary>
		public static void LoadTilePrefab(string mapAddress, string name, Transform parent, Vector3 localPosition)
		{
			name = $"{mapAddress}/{name}.prefab";
			CResourceManager.InstantiatePrefab(name, parent, localPosition);
		}

		public static void LoadSkillPrefab(string name)
		{
			//string path = string.Format("{0}{1}", FOLDER_ROOT_SKILL, name);
			//GameObject go = CResourceManager.LoadAndCreatePrefab(path);
			//return go;
		}
	}
}