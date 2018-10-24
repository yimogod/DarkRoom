using System;
using System.Collections.Generic;
using DarkRoom.GamePlayAbility;

namespace Sword
{
	/// <summary>
	/// 消耗的资源合集
	/// </summary>
	public class SwordResourceAttributeSet
	{
		public SwordHealthAttribute MaxHealth { private set; get; }
		public SwordManaAttribute MaxMana { private set; get; }
		public SwordStaminaAttribute MaxStamina { private set; get; }

		private SwordPrimaryAttributeSet m_primary;


		public SwordResourceAttributeSet(SwordPrimaryAttributeSet primary)
		{
			m_primary = primary;
			MaxHealth = new SwordHealthAttribute(m_primary);
			MaxMana = new SwordManaAttribute(m_primary);
			MaxStamina = new SwordStaminaAttribute(m_primary);
		}

		public void InitClassAndRace(ActorBornAttributeMeta classMeta, ActorBornAttributeMeta raceMeta, float healthRank)
		{
			MaxHealth.InitHealthRatingRank(classMeta.HealthRating + raceMeta.HealthRating, healthRank);
			MaxHealth.InitResourceFromClassRace(classMeta.Health, raceMeta.Health);

			MaxMana.InitResourceFromClassRace(classMeta.Mana, raceMeta.Mana);
			MaxStamina.InitResourceFromClassRace(classMeta.Stamina, raceMeta.Stamina);
		}

		public void SetLevel(int level)
		{
			MaxHealth.SetLevel(level);
			MaxMana.SetLevel(level);
			MaxStamina.SetLevel(level);
		}
	}

	/// <summary>
	/// 最大血量属性
	/// </summary>
	public class SwordHealthAttribute : SwordActorResource
	{
		// 每次升级增加的生命
		private float m_healthRating;

		//为npc用的计算生命的系数
		private float m_healthRank = 1f;

		public override float BaseValue => m_healthRank *
					(m_initValueFromClassRace + m_initValueFromLevel + m_primary.Constitution.Value * 4f);

		public SwordHealthAttribute(SwordPrimaryAttributeSet primary) : 
			base(primary, Resource.Health)
		{
		}

		public void InitHealthRatingRank(float healthRating, float healthRank)
		{
			m_healthRating = healthRating;
			m_healthRank = healthRank;
		}

		public override void SetLevel(int lv)
		{
			//每升一级, 增加的血量 Life Rating * (1.1+(current_level/40)) 
			//相当于lv!来计算血量 lv从2开始计算

			m_initValueFromLevel = 0;
			for (int i = 2; i <= lv; i++)
			{
				m_initValueFromLevel += i * 0.025f;
			}

			m_initValueFromLevel += 1.1f * (lv - 1);
			m_initValueFromLevel *= m_healthRating;
		}
	}

	/// <summary>
	/// 最大体力属性
	/// </summary>
	public class SwordStaminaAttribute : SwordActorResource
	{
		public override float BaseValue => base.BaseValue + m_primary.Willpower.Value * 2.5f;

		public SwordStaminaAttribute(SwordPrimaryAttributeSet primary) :
			base(primary, Resource.Stamina)
		{
		}

		public override void SetLevel(int lv)
		{
			//每升一级, 获取3点体力
			//计算属性, lv从2开始计算
			m_initValueFromLevel = (lv - 1) * 3f;
		}
	}

	/// <summary>
	/// 最大法力属性
	/// </summary>
	public class SwordManaAttribute : SwordActorResource
	{
		public override float BaseValue => base.BaseValue + m_primary.Willpower.Value * 5f;

		public SwordManaAttribute(SwordPrimaryAttributeSet primary):
			base(primary, Resource.Mana)
		{
		}

		public override void SetLevel(int lv)
		{
			//每升一级, 获取4点法力
			//计算属性, lv从2开始计算
			m_initValueFromLevel = (lv - 1) * 4f;
		}
	}



	public class SwordActorResource : CAbilityAttribute
	{
		protected SwordPrimaryAttributeSet m_primary;

		//由等级获取到初始值
		protected float m_initValueFromLevel = 0;

		//由职业和种族提供的初始化属性
		protected float m_initValueFromClassRace = 0;

		public override float BaseValue => base.BaseValue + m_initValueFromClassRace + m_initValueFromLevel;

		public SwordActorResource(SwordPrimaryAttributeSet primary, Resource id, 
			float initialValue = 0, float persistentValue = 0) : base((int)id, initialValue, persistentValue)
		{
			m_primary = primary;
		}

		public void InitResourceFromClassRace(float classResource, float raceResource)
		{
			m_initValueFromClassRace = classResource + raceResource;
		}

		//计算属性, lv从2开始计算
		public virtual void SetLevel(int lv)
		{
		}
	}
}
