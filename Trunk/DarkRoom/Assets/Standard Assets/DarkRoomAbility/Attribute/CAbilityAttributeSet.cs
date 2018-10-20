using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility
{
	/// <summary>
	/// 血, 蓝, 力, 敏, 耐等的属性定义
	/// </summary>
	public class CAbilityAttribute
	{
		//属性定义的唯一值
		public int Id { get; private set; }

		//属性当前的值
		public float Value => BaseValue + AddOnValue;

		public float BaseValue => InitialValue + PersistentValue;

		//属性的初始值
		public virtual float InitialValue { get; private set; }

		//属性持久化增加的值, 玩家分配的属性点
		public float PersistentValue { get; private set; }

		//属性临时添加的值, 比如武器, buff等, 加减法
		public float AddOnValue { get; private set; }

		//属性临时添加的值, 乘除法
		public float AddOnFactor { get; private set; }

		public CAbilityAttribute(int id, float initialValue = 0, float persistentValue = 0)
		{
			Id = id;
			InitialValue = initialValue;
			PersistentValue = persistentValue;
		}

		public void ResetInitialValue(float value)
		{
			InitialValue = value;
		}

		public void AddPersistentValue(float value)
		{
			PersistentValue += value;
		}

		public void AddAddOnValue(float value)
		{
			AddOnValue += value;
		}

		public void AddAddOnFactor(float value)
		{
			AddOnFactor += value;
		}
	}

	public class CAbilityAttributeSet
	{
		/// <summary>
		/// 对属性的预处理
		/// </summary>
		public virtual void PreAttributeChange(CAbilityAttribute attribute, float newValue)
		{
		}

		/// <summary>
		/// 对属性修改的实际操作
		/// </summary>
		public virtual void PostAttributeExecute(CAbilityAttribute attribute)
		{
		}
	}

	public struct CGameplayEffectModCallbackData
	{
		public CEffect EffectSpec; // The spec that the mod came from
		public CGamePlayModifierEvaluatedData EvaluatedData; // The 'flat'/computed data to be applied to the target
		public CAbilitySystem Target; // Target we intend to apply to

		public CGameplayEffectModCallbackData(CEffect inEffectSpec,
			CGamePlayModifierEvaluatedData inEvaluatedData, CAbilitySystem inTarget)
		{
			EffectSpec = inEffectSpec;
			EvaluatedData = inEvaluatedData;
			Target = inTarget;
		}
	}

	/// <summary>
	/// 对属性的修改操作
	/// </summary>
	public class CGamePlayModifierEvaluatedData
	{
		/// <summary>
		/// 属性的英文字符, 我们用反射找对应的属性
		/// </summary>
		public string Attribute;

		/// <summary>
		/// 对属性的操作
		/// </summary>
		public CAttributeModOp ModifierOp;

		/// <summary>
		/// 修改的方法
		/// </summary>
		public CAttributeModMethod Method;

		/// <summary>
		/// 修改值
		/// </summary>
		public float Magnitude = 0;

		/// <summary>
		/// 自定义修改值的属性
		/// </summary>
		public string ModifyCalculation;

		/// <summary>
		/// buff来源的身上必须满足的tag条件
		/// </summary>
		public CGameplayTagRequirement SourceTagsRequire;

		/// <summary>
		/// buff应用的目标身上必须满足的tag条件
		/// </summary>
		public CGameplayTagRequirement TargetTagsRequire;
	}
}