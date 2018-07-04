using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility
{
    public class FGameplayAbilitySpecDef
    {
        FGameplayAbilitySpecDef()
        {

        }

        //TSubclassOf<UGameplayAbility> Ability;

        // Deprecated for LevelScalableFloat
        int Level;
        float LevelScalableFloat;
        int InputID;
        //EGameplayEffectGrantedAbilityRemovePolicy RemovalPolicy;

        CUnitEntity SourceObject;

        // SetbyCaller Magnitudes that were passed in to this ability by a GE (GE's that grant abilities). Made available so that 
        Dictionary<CGameplayTag, float> SetByCallerTagMagnitudes;

        /** This handle can be set if the SpecDef is used to create a real FGameplaybilitySpec */
        //FGameplayAbilitySpecHandle AssignedHandle;
    };
}