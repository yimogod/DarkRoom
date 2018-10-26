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

		/// <summary>
		/// 行凶者伤害受害者
		/// </summary>
		public void CalDamage(ActorEntity instigator, ActorEntity victim)
		{
			var attr1 = instigator.AttributeSet;
			var attr2 = victim.AttributeSet;


		}


		public void ModifyDamage(SwordDamagePacket damagePacket)
		{
			if (damagePacket.TargetDodgeChance > 50.0f)
			{
				Debug.LogWarning("Target DodgeChance is bigger than 0.5f");
				return;
			}

			float randDoage = Random.Range(0.0f, 100.0f);
			if (randDoage < damagePacket.TargetDodgeChance)
			{
				damagePacket.OutDamage = 0;
				damagePacket.OutIsDodged = true;
				return;
			}

			if (damagePacket.SourceCritChance > 90.0f)
			{
				Debug.LogWarning("Source CritChance is bigger than 0.9f");
				return;
			}

			/*float value = damagePacket.SourceOriginalDamage;
			float randCrit = Random.Range(0.0f, 100.0f);
			if (randCrit < damagePacket.SourceCritChance)
			{
				if (damagePacket.SourceCritMultiplier < 1.5f) damagePacket.SourceCritMultiplier = 1.5f;
				value *= damagePacket.SourceCritMultiplier;
			}*/

			//float minDamage = value * 0.1f;
			//float modifyDamage = 0;//value - damagePacket.TargetArmorReduction;
			//if (modifyDamage < minDamage) modifyDamage = minDamage;
			//damagePacket.OutDamage = modifyDamage;
		}

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

		//---------------------------------------------------------------------------------
	}
}