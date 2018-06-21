using System;
using System.Collections.Generic;

namespace DarkRoom.Game
{
    /// <summary>
    /// 资源集合, 用了交集差集
    /// </summary>
    public struct FGameplayResourceSet
    {
        public static int MaxResources = 64;

        private int Flags;

        public FGameplayResourceSet(int InFlag)
        {
            Flags = InFlag;
        }

        public int GetFlags()
        {
            return Flags;
        }

        public bool IsEmpty()
        {
            return Flags == 0;
        }

        public FGameplayResourceSet AddID(int ResourceID)
        {
            Flags |= (1 << ResourceID);
            return this;
        }

        public FGameplayResourceSet RemoveID(int ResourceID)
        {
            Flags &= ~(1 << ResourceID);
            return this;
        }

        public bool HasID(int ResourceID)
        {
            return (Flags & (1 << ResourceID)) != 0;
        }

        public FGameplayResourceSet AddSet(FGameplayResourceSet Other)
        {
            Flags |= Other.Flags;
            return this;
        }

        public FGameplayResourceSet RemoveSet(FGameplayResourceSet Other)
        {
            Flags &= ~Other.Flags;
            return this;
        }

        public void Clear()
        {
            Flags = 0;
        }

        public bool HasAllIDs(FGameplayResourceSet Other)
        {
            return (Flags & Other.Flags) == Other.Flags;
        }

        public bool HasAnyID(FGameplayResourceSet Other)
        {
            return (Flags & Other.Flags) != 0;
        }

        public FGameplayResourceSet GetOverlap(FGameplayResourceSet Other)
        {
            return new FGameplayResourceSet(Flags & Other.Flags);
        }

        public FGameplayResourceSet GetDifference(FGameplayResourceSet Other)
        {
            return new FGameplayResourceSet(Flags & ~(Flags & Other.Flags));
        }

        public static bool operator ==(FGameplayResourceSet Me, FGameplayResourceSet Other)
        {
            return Me.Flags == Other.Flags;
        }

        public static bool operator !=(FGameplayResourceSet Me, FGameplayResourceSet Other)
        {
            return Me.Flags != Other.Flags;
        }

        public static FGameplayResourceSet AllResources()
        {
            return new FGameplayResourceSet(-1);
        }

        public static FGameplayResourceSet NoResources()
        {
            return new FGameplayResourceSet(0);
        }

        public string GetDebugDescription()
        {
            return "";
        }
    }
}