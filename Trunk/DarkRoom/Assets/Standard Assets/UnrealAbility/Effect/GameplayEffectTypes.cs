using System;
using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.GamePlayAbility
{
    public enum EGameplayModEvaluationChannel
    {
        Channel0,
        Channel1,
        Channel2,
        Channel3,
        Channel4,
        Channel5,
        Channel6,
        Channel7,
        Channel8,
        Channel9,

        // Always keep last
        Channel_MAX
    }

    public enum EGameplayEffectAttributeCaptureSource
    {
        /** Source (caster) of the gameplay effect. */
        Source,

        /** Target (recipient) of the gameplay effect. */
        Target
    }

    public enum EGameplayModOp
    {
        /** Numeric. */
        Additive = 0, //        UMETA(DisplayName = "Add"),

        /** Numeric. */
        Multiplicitive, //     UMETA(DisplayName = "Multiply"),

        /** Numeric. */
        Division, //           UMETA(DisplayName = "Divide"),

        /** Other. */
        Override, //            UMETA(DisplayName = "Override"),    // This should always be the first non numeric ModOp

        // This must always be at the end.
        Max //               UMETA(DisplayName = "Invalid")
    }

    public class FGameplayEffectConstants
    {
        /* Infinite duration */
        public const float INFINITE_DURATION = -1f;

        /* No duration; Time specifying instant application of an effect */
        public const float INSTANT_APPLICATION = -1f;

        /* ant specifying that the combat effect has no period and doesn't check for over time application */
        public const float NO_PERIOD = -1f;

        /* No Level/Level not set */
        public const float INVALID_LEVEL = -1f;
    }

    public struct FGameplayEffectModCallbackData
    {
        public FGameplayEffectSpec EffectSpec; // The spec that the mod came from
        public FGameplayModifierEvaluatedData EvaluatedData; // The 'flat'/computed data to be applied to the target

        public UAbilitySystemComponent Target; // Target we intend to apply to

        public FGameplayEffectModCallbackData(FGameplayEffectSpec InEffectSpec,
            FGameplayModifierEvaluatedData InEvaluatedData, UAbilitySystemComponent InTarget)
        {
            EffectSpec = InEffectSpec;
            EvaluatedData = InEvaluatedData;
            Target = InTarget;
        }
    }

    public class FGameplayEffectAttributeCaptureDefinition
    {
        public FGameplayEffectAttributeCaptureDefinition(FGameplayAttribute InAttribute,
            EGameplayEffectAttributeCaptureSource InSource, bool InSnapshot)
        {
        }

        /* Gameplay attribute to capture */
        public FGameplayAttribute AttributeToCapture;

        /* Source of the gameplay attribute */
        public EGameplayEffectAttributeCaptureSource AttributeSource;

        /* Whether the attribute should be snapshotted or not */
        public bool bSnapshot;

        /**
          Get type hash for the capture definition; Implemented to allow usage in TMap
         *
          @param CaptureDef Capture definition to get the type hash of
         */
        public uint GetTypeHash(FGameplayEffectAttributeCaptureDefinition CaptureDef)
        {
            uint Hash = 0;
            return Hash;
        }

        public string ToSimpleString()
        {
            return "";
        }
    }

    public class FTagContainerAggregator
    {
        private FGameplayTagContainer CapturedSpecTags;
        private FGameplayTagContainer ScopedTags;

        private FGameplayTagContainer CachedAggregator;
        private bool CacheIsValid;

        public FTagContainerAggregator(FTagContainerAggregator Other)
        {
        }

        public FGameplayTagContainer GetActorTags()
        {
            return null;
        }

        public FGameplayTagContainer GetSpecTags()
        {
            return null;
        }

        public FGameplayTagContainer GetAggregatedTags()
        {
            return null;
        }
    }


    public class FGameplayModifierEvaluatedData
    {
        public FGameplayModifierEvaluatedData(FGameplayAttribute InAttribute, EGameplayModOp InModOp, float InMagnitude,
            FActiveGameplayEffectHandle InHandle)
        {
        }

        FGameplayAttribute Attribute;

        /** The numeric operation of this modifier: Override, Add, Multiply, etc  */
        EGameplayModOp ModifierOp;


        float Magnitude;

        /** Handle of the active gameplay effect that originated us. Will be invalid in many cases */
        FActiveGameplayEffectHandle Handle;

        bool IsValid;

        public string ToSimpleString()
        {
            return "";
        }
    }

    public class FActiveGameplayEffectHandle
    {
        private int Handle;
        private bool bPassedFiltersAndWasExecuted = true;

        public static FActiveGameplayEffectHandle GenerateNewHandle(UAbilitySystemComponent OwningComponent)
        {
            return null;
        }

        public static void ResetGlobalHandleMap()
        {
        }

        public FActiveGameplayEffectHandle()
        {
        }

        public FActiveGameplayEffectHandle(int InHandle)
        {
        }

        public bool IsValid()
        {
            return Handle != -1;
        }

        public bool WasSuccessfullyApplied()
        {
            return bPassedFiltersAndWasExecuted;
        }

        public UAbilitySystemComponent GetOwningAbilitySystemComponent()
        {
            return null;
        }

        public void RemoveFromGlobalMap()
        {
        }

        public int GetTypeHash(FActiveGameplayEffectHandle InHandle)
        {
            return InHandle.Handle;
        }

        public string ToString()
        {
            //return FString::Printf(TEXT("%d"), Handle);
            return "";
        }

        public void Invalidate()
        {
            Handle = -1;
        }
    }

    public struct FGameplayTagRequirements
    {
        /** All of these tags must be present */
        public FGameplayTagContainer RequireTags;

        /** None of these tags may be present */
        public FGameplayTagContainer IgnoreTags;

        public bool RequirementsMet(FGameplayTagContainer Container)
        {
            return false;
        }

        public bool IsEmpty()
        {
            return false;
        }

        public string ToString()
        {
            return "";
        }
    }


    public class FGameplayEffectQuery
    {
        //public FGameplayEffectQuery(FActiveGameplayEffectQueryCustomMatch InCustomMatchDelegate)
        //{
        //}

        /** Native delegate for providing custom matching conditions. */
        Action<FActiveGameplayEffect> CustomMatchDelegate;

        /** BP-exposed delegate for providing custom matching conditions. */
        Action<FActiveGameplayEffect, bool> CustomMatchDelegate_BP;

        /** Query that is matched against tags this GE gives */
        FGameplayTagQuery OwningTagQuery;

        /** Query that is matched against tags this GE has */
        FGameplayTagQuery EffectTagQuery;

        /** Query that is matched against tags the source of this GE has */
        FGameplayTagQuery SourceTagQuery;

        /** Matches on GameplayEffects which modify given attribute. */
        FGameplayAttribute ModifyingAttribute;

        /** Matches on GameplayEffects which come from this source */
        GameObject EffectSource;

        /** Matches on GameplayEffects with this definition */
        UGameplayEffect EffectDefinition;

        /** Handles to ignore as matches, even if other criteria is met */
        public List<FActiveGameplayEffectHandle> IgnoreHandles;

        /** Returns true if Effect matches all specified criteria of this query, including CustomMatch delegates if bound. Returns false otherwise. */
        public bool Matches(FActiveGameplayEffect Effect)
        {
            return false;
        }

        /** Returns true if Effect matches all specified criteria of this query. This does NOT check FActiveGameplayEffectQueryCustomMatch since this is performed on the spec (possibly prior to applying).
         *	Note: it would be reasonable to support a custom delegate that operated on the FGameplayEffectSpec itself.
         */
        public bool Matches(FGameplayEffectSpec Effect)
        {
            return false;
        }

        /** Returns true if the query is empty/default. E.g., it has no data set. */
        public  bool IsEmpty()
        {
            return false;
        }

        /** 
         * Shortcuts for easily creating common query types 
         * @todo: add more as dictated by use cases
         */

        /** Creates an effect query that will match if there are any common tags between the given tags and an ActiveGameplayEffect's owning tags */
        public static FGameplayEffectQuery MakeQuery_MatchAnyOwningTags(FGameplayTagContainer InTags)
        {
            return null;
        }

        /** Creates an effect query that will match if all of the given tags are in the ActiveGameplayEffect's owning tags */
        public static FGameplayEffectQuery MakeQuery_MatchAllOwningTags(FGameplayTagContainer InTags)
        {
            return null;
        }

        /** Creates an effect query that will match if there are no common tags between the given tags and an ActiveGameplayEffect's owning tags */
        public static FGameplayEffectQuery MakeQuery_MatchNoOwningTags(FGameplayTagContainer InTags)
        {
            return null;
        }

        /** Creates an effect query that will match if there are any common tags between the given tags and an ActiveGameplayEffect's tags */
        public static FGameplayEffectQuery MakeQuery_MatchAnyEffectTags(FGameplayTagContainer InTags)
        {
            return null;
        }

        /** Creates an effect query that will match if all of the given tags are in the ActiveGameplayEffect's tags */
        public static FGameplayEffectQuery MakeQuery_MatchAllEffectTags(FGameplayTagContainer InTags)
        {
            return null; 
        }

        /** Creates an effect query that will match if there are no common tags between the given tags and an ActiveGameplayEffect's tags */
        public static FGameplayEffectQuery MakeQuery_MatchNoEffectTags(FGameplayTagContainer InTags)
        {
            return null;
        }

        /** Creates an effect query that will match if there are any common tags between the given tags and an ActiveGameplayEffect's source tags */
        public static FGameplayEffectQuery MakeQuery_MatchAnySourceTags(FGameplayTagContainer InTags)
        {
            return null;
        }

        /** Creates an effect query that will match if all of the given tags are in the ActiveGameplayEffect's source tags */
        public static FGameplayEffectQuery MakeQuery_MatchAllSourceTags(FGameplayTagContainer InTags)
        {
            return null;
        }

        /** Creates an effect query that will match if there are no common tags between the given tags and an ActiveGameplayEffect's source tags */
        public static FGameplayEffectQuery MakeQuery_MatchNoSourceTags(FGameplayTagContainer InTags)
        {
            return null;
        }
    }

    public class FActiveGameplayEffectQuery
    {
        /** Bind this to override the default query-matching code. */
        Action<FActiveGameplayEffect> CustomMatch;

        /** Returns true if Effect matches the criteria of this query, which will be overridden by CustomMatch if it is bound. Returns false otherwise. */
        public bool Matches(FActiveGameplayEffect Effect)
        {
            return false;
        }

        /** used to match with InheritableOwnedTagsContainer */
        public FGameplayTagContainer OwningTagContainer;

        /** used to match with InheritableGameplayEffectTags */
        public FGameplayTagContainer EffectTagContainer;

        /** used to reject matches with InheritableOwnedTagsContainer */
        public FGameplayTagContainer OwningTagContainer_Rejection;

        /** used to reject matches with InheritableGameplayEffectTags */
        public FGameplayTagContainer EffectTagContainer_Rejection;

        // Matches on GameplayEffects which modify given attribute
        public FGameplayAttribute ModifyingAttribute;

        // Matches on GameplayEffects which come from this source
        public GameObject EffectSource;

        // Matches on GameplayEffects with this definition
        public UGameplayEffect EffectDef;

        // Handles to ignore as matches, even if other criteria is met
        List<FActiveGameplayEffectHandle> IgnoreHandles;
    }


    /**
 * FGameplayEffectRemovalInfo
 *	Data struct for containing information pertinent to GameplayEffects as they are removed.
 */
    public struct FGameplayEffectRemovalInfo
    {
        /** True when the gameplay effect's duration has not expired, meaning the gameplay effect is being forcefully removed.  */
        bool bPrematureRemoval;

        /** Number of Stacks this gameplay effect had before it was removed. */
        int StackCount;

        /** Actor this gameplay effect was targeting. */
        FGameplayEffectContextHandle EffectContext;
    };
}