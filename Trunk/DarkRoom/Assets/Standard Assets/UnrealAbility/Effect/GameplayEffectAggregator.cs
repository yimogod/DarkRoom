using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DarkRoom.GamePlayAbility
{


    public class FAggregator
    {
        public Action OnDirty;
        public Action OnDirtyRecursive;    // Called in case where we are in a recursive dirtying chain. This will force the backing uproperty to update but not call the game code delegates

        private float BaseValue;
        private FAggregatorModChannelContainer ModChannels;


        /** Custom meta data for the aggregator */
        private Action<FAggregator> EvaluationMetaData;

        /** NetworkID that we had our last update from. Will only be set on clients and is only used to ensure we don't recompute server base value twice. */
        private int NetUpdateID;

        /** ActiveGE handles that we need to notify if we change. NOT copied over during snapshots. */
        private List<FActiveGameplayEffectHandle> Dependents;
        private int BroadcastingDirtyCount;

        public static float StaticExecModOnBaseValue(float BaseValue, EGameplayModOp ModifierOp,
            float EvaluatedMagnitude)
        {
            return 1f;
        }

        public FAggregator(float InBaseValue)
        {

        }

        /** Simple accessor to base value */
        public float GetBaseValue()
        {
            return 1f;
        }

        public void SetBaseValue(float NewBaseValue, bool BroadcastDirtyEvent = true)
        {

        }

        public void ExecModOnBaseValue(EGameplayModOp ModifierOp, float EvaluatedMagnitude)
        {

        }

        public void AddAggregatorMod(float EvaluatedData, EGameplayModOp ModifierOp, EGameplayModEvaluationChannel ModifierChannel, FGameplayTagRequirements SourceTagReqs, FGameplayTagRequirements TargetTagReqs, bool IsPredicted, FActiveGameplayEffectHandle ActiveHandle)
        {

        }

        /** Removes all mods for the passed in handle and marks this as dirty to recalculate the aggregator */
        public void RemoveAggregatorMod(FActiveGameplayEffectHandle ActiveHandle)
        {

        }

        /** Updates the aggregators for the past in handle, this will handle it so the UAttributeSets stats only get one update for the delta change */
        public void UpdateAggregatorMod(FActiveGameplayEffectHandle ActiveHandle, FGameplayAttribute Attribute,
            FGameplayEffectSpec Spec, bool bWasLocallyGenerated, FActiveGameplayEffectHandle InHandle)
        {

        }

        /** Evaluates the Aggregator with the internal base value and given parameters */
        public float Evaluate(FAggregatorEvaluateParameters Parameters)
        {
            return 1f;
        }

        /** Evaluates the aggregator with the internal base value and given parameters, up to the specified evaluation channel (inclusive) */
        public float EvaluateToChannel(FAggregatorEvaluateParameters Parameters,
            EGameplayModEvaluationChannel FinalChannel)
        {
            return 1f;
        }

        /** Works backwards to calculate the base value. Used on clients for doing predictive modifiers */
        public float ReverseEvaluate(float FinalValue, FAggregatorEvaluateParameters Parameters)
        {
            return 1f;
        }

        /** Evaluates the Aggregator with an arbitrary base value */
        public float EvaluateWithBase(float InlineBaseValue, FAggregatorEvaluateParameters Parameters)
        {
            return 1f;
        }

        /** Evaluates the Aggregator to compute its "bonus" (final - base) value */
        public float EvaluateBonus(FAggregatorEvaluateParameters Parameters)
        {
            return 1f;
        }

        /** Evaluates the contribution from the GE associated with ActiveHandle */
        public float EvaluateContribution(FAggregatorEvaluateParameters Parameters,
            FActiveGameplayEffectHandle ActiveHandle)
        {
            return 1f;
        }

        /** Calls ::UpdateQualifies on each mod. Useful for when you need to manually inspect aggregators */
        public void EvaluateQualificationForAllMods(FAggregatorEvaluateParameters Parameters)
        {

        }

        public void TakeSnapshotOf(FAggregator AggToSnapshot)
        {

        }

        public void AddModsFrom(FAggregator SourceAggregator)
        {

        }

        public void AddDependent(FActiveGameplayEffectHandle Handle)
        {

        }

        public void RemoveDependent(FActiveGameplayEffectHandle Handle)
        {

        }

        /**
	     * Populate a mapping of channel to corresponding mods
	     * 
	     * @param OutMods	Mapping of channel enum to mods
	     */
        public void GetAllAggregatorMods(Dictionary<EGameplayModEvaluationChannel, List<FAggregatorMod>> OutMods)
        {

        }

        /**
         * Called when the aggregator's gameplay effect dependencies have potentially been swapped out for new ones, like when GE arrays are cloned.
         * Updates the modifier channel container appropriately, as well as directly-specified dependents.
         * 
         * @param SwappedDependencies	Mapping of old gameplay effect handles to new replacements
         */
        public void OnActiveEffectDependenciesSwapped(
            Dictionary<FActiveGameplayEffectHandle, FActiveGameplayEffectHandle> SwappedDependencies)
        {

        }

        private void BroadcastOnDirty()
        {

        }
    }

    public class FAggregatorMod
    {
        /** This bool is updated by UpdateQualifiees. Think of it as a transient bool, we make a full pass on all qualifiers first (::UpdateQualifies) then while evaluating we check what the bool was set to (::Qualifies) */
        private bool IsQualified;

        public FGameplayTagRequirements SourceTagReqs;
        public FGameplayTagRequirements TargetTagReqs;

        public float EvaluatedMagnitude;       // Magnitude this mod was last evaluated at
        public float StackCount;

        public FActiveGameplayEffectHandle ActiveHandle;   // Handle of the active GameplayEffect we are tied to (if any)
        public bool IsPredicted;

        public bool Qualifies() { return IsQualified; }

    /** Called to update the Qualifies bool */
        public void UpdateQualifies(FAggregatorEvaluateParameters Parameters)
        {

        }

        /** Intended to be used by FAggregatorEvaluateMetaData::CustomQualifiesFunc to toggle qualifications of mods */
        public void SetExplicitQualifies(bool NewQualifies) { IsQualified = NewQualifies; }
    }

    public class FAggregatorEvaluateParameters
    {
        public FGameplayTagContainer SourceTags;
        public FGameplayTagContainer TargetTags;

        /** Any mods with one of these handles will be ignored during evaluation */
        public List<FActiveGameplayEffectHandle> IgnoreHandles;

        /** If any tags are specified in the filter, a mod's owning active gameplay effect's source tags must match ALL of them in order for the mod to count during evaluation */
        public FGameplayTagContainer AppliedSourceTagFilter;

        /** If any tags are specified in the filter, a mod's owning active gameplay effect's target tags must match ALL of them in order for the mod to count during evaluation */
        public FGameplayTagContainer AppliedTargetTagFilter;

        public bool IncludePredictiveMods;
    }

    public class FAggregatorModChannel
    {
        /** Collection of modifers within the channel, organized by modifier operation */
        private List<FAggregatorMod> Mods;


        /**
      * Helper function to sum all of the mods in the specified array, using the specified modifier bias and evaluation parameters
      * 
      * @param InMods		Mods to sum
      * @param Bias			Bias to apply to modifier magnitudes
      * @param Parameters	Evaluation parameters
      * 
      * @return Summed value of mods
      */
        public static float SumMods(List<FAggregatorMod> InMods, float Bias, FAggregatorEvaluateParameters Parameters)
        {
            return 1f;
        }

        /**
	     * Evaluates the channel's mods with the specified base value and evaluation parameters
	     * 
	     * @param InlineBaseValue	Base value to use for the evaluation
	     * @param Parameters		Additional evaluation parameters to use
	     * 
	     * @return Evaluated value based upon the channel's mods
	     */
        public float EvaluateWithBase(float InlineBaseValue, FAggregatorEvaluateParameters Parameters)
        {
            return 1f;
        }

        /**
	     * Evaluates a final value in reverse, attempting to determine a base value from the modifiers within the channel.
	     * Certain conditions (such as the use of override mods) can prevent this from computing correctly, at which point false
	     * will be returned. This is predominantly used for filling in base values on clients from replication for float-based attributes.
	     * 
	     * @note This will be deprecated/removed soon with the transition to struct-based attributes.
	     * 
	     * @param FinalValue	Final value to reverse evaluate
	     * @param Parameters	Evaluation parameters to use for the reverse evaluation
	     * @param ComputedValue	[OUT] Reverse evaluated base value
	     * 
	     * @return True if the reverse evaluation was successful, false if it was not
	     */
        public bool ReverseEvaluate(float FinalValue, FAggregatorEvaluateParameters Parameters, ref float ComputedValue)
        {
            return false;
        }

        /**
	     * Add a modifier to the channel
	     * 
	     * @param EvaluatedMagnitude	Magnitude of the modifier
	     * @param ModOp					Operation of the modifier
	     * @param SourceTagReqs			Source tag requirements of the modifier
	     * @param TargetTagReqs			Target tag requirements of the modifier
	     * @param bIsPredicted			Whether the mod is predicted or not
	     * @param ActiveHandle			Handle of the active gameplay effect that's applying the mod
	     */
        public void AddMod(float EvaluatedMagnitude, EGameplayModOp ModOp, FGameplayTagRequirements SourceTagReqs,
            FGameplayTagRequirements TargetTagReqs, bool bIsPredicted, FActiveGameplayEffectHandle ActiveHandle)
        {

        }

        /**
	     * Remove all mods from the channel that match the specified gameplay effect handle
	     * 
	     * @param Handle	Handle to use for removal
	     */
        public void RemoveModsWithActiveHandle(FActiveGameplayEffectHandle Handle)
        {

        }

        /**
	     * Add the specified channel's mods into this channel
	     * 
	     * @param Other	Other channel to add mods from
	     */
        public void AddModsFrom(FAggregatorModChannel Other)
        {

        }

        /** runs UpdateQualifies on all mods */
        public void UpdateQualifiesOnAllMods(FAggregatorEvaluateParameters Parameters)
        {

        }

        /**
	     * Populate a mapping of channel to corresponding mods
	     * 
	     * @param Channel	Enum channel associated with this channel
	     * @param OutMods	Mapping of channel enum to mods
	     */
        public void GetAllAggregatorMods(EGameplayModEvaluationChannel Channel,
            Dictionary<EGameplayModEvaluationChannel, List<FAggregatorMod>> OutMods)
        {

        }


        /**
         * Called when the mod channel's gameplay effect dependencies have potentially been swapped out for new ones, like when GE arrays are cloned.
         * Updates mod handles appropriately.
         * 
         * @param SwappedDependencies	Mapping of old gameplay effect handles to new replacements
         */
        public void OnActiveEffectDependenciesSwapped(Dictionary<FActiveGameplayEffectHandle, FActiveGameplayEffectHandle> SwappedDependencies)
        {

        }

      
}

    public class FAggregatorModChannelContainer
    {
        /** Mapping of evaluation channel enumeration to actual struct representation */
        private Dictionary<EGameplayModEvaluationChannel, FAggregatorModChannel> ModChannelsMap;

        /**
	     * Find or add a modifier channel for the specified enum value
	     * 
	     * @param Channel	Channel to find or add a modifier channel for
	     * 
	     * @return Modifier channel for the specified enum value
	     */
        public FAggregatorModChannel FindOrAddModChannel(EGameplayModEvaluationChannel Channel)
        {
            return null;
        }

        /** Simple accessor to the current number of modifier channels active */
        public int GetNumChannels()
        {
            return 1;
        }

        /**
	     * Evaluates the result of the specified base value run through each existing evaluation channel's modifiers in numeric order
	     * with the specified evaluation parameters. The result of the evaluation of an individual channel acts as the new base value
	     * to the channel that follows it until all channels have been evaluated.
	     * 
	     * EXAMPLE: Base Value: 2, Channel 0 has a +2 Additive Mod, Channel 1 is provided a base value of 4 to run through its modifiers
	     * 
	     * @param InlineBaseValue	Initial base value to use in the first evaluation channel
	     * @param Parameters		Additional evaluation parameters
	     * 
	     * @return Result of the specified base value run through each modifier in each evaluation channel in numeric order
	     */
        public float EvaluateWithBase(float InlineBaseValue, FAggregatorEvaluateParameters Parameters)
        {
            return 1f;
        }

        /**
	     * Similar to EvaluateWithBase (see comment there for details), but terminates early after evaluating the specified final channel instead of
	     * continuing through every possible channel
	     * 
	     * @param InlineBaseValue	Initial base value to use in the first evaluation channel
	     * @param Parameters		Additional evaluation parameters
	     * @param FinalChannel		Channel to terminate evaluation with (inclusive)
	     * 
	     * @return Result of the specified base value run through each modifier in each evaluation channel in numeric order
	     */
        public float EvaluateWithBaseToChannel(float InlineBaseValue, FAggregatorEvaluateParameters Parameters,
            EGameplayModEvaluationChannel FinalChannel)
        {
            return 1f;
        }

        /**
	     * Evaluates a final value in reverse, attempting to determine a base value from the modifiers within all of the channels.
	     * The operation proceeds through all channels in reverse order, with the result of the evaluation of an individual channel used
	     * as the new final value to the channel that precedes it numerically. If any channel has a condition that prevents it from
	     * computing correctly (such as an override mod), this function just returns the original final value. This is predominantly used
	     * for filling in base values on clients from replication for float-based attributes.
	     * 
	     * @note This will be deprecated/removed soon with the transition to struct-based attributes.
	     * 
	     * @param FinalValue	Final value to reverse evaluate
	     * @param Parameters	Evaluation parameters to use for the reverse evaluation
	     * 
	     * @return If possible, the base value from the reverse evaluation. If not possible, the original final value is returned.
	     */
        public float ReverseEvaluate(float FinalValue, FAggregatorEvaluateParameters Parameters)
        {
            return 1f;
        }

        /** Calls ::UpdateQualifies on each mod */
        public void EvaluateQualificationForAllMods(FAggregatorEvaluateParameters Parameters)
        {

        }

        /**
	     * Removes any mods from every channel matching the specified handle
	     * 
	     * @param ActiveHandle	Handle to use for removal
	     */
        public void RemoveAggregatorMod(FActiveGameplayEffectHandle ActiveHandle)
        {

        }

        /**
	     * Adds the mods from specified container to this one
	     * 
	     * @param Other	Container to add mods from
	     */
        public void AddModsFrom(FAggregatorModChannelContainer Other)
        {
        }

        /**
	     * Populate a mapping of channel to corresponding mods for debugging purposes
	     * 
	     * @param OutMods	Mapping of channel enum to mods
	     */
        public void GetAllAggregatorMods(Dictionary<EGameplayModEvaluationChannel, List<FAggregatorMod>> OutMods)
        {

        }

        /**
         * Called when the container's gameplay effect dependencies have potentially been swapped out for new ones, like when GE arrays are cloned.
         * Updates each channel appropriately.
         * 
         * @param SwappedDependencies	Mapping of old gameplay effect handles to new replacements
         */
        public void OnActiveEffectDependenciesSwapped(Dictionary<FActiveGameplayEffectHandle, FActiveGameplayEffectHandle> SwappedDependencies)
        {

        }

	    
    }
}
