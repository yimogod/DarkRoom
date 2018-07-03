using System;
using System.Collections.Generic;
using DarkRoom.Core;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility
{
    public struct FActiveGameplayEffectEvents
    {
        Action<FGameplayEffectRemovalInfo> OnEffectRemoved;
        Action<FActiveGameplayEffectHandle, int, int> OnStackChanged;
        Action<FActiveGameplayEffectHandle, float, float> OnTimeChanged;
        Action<FActiveGameplayEffectHandle, bool> OnInhibitionChanged;
    };


    public class FActiveGameplayEffect
    {
        // ---------------------------------------------------------------------------------------------------------------------------------
        //  IMPORTANT: Any new state added to FActiveGameplayEffect must be handled in the copy/move constructor/operator
        //	(When VS2012/2013 support is dropped, we can use compiler generated operators, but until then these need to be maintained manually)
        // ---------------------------------------------------------------------------------------------------------------------------------
        FActiveGameplayEffect(FActiveGameplayEffectHandle InHandle,  FGameplayEffectSpec InSpec, 
            float CurrentWorldTime, float InStartServerWorldTime)
        {

        }


        public float GetTimeRemaining(float WorldTime)
        {
            float Duration = GetDuration();
            return (Duration == FGameplayEffectConstants.INFINITE_DURATION
                ? -1.0f : Duration - (WorldTime - StartWorldTime));
        }

        public float GetDuration()
        {
            return Spec.GetDuration();
        }

        public float GetPeriod()
        {
            return Spec.GetPeriod();
        }

        public float GetEndTime()
        {
            float Duration = GetDuration();
            return (Duration == FGameplayEffectConstants.INFINITE_DURATION ? -1.0f : Duration + StartWorldTime);
        }

        public void CheckOngoingTagRequirements(FGameplayTagContainer OwnerTags,
            FActiveGameplayEffectsContainer OwningContainer,
            bool bInvokeGameplayCueEvents = false)
        {

        }

        public void PrintAll()
        {

        }

        // Debug string used by Fast Array serialization
        public string GetDebugString()
        {
            return "";
        }

/** Refreshes the cached StartWorldTime for this effect. To be used when the server/client world time delta changes significantly to keep the start time in sync. */
        public void RecomputeStartWorldTime(FActiveGameplayEffectsContainer InArray)
        {

        }

/** Refreshes the cached StartWorldTime for this effect. To be used when the server/client world time delta changes significantly to keep the start time in sync. */
        public void RecomputeStartWorldTime(float WorldTime, float ServerWorldTime)
        {

        }


// ---------------------------------------------------------------------------------------------------------------------------------

/** Globally unique ID for identify this active gameplay effect. Can be used to look up owner. Not networked. */
        public FActiveGameplayEffectHandle Handle;

        public FGameplayEffectSpec Spec;

        public float StartServerWorldTime;

/** Used for handling duration modifications being replicated */
        public float CachedStartServerWorldTime;

        public float StartWorldTime;

        // Not sure if this should replicate or not. If replicated, we may have trouble where IsInhibited doesn't appear to change when we do tag checks (because it was previously inhibited, but replication made it inhibited).
        bool bIsInhibited;

/** When replicated down, we cue the GC events until the entire list of active gameplay effects has been received */
        public bool bPendingRepOnActiveGC;
        public bool bPendingRepWhileActiveGC;

        public bool IsPendingRemove;

/** Last StackCount that the client had. Used to tell if the stackcount has changed in PostReplicatedChange */
        public int ClientCachedStackCount;

        public FTimerHandle PeriodHandle;
        public FTimerHandle DurationHandle;

        public FActiveGameplayEffect PendingNext = null;

/** All the bindable events for this active effect (bundled to allow easier non-const access to these events via the ASC) */
        public FActiveGameplayEffectEvents EventSet;
    }
}