using System.Collections.Generic;
using DarkRoom.Game;
using UnityEngine;

namespace DarkRoom.GamePlayAbility
{
    public class FGameplayEffectContext
    {
        // The object pointers here have to be weak because contexts aren't necessarily tracked by GC in all cases

        /* Instigator actor, the actor that owns the ability system component */
        protected CUnitEntity Instigator;

        /* The physical actor that actually did the damage, can be a weapon or projectile */


        protected CUnitEntity EffectCauser;

        /* the ability CDO that is responsible for this effect context (replicated) */
        protected UGameplayAbility AbilityCDO;

        /* the ability instance that is responsible for this effect context (NOT replicated) */
        protected UGameplayAbility AbilityInstanceNotReplicated;




        protected int AbilityLevel;

        /* Object this effect was created from, can be an actor or static object. Useful to bind an effect to a gameplay object */
        protected CUnitEntity SourceObject;

        /* The ability system component that's bound to instigator */
        protected UAbilitySystemComponent InstigatorAbilitySystemComponent;


        protected List<CUnitEntity> Actors;

        /* Trace information - may be NULL in many cases */
        //protected RaycastHit HitResult;

        protected Vector3 WorldOrigin;

        protected bool bHasWorldOrigin = true;

        /* True if the SourceObject can be replicated. This bool is not replicated itself. */
        protected bool bReplicateSourceObject = true;

        FGameplayEffectContext(CUnitEntity InInstigator, CUnitEntity InEffectCauser)
        {
            AddInstigator(InInstigator, InEffectCauser);
        }

        /* Returns the list of gameplay tags applicable to this effect, defaults to the owner's tags */
        public virtual void GetOwnedGameplayTags(ref CGameplayTagContainer ActorTagContainer, ref CGameplayTagContainer SpecTagContainer)
        {

        }

        /* Sets the instigator and effect causer. Instigator is who owns the ability that spawned this, EffectCauser is the actor that is the physical source of the effect, such as a weapon. They can be the same. */
        public virtual void AddInstigator(CUnitEntity InInstigator, CUnitEntity InEffectCauser)
        {

        }

        /* Sets the ability that was used to spawn this */
        public virtual void SetAbility(UGameplayAbility InGameplayAbility)
        {

        }

        /* Returns the immediate instigator that applied this effect */
        public virtual CUnitEntity GetInstigator()
        {
            return Instigator;
        }

        /* returns the CDO of the ability used to instigate this context */
        public UGameplayAbility GetAbility()
        {
            return null;
        }

        public UGameplayAbility GetAbilityInstance_NotReplicated()
        {
            return null;
        }

        public int GetAbilityLevel()
        {
            return AbilityLevel;
        }

        /* Returns the ability system component of the instigator of this effect */
        public virtual UAbilitySystemComponent GetInstigatorAbilitySystemComponent()
        {
            return InstigatorAbilitySystemComponent;
        }

        /* Returns the physical actor tied to the application of this effect */
        public virtual CUnitEntity GetEffectCauser()
        {
            return EffectCauser;
        }

        public void SetEffectCauser(CUnitEntity InEffectCauser)
        {
            EffectCauser = InEffectCauser;
        }

        /* Should always return the original instigator that started the whole chain. Subclasses can override what this does */
        public virtual CUnitEntity GetOriginalInstigator()
        {
            return Instigator;
        }

        /* Returns the ability system component of the instigator that started the whole chain */
        public virtual UAbilitySystemComponent GetOriginalInstigatorAbilitySystemComponent()
        {
            return InstigatorAbilitySystemComponent;
        }

        /* Sets the object this effect was created from. */
        public virtual void AddSourceObject(CUnitEntity NewSourceObject)
        {
        }

        /* Returns the object this effect was created from. */
        public virtual CUnitEntity GetSourceObject()
        {
            return SourceObject;
        }

        public virtual void AddActors(List<CUnitEntity> IActor, bool bReset = false)
        {

        }

        //public virtual void AddHitResult(RaycastHit InHitResult, bool bReset = false)
      //  {

     //   }

        public virtual List<CUnitEntity> GetActors()
        {
            return Actors;
        }

     //   public virtual RaycastHit GetHitResult()
    //    {
   //         return new RaycastHit();
   //     }

        public virtual void AddOrigin(Vector3 InOrigin)
        {

        }

        public virtual Vector3 GetOrigin()
        {
            return WorldOrigin;
        }

        public virtual bool HasOrigin()
        {
            return bHasWorldOrigin;
        }

        public virtual string ToString()
        {
            //return Instigator.IsValid() ? Instigator->GetName() :("NONE");
            return "";
        }

        /* Creates a copy of this context, used to duplicate for later modifications */
        public virtual FGameplayEffectContext Duplicate()
        {
            FGameplayEffectContext NewContext = new FGameplayEffectContext(null, null);
            return NewContext;
        }

        public virtual bool IsLocallyControlled()
        {
            return false;
        }

        public virtual bool IsLocallyControlledPlayer()
        {
            return false;
        }
    }

    public class FGameplayEffectContextHandle
    {
        private FGameplayEffectContext Data;
        /* ructs from an existing context, should be allocated by new */
        FGameplayEffectContextHandle(FGameplayEffectContext DataPtr)
        {
            Data = DataPtr;
        }

        void Clear()
        {
            //Data.Reset();
        }

        public bool IsValid()
        {
            //return Data.IsValid();
            return false;
        }

        public FGameplayEffectContext Get()
        {
            //return IsValid() ? Data.Get() : null;
            return null;
        }

        /* Returns the list of gameplay tags applicable to this effect, defaults to the owner's tags */
        public void GetOwnedGameplayTags(ref CGameplayTagContainer ActorTagContainer, ref CGameplayTagContainer SpecTagContainer)
        {
            if (IsValid())
            {
                //Data->GetOwnedGameplayTags(ActorTagContainer, SpecTagContainer);
            }
        }

        /* Sets the instigator and effect causer. Instigator is who owns the ability that spawned this, EffectCauser is the actor that is the physical source of the effect, such as a weapon. They can be the same. */
        public void AddInstigator(CUnitEntity  InInstigator, CUnitEntity  InEffectCauser)
	    {
		    if (IsValid())
		    {
			    //Data->AddInstigator(InInstigator, InEffectCauser);
            }
        }

        public void SetAbility(UGameplayAbility InGameplayAbility)
        {
            if (IsValid())
            {
                //Data->SetAbility(InGameplayAbility);
            }
        }

        /* Returns the immediate instigator that applied this effect */
        public virtual CUnitEntity GetInstigator()
        {
            if (IsValid())
            {
                //return Data->GetInstigator();
            }
            return null;
        }

        /* Returns the Ability CDO */
        public UGameplayAbility GetAbility()
        {
            if (IsValid())
            {
                //return Data->GetAbility();
            }
            return null;
        }

        /* Returns the Ability Instance (never replicated) */
        public UGameplayAbility GetAbilityInstance_NotReplicated()
        {
            if (IsValid())
            {
                //return Data->GetAbilityInstance_NotReplicated();
            }
            return null;
        }

        public int GetAbilityLevel()
        {
            if (IsValid())
            {
               // return Data->GetAbilityLevel();
            }
            return 1;
        }

        /* Returns the ability system component of the instigator of this effect */
        public virtual UAbilitySystemComponent GetInstigatorAbilitySystemComponent()
        {
            if (IsValid())
            {
                //return Data->GetInstigatorAbilitySystemComponent();
            }
            return null;
        }

        /* Returns the physical actor tied to the application of this effect */
        public virtual CUnitEntity GetEffectCauser()
        {
            if (IsValid())
            {
                //return Data->GetEffectCauser();
            }
            return null;
        }

        /* Should always return the original instigator that started the whole chain. Subclasses can override what this does */
        public CUnitEntity GetOriginalInstigator()
        {
            if (IsValid())
            {
                //return Data->GetOriginalInstigator();
            }
            return null;
        }

        /* Returns the ability system component of the instigator that started the whole chain */
        public UAbilitySystemComponent GetOriginalInstigatorAbilitySystemComponent()
        {
            if (IsValid())
            {
                //return Data->GetOriginalInstigatorAbilitySystemComponent();
            }
            return null;
        }

        /* Sets the object this effect was created from. */
        public void AddSourceObject(CUnitEntity NewSourceObject)
        {
            if (IsValid())
            {
                //Data->AddSourceObject(NewSourceObject);
            }
        }

        /* Returns the object this effect was created from. */
        public CUnitEntity GetSourceObject()
        {
            if (IsValid())
            {
                //return Data->GetSourceObject();
            }
            return null;
        }

        /* Returns if the instigator is locally controlled */
        public bool IsLocallyControlled()
        {
            if (IsValid())
            {
                //return Data->IsLocallyControlled();
            }
            return false;
        }

        public bool IsLocallyControlledPlayer()
        {
            if (IsValid())
            {
                //return Data->IsLocallyControlledPlayer();
            }
            return false;
        }

        public void AddActors(List<CUnitEntity> InActors, bool bReset = false)
        {
            if (IsValid())
            {
                //Data->AddActors(InActors, bReset);
            }
        }

  //      public void AddHitResult(RaycastHit InHitResult, bool bReset = false)
  //      {
  //          if (IsValid())
  //          {
                //Data->AddHitResult(InHitResult, bReset);
   //         }
   //     }

        public List<CUnitEntity> GetActors()
        {
            //return Data->GetActors();
            return null;
        }

    //    public RaycastHit GetHitResult()
   //     {
   //         if (IsValid())
   //         {
                //return Data->GetHitResult();
     //       }
     //       RaycastHit a = new RaycastHit();
    //        return a;
    //    }

        public void AddOrigin(Vector3 InOrigin)
        {
            if (IsValid())
            {
                //Data->AddOrigin(InOrigin);
            }
        }

        public Vector3 GetOrigin()
        {
            if (IsValid())
            {
                //return Data->GetOrigin();
            }

            return Vector3.zero;
        }

        public virtual bool HasOrigin()
        {
            if (IsValid())
            {
                //return Data->HasOrigin();
            }
            return false;
        }

        public string ToString()
        {
            //return IsValid() ? Data->ToString() : FString(TEXT("NONE"));
            return "";
        }

        /* Creates a deep copy of this handle, used before modifying */
        public FGameplayEffectContextHandle Duplicate()
        {
            /*if (IsValid())
            {
                FGameplayEffectContext NewContext = Data->Duplicate();
                return FGameplayEffectContextHandle(NewContext);
            }
            else
            {
                return FGameplayEffectContextHandle();
            }*/
            return null;
        }
        
    }
}
