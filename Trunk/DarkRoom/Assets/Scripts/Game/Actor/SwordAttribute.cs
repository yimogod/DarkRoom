using System;
using System.Collections.Generic;
using DarkRoom.GamePlayAbility;

namespace Sword
{
	/// <summary>
	/// 主属性合集
	/// </summary>
	public class SwordPrimaryAttributeSet
	{
		public CAbilityAttribute Strength;
		public CAbilityAttribute Dexterity;
		public CAbilityAttribute Intelligence;
		public CAbilityAttribute Constitution;
		public CAbilityAttribute Willpower;
		public CAbilityAttribute Luck;

		public void InitFromClassAndRace(ActorBornAttributeMeta classMeta, ActorBornAttributeMeta raceMeta)
		{
			//所有一级属性的基础值都是10, 然后根据种族和职业进行修正
			float baseValue = 0f;
			float v1 = 0;
			v1 = classMeta.Strength + raceMeta.Strength + baseValue;
			Strength = new CAbilityAttribute((int) PrimaryAttribute.Strength, v1);

			v1 = classMeta.Dexterity + raceMeta.Dexterity + baseValue;
			Dexterity = new CAbilityAttribute((int) PrimaryAttribute.Dexterity, v1);

			v1 = classMeta.Constitution + raceMeta.Constitution + baseValue;
			Constitution = new CAbilityAttribute((int) PrimaryAttribute.Constitution, v1);

			v1 = classMeta.Intelligence + raceMeta.Intelligence + baseValue;
			Intelligence = new CAbilityAttribute((int) PrimaryAttribute.Intelligence, v1);

			v1 = classMeta.Willpower + raceMeta.Willpower + baseValue;
			Willpower = new CAbilityAttribute((int) PrimaryAttribute.Willpower, v1);

			v1 = classMeta.Luck + raceMeta.Luck + baseValue;
			Luck = new CAbilityAttribute((int) PrimaryAttribute.Luck, v1);
		}
	}
}