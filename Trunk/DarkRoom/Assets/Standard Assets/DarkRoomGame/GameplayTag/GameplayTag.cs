namespace DarkRoom.GamePlayAbility
{
    public enum EGameplayTagMatchType
    {
        Explicit,           // This will check for a match against just this tag
        IncludeParentTags,  // This will also check for matches against all parent tags
    }

    /** A single gameplay tag, which represents a hierarchical name of the form x.y that is registered in the GameplayTagsManager */
    public class FGameplayTag
    {
        /** An empty Gameplay Tag */
        public static FGameplayTag EmptyTag;

        /** This Tags Name */
        protected string TagName;

        /** Intentionally private so only the tag manager can use */
        protected FGameplayTag(string InTagName)
        {

        }


        /**
	     * Gets the FGameplayTag that corresponds to the TagName
	     *
	     * @param TagName The Name of the tag to search for
	     * 
	     * @param ErrorIfNotfound: ensure() that tag exists.
	     * 
	     * @return Will return the corresponding FGameplayTag or an empty one if not found.
	     */
        public static FGameplayTag RequestGameplayTag(string TagName, bool ErrorIfNotFound = true)
        {
            return null;
        }

	    /**
	     * Determine if this tag matches TagToCheck, expanding our parent tags
	     * "A.1".MatchesTag("A") will return True, "A".MatchesTag("A.1") will return False
	     * If TagToCheck is not Valid it will always return False
	     * 
	     * @return True if this tag matches TagToCheck
	     */
        public bool MatchesTag(FGameplayTag TagToCheck)
        {
            return false;
        }

        /**
         * Determine if TagToCheck is valid and exactly matches this tag
         * "A.1".MatchesTagExact("A") will return False
         * If TagToCheck is not Valid it will always return False
         * 
         * @return True if TagToCheck is Valid and is exactly this tag
         */
        public bool MatchesTagExact(FGameplayTag TagToCheck)
	    {
		    if (!TagToCheck.IsValid())
		    {
			    return false;
		    }
		    // Only check check explicit tag list
		    return TagName == TagToCheck.TagName;
	    }

	    /**
	     * Check to see how closely two FGameplayTags match. Higher values indicate more matching terms in the tags.
	     *
	     * @param TagToCheck	Tag to match against
	     *
	     * @return The depth of the match, higher means they are closer to an exact match
	     */
	    public int MatchesTagDepth(FGameplayTag TagToCheck)
	    {
	        return 0;
	    }

        /**
         * Checks if this tag matches ANY of the tags in the specified container, also checks against our parent tags
         * "A.1".MatchesAny({"A","B"}) will return True, "A".MatchesAny({"A.1","B"}) will return False
         * If ContainerToCheck is empty/invalid it will always return False
         *
         * @return True if this tag matches ANY of the tags of in ContainerToCheck
         */
        public bool MatchesAny(FGameplayTagContainer ContainerToCheck)
        {
            return true;
        }

        /**
         * Checks if this tag matches ANY of the tags in the specified container, only allowing exact matches
         * "A.1".MatchesAny({"A","B"}) will return False
         * If ContainerToCheck is empty/invalid it will always return False
         *
         * @return True if this tag matches ANY of the tags of in ContainerToCheck exactly
         */
        public bool MatchesAnyExact(FGameplayTagContainer ContainerToCheck)
        {
            return false;
        }

        /** Returns whether the tag is valid or not; Invalid tags are set to NAME_None and do not exist in the game-specific global dictionary */
        public bool IsValid()
	    {
		    return string.IsNullOrEmpty(TagName);
	    }

        /** Returns reference to a GameplayTagContainer containing only this tag */
        public FGameplayTagContainer GetSingleTagContainer()
        {
            return null;
        }

        /** Returns direct parent GameplayTag of this GameplayTag, calling on x.y will return x */
        public FGameplayTag RequestDirectParent()
        {
            return null;
        }

        /** Returns a new container explicitly containing the tags of this tag */
        public FGameplayTagContainer GetGameplayTagParents()
        {
            return null;
        }

        /** Used so we can have a TMap of this struct */
        public int GetTypeHash(FGameplayTag Tag)
        {
            return 1;
        }

        /** Displays gameplay tag as a string for blueprint graph usage */
        public string ToString()
	    {
		    return TagName.ToString();
	    }

        /** Get the tag represented as a name */
        public string GetTagName()
	    {
		    return TagName;
	    }

        /** Sets from a ImportText string, used in asset registry */
        void FromExportString(string ExportString)
        {

        }
    }
}