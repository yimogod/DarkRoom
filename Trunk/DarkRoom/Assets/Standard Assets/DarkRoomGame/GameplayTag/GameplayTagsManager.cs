using System;
using System.Collections.Generic;
using DarkRoom.Core;

namespace DarkRoom.Game
{
    public class GameplayTagsManager : CSingleton<GameplayTagsManager>
    {
        public Action OnLastChanceToAddNativeTags;

        public Dictionary<FGameplayTag, int> ReplicationCountMap;
        public Dictionary<FGameplayTag, int> ReplicationCountMap_SingleTags;
        public Dictionary<FGameplayTag, int> ReplicationCountMap_Containers;

        /** Cached number of bits we need to replicate tags. That is, Log2(Number of Tags). Will always be <= 16. */
        public int NetIndexTrueBitNum;

        /** The length in bits of the first segment when net serializing tags. We will serialize NetIndexFirstBitSegment + 1 bit to indicatore "more" (more = second segment that is NetIndexTrueBitNum - NetIndexFirstBitSegment) */
        public int NetIndexFirstBitSegment;

        /** Numbers of bits to use for replicating container size. This can be set via config. */
        public int NumBitsForContainerSize;

        /** This is the actual value for an invalid tag "None". This is computed at runtime as (Total number of tags) + 1 */
        public int InvalidTagNetIndex;


        /** Roots of gameplay tag nodes */
        private FGameplayTagNode GameplayRootTag;

        /** Map of Tags to Nodes - Internal use only. FGameplayTag is inside node structure, do not use FindKey! */
        private Dictionary<FGameplayTag, FGameplayTagNode> GameplayTagNodeMap;

        /** Our aggregated, sorted list of commonly replicated tags. These tags are given lower indices to ensure they replicate in the first bit segment. */
        private List<FGameplayTag> CommonlyReplicatedTags;

        /** List of native tags to add when reconstructing tree */
        private HashSet<string> NativeTagsToAdd;

        /** Cached runtime value for whether we are using fast replication or not. Initialized from config setting. */
        private bool bUseFastReplication;

        /** Cached runtime value for whether we should warn when loading invalid tags */
        private bool bShouldWarnOnInvalidTags;

        /** True if native tags have all been added and flushed */
        private bool bDoneAddingNativeTags;

        /** Sorted list of nodes, used for network replication */
        private List<FGameplayTagNode> NetworkGameplayTagNodeIndex;

        /** Holds all of the valid gameplay-related tags that can be applied to assets */
        private List<CBaseMeta> GameplayTagTables;

        /** The map of ini-configured tag redirectors */
        private Dictionary<string, FGameplayTag> TagRedirects;


        /**
             * Gets the FGameplayTag that corresponds to the TagName
             *
             * @param TagName The Name of the tag to search for
             * @param ErrorIfNotfound: ensure() that tag exists.
             * 
             * @return Will return the corresponding FGameplayTag or an empty one if not found.
             */
        public FGameplayTag RequestGameplayTag(string TagName, bool ErrorIfNotFound = true)
        {
            return null;
        }

        /**
         *	Searches for a gameplay tag given a partial string. This is slow and intended mainly for console commands/utilities to make
         *	developer life's easier. This will attempt to match as best as it can. If you pass "A.b" it will match on "A.b." before it matches "a.b.c".
         */
        public FGameplayTag FindGameplayTagFromPartialString_Slow(string PartialString)
        {
            return null;
        }

        /**
         * Registers the given name as a gameplay tag, and tracks that it is being directly referenced from code
         * This can only be called during engine initialization, the table needs to be locked down before replication
         *
         * @param TagName The Name of the tag to add
         * @param TagDevComment The developer comment clarifying the usage of the tag
         * 
         * @return Will return the corresponding FGameplayTag
         */
        public FGameplayTag AddNativeGameplayTag(string TagName, string TagDevComment = "(Native)")
        {
            return null;
        }

        /** Call to flush the list of native tags, once called it is unsafe to add more */
        public void DoneAddingNativeTags()
        {
        }

        public void CallOrRegister_OnDoneAddingNativeTagsDelegate(Action Delegate)
        {
        }

        /**
         * Gets a Tag Container containing the supplied tag and all of it's parents as explicit tags
         *
         * @param GameplayTag The Tag to use at the child most tag for this container
         * 
         * @return A Tag Container with the supplied tag and all its parents added explicitly
         */
        public FGameplayTagContainer RequestGameplayTagParents(FGameplayTag GameplayTag)
        {
            return null;
        }

        /**
         * Gets a Tag Container containing the all tags in the hierarchy that are children of this tag. Does not return the original tag
         *
         * @param GameplayTag					The Tag to use at the parent tag
         * 
         * @return A Tag Container with the supplied tag and all its parents added explicitly
         */
        public FGameplayTagContainer RequestGameplayTagChildren(FGameplayTag GameplayTag)
        {
            return null;
        }

        /** Returns direct parent GameplayTag of this GameplayTag, calling on x.y will return x */
        public FGameplayTag RequestGameplayTagDirectParent(FGameplayTag GameplayTag)
        {
            return null;
        }

        /**
         * Helper function to get the stored TagContainer containing only this tag, which has searchable ParentTags
         * @param GameplayTag		Tag to get single container of
         * @return					Pointer to container with this tag
         */
        public FGameplayTagContainer GetSingleTagContainer(FGameplayTag GameplayTag)
        {
            FGameplayTagNode TagNode = FindTagNode(GameplayTag);
            if (TagNode.IsValid())
            {
                return TagNode.GetSingleTagContainer();
            }

            return null;
        }

        /**
         * Checks node tree to see if a FGameplayTagNode with the tag exists
         *
         * @param TagName	The name of the tag node to search for
         *
         * @return A shared pointer to the FGameplayTagNode found, or NULL if not found.
         */
        public FGameplayTagNode FindTagNode(FGameplayTag GameplayTag)
        {
            FGameplayTagNode Node = GameplayTagNodeMap[GameplayTag];
            return Node;
        }

        /**
         * Checks node tree to see if a FGameplayTagNode with the name exists
         *
         * @param TagName	The name of the tag node to search for
         *
         * @return A shared pointer to the FGameplayTagNode found, or NULL if not found.
         */
        public FGameplayTagNode FindTagNode(string TagName)
        {
            FGameplayTag PossibleTag = new FGameplayTag(TagName);
            return FindTagNode(PossibleTag);
        }

        /** Loads the tag tables referenced in the GameplayTagSettings object */
        public void LoadGameplayTagTables()
        {
        }

        /** Helper function toruct the gameplay tag tree */
        public void constructGameplayTagTree()
        {
        }

        /** Helper function to destroy the gameplay tag tree */
        public void DestroyGameplayTagTree()
        {
        }

        /** Splits a tag such as x.y.z into an array of names {x,y,z} */
        public void SplitGameplayTagstring(FGameplayTag Tag, List<string> OutNames)
        {
        }

        /** Gets the list of all tags in the dictionary */
        public void RequestAllGameplayTags(FGameplayTagContainer TagContainer, bool OnlyIncludeDictionaryTags)
        {
        }

        /** Returns true if if the passed in name is in the tag dictionary and can be created */
        public bool ValidateTagCreation(string TagName)
        {
            return false;
        }

/**
 * Check to see how closely two FGameplayTags match. Higher values indicate more matching terms in the tags.
 *
 * @param GameplayTagOne	The first tag to compare
 * @param GameplayTagTwo	The second tag to compare
 *
 * @return the length of the longest matching pair
 */
        public int GameplayTagsMatchDepth(FGameplayTag GameplayTagOne, FGameplayTag GameplayTagTwo)
        {
            return 1;
        }

        /** Returns true if we should import tags from UGameplayTagsSettings objects (configured by INI files) */
        public bool ShouldImportTagsFromINI()
        {
            return false;
        }

        /** Should we print loading errors when trying to load invalid tags */
        public bool ShouldWarnOnInvalidTags()
        {
            return bShouldWarnOnInvalidTags;
        }

        /** Should use fast replication */
        public bool ShouldUseFastReplication()
        {
            return bUseFastReplication;
        }

        /** Handles redirectors for an entire container, will also error on invalid tags */
        public void RedirectTagsForContainer(FGameplayTagContainer Container)
        {
        }

        /** Handles redirectors for a single tag, will also error on invalid tag. This is only called for when individual tags are serialized on their own */
        public void RedirectSingleGameplayTag(FGameplayTag Tag)
        {
        }

        /** Handles establishing a single tag from an imported tag name (accounts for redirects too). Called when tags are imported via text. */
        public bool ImportSingleGameplayTag(FGameplayTag Tag, string ImportedTagName)
        {
            return false;
        }

        /** Gets a tag name from net index and vice versa, used for replication efficiency */
        public string GetTagNameFromNetIndex(int Index)
        {
            return "";
        }

        public int GetNetIndexFromTag(FGameplayTag InTag)
        {
            return 1;
        }

        public List<FGameplayTagNode> GetNetworkGameplayTagNodeIndex()
        {
            return NetworkGameplayTagNodeIndex;
        }

        public bool IsNativelyAddedTag(FGameplayTag Tag)
        {
            return false;
        }


        public void PrintReplicationIndices()
        {
        }

        /** Mechanism for tracking what tags are frequently replicated */
        public void PrintReplicationFrequencyReport()
        {
        }

        public void NotifyTagReplicated(FGameplayTag Tag, bool WasInContainer)
        {
        }


        /** Initializes the manager */
        private static void InitializeManager()
        {
        }

        /** finished loading/adding native tags */
        private Action OnDoneAddingNativeTagsDelegate;

        /**
         * Helper function to insert a tag into a tag node array
         *
         * @param Tag					Tag to insert
         * @param ParentNode			Parent node, if any, for the tag
         * @param NodeArray				Node array to insert the new node into, if necessary (if the tag already exists, no insertion will occur)
         * @param SourceName			File tag was added from
         * @param DevComment			Comment from developer about this tag
         *
         * @return Index of the node of the tag
         */
        private int InsertTagIntoNodeArray(string Tag, FGameplayTagNode ParentNode, List<FGameplayTagNode> NodeArray,
            string SourceName, string DevComment)
        {
            return 1;
        }

        /** Helper function to populate the tag tree from each table */
        private void PopulateTreeFromDataTable(CBaseMeta meta)
        {
        }

        private void AddTagTableRow(CBaseMeta TagRow, string SourceName)
        {
        }

        private void AddChildrenTags(FGameplayTagContainer TagContainer, FGameplayTagNode GameplayTagNode,
            bool RecurseAll = true, bool OnlyIncludeDictionaryTags = false)
        {
        }

        /**
         * Helper function for GameplayTagsMatch to get all parents when doing a parent match,
         * NOTE: Must never be made public as it uses the strings which should never be exposed
         * 
         * @param NameList		The list we are adding all parent complete names too
         * @param GameplayTag	The current Tag we are adding to the list
         */
        private void GetAllParentNodeNames(HashSet<string> NamesList, FGameplayTagNode GameplayTag)
        {
        }

        /**ructs the net indices for each tag */
        private void ConstructNetIndex()
        {
        }
    }
}