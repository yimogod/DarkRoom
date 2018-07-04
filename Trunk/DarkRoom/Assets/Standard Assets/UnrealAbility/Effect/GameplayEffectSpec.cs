using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility
{
    public class FGameplayEffectSpec
    {
        // -----------------------------------------------------------------------

        /* GameplayEfect definition. The static data that this spec points to. */
        public UGameplayEffect Def;

        /* A list of attributes that were modified during the application of this spec */
        public List<FGameplayEffectModifiedAttribute> ModifiedAttributes;

        /* Attributes captured by the spec that are relevant to custom calculations, potentially in owned modifiers, etc.; NOT replicated to clients */
        public FGameplayEffectAttributeCaptureSpecContainer CapturedRelevantAttributes;

        /* other effects that need to be applied to the target if this effect is successful */
        public List<FGameplayEffectSpecHandle> TargetEffectSpecs;

        // The duration in seconds of this effect
        // instantaneous effects should have a duration of FGameplayEffectConstants::INSTANT_APPLICATION
        // effects that last forever should have a duration of FGameplayEffectConstants::INFINITE_DURATION
        public float Duration;

        // The period in seconds of this effect.
        // Nonperiodic effects should have a period of FGameplayEffectConstants::NO_PERIOD
        public float Period;

        // The chance, in a 0.0-1.0 range, that this GameplayEffect will be applied to the target Attribute or GameplayEffect.
        public float ChanceToApplyToTarget;

        // Captured Source Tags on GameplayEffectSpec creation.	
        public FTagContainerAggregator CapturedSourceTags;

        // Tags from the target, captured during execute	
        public FTagContainerAggregator CapturedTargetTags;

        /* Tags that are granted and that did not come from the UGameplayEffect def. These are replicated. */
        public CGameplayTagContainer DynamicGrantedTags;

        /* Tags that are on this effect spec and that did not come from the UGameplayEffect def. These are replicated. */
        public CGameplayTagContainer DynamicAssetTags;

        public List<FModifierSpec> Modifiers;

        public int StackCount;

        /* Whether the spec has had its source attribute capture completed or not yet */
        private bool bCompletedSourceAttributeCapture = true;

        /* Whether the spec has had its target attribute capture completed or not yet */
        private bool bCompletedTargetAttributeCapture = true;

        /* Whether the duration of the spec is locked or not; If it is, attempts to set it will fail */
        private bool bDurationLocked = true;

        private List<FGameplayAbilitySpecDef> GrantedAbilitySpecs;

        /* Map of set by caller magnitudes */
        public Dictionary<string, float> SetByCallerNameMagnitudes;
        public Dictionary<CGameplayTag, float> SetByCallerTagMagnitudes;

        private FGameplayEffectContextHandle EffectContext; // This tells us how we got here (who / what applied us)
        private float Level;

        FGameplayEffectSpec(UGameplayEffect InDef, FGameplayEffectContextHandle InEffectContext,
            float Level)
        {

        }

        FGameplayEffectSpec(FGameplayEffectSpec Other, FGameplayEffectContextHandle InEffectContext)
        {

        }


        // Can be called manually but it is preferred to use the 3 parameter ructor
        public void Initialize(UGameplayEffect InDef, FGameplayEffectContextHandle InEffectContext,
            float Level = FGameplayEffectConstants.INVALID_LEVEL)
        {

        }

        // Initialize the spec as a linked spec. The original spec's context is preserved except for the original GE asset tags, which are stripped out
        public void InitializeFromLinkedSpec(UGameplayEffect InDef, FGameplayEffectSpec OriginalSpec)
        {

        }

        // Copies SetbyCallerMagnitudes from OriginalSpec into this
        public void CopySetByCallerMagnitudes(FGameplayEffectSpec OriginalSpec)
        {

        }

        // Copies SetbuCallerMagnitudes, but only if magnitudes don't exist in our map (slower but preserves data)
        public void MergeSetByCallerMagnitudes(Dictionary<CGameplayTag, float> Magnitudes)
        {

        }

        /**
	      Determines if the spec has capture specs with valid captures for all of the specified definitions.
	      
	      @param InCaptureDefsToCheck	Capture definitions to check for
	      
	      @return True if the container has valid capture attributes for all of the specified definitions, false if it does not
	     */
        public bool HasValidCapturedAttributes(List<FGameplayEffectAttributeCaptureDefinition> InCaptureDefsToCheck)
        {
            return false;
        }

        /* Looks for an existing modified attribute struct, may return NULL */
        public FGameplayEffectModifiedAttribute GetModifiedAttribute(FGameplayAttribute Attribute)
        {
            FGameplayEffectModifiedAttribute a = new FGameplayEffectModifiedAttribute();
            return a;
        }

        /* Adds a new modified attribute struct, will always add so check to see if it exists first */
        public FGameplayEffectModifiedAttribute AddModifiedAttribute(FGameplayAttribute Attribute)
        {
            FGameplayEffectModifiedAttribute a = new FGameplayEffectModifiedAttribute();
            return a;
        }

        /**
	      Helper function to attempt to calculate the duration of the spec from its GE definition
	      
	      @param OutDefDuration	Computed duration of the spec from its GE definition; Not the actual duration of the spec
	      
	      @return True if the calculation was successful, false if it was not
	     */
        public bool AttemptCalculateDurationFromDef(ref float OutDefDuration)
        {
            return true;
        }

        /* Sets duration. This should only be called as the GameplayEffect is being created and applied; Ignores calls after attribute capture */
        public void SetDuration(float NewDuration, bool bLockDuration)
        {
        }

        public float GetDuration()
        {
            return 1f;
        }

        public float GetPeriod()
        {
            return 1f;
        }

        public float GetChanceToApplyToTarget()
        {
            return 1f;
        }

        /* Set the context info: who and where this spec came from. */
        public void SetContext(FGameplayEffectContextHandle NewEffectContext)
        {

        }

        public FGameplayEffectContextHandle GetContext()
        {
            return EffectContext;
        }

        // Appends all tags granted by this gameplay effect spec
        public void GetAllGrantedTags(ref CGameplayTagContainer Container)
        {

        }

        // Appends all tags that apply to this gameplay effect spec
        public void GetAllAssetTags(ref CGameplayTagContainer Container)
        {

        }

        /* Sets the magnitude of a SetByCaller modifier */
        public void SetSetByCallerMagnitude(string DataName, float Magnitude)
        {

        }

        /* Sets the magnitude of a SetByCaller modifier */
        public void SetSetByCallerMagnitude(CGameplayTag DataTag, float Magnitude)
        {

        }

        /* Returns the magnitude of a SetByCaller modifier. Will return 0.f and Warn if the magnitude has not been set. */
        public float GetSetByCallerMagnitude(string DataName, bool WarnIfNotFound = true, float DefaultIfNotFound = 0f)
        {
            return 1f;
        }

        /* Returns the magnitude of a SetByCaller modifier. Will return 0.f and Warn if the magnitude has not been set. */
        public float GetSetByCallerMagnitude(CGameplayTag DataTag, bool WarnIfNotFound = true, float DefaultIfNotFound = 0f)
        {
            return 1f;
        }

        public void SetLevel(float InLevel)
        {

        }

        public float GetLevel()
        {
            return 1f;
        }

        public void PrintAll()
        {

        }

        public string ToSimpleString()
        {
            return "";
        }

        public FGameplayEffectContextHandle GetEffectContext()
        {
            return EffectContext;
        }

        public void DuplicateEffectContext()
        {
            //EffectContext = EffectContext.Duplicate();
        }

        public void CaptureAttributeDataFromTarget(UAbilitySystemComponent TargetAbilitySystemComponent)
        {

        }

        /**
          Get the computed magnitude of the modifier on the spec with the specified index
          
          @param ModifierIndx			Modifier to get
          @param bFactorInStackCount	If true, the calculation will include the stack count
          
          @return Computed magnitude
         */
        public float GetModifierMagnitude(int ModifierIdx, bool bFactorInStackCount)
        {
            return 1f;
        }

        public void CalculateModifierMagnitudes()
        {

        }

        /* Recapture attributes from source and target for cloning */
        public void RecaptureAttributeDataForClone(UAbilitySystemComponent OriginalASC, UAbilitySystemComponent NewASC)
        {

        }

        /* Recaptures source actor tags of this spec without modifying anything else */
        public void RecaptureSourceActorTags()
        {

        }

        /* Helper function to initialize all of the capture definitions required by the spec */
        public void SetupAttributeCaptureDefinitions()
        {

        }

        /* Helper function that returns the duration after applying relevant modifiers from the source and target ability system components */
        public float CalculateModifiedDuration()
        {
            return 1f;
        }

        private void CaptureDataFromSource()
        {

        }

        
    }

    public class FGameplayEffectSpecHandle
    {

        FGameplayEffectSpecHandle(FGameplayEffectSpec DataPtr)
        {

        }

        FGameplayEffectSpec Data;

        bool IsValidCache;

        public void Clear()
        {
            //Data.Reset();
        }

        public bool IsValid()
        {
            //return Data.IsValid();
            return true;
        }
    }
}
