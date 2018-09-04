using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace Sword
{
	public class GameUtil
	{
		//相机盯住英雄
		public static void CameraFocusHero()
		{
			//CUnitEntity hero = TMap.Instance.hero;
			// Camera cam = Camera.main;
			// if (cam == null) return;

			//FollowObjectBehaviour fo = cam.GetComponent<FollowObjectBehaviour>();
			//if (fo == null) return;

			//fo.Target = hero.transform;
		}

		/// <summary>
		/// 战斗属性的最终计算要把值做个重计算. 体现出边际效益
		/// </summary>
		public static float GetScaledBattleAttribute(float rawValue)
		{
			float result = 0;
			if (rawValue <= 20f)
			{
				result = rawValue;
			}
			else if (rawValue <= 40f)
			{
				result = 20f + (rawValue - 20f) * 0.5f;
			}
			else if (rawValue <= 60f)
			{
				result = 30f + (rawValue - 40f) * 0.3333f;
			}
			else if (rawValue <= 80f)
			{
				result = 36.6667f + (rawValue - 60f) * 0.25f;
			}
			else
			{
				result = 41.6667f + (rawValue - 80f) * 0.2f;
			}

			return result;
		}
	}
}