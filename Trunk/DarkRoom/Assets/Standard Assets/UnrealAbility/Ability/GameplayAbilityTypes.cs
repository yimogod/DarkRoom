using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility
{
    public class EGameplayAbilityInstancingPolicy
    {
        /**
         *	How the ability is instanced when executed. This limits what an ability can do in its implementation. For example, a NonInstanced
         *	Ability cannot have state. It is probably unsafe for an InstancedPerActor ability to have latent actions, etc.
         */

        enum Type
        {
            // This ability is never instanced. Anything that executes the ability is operating on the CDO.
            NonInstanced,

            // Each actor gets their own instance of this ability. State can be saved, replication is possible.
            InstancedPerActor,

            // We instance this ability each time it is executed. Replication possible but not recommended.
            InstancedPerExecution,
        };
    }

    public class EGameplayAbilityTriggerSource
    {
        /**	Defines what type of trigger will activate the ability, paired to a tag */

        enum Type
        {
            // Triggered from a gameplay event, will come with payload
            GameplayEvent,

            // Triggered if the ability's owner gets a tag added, triggered once whenever it's added
            OwnedTagAdded,

            // Triggered if the ability's owner gets tag added, removed when the tag is removed
            OwnedTagPresent,
        };
    }

    public struct CGameplayAbilityActorInfo
    {
        //CActorEntity OwnerActor;

        ///** The physical representation of the owner, used for targeting and animation. This will often be null! */
        //CActorEntity AvatarActor;

        ///** PlayerController associated with the owning actor. This will often be null! */
        //CPlayerController PlayerController;

        ///** Ability System component associated with the owner actor, shouldn't be null */
        //CAbilitySystemComponent AbilitySystemComponent;

        ///** Skeletal mesh of the avatar actor. Often null */
        //TWeakObjectPtr<USkeletalMeshComponent> SkeletalMeshComponent;

        ///** Anim instance of the avatar actor. Often null */
        //TWeakObjectPtr<UAnimInstance> AnimInstance;

        ///** Movement component of the avatar actor. Often null */
        //TWeakObjectPtr<UMovementComponent> MovementComponent;

        ///** Accessor to get the current anim instance from the SkeletalMeshComponent */
        //UAnimInstance* GetAnimInstance()
        //{
        //    return (SkeletalMeshComponent.IsValid() ? SkeletalMeshComponent->GetAnimInstance() : nullptr);
        //}

        ///** Returns true if this actor is locally controlled. Only true for players on the client that owns them */
        //bool IsLocallyControlled()
        //{
        //    return true;
        //}

        //bool IsLocallyControlledPlayer()
        //{
        //    return true;
        //}

        ///** Returns true if the owning actor has net authority */
        //bool IsNetAuthority()
        //{
        //    return true;
        //}

        ///** Initializes the info from an owning actor. Will set both owner and avatar */
        //virtual void InitFromActor(AActor* OwnerActor, AActor* AvatarActor,
        //    UAbilitySystemComponent* InAbilitySystemComponent)
        //{

        //}

        ///** Sets a new avatar actor, keeps same owner and ability system component */
        //virtual void SetAvatarActor(AActor* AvatarActor)
        //{

        //}

        ///** Clears out any actor info, both owner and avatar */
        //virtual void ClearActorInfo()
        //{

        //}
    }
}
