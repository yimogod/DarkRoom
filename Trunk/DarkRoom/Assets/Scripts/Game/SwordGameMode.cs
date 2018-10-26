using System;
using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sword
{
	public class SwordGameMode : CGameMode
	{
		/// <summary>
		/// 角色的最高等级
		/// </summary>
		public const int MAX_LEVEL = 9;

		/** 当前的经验是否可以升级 */
		public bool CanLevelUp(int currLevel, int currExp)
		{
			int nextLevel = currLevel + 1;
			if (nextLevel > MAX_LEVEL) nextLevel = MAX_LEVEL;
			int requiredExp = 0; //HeroExpList[nextLevel];
			return currExp >= requiredExp;
		}

		/// <summary>
		/// 计算升到下一级需要的经验
		/// </summary>
		public int GetNextLevelUpExp(int level)
		{
			float result = 0;
			//ceil(10 - L(L^2 - 88L + 87)/10)
			if (level <= 29)
			{
				result = 10 - level * (level * level - 88f * level + 87f) * 0.1f;
			}
			else //ceil(10 - L(L^2 - 173L + 172)/20)
			{
				result = 10 - level * (level * level - 173f * level + 172f) * 0.05f;
			}
			return Mathf.CeilToInt(result);
		}

		public void PrintExpList(int maxLevel = 10)
		{
			for (int i = 1; i <= maxLevel; i++)
			{
				int exp = GetNextLevelUpExp(i);
				Debug.Log($"{i} lv = {exp}");
			}
		}

		//---------------------------------------------------------------------------------
	}
}