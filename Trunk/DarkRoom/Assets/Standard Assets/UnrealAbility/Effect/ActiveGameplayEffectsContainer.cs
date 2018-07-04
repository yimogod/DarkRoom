using System;
using System.Collections.Generic;

namespace DarkRoom.GamePlayAbility
{

    public struct FActiveGameplayEffectsContainer
    {
        public UAbilitySystemComponent Owner;

        public Action<FActiveGameplayEffect> OnActiveGameplayEffectRemovedDelegate;

    }
}
