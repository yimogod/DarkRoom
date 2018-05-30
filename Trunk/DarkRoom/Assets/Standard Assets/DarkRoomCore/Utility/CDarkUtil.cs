using System;
using System.Collections.Generic;
using UnityEngine;
namespace DarkRoom.Core
{
    public class CDarkUtil{
		public static void AddChild(Transform parent, Transform child, Vector3 pos){
			child.SetParent(parent, false);
			child.localScale = Vector3.one;
			child.localPosition = pos;
		}

		public static void AddChildWorld(Transform parent, Transform child, Vector3 pos){
			child.SetParent(parent, false);
			child.localScale = Vector3.one;
			child.position = pos;
		}

		public static void AddChild(Transform parent, Transform child, Vector3 pos, Vector3 scale, Vector3 euler){
			child.SetParent(parent, false);
			child.localScale = scale;
			child.localPosition = pos;
			child.localEulerAngles = euler;
		}

		public static void AddChild(Transform parent, Transform child){
			child.SetParent(parent, false);
			child.localScale = Vector3.one;
			child.localPosition = Vector3.zero;
		}

		public static void DestroyChildren(Transform parent){
			int num = parent.childCount;
			if (num == 0)return;

			for (int i = num - 1; i >= 0; i--){
				Transform child = parent.GetChild(i);
				GameObject.Destroy(child.gameObject);
			}
		}

		public static Quaternion GetRotationByDirection(Vector3 dir){
			Vector3 r = Vector3.zero;
			if(dir.z == 0){
				r.y = dir.x > 0 ? 0f : 180f;
			}if(dir.x == 0){
				r.y = dir.z > 0 ? -90f : 90f;
			}
			return Quaternion.Euler(r);
		}

		public static int GetKeyFromVec3(Vector3 value){
			return 10000 * (int)value.z + (int)value.x;
		}

		public static Vector3 GetVec3FromKey(int key){
			int row = (int)(key / 10000);
			int col = key % 10000;
			return new Vector3(col, 0, row);
		}

		/// <summary>
		/// 从格式为x,y,z的字符串中获取矢量
		/// </summary>
		public static Vector3 GetVec3FromStr(string str){
			Vector3 v = Vector3.zero;
			string[] list = str.Split(',');
			v.x = float.Parse(list[0]);
			v.y = float.Parse(list[1]);
			v.z = float.Parse(list[2]);
			return v;
		}

		/// <summary>
		/// 去除列表中的重复元素
		/// </summary>
		public static void UniqueList<T>(List<T> list){
			List<T> newList = new List<T>();
			newList.AddRange(list);
			list.Clear();

			foreach(T t in newList) {
				int i = newList.IndexOf(t);
				if(i == -1)continue;

				list.Add(t);
			}
		}
	}
}
