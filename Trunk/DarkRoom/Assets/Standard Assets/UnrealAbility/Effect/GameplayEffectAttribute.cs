using System.Collections.Generic;

namespace DarkRoom.GamePlayAbility
{
    public struct FGameplayEffectModifiedAttribute
    {
        /** The attribute that has been modified */
        FGameplayAttribute Attribute;

        /** Total magnitude applied to that attribute */
        float TotalMagnitude;
    }

    public struct FModifierSpec
    {
        public float GetEvaluatedMagnitude() { return EvaluatedMagnitude; }
        // @todo: Probably need to make the modifier info private so people don't go gunking around in the magnitude
        /** In the event that the modifier spec requires custom magnitude calculations, this is the authoritative, last evaluated value of the magnitude */
        float EvaluatedMagnitude;
    }



    public class FGameplayEffectAttributeCaptureSpec
    {
        private FGameplayEffectAttributeCaptureDefinition BackingDefinition;

        /** Ref to the aggregator for the captured attribute */
        private FAggregator AttributeAggregator;

        public FGameplayEffectAttributeCaptureSpec()
        {

        }

        public FGameplayEffectAttributeCaptureSpec(FGameplayEffectAttributeCaptureDefinition InDefinition)
        {

        }

        /**
	     * Returns whether the spec actually has a valid capture yet or not
	     * 
	     * @return True if the spec has a valid attribute capture, false if it does not
	     */
        public bool HasValidCapture()
        {
            return true;
        }

        /**
	     * Attempts to calculate the magnitude of the captured attribute given the specified parameters. Can fail if the spec doesn't have
	     * a valid capture yet.
	     * 
	     * @param InEvalParams	Parameters to evaluate the attribute under
	     * @param OutMagnitude	[OUT] Computed magnitude
	     * 
	     * @return True if the magnitude was successfully calculated, false if it was not
	     */
        public bool AttemptCalculateAttributeMagnitude(FAggregatorEvaluateParameters InEvalParams, ref float OutMagnitude)
        {
            return true;
        }

        /**
	     * Attempts to calculate the magnitude of the captured attribute given the specified parameters, up to the specified evaluation channel (inclusive).
	     * Can fail if the spec doesn't have a valid capture yet.
	     * 
	     * @param InEvalParams	Parameters to evaluate the attribute under
	     * @param FinalChannel	Evaluation channel to terminate the calculation at
	     * @param OutMagnitude	[OUT] Computed magnitude
	     * 
	     * @return True if the magnitude was successfully calculated, false if it was not
	     */
        public bool AttemptCalculateAttributeMagnitudeUpToChannel(FAggregatorEvaluateParameters InEvalParams, EGameplayModEvaluationChannel FinalChannel, ref float OutMagnitude)
        {
            return false;
        }

        /**
	     * Attempts to calculate the magnitude of the captured attribute given the specified parameters, including a starting base value. 
	     * Can fail if the spec doesn't have a valid capture yet.
	     * 
	     * @param InEvalParams	Parameters to evaluate the attribute under
	     * @param InBaseValue	Base value to evaluate the attribute under
	     * @param OutMagnitude	[OUT] Computed magnitude
	     * 
	     * @return True if the magnitude was successfully calculated, false if it was not
	     */
        public bool AttemptCalculateAttributeMagnitudeWithBase(FAggregatorEvaluateParameters InEvalParams, float InBaseValue, ref float OutMagnitude)
        {
            return false;
        }

        /**
	     * Attempts to calculate the base value of the captured attribute given the specified parameters. Can fail if the spec doesn't have
	     * a valid capture yet.
	     * 
	     * @param OutBaseValue	[OUT] Computed base value
	     * 
	     * @return True if the base value was successfully calculated, false if it was not
	     */
        public bool AttemptCalculateAttributeBaseValue(ref float OutBaseValue)
        {
            return false;
        }

        /**
	     * Attempts to calculate the "bonus" magnitude (final - base value) of the captured attribute given the specified parameters. Can fail if the spec doesn't have
	     * a valid capture yet.
	     * 
	     * @param InEvalParams		Parameters to evaluate the attribute under
	     * @param OutBonusMagnitude	[OUT] Computed bonus magnitude
	     * 
	     * @return True if the bonus magnitude was successfully calculated, false if it was not
	     */
        public bool AttemptCalculateAttributeBonusMagnitude(FAggregatorEvaluateParameters InEvalParams, ref float OutBonusMagnitude)
        {
            return false;
        }

        /**
	     * Attempts to calculate the contribution of the specified GE to the captured attribute given the specified parameters. Can fail if the spec doesn't have
	     * a valid capture yet.
	     *
	     * @param InEvalParams		Parameters to evaluate the attribute under
	     * @param ActiveHandle		Handle of the gameplay effect to query about
	     * @param OutBonusMagnitude	[OUT] Computed bonus magnitude
	     *
	     * @return True if the bonus magnitude was successfully calculated, false if it was not
	     */
        public bool AttemptCalculateAttributeContributionMagnitude(FAggregatorEvaluateParameters InEvalParams,
            FActiveGameplayEffectHandle ActiveHandle, ref float OutBonusMagnitude)
        {
            return false;
        }

        /**
	     * Attempts to populate the specified aggregator with a snapshot of the backing captured aggregator. Can fail if the spec doesn't have
	     * a valid capture yet.
	     *
	     * @param OutAggregatorSnapshot	[OUT] Snapshotted aggregator, if possible
	     *
	     * @return True if the aggregator was successfully snapshotted, false if it was not
	     */
        public bool AttemptGetAttributeAggregatorSnapshot(FAggregator OutAggregatorSnapshot)
        {
            return false;
        }

        /**
	     * Attempts to populate the specified aggregator with all of the mods of the backing captured aggregator. Can fail if the spec doesn't have
	     * a valid capture yet.
	     *
	     * @param OutAggregatorToAddTo	[OUT] Aggregator with mods appended, if possible
	     *
	     * @return True if the aggregator had mods successfully added to it, false if it did not
	     */
        public bool AttemptAddAggregatorModsToAggregator(FAggregator OutAggregatorToAddTo)
        {
            return false;
        }

        /** Gathers made for a given capture. Note all mods are returned but only some will be qualified (use Qualified() func to determine) */
        public bool AttemptGatherAttributeMods(FAggregatorEvaluateParameters InEvalParams,
            Dictionary<EGameplayModEvaluationChannel, List<FAggregatorMod>> OutModMap)
        {
            return false;
        }

        /** Simple accessor to backing capture definition */
        public FGameplayEffectAttributeCaptureDefinition GetBackingDefinition()
        {
            return null;
        }

        /** Register this handle with linked aggregators */
        public void RegisterLinkedAggregatorCallback(FActiveGameplayEffectHandle Handle)
        {

        }

        /** Unregister this handle with linked aggregators */
        public void UnregisterLinkedAggregatorCallback(FActiveGameplayEffectHandle Handle)
        {

        }

        /** Return true if this capture should be recalculated if the given aggregator has changed */
        public bool ShouldRefreshLinkedAggregator(FAggregator ChangedAggregator)
        {
            return false;
        }

        /** Swaps any internal references From aggregator To aggregator. Used when cloning */
        public void SwapAggregator(FAggregator From, FAggregator To)
        {

        }

        
    }


public class FGameplayEffectAttributeCaptureSpecContainer
    {
        /** Swaps any internal references From aggregator To aggregator. Used when cloning */
        public void SwapAggregator(FAggregator From, FAggregator To) { }


        /** Captured attributes from the source of a gameplay effect */
        private List<FGameplayEffectAttributeCaptureSpec> SourceAttributes;

        /** Captured attributes from the target of a gameplay effect */
        private List<FGameplayEffectAttributeCaptureSpec> TargetAttributes;

        /** If true, has at least one capture spec that did not request a snapshot */
        private bool bHasNonSnapshottedAttributes;

        public FGameplayEffectAttributeCaptureSpecContainer(FGameplayEffectAttributeCaptureSpecContainer Other)
        {

        }

        /**
	     * Add a definition to be captured by the owner of the container. Will not add the definition if its exact
	     * match already exists within the container.
	     * 
	     * @param InCaptureDefinition	Definition to capture with
	     */
        public void AddCaptureDefinition(FGameplayEffectAttributeCaptureDefinition InCaptureDefinition)
        {

        }

        /**
	     * Capture source or target attributes from the specified component. Should be called by the container's owner.
	     * 
	     * @param InAbilitySystemComponent	Component to capture attributes from
	     * @param InCaptureSource			Whether to capture attributes as source or target
	     */
        public void CaptureAttributes(UAbilitySystemComponent InAbilitySystemComponent,
            EGameplayEffectAttributeCaptureSource InCaptureSource)
        {

        }

        /**
	        * Find a capture spec within the container matching the specified capture definition, if possible.
	        * 
	        * @param InDefinition				Capture definition to use as the search basis
	        * @param bOnlyIncludeValidCapture	If true, even if a spec is found, it won't be returned if it doesn't also have a valid capture already
	        * 
	        * @return The found attribute spec matching the specified search params, if any
	        */
        public FGameplayEffectAttributeCaptureSpec FindCaptureSpecByDefinition(FGameplayEffectAttributeCaptureDefinition InDefinition,
            bool bOnlyIncludeValidCapture)
        {
            FGameplayEffectAttributeCaptureSpec a = new FGameplayEffectAttributeCaptureSpec();
            return a;
        }

        /**
	     * Determines if the container has specs with valid captures for all of the specified definitions.
	     * 
	     * @param InCaptureDefsToCheck	Capture definitions to check for
	     * 
	     * @return True if the container has valid capture attributes for all of the specified definitions, false if it does not
	     */
        public bool HasValidCapturedAttributes(List<FGameplayEffectAttributeCaptureDefinition> InCaptureDefsToCheck)
        {
            return false;
        }

        /** Returns whether the container has at least one spec w/o snapshotted attributes */
        public bool HasNonSnapshottedAttributes()
        {
            return false;
        }

        /** Registers any linked aggregators to notify this active handle if they are dirtied */
        public void RegisterLinkedAggregatorCallbacks(FActiveGameplayEffectHandle Handle)
        {

        }

        /** Unregisters any linked aggregators from notifying this active handle if they are dirtied */
        public void UnregisterLinkedAggregatorCallbacks(FActiveGameplayEffectHandle Handle)
        {

        }

    
    }
}
