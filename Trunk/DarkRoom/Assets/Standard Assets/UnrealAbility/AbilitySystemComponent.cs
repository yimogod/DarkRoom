using System;
using System.Collections.Generic;
using DarkRoom.Anim;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.GamePlayAbility
{
    /// <summary>
    /// UE4 的这个类4000多行, 我先抽出来, 然后进行重构, 结合
    /// 
    /// IMPORTANT 有个关键点是--虚幻用了SubClass这种反射机制, 意味着可以直接选择class进行编程
    /// 我简化为string, 通过string, 即每个这种类型的Class都必须实现, i have class name 接口.
    /// 我们通过名字去实例化具体的类, 而不是反射. 有些跟星际一样了
    /// </summary>
    public class UAbilitySystemComponent : MonoBehaviour
    {
        public virtual void InitializeComponent()
        {
        }

        public virtual void UninitializeComponent()
        {
        }

        public virtual void OnComponentDestroyed(bool bDestroyingHierarchy)
        {
        }

        public virtual void TickComponent()
        {
        }

        /* retrieves information whether this component should be ticking taken current
         *	activity into consideration*/
        public virtual bool GetShouldTick()
        {
            return false;
        }

        /* Finds existing AttributeSet */
        public T GetSet<T>()
        {
            //return GetAttributeSubobject(typeof(T)) as T;
            return default(T);
        }

        /* Finds existing AttributeSet. Asserts if it isn't there. */

        public T GetSetChecked<T>()
        {
            //return GetAttributeSubobjectChecked(typeof(T)) as T;
            return default(T);
        }

        /* Adds a new AttributeSet (initialized to default values) */
        public T AddSet<T>()
        {
            //return (T) GetOrCreateAttributeSubobject(typeof(T));
            return default(T);
        }

        /* Adds a new AttributeSet that is a DSO (created by called in their CStor) */
        public T AddDefaultSubobjectSet<T>(T Subobject)
        {
            //SpawnedAttributes.AddUnique(Subobject);
            return Subobject;
        }

        /**
         Does this ability system component have this attribute?
        *
         @param Attribute	Handle of the gameplay effect to retrieve target tags from
        *
         @return true if Attribute is valid and this ability system component contains an attribute set that contains Attribute. Returns false otherwise.
        */
        public bool HasAttributeSetForAttribute(FGameplayAttribute Attribute)
        {
            return false;
        }

        public UAttributeSet InitStats(CBaseMeta DataTable)
        {
            return null;
        }

        /* Returns a list of all attributes for this abiltiy system component */
        public void GetAllAttributes(List<FGameplayAttribute> Attributes)
        {
        }


        public List<UAttributeSet> SpawnedAttributes;

        /* Sets the base value of an attribute. Existing active modifiers are NOT cleared and will act upon the new base value. */
        public void SetNumericAttributeBase(FGameplayAttribute Attribute, float NewBaseValue)
        {
        }

        /* Gets the base value of an attribute. That is, the value of the attribute with no stateful modifiers */
        public float GetNumericAttributeBase(FGameplayAttribute Attribute)
        {
            return 1f;
        }

        /**
         *	Applies an inplace mod to the given attribute. This correctly update the attribute's aggregator, updates the attribute set property,
         *	and invokes the OnDirty callbacks.
         *	
         *	This does not invoke Pre/PostGameplayEffectExecute calls on the attribute set. This does no tag checking, application requirements, immunity, etc.
         *	No GameplayEffectSpec is created or is applied!
         *
         *	This should only be used in cases where applying a real GameplayEffectSpec is too slow or not possible.
         */
        public void ApplyModToAttribute(FGameplayAttribute Attribute, EGameplayModOp ModifierOp,
            float ModifierMagnitude)
        {
        }

        /**
           Applies an inplace mod to the given attribute. Unlike ApplyModToAttribute this function will run on the client or server.
           This may result in problems related to prediction and will not roll back properly.
         */
        public void ApplyModToAttributeUnsafe(FGameplayAttribute Attribute,
            EGameplayModOp ModifierOp, float ModifierMagnitude)
        {
        }

        /* Returns current (final) value of an attribute */
        public float GetNumericAttribute(FGameplayAttribute Attribute)
        {
            return 1f;
        }

        public float GetNumericAttributeChecked(FGameplayAttribute Attribute)
        {
            return 1f;
        }

        // ----------------------------------------------------------------------------------------------------------------
        //
        //	GameplayEffects	
        //	
        // ----------------------------------------------------------------------------------------------------------------

        // --------------------------------------------
        // Primary outward facing API for other systems:
        // --------------------------------------------
        public FActiveGameplayEffectHandle ApplyGameplayEffectSpecToTarget(FGameplayEffectSpec GameplayEffect,
            UAbilitySystemComponent Target)
        {
            return null;
        }

        public FActiveGameplayEffectHandle ApplyGameplayEffectSpecToSelf(FGameplayEffectSpec GameplayEffect)
        {
            return null;
        }


        public FActiveGameplayEffectHandle BP_ApplyGameplayEffectSpecToTarget(FGameplayEffectSpecHandle
            SpecHandle, UAbilitySystemComponent Target)
        {
            return null;
        }


        public FActiveGameplayEffectHandle BP_ApplyGameplayEffectSpecToSelf(FGameplayEffectSpecHandle
            SpecHandle)
        {
            return null;
        }

        /* Gets the FActiveGameplayEffect based on the passed in Handle */
        public UGameplayEffect GetGameplayEffectDefForHandle(FActiveGameplayEffectHandle Handle)
        {
            return null;
        }

        /* Removes GameplayEffect by Handle. StacksToRemove=-1 will remove all stacks. */
        public bool RemoveActiveGameplayEffect(FActiveGameplayEffectHandle Handle, int StacksToRemove = -1)
        {
            return false;
        }

        /* 
          Remove active gameplay effects whose backing definition are the specified gameplay effect class
         *
          @param GameplayEffect					Class of gameplay effect to remove; Does nothing if left null
          @param InstigatorAbilitySystemComponent	If specified, will only remove gameplay effects applied from this instigator ability system component
          @param StacksToRemove					Number of stacks to remove, -1 means remove all
         */
        public void RemoveActiveGameplayEffectBySourceEffect(UGameplayEffect GameplayEffect,
            UAbilitySystemComponent InstigatorAbilitySystemComponent, int StacksToRemove = -1)
        {
        }

        /* Get an outgoing GameplayEffectSpec that is ready to be applied to other things. */
        public FGameplayEffectSpecHandle MakeOutgoingSpec(UGameplayEffect GameplayEffectClass, float Level,
            FGameplayEffectContextHandle Context)
        {
            return null;
        }

        /* Create an EffectContext for the owner of this AbilitySystemComponent */
        public FGameplayEffectContextHandle MakeEffectContext()
        {
            return null;
        }

        /**
          Get the count of the specified source effect on the ability system component. For non-stacking effects, this is the sum of all active instances.
          For stacking effects, this is the sum of all valid stack counts. If an instigator is specified, only effects from that instigator are counted.
          
          @param SourceGameplayEffect					Effect to get the count of
          @param OptionalInstigatorFilterComponent		If specified, only count effects applied by this ability system component
          
          @return Count of the specified source effect
         */
        public int GetGameplayEffectCount(UGameplayEffect SourceGameplayEffect,
            UAbilitySystemComponent OptionalInstigatorFilterComponent, bool bEnforceOnGoingCheck = true)
        {
            return 1;
        }

        /* Returns the sum of StackCount of all gameplay effects that pass query */
        public int GetAggregatedStackCount(FGameplayEffectQuery Query)
        {
            return 1;
        }

        /* This only exists so it can be hooked up to a multicast delegate */
        public void RemoveActiveGameplayEffect_NoReturn(FActiveGameplayEffectHandle Handle, int StacksToRemove = -1)
        {
            RemoveActiveGameplayEffect(Handle, StacksToRemove);
        }

        /* Needed for delegate callback for tag prediction */
        public void RemoveOneTagCount_NoReturn(FGameplayTag Tag)
        {
            //UpdateTagMap(Tag, -1);
        }

        /* Called for predictively added gameplay cue. Needs to remove tag count and possible invoke OnRemove event if misprediction */
        public void OnPredictiveGameplayCueCatchup(FGameplayTag Tag)
        {
        }

        public float GetGameplayEffectDuration(FActiveGameplayEffectHandle Handle)
        {
            return 1f;
        }

        /* Called whenever the server time replicates via the game state to keep our cooldown timers in sync with the server */
        public void RecomputeGameplayEffectStartTimes(float WorldTime, float ServerWorldTime)
        {
        }

        public void GetGameplayEffectStartTimeAndDuration(FActiveGameplayEffectHandle Handle, float StartEffectTime,
            float Duration)
        {
        }

        /* Updates the level of an already applied gameplay effect. The intention is that this is 'seemless' and doesnt behave like removing/reapplying */
        public void SetActiveGameplayEffectLevel(FActiveGameplayEffectHandle ActiveHandle, int NewLevel)
        {
        }

        /* Updates the level of an already applied gameplay effect. The intention is that this is 'seemless' and doesnt behave like removing/reapplying */
        public void SetActiveGameplayEffectLevelUsingQuery(FGameplayEffectQuery Query, int NewLevel)
        {
        }

        // Not happy with this interface but don't see a better way yet. How should outside code (UI, etc) ask things like 'how much is this gameplay effect modifying my damage by'
        // (most likely we want to catch this on the backend - when damage is applied we can get a full dump/history of how the number got to where it is. But still we may need polling methods like below (how much would my damage be)
        public float GetGameplayEffectMagnitude(FActiveGameplayEffectHandle Handle, FGameplayAttribute Attribute)
        {
            return 1;
        }

        /* Returns current stack count of an already applied GE */
        public int GetCurrentStackCount(FActiveGameplayEffectHandle Handle)
        {
            return 1;
        }
    }
}