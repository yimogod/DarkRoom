using System;
using System.Collections.Generic;
using DarkRoom.Game;

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
}