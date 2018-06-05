using System.Collections.Generic;
using System.Linq;

namespace DarkRoom.Game
{
    public enum EGameplayContainerMatchType
    {
        Any,	//	Means the filter is populated by any tag matches in this container.
        All     //	Means the filter is only populated if all of the tags in this container match.
    }

    public class FGameplayTagContainer
    {
        /** An empty Gameplay Tag Container */
        static FGameplayTagContainer EmptyContainer;

        /** Array of gameplay tags */
        public List<FGameplayTag> GameplayTags = new List<FGameplayTag>();

        /** Array of expanded parent tags, in addition to GameplayTags. Used to accelerate parent searches. May contain duplicates in some cases */
        public List<FGameplayTag> ParentTags = new List<FGameplayTag>();

        public FGameplayTagContainer()
        {
        }

        /** Creates a container from an array of tags, this is more efficient than adding them all individually */
        public static FGameplayTagContainer CreateFromArray(List<FGameplayTag> SourceTags)
        {
            FGameplayTagContainer Container = new FGameplayTagContainer();
            Container.GameplayTags.AddRange(SourceTags);
            Container.FillParentTags();
            return Container;
        }

        /**
         * Determine if TagToCheck is present in this container, also checking against parent tags
         * {"A.1"}.HasTag("A") will return True, {"A"}.HasTag("A.1") will return False
         * If TagToCheck is not Valid it will always return False
         * 
         * @return True if TagToCheck is in this container, false if it is not
         */
        public bool HasTag(FGameplayTag TagToCheck)
        {
            if (!TagToCheck.IsValid())
            {
                return false;
            }

            // Check explicit and parent tag list 
            return GameplayTags.Contains(TagToCheck) || ParentTags.Contains(TagToCheck);
        }

        /**
         * Determine if TagToCheck is explicitly present in this container, only allowing exact matches
         * {"A.1"}.HasTagExact("A") will return False
         * If TagToCheck is not Valid it will always return False
         * 
         * @return True if TagToCheck is in this container, false if it is not
         */
        public bool HasTagExact(FGameplayTag TagToCheck)
        {
            if (!TagToCheck.IsValid())
            {
                return false;
            }

            // Only check check explicit tag list
            return GameplayTags.Contains(TagToCheck);
        }

        /**
         * Checks if this container contains ANY of the tags in the specified container, also checks against parent tags
         * {"A.1"}.HasAny({"A","B"}) will return True, {"A"}.HasAny({"A.1","B"}) will return False
         * If ContainerToCheck is empty/invalid it will always return False
         *
         * @return True if this container has ANY of the tags of in ContainerToCheck
         */
        public bool HasAny(FGameplayTagContainer ContainerToCheck)
        {
            if (ContainerToCheck.IsEmpty())
            {
                return false;
            }

            foreach (FGameplayTag OtherTag in ContainerToCheck.GameplayTags)
            {
                if (GameplayTags.Contains(OtherTag) || ParentTags.Contains(OtherTag))
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * Checks if this container contains ANY of the tags in the specified container, only allowing exact matches
         * {"A.1"}.HasAny({"A","B"}) will return False
         * If ContainerToCheck is empty/invalid it will always return False
         *
         * @return True if this container has ANY of the tags of in ContainerToCheck
         */
        public bool HasAnyExact(FGameplayTagContainer ContainerToCheck)
        {
            if (ContainerToCheck.IsEmpty())
            {
                return false;
            }

            foreach (FGameplayTag OtherTag in ContainerToCheck.GameplayTags)
            {
                if (GameplayTags.Contains(OtherTag))
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * Checks if this container contains ALL of the tags in the specified container, also checks against parent tags
         * {"A.1","B.1"}.HasAll({"A","B"}) will return True, {"A","B"}.HasAll({"A.1","B.1"}) will return False
         * If ContainerToCheck is empty/invalid it will always return True, because there were no failed checks
         *
         * @return True if this container has ALL of the tags of in ContainerToCheck, including if ContainerToCheck is empty
         */
        public bool HasAll(FGameplayTagContainer ContainerToCheck)
        {
            if (ContainerToCheck.IsEmpty())
            {
                return true;
            }

            foreach (FGameplayTag OtherTag in ContainerToCheck.GameplayTags)
            {
                if (!GameplayTags.Contains(OtherTag) & !ParentTags.Contains(OtherTag))
                {
                    return false;
                }
            }

            return true;
        }

        /**
         * Checks if this container contains ALL of the tags in the specified container, only allowing exact matches
         * {"A.1","B.1"}.HasAll({"A","B"}) will return False
         * If ContainerToCheck is empty/invalid it will always return True, because there were no failed checks
         *
         * @return True if this container has ALL of the tags of in ContainerToCheck, including if ContainerToCheck is empty
         */
        public bool HasAllExact(FGameplayTagContainer ContainerToCheck)
        {
            if (ContainerToCheck.IsEmpty())
            {
                return true;
            }

            foreach (FGameplayTag OtherTag in ContainerToCheck.GameplayTags)
            {
                if (!GameplayTags.Contains(OtherTag))
                {
                    return false;
                }
            }

            return true;
        }

        /** Returns the number of explicitly added tags */
        public int Num()
        {
            return GameplayTags.Count;
        }

        /** Returns whether the container has any valid tags */
        public bool IsValid()
        {
            return GameplayTags.Count > 0;
        }

        /** Returns true if container is empty */
        public bool IsEmpty()
        {
            return GameplayTags.Count() == 0;
        }

        /** Returns a new container explicitly containing the tags of this container and all of their parent tags */
        public FGameplayTagContainer GetGameplayTagParents()
        {
            return null;
        }

        /**
         * Returns a filtered version of this container, returns all tags that match against any of the tags in OtherContainer, expanding parents
         *
         * @param OtherContainer		The Container to filter against
         *
         * @return A FGameplayTagContainer containing the filtered tags
         */
        public FGameplayTagContainer Filter(FGameplayTagContainer OtherContainer)
        {
            return null;
        }

        /**
         * Returns a filtered version of this container, returns all tags that match exactly one in OtherContainer
         *
         * @param OtherContainer		The Container to filter against
         *
         * @return A FGameplayTagContainer containing the filtered tags
         */
        public FGameplayTagContainer FilterExact(FGameplayTagContainer OtherContainer)
        {
            return null;
        }

        /** 
         * Checks if this container matches the given query.
         *
         * @param Query		Query we are checking against
         *
         * @return True if this container matches the query, false otherwise.
         */
        public bool MatchesQuery(FGameplayTagQuery Query)
        {
            return false;
        }

        /** 
         * Adds all the tags from one container to this container 
         * NOTE: From set theory, this effectively is the union of the container this is called on with Other.
         *
         * @param Other TagContainer that has the tags you want to add to this container 
         */
        public void AppendTags(FGameplayTagContainer Other)
        {
        }

        /** 
         * Adds all the tags that match between the two specified containers to this container.  WARNING: This matches any
         * parent tag in A, not just exact matches!  So while this should be the union of the container this is called on with
         * the intersection of OtherA and OtherB, it's not exactly that.  Since OtherB matches against its parents, any tag
         * in OtherA which has a parent match with a parent of OtherB will count.  foreachexample, if OtherA has Color.Green
         * and OtherB has Color.Red, that will count as a match due to the Color parent match!
         * If you want an exact match, you need to call A.FilterExact(B) (above) to get the intersection of A with B.
         * If you need the disjunctive union (the union of two sets minus their intersection), use AppendTags to create
         * Union, FilterExact to create Intersection, and then call Union.RemoveTags(Intersection).
         *
         * @param OtherA TagContainer that has the matching tags you want to add to this container, these tags have their parents expanded
         * @param OtherB TagContainer used to check foreachmatching tags.  If the tag matches on any parent, it counts as a match.
         */
        public void AppendMatchingTags(FGameplayTagContainer OtherA, FGameplayTagContainer OtherB)
        {
        }

        /**
         * Add the specified tag to the container
         *
         * @param TagToAdd Tag to add to the container
         */
        public void AddTag(FGameplayTag TagToAdd)
        {
        }

        /**
         * Add the specified tag to the container without checking foreachuniqueness
         *
         * @param TagToAdd Tag to add to the container
         * 
         * Useful when building container from another data struct (TMap foreachexample)
         */
        public void AddTagFast(FGameplayTag TagToAdd)
        {
        }

        /**
         * Adds a tag to the container and removes any direct parents, wont add if child already exists
         *
         * @param Tag			The tag to try and add to this container
         * 
         * @return True if tag was added
         */
        public bool AddLeafTag(FGameplayTag TagToAdd)
        {
            return false;
        }

        /**
         * Tag to remove from the container
         * 
         * @param TagToRemove	Tag to remove from the container
         */
        public bool RemoveTag(FGameplayTag TagToRemove)
        {
            return false;
        }

        /**
         * Removes all tags in TagsToRemove from this container
         *
         * @param TagsToRemove	Tags to remove from the container
         */
        public void RemoveTags(FGameplayTagContainer TagsToRemove)
        {
        }

        /** Remove all tags from the container. Will maintain slack by default */
        public void Reset(int Slack = 0)
        {
        }


        /** Returns string version of container in ImportText format */
        public string ToString()
        {
            return "";
        }

        /** Sets from a ImportText string, used in asset registry */
        public void FromExportString(string ExportString)
        {
        }

        /** Returns abbreviated human readable Tag list without parens or property names. If bQuoted is true it will quote each tag */
        public string ToStringSimple(bool bQuoted = false)
        {
            return "";
        }

        /** Returns human readable description of what match is being looked foreachon the readable tag list. */
        public string ToMatchingText(EGameplayContainerMatchType MatchType, bool bInvertCondition)
        {
            return "";
        }

        /** Gets the explicit list of gameplay tags */
        public void GetGameplayTagArray(List<FGameplayTag> InOutGameplayTags)
        {
            InOutGameplayTags = GameplayTags;
        }

        public bool IsValidIndex(int Index)
        {
            if (Index < 0) return false;
            if (Index >= GameplayTags.Count) return false;
            return true;
        }

        public FGameplayTag GetByIndex(int Index)
        {
            if (IsValidIndex(Index))
            {
                return GameplayTags[Index];
            }

            return null;
        }

        public FGameplayTag First()
        {
            return GameplayTags.Count > 0 ? GameplayTags[0] : null;
        }

        public FGameplayTag Last()
        {
            return GameplayTags.Count > 0 ? GameplayTags.Last() : null;
        }


        /** Version of above that is called from conditions where you know tag is valid */
        public bool HasTagFast(FGameplayTag TagToCheck, EGameplayTagMatchType TagMatchType,
            EGameplayTagMatchType TagToCheckMatchType)
        {
            bool bResult;
            if (TagToCheckMatchType == EGameplayTagMatchType.Explicit)
            {
                // Always check explicit
                bResult = GameplayTags.Contains(TagToCheck);
                if (!bResult & TagMatchType == EGameplayTagMatchType.IncludeParentTags)
                {
                    // Check parent tags as well
                    bResult = ParentTags.Contains(TagToCheck);
                }
            }
            else
            {
                bResult = ComplexHasTag(TagToCheck, TagMatchType, TagToCheckMatchType);
            }

            return bResult;
        }

        /**
         * Determine if the container has the specified tag
         * 
         * @param TagToCheck			Tag to check if it is present in the container
         * @param TagMatchType			Type of match to use foreachthe tags in this container
         * @param TagToCheckMatchType	Type of match to use foreachthe TagToCheck Param
         * 
         * @return True if the tag is in the container, false if it is not
         */
        public bool ComplexHasTag(FGameplayTag TagToCheck, EGameplayTagMatchType TagMatchType,
            EGameplayTagMatchType TagToCheckMatchType)
        {
            return false;
        }

        /**
         * Checks if this container matches ANY of the tags in the specified container. Performs matching by expanding this container out
         * to include its parent tags.
         *
         * @param Other					Container we are checking against
         * @param bCountEmptyAsMatch	If true, the parameter tag container will count as matching even if it's empty
         *
         * @return True if this container has ANY the tags of the passed in container


        /**
         * Returns true if the tags in this container match the tags in OtherContainer foreachthe specified matching types.
         *
         * @param OtherContainer		The Container to filter against
         * @param TagMatchType			Type of match to use foreachthe tags in this container
         * @param OtherTagMatchType		Type of match to use foreachthe tags in the OtherContainer param
         * @param ContainerMatchType	Type of match to use foreachfiltering
         *
         * @return Returns true if ContainerMatchType is Any and any of the tags in OtherContainer match the tags in this or ContainerMatchType is All and all of the tags in OtherContainer match at least one tag in this. Returns false otherwise.
         */
        public bool DoesTagContainerMatch(FGameplayTagContainer OtherContainer,
            EGameplayTagMatchType TagMatchType,
            EGameplayTagMatchType OtherTagMatchType, EGameplayContainerMatchType ContainerMatchType)
        {
            bool bResult;
            if (OtherTagMatchType == EGameplayTagMatchType.Explicit)
            {
                // Start true foreachall, start false foreachany
                bResult = (ContainerMatchType == EGameplayContainerMatchType.All);
                foreach (FGameplayTag OtherTag in OtherContainer.GameplayTags)
                {
                    if (HasTagFast(OtherTag, TagMatchType, OtherTagMatchType))
                    {
                        if (ContainerMatchType == EGameplayContainerMatchType.Any)
                        {
                            bResult = true;
                            break;
                        }
                    }
                    else if (ContainerMatchType == EGameplayContainerMatchType.All)
                    {
                        bResult = false;
                        break;
                    }
                }
            }
            else
            {
                FGameplayTagContainer OtherExpanded = OtherContainer.GetGameplayTagParents();
                return DoesTagContainerMatch(OtherExpanded, TagMatchType, EGameplayTagMatchType.Explicit,
                    ContainerMatchType);
            }

            return bResult;
        }

        /**
         * Returns true if the tags in this container match the tags in OtherContainer foreachthe specified matching types.
         *
         * @param OtherContainer		The Container to filter against
         * @param TagMatchType			Type of match to use foreachthe tags in this container
         * @param OtherTagMatchType		Type of match to use foreachthe tags in the OtherContainer param
         * @param ContainerMatchType	Type of match to use foreachfiltering
         *
         * @return Returns true if ContainerMatchType is Any and any of the tags in OtherContainer match the tags in this or ContainerMatchType is All and all of the tags in OtherContainer match at least one tag in this. Returns false otherwise.
         */
        protected bool DoesTagContainerMatchComplex(FGameplayTagContainer OtherContainer,
            EGameplayTagMatchType TagMatchType,
            EGameplayTagMatchType OtherTagMatchType, EGameplayContainerMatchType ContainerMatchType)
        {
            return false;
        }

        /**
         * If a Tag with the specified tag name explicitly exists, it will remove that tag and return true.  Otherwise, it 
           returns false.  It does NOT check the TagName foreachvalidity (i.e. the tag could be obsolete and so not exist in
           the table). It also does NOT check parents (because it cannot do so foreacha tag that isn't in the table).
           NOTE: This function should ONLY ever be used by GameplayTagsManager when redirecting tags.  Do NOT make this
           function public!
         */
        protected bool RemoveTagByExplicitName(string TagName)
        {
            return false;
        }

        /** Adds parent tags foreacha single tag */
        protected void AddParentsForTag(FGameplayTag Tag)
        {
        }

        /** Fills in ParentTags from GameplayTags */
        protected void FillParentTags()
        {
        }
    }
}