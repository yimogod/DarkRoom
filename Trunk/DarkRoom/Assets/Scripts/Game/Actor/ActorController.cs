using System;
using System.Collections.Generic;
using DarkRoom.AI;
using DarkRoom.Game;
using DarkRoom.GamePlayAbility;
using UnityEngine;

namespace Sword
{
	public class ActorController : CPlayerController, IGameplayAbilityUnit
	{

		protected SwordGameState gameState =>
			CWorld.Instance.GetGameState<SwordGameState>();

		//------------------ 实现接口 ------------------
		public GameObject GameObject => gameObject;
		public CAbilitySystem AbilitySystem { get; }
		public GameObject BuffLayer { get; }
		public GameObject EffectLayer { get; }
		public bool InValid => m_pawn.DeadOrDying;
		//-----------------------------------------------

		public bool IsFriendTeam(IGameplayAbilityUnit target)
		{
			ActorController ac = target as ActorController;
			return m_pawn.IsFriendTeam(ac.Pawn);
		}

		/// <summary>
		/// 两者是否敌人
		/// </summary>
		public bool IsEnemyTeam(IGameplayAbilityUnit target)
		{
			ActorController ac = target as ActorController;
			return m_pawn.IsEnemyTeam(ac.Pawn);
		}

		public bool CostEnoughForAbility(CAbilityCostType type, int count)
		{
			return true;
		}

		public void AbilityUseCost(CAbilityCostType type, int count)
		{
		}

		/// <summary>
		/// 查询符合的对象, 放入列表
		/// </summary>
		public void SearchUnitsWithQuery(List<IGameplayAbilityUnit> searchResult)
		{
		}

		public float GetSquaredXZDistanceTo_NoRadius(IGameplayAbilityUnit target)
		{
			return GetSquaredXZDistanceTo_NoRadius(target.LocalPosition);
		}
	}
}