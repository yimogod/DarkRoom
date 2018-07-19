using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace Sword{
	public class AssetManager{
		//这些配置用在不同的项目中可以进行修改
		public static string FOLDER_ROOT_UI = "Prefabs/UI/";

		public static string FOLDER_ROOT_ITEM_ICON = "Prefabs/Icon/item/";
		public static string FOLDER_ROOT_HEAD_ICON = "Prefabs/Icon/head/";
		public static string FOLDER_ROOT_BODY_ICON = "Prefabs/Icon/body/";
		public static string FOLDER_ROOT_EQUIP_ICON = "Prefabs/Icon/equip/";

		public static string FOLDER_ROOT_MAP = "Prefabs/Map/{0}/{1}";

		public static string FOLDER_ROOT_TRIGGER = "Prefabs/Trigger/";
		public static string FOLDER_ROOT_ACTOR = "Prefabs/Actor/";
		public static string FOLDER_ROOT_EQUIP = "Prefabs/Equip/";
		public static string FOLDER_ROOT_SKILL = "Prefabs/Skill/";

		public static void LoadItemIcon(string name){
			//string path = string.Format("{0}{1}", FOLDER_ROOT_ITEM_ICON, name);
			//Texture2D texture = CResourceManager.LoadTexture2D(path);
			//Rect rect = new Rect(0, 0, texture.width, texture.height);
			//Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);
		}

		public static void LoadHeadIcon(string name){
			//string path = string.Format("{0}{1}", FOLDER_ROOT_HEAD_ICON, name);
			//Texture2D texture = CResourceManager.LoadTexture2D(path);
			//Rect rect = new Rect(0, 0, texture.width, texture.height);
			//Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

			//return sprite;
		}

		public static void LoadBodyIcon(string name){
			//string path = string.Format("{0}{1}", FOLDER_ROOT_BODY_ICON, name);
			//Texture2D texture = CResourceManager.LoadTexture2D(path);
			//Rect rect = new Rect(0, 0, texture.width, texture.height);
			//Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

			//return sprite;
		}

		public static void LoadEquipIcon(string name){
			//string path = string.Format("{0}{1}", FOLDER_ROOT_EQUIP_ICON, name);
			//Texture2D texture = CResourceManager.LoadTexture2D(path);
			//Rect rect = new Rect(0, 0, texture.width, texture.height);
			//Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

			//return sprite;
		}

		public static void LoadUIPrefab(string name){
			//string path = string.Format("{0}{1}", FOLDER_ROOT_UI, name);
			//GameObject go = CResourceManager.LoadAndCreatePrefab(path);
			//return go;
		}

		public static void LoadMapPrefab(string mapFolder, string name){
			//string path = string.Format(FOLDER_ROOT_MAP, mapFolder, name);
			//GameObject go = CResourceManager.LoadAndCreatePrefab(path);
			//return go;
		}

		public static void LoadActorPrefab(string name){
			//string path = string.Format("{0}{1}", FOLDER_ROOT_ACTOR, name);
			//GameObject go = CResourceManager.LoadAndCreatePrefab(path);
			//return go;
		}

		public static void LoadSkillPrefab(string name){
			//string path = string.Format("{0}{1}", FOLDER_ROOT_SKILL, name);
			//GameObject go = CResourceManager.LoadAndCreatePrefab(path);
			//return go;
		}

		public static void LoadEquipPrefab(string name){
			//string path = string.Format("{0}{1}", FOLDER_ROOT_EQUIP, name);
			//GameObject go = CResourceManager.LoadAndCreatePrefab(path);
			//return go;
		}

		public static void LoadTriggerPrefab(string name){
			//string path = string.Format("{0}{1}", FOLDER_ROOT_TRIGGER, name);
			//GameObject go = CResourceManager.LoadAndCreatePrefab(path);
			//return go;
		}

		public static void LoadAndCreatePrefab(string path){
			//GameObject go = CResourceManager.LoadAndCreatePrefab(path);
			//return go;
		}
	}
}