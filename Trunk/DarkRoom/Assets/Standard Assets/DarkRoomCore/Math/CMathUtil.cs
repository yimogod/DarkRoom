using System;
using UnityEngine;
namespace DarkRoom.Core
{
	public class CMathUtil {
		public static bool FloatEqual(float v1, float v2, float gap = 0.0001f) {
			float d = v1 - v2;
			if (d >= 0) return d <= gap;
			return d >= -gap;
		}

		public static bool FloatEqual(double v1, double v2, double gap = 0.0001) {
			double d = v1 - v2;
			if (d >= 0) return d <= gap;
			return d >= -gap;
		}

		public static bool IsOne(float v){
			return FloatEqual(v, 1f);
		}

		/// <summary>
		/// float=0
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public static bool IsZero(float v)
		{
			return FloatEqual(v, 0);
		}

		public static bool IsZero(double v)
		{
			return FloatEqual(v, 0);
		}

		public static bool Vector2Equal(Vector2 v1, Vector2 v2) {
			bool result = FloatEqual(v1.x, v2.x);
			if (!result) return false;

			result = FloatEqual(v1.y, v2.y);
			if (!result) return false;

			return true;
		}

		/// <summary>
		/// 三维向量是否一样
		/// </summary>
		public static bool Vector3Equal(Vector3 v1, Vector3 v2, float gap = 0.0001f) {
			bool result = FloatEqual(v1.x, v2.x, gap);
			if (!result) return false;

			result = FloatEqual(v1.y, v2.y, gap);
			if (!result) return false;

			result = FloatEqual(v1.z, v2.z, gap);
			if (!result) return false;

			return true;
		}

		/// <summary>
		/// vector是否是零向量
		/// </summary>
		public static bool ZeroVector3(Vector3 v)
		{
			return Vector3Equal(v, Vector3.zero);
		}

		public static bool QuaternionEqual(Quaternion v1, Quaternion v2) {
			bool result = FloatEqual(v1.x, v2.x);
			if (!result) return false;

			result = FloatEqual(v1.y, v2.y);
			if (!result) return false;

			result = FloatEqual(v1.z, v2.z);
			if (!result) return false;

			result = FloatEqual(v1.w, v2.w);
			if (!result) return false;

			return true;
		}

		//两个bound在xz平面上是否碰撞
		public static bool IsBoundOverLapsInXZ(Bounds a, Bounds b) {
			Rect ar = new Rect(a.min.x, a.min.z, a.size.x, a.size.z);
			Rect br = new Rect(b.min.x, b.min.z, b.size.x, b.size.z);
			return ar.Overlaps(br);
		}

		public static Bounds GetRectByCenter(Vector3 center, float length, float width) {
			return new Bounds(center, new Vector3(length, 1f, width));
		}

		/*pos是矩形的一个<尾>边的中心, 根据方向, 计算出矩形范围, dir方向上的线是length, 与dir垂直的是width*/
		public static Bounds GetRectByTailEdgeCenter(Vector3 pos, Vector3 dir, float length, float width) {
			float centerX = pos.x;
			float centerZ = pos.z;
			//左右方向
			if (dir.z == 0) {
				centerX = pos.x + dir.x * length * 0.5f;
			}

			if (dir.x == 0) {
				centerZ = pos.z + dir.z * length * 0.5f; ;
			}

			Vector3 center = new Vector3(centerX, 0, centerZ);
			return GetRectByCenter(center, length, width);
		}

		/// <summary>
		/// 从","分割的字符串获取vector
		/// </summary>
		/// <returns></returns>
		public static Vector3 GetVec3FormStr(string str) {
			string[] chara = str.Split(',');
			Vector3 v = new Vector3(float.Parse(chara[0]), float.Parse(chara[1]), float.Parse(chara[2]));
			return v;
		}

		/// <summary>
		/// 压缩角度在min,max范围中
		/// </summary>
		public static float ClampAngle(float angle, float min, float max) {
			if (angle < -360) angle += 360.0f;
			if (angle > 360) angle -= 360.0f;
			return Mathf.Clamp(angle, min, max);
		}

		/// <summary>
		/// 由Vector3计算出在xz平面的角度. 返回值如果小于0, 说明不合法
		/// </summary>
		public static float GetVec3AngleInXZ(Vector3 v)
		{

			if (FloatEqual(v.x, 0)) {
				if (v.z > 0)return 90f;
				if (v.z < 0)return 270f;
				Debug.LogError("Zero Vector Has no directon");
				return -1f;
			}


			if (FloatEqual(v.z, 0)) {
				if (v.x > 0)return 0f;
				if (v.x < 0)return 180f;
				Debug.LogError("Zero Vector Has no directon");
				return -1f;
			}

			//TODO 用查表进行优化
			float angle = Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg;
			if (angle < 0) angle += 180f;

			return angle;
		}

		/// <summary>
		/// 极坐标换算二位坐标
		/// angle是角度
		/// </summary>
		public static Vector3 GetPosByPolarInXZ(float radius, float angle){
			Vector3 r = Vector3.zero;
			r.x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
			r.z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
			return r;
		}
	}
}