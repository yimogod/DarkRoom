using System;
using System.Collections.Generic;

namespace DarkRoom.GamePlayAbility
{
    // A pending activation that cannot be activated yet, will be rechecked at a later point
    public struct FPendingAbilityInfo
    {
        public static bool operator ==(FPendingAbilityInfo Me, FPendingAbilityInfo Other)
        {
            // Don't compare event data, not valid to have multiple activations in flight with same key and handle but different event data
            return Me.PredictionKey == Other.PredictionKey && Me.Handle == Other.Handle;
        }

        public static bool operator !=(FPendingAbilityInfo Me, FPendingAbilityInfo Other)
        {
            // Don't compare event data, not valid to have multiple activations in flight with same key and handle but different event data
            return Me.PredictionKey != Other.PredictionKey || Me.Handle != Other.Handle;
        }

        /* Properties of the ability that needs to be activated */
        public FGameplayAbilitySpecHandle Handle;
        public FPredictionKey PredictionKey;
        public FGameplayEventData TriggerEventData;

        /* True if this ability was activated remotely and needs to follow up, false if the ability hasn't been activated at all yet */
        public bool bPartiallyActivated;


        public FPendingAbilityInfo()
            : bPartiallyActivated(false)
        { }
    }


    public enum EAbilityExecutionState
    {
        Executing,
        Succeeded,
        Failed,
    };

    public struct FExecutingAbilityInfo
    {
        FExecutingAbilityInfo() : State(EAbilityExecutionState::Executing) { };

        bool operator ==(FExecutingAbilityInfo Other)
        {
            return PredictionKey == Other.PredictionKey & State == Other.State;
        }

        public FPredictionKey PredictionKey;
        public EAbilityExecutionState State;
        public FGameplayAbilitySpecHandle Handle;
    };

    public struct FAbilitySystemComponentDebugInfo
    {
        FAbilitySystemComponentDebugInfo()
        {
        }

        bool bPrintToLog;

        bool bShowAttributes;
        bool bShowGameplayEffects;;
        bool bShowAbilities;

        float XPos;
        float YPos;
        float OriginalX;
        float OriginalY;
        float MaxY;
        float NewColumnYPadding;
        float YL;

        bool Accumulate;
        List<FString> Strings;

        int GameFlags; // arbitrary flags for games to set/read in Debug_Internal
    }
}
