using System;
using System.Collections.Generic;

namespace DarkRoom.GamePlayAbility
{
   public  struct DebugExecutedGameplayEffectData
    {
        FString GameplayEffectName;
        FString ActivationState;
        FGameplayAttribute Attribute;
        TEnumAsByte<EGameplayModOp::Type> ModifierOp;
        float Magnitude;
        int StackCount;
    }

    public struct FActiveGameplayEffectsContainer
{
   public  UAbilitySystemComponent Owner;

        public Action<FActiveGameplayEffect> OnActiveGameplayEffectRemovedDelegate;

        public void GetActiveGameplayEffectDataByAttribute(TMultiMap<FGameplayAttribute, FActiveGameplayEffectsContainer::DebugExecutedGameplayEffectData> EffectMap) {
        }

        public void RegisterWithOwner(UAbilitySystemComponent Owner);

        public FActiveGameplayEffect ApplyGameplayEffectSpec(FGameplayEffectSpec Spec, FPredictionKey InPredictionKey, bool bFoundExistingStackableGE);

        public FActiveGameplayEffect GetActiveGameplayEffect(FActiveGameplayEffectHandle Handle);

        public FActiveGameplayEffect GetActiveGameplayEffect(FActiveGameplayEffectHandle Handle) {}

        public void ExecuteActiveEffectsFrom(FGameplayEffectSpec Spec, FPredictionKey PredictionKey = FPredictionKey());

        // This should not be outward facing to the skill system API, should only be called by the owning AbilitySystemComponent
        public  ExecutePeriodicGameplayEffect(FActiveGameplayEffectHandle Handle) 
        {

        }

        public bool RemoveActiveGameplayEffect(FActiveGameplayEffectHandle Handle, int StacksToRemove);

        public void GetGameplayEffectStartTimeAndDuration(FActiveGameplayEffectHandle Handle, float EffectStartTime, float EffectDuration) {}

        public float GetGameplayEffectMagnitude(FActiveGameplayEffectHandle Handle, FGameplayAttribute Attribute) {}

        public void SetActiveGameplayEffectLevel(FActiveGameplayEffectHandle ActiveHandle, int NewLevel);

        public void SetAttributeBaseValue(FGameplayAttribute Attribute, float NewBaseValue);

        public float GetAttributeBaseValue(FGameplayAttribute Attribute) {}

        public float GetEffectContribution(FAggregatorEvaluateParameters Parameters, FActiveGameplayEffectHandle ActiveHandle, FGameplayAttribute Attribute);

        /* Actually applies given mod to the attribute */
        public void ApplyModToAttribute(FGameplayAttribute Attribute, TEnumAsByte<EGameplayModOp::Type> ModifierOp, float ModifierMagnitude, FGameplayEffectModCallbackData ModData = null);

        /**
          Get the source tags from the gameplay spec represented by the specified handle, if possible
          
          @param Handle	Handle of the gameplay effect to retrieve source tags from
          
          @return Source tags from the gameplay spec represented by the handle, if possible
         */
        public FGameplayTagContainer GetGameplayEffectSourceTagsFromHandle(FActiveGameplayEffectHandle Handle) {}

        /**
          Get the target tags from the gameplay spec represented by the specified handle, if possible
          
          @param Handle	Handle of the gameplay effect to retrieve target tags from
          
          @return Target tags from the gameplay spec represented by the handle, if possible
         */
        public FGameplayTagContainer GetGameplayEffectTargetTagsFromHandle(FActiveGameplayEffectHandle Handle) {}

        /**
          Populate the specified capture spec with the data necessary to capture an attribute from the container
          
          @param OutCaptureSpec	[OUT] Capture spec to populate with captured data
         */
        public void CaptureAttributeForGameplayEffect(FGameplayEffectAttributeCaptureSpec OutCaptureSpec);

        public void PrintAllGameplayEffects() {}

    /**
	 *	Returns the total number of gameplay effects.
	 *	NOTE this does include GameplayEffects that pending removal.
	 *	Any pending remove gameplay effects are deleted at the end of their scope lock
	 */
    public int GetNumGameplayEffects()
	{
		int NumPending = 0;
    FActiveGameplayEffect PendingGameplayEffect = PendingGameplayEffectHead;
    FActiveGameplayEffect Stop = *PendingGameplayEffectNext;
		while (PendingGameplayEffect  PendingGameplayEffect != Stop)
		{
			++NumPending;
			PendingGameplayEffect = PendingGameplayEffect->PendingNext;
		}

		return GameplayEffects_Internal.Num() + NumPending;
	}

        public void CheckDuration(FActiveGameplayEffectHandle Handle);

        public void Uninitialize();

        // ------------------------------------------------

        public bool CanApplyAttributeModifiers(UGameplayEffect GameplayEffect, float Level, FGameplayEffectContextHandle EffectContext);

        public TArray<float> GetActiveEffectsTimeRemaining(FGameplayEffectQuery Query) {}

        public TArray<float> GetActiveEffectsDuration(FGameplayEffectQuery Query) {}

        public TArray<TPair<float, float>> GetActiveEffectsTimeRemainingAndDuration(FGameplayEffectQuery Query) {}

        public TArray<FActiveGameplayEffectHandle> GetActiveEffects(FGameplayEffectQuery Query) {}

        public float GetActiveEffectsEndTime(FGameplayEffectQuery Query, TArray<AActor*> Instigators) {}
        public bool GetActiveEffectsEndTimeAndDuration(FGameplayEffectQuery Query, float EndTime, float Duration, TArray<AActor*> Instigators) {}

        /* Returns an array of all of the active gameplay effect handles */
        public TArray<FActiveGameplayEffectHandle> GetAllActiveEffectHandles() {}

        public void ModifyActiveEffectStartTime(FActiveGameplayEffectHandle Handle, float StartTimeDiff);

        public int RemoveActiveEffects(FGameplayEffectQuery Query, int StacksToRemove);

        /**
          Get the count of the effects matching the specified query (including stack count)
          
          @return Count of the effects matching the specified query
         */
        public int GetActiveEffectCount(FGameplayEffectQuery Query, bool bEnforceOnGoingCheck = true) {}

        public bool IsServerWorldTimeAvailable() {}

        public float GetServerWorldTime() {}

        public float GetWorldTime() {}

        public bool HasReceivedEffectWithPredictedKey(FPredictionKey PredictionKey) {}

        public bool HasPredictedEffectWithPredictedKey(FPredictionKey PredictionKey) {}

        public void SetBaseAttributeValueFromReplication(FGameplayAttribute Attribute, float BaseBalue);

        public void GetAllActiveGameplayEffectSpecs(TArray<FGameplayEffectSpec> OutSpecCopies) {}

        public void DebugCyclicAggregatorBroadcasts(FAggregator Aggregator);

        /* Performs a deep copy on the source container, duplicating all gameplay effects and reconstructing the attribute aggregator map to match the passed in source. */
        public void CloneFrom(FActiveGameplayEffectsContainer Source);

        // -------------------------------------------------------------------------------------------


        public FOnGameplayAttributeValueChange GetGameplayAttributeValueChangeDelegate(FGameplayAttribute Attribute);

        public void OnOwnerTagChange(FGameplayTag TagChange, int NewCount);

        public bool HasApplicationImmunityToSpec(FGameplayEffectSpec SpecToApply, FActiveGameplayEffect OutGEThatProvidedImmunity) {}

        public void IncrementLock();
        public void DecrementLock();

        /* Recomputes the start time for all active abilities */
        public void RecomputeStartWorldTimes(float WorldTime, float ServerWorldTime);

	/**
	 *	Accessors for internal functions to get GameplayEffects directly by index.
	 *	Note this will return GameplayEffects that are pending removal!
	 *	
	 *	To iterate over all 'valid' gameplay effects, use the CreateConstIterator/CreateIterator or the stl style range iterator
	 */
	public FActiveGameplayEffect GetActiveGameplayEffect(int idx)
	{
		return const_cast<FActiveGameplayEffectsContainer*>(this)->GetActiveGameplayEffect(idx);
}

public FActiveGameplayEffect GetActiveGameplayEffect(int idx)
{
    if (idx < GameplayEffects_Internal.Num())
    {
        return GameplayEffects_Internal[idx];
    }

    idx -= GameplayEffects_Internal.Num();
    FActiveGameplayEffect Ptr = PendingGameplayEffectHead;
    FActiveGameplayEffect Stop = *PendingGameplayEffectNext;

    // Advance until the desired index or until hitting the actual end of the pending list currently in use (need to check both Ptr and Ptr->PendingNext to prevent hopping
    // the pointer too far along)
    return idx <= 0 ? Ptr : null;
}

        /* Our active list of Effects. Do not access this directly (Even from internal functions!) Use GetNumGameplayEffect() / GetGameplayEffect() ! */


        private TArray<FActiveGameplayEffect> GameplayEffects_Internal;

        private void InternalUpdateNumericalAttribute(FGameplayAttribute Attribute, float NewValue, FGameplayEffectModCallbackData ModData, bool bFromRecursiveCall = false);

        /* Cached pointer to current mod data needed for callbacks. We cache it in the AGE struct to avoid passing it through all the delegate/aggregator plumbing */
        private FGameplayEffectModCallbackData CurrentModcallbackData;

        /**
          Helper function to execute a mod on owned attributes
          
          @param Spec			Gameplay effect spec executing the mod
          @param ModEvalData	Evaluated data for the mod
          
          @return True if the mod successfully executed, false if it did not
         */
        private bool InternalExecuteMod(FGameplayEffectSpec Spec, FGameplayModifierEvaluatedData ModEvalData);

        /* Called internally to actually remove a GameplayEffect or to reduce its StackCount. Returns true if we resized our internal GameplayEffect array. */
        private  bool InternalRemoveActiveGameplayEffect(int Idx, int StacksToRemove, bool bPrematureRemoval);

        /* Called both in server side creation and replication creation/deletion */
        private void InternalOnActiveGameplayEffectAdded(FActiveGameplayEffect Effect);
        private void InternalOnActiveGameplayEffectRemoved(FActiveGameplayEffect Effect, bool bInvokeGameplayCueEvents, FGameplayEffectRemovalInfo GameplayEffectRemovalInfo);

        private void RemoveActiveGameplayEffectGrantedTagsAndModifiers(FActiveGameplayEffect Effect, bool bInvokeGameplayCueEvents);
        private void AddActiveGameplayEffectGrantedTagsAndModifiers(FActiveGameplayEffect Effect, bool bInvokeGameplayCueEvents);

        /* Updates tag dependency map when a GameplayEffect is removed */
        private void RemoveActiveEffectTagDependency(FGameplayTagContainer Tags, FActiveGameplayEffectHandle Handle);

        /* Internal helper function to bind the active effect to all of the custom modifier magnitude external dependency delegates it contains, if any */
        private void AddCustomMagnitudeExternalDependencies(FActiveGameplayEffect Effect);

        /* Internal helper function to unbind the active effect from all of the custom modifier magnitude external dependency delegates it may have bound to, if any */
        private void RemoveCustomMagnitudeExternalDependencies(FActiveGameplayEffect Effect);

        /* Internal callback fired as a result of a custom modifier magnitude external dependency delegate firing; Updates affected active gameplay effects as necessary */
        private void OnCustomMagnitudeExternalDependencyFired(TSubclassOf<UGameplayModMagnitudeCalculation> MagnitudeCalculationClass);

        /* Internal helper function to apply expiration effects from a removed/expired gameplay effect spec */
        private void InternalApplyExpirationEffects(FGameplayEffectSpec ExpiringSpec, bool bPrematureRemoval);

        private void RestartActiveGameplayEffectDuration(FActiveGameplayEffect ActiveGameplayEffect);

        // -------------------------------------------------------------------------------------------

        private TMap<FGameplayAttribute, FAggregatorRef> AttributeAggregatorMap;

        // DEPRECATED: use AttributeValueChangeDelegates
        private TMap<FGameplayAttribute, FOnGameplayAttributeChange> AttributeChangeDelegates;

        private TMap<FGameplayAttribute, FOnGameplayAttributeValueChange> AttributeValueChangeDelegates;


        private TMap<FGameplayTag, TSet<FActiveGameplayEffectHandle>> ActiveEffectTagDependencies;

        /* Mapping of custom gameplay modifier magnitude calculation class to dependency handles for triggering updates on external delegates firing */
        private TMap<FObjectKey, FCustomModifierDependencyHandle> CustomMagnitudeClassDependencies;

        /* A map to manage stacking while we are the source */
        private TMap<TWeakObjectPtr<UGameplayEffect>, TArray<FActiveGameplayEffectHandle>> SourceStackingMap;

        /* Acceleration struct for immunity tests */
        private FGameplayTagCountContainer ApplicationImmunityGameplayTagCountContainer;

        /* Active GEs that have immunity queries. This is an acceleration list to avoid searching through the Active GameplayEffect list frequetly. (We only search for the active GE if immunity procs) */


        private TArray<UGameplayEffect*> ApplicationImmunityQueryEffects;

        private FAggregatorRef FindOrCreateAttributeAggregator(FGameplayAttribute Attribute);

        private void OnAttributeAggregatorDirty(FAggregator Aggregator, FGameplayAttribute Attribute, bool FromRecursiveCall = false);

        private void OnMagnitudeDependencyChange(FActiveGameplayEffectHandle Handle, FAggregator ChangedAgg);

        private void OnStackCountChange(FActiveGameplayEffect ActiveEffect, int OldStackCount, int NewStackCount);

        private void OnDurationChange(FActiveGameplayEffect ActiveEffect);

        private void UpdateAllAggregatorModMagnitudes(FActiveGameplayEffect ActiveEffect);

        private void UpdateAggregatorModMagnitudes(TSet<FGameplayAttribute> AttributesToUpdate, FActiveGameplayEffect ActiveEffect);

        /* Helper function to find the active GE that the specified spec can stack with, if any */
        private FActiveGameplayEffect FindStackableActiveGameplayEffect(FGameplayEffectSpec Spec);

        /* Helper function to handle the case of same-effect stacking overflow; Returns true if the overflow application should apply, false if it should not */
        private bool HandleActiveGameplayEffectStackOverflow(FActiveGameplayEffect ActiveStackableGE, FGameplayEffectSpec OldSpec, FGameplayEffectSpec OverflowingSpec);

        /* After application has gone through, give stacking rules a chance to do something as the source of the gameplay effect (E.g., remove an old version) */
        private virtual void ApplyStackingLogicPostApplyAsSource(UAbilitySystemComponent Target, FGameplayEffectSpec SpecApplied, FActiveGameplayEffectHandle ActiveHandle) { }

        private bool ShouldUseMinimalReplication();

        private int ScopedLockCount;
        private int PendingRemoves;

        private FActiveGameplayEffect PendingGameplayEffectHead;   // Head of pending GE linked list
        private FActiveGameplayEffect PendingGameplayEffectNext;  // Points to the where to store the next pending GE (starts pointing at head, as more are added, points further down the list).
}

}
