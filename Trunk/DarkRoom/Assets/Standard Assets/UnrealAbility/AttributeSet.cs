using System;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility
{
    public class UAttributeSet
    {
        /** 在修改任何一个属性前会被调用
         * 这里没有提供多余的context, 因为任何事情都可能触发这个. 比如 执行的effect, dot effect, 被移除的effect, apply的immunity(无敌, 免疫)
         * 这个方法给机会处理类似于 Health = Clamp(Health, 0, MaxHealth) 而不是 trigger this extra thing if damage is applied, etc
         * NewValue有可会被改变
         */
        public virtual void PreAttributeChange(FGameplayAttribute Attribute, ref float NewValue)
        {

        }

        /**
         * 任何修改attribute的 base value前会被调用
         * 这个方法不能调用gameplay相关的事件或者callback
         */
        public virtual void PreAttributeBaseChange(FGameplayAttribute Attribute, ref float NewValue)
        {

        }

        /**
         *	Called just before a GameplayEffect is executed to modify the base value of an attribute. No more changes can be made.
         *	Note this is only called during an 'execute'. E.g., a modification to the 'base value' of an attribute. It is not called during an application of a GameplayEffect, such as a 5 ssecond +10 movement speed buff.
         */
        public virtual void PostGameplayEffectExecute(ref FGameplayEffectModCallbackData Data)
        {

        }

        public virtual void InitFromMetaDataTable(CBaseMeta DataTable)
        {

        }

        public CUnitEntity GetOwningActor()
        {
            return null;
        }

        public UAbilitySystemComponent GetOwningAbilitySystemComponent()
        {
            return null;
        }

        public CGameplayAbilityActorInfo GetActorInfo()
        {
            CGameplayAbilityActorInfo a;
            return a;
        }

        public virtual void PrintDebug()
        {

        }
    }

    public struct FGameplayAttribute
    {
        private string Attribute;
        private object AttributeOwner;
        private string AttributeName;

        public FGameplayAttribute(string NewProperty)
        {
            Attribute = NewProperty;
            AttributeOwner = null;
            AttributeName = "";
        }

        public bool IsValid()
	    {
		    return Attribute != null;
	    }

        public void SetUProperty(string NewProperty)
        {
            Attribute = NewProperty;
            if (NewProperty != null)
            {
                //AttributeOwner = Attribute->GetOwnerStruct();
                //Attribute->GetName(AttributeName);
            }
            else
            {
                AttributeOwner = null;
                AttributeName = null;
            }
        }

        public object GetUProperty()
	    {
		    return Attribute;
	    }

	    public Type GetAttributeSetClass()
	    {
	        return Attribute.GetType();
	    }

        public bool IsSystemAttribute()
        {
            return false;
        }

        /** Returns true if the variable associated with Property is of type FGameplayAttributeData or one of its subclasses */
        public static bool IsGameplayAttributeDataProperty(object Property)
        {
            return false;
        }

        public void SetNumericValueChecked(float NewValue, UAttributeSet Dest)
        {

        }

        public float GetNumericValue(UAttributeSet Src)
        {
            return 1f;
        }

        public float GetNumericValueChecked(UAttributeSet Src)
        {
            return 1f;
        }

        public FGameplayAttributeData GetGameplayAttributeData(UAttributeSet Src)
        {
            FGameplayAttributeData a = new FGameplayAttributeData();
            return a;
        }

        public FGameplayAttributeData GetGameplayAttributeDataChecked(UAttributeSet Src)
        {
            FGameplayAttributeData a = new FGameplayAttributeData();
            return a;
        }

        public int GetTypeHash(FGameplayAttribute InAttribute )
        {
            // FIXME: Use ObjectID or something to get a better, less collision prone hash
            //return PointerHash(InAttribute.Attribute);
            return 0;
        }

        public string GetName()
        {
            return "";
        }

        public bool Equal(FGameplayAttribute Dest)
        {
            return string.Equals(Attribute, Dest.GetUProperty());
        }
    }

    public struct FGameplayAttributeData
    {
        private float BaseValue;
        private float CurrentValue;

        public FGameplayAttributeData(float DefaultValue)
        {
            BaseValue = DefaultValue;
            CurrentValue = DefaultValue;
        }

        public float GetCurrentValue()
        {
            return CurrentValue;
        }

        public void SetCurrentValue(float NewValue)
        {
            CurrentValue = NewValue;
        }

        public float GetBaseValue()
        {
            return BaseValue;
        }

        public void SetBaseValue(float NewValue)
        {
            BaseValue = NewValue;
        }
    }
}
