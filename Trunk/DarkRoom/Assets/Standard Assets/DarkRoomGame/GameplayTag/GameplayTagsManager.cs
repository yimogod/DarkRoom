using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.Game
{
    public class CGameplayTagMeta : CBaseMeta
    {
        private List<string> m_subTags = null;

        public CGameplayTagMeta(string id) : base(id)
        {
        }

        public string TagName{ get { return NameKey; } }

        public List<string> SubTags
        {
            get
            {
                if (m_subTags == null)
                {
                    var arr = TagName.Split('.');
                    m_subTags = new List<string>(arr);
                }

                return m_subTags;
            }
        }
    }

    public class UGameplayTagsManager : CSingleton<UGameplayTagsManager>
    {
        public Action OnLastChanceToAddNativeTags;

        public Dictionary<FGameplayTag, int> ReplicationCountMap;
        public Dictionary<FGameplayTag, int> ReplicationCountMap_SingleTags;
        public Dictionary<FGameplayTag, int> ReplicationCountMap_Containers;

        /** Cached number of bits we need to replicate tags. That is, Log2(Number of Tags). Will always be <= 16. */
        //public int NetIndexTrueBitNum;

        /** The length in bits of the first segment when net serializing tags. We will serialize NetIndexFirstBitSegment + 1 bit to indicatore "more" (more = second segment that is NetIndexTrueBitNum - NetIndexFirstBitSegment) */
        //public int NetIndexFirstBitSegment;

        /** Numbers of bits to use for replicating container size. This can be set via config. */
        //public int NumBitsForContainerSize;

        /** This is the actual value for an invalid tag "None". This is computed at runtime as (Total number of tags) + 1 */
       // public int InvalidTagNetIndex;


        /** Roots of gameplay tag nodes */
        private FGameplayTagNode GameplayRootTag;

        /**
         * key 是tag name, 内部使用
         */
        private Dictionary<string, FGameplayTagNode> GameplayTagNodeMap = new Dictionary<string, FGameplayTagNode>();

        /** Our aggregated, sorted list of commonly replicated tags. These tags are given lower indices to ensure they replicate in the first bit segment. */
        //private List<FGameplayTag> CommonlyReplicatedTags = new List<FGameplayTag>();

        /** List of native tags to add when reconstructing tree
         *  本地tag, 重建tag树的时候需要
         */
        //private HashSet<string> NativeTagsToAdd = new HashSet<string>();

        /** Cached runtime value for whether we are using fast replication or not. Initialized from config setting. */
        //private bool bUseFastReplication;

        /** Cached runtime value for whether we should warn when loading invalid tags */
        private bool bShouldWarnOnInvalidTags;

        /** True if native tags have all been added and flushed */
        //private bool bDoneAddingNativeTags;

        /** Sorted list of nodes, used for network replication */
        //private List<FGameplayTagNode> NetworkGameplayTagNodeIndex;

        /**
         * 所有可用tag的元数据
         */
        private List<CGameplayTagMeta> GameplayTagTables = new List<CGameplayTagMeta>();

        /** The map of ini-configured tag redirectors */
        //private Dictionary<string, FGameplayTag> TagRedirects;

        /** 我们请求创建的不存在的tagname列表 */
        private HashSet<string> MissingTagName;

        /**
         * 根据传入的TagName创建FGameplayTag
         * 会使用TagName去GameplayTagManager里面搜索看是否存在此tag
         * 如果ErrorIfNotFound为true, 且搜索未发现TagName, 则返回null
         */
        public FGameplayTag RequestGameplayTag(string TagName, bool ErrorIfNotFound = true)
        {
            if (GameplayTagNodeMap.ContainsKey(TagName))return null;

            if (ErrorIfNotFound && !MissingTagName.Contains(TagName))
            {
                MissingTagName.Add(TagName);
                Debug.LogWarning(string.Format("Requested Tag {0} was not found. Check tag data table.", TagName));
            }
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

        /** 返回包含本tag的所有父亲tag的container */
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

        /**
         * 返回直系的父亲
         * calling on x.y will return x
         */
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
            FGameplayTagNode Node = GameplayTagNodeMap[GameplayTag.GetTagName()];
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

        /** 从配置文件读取数据 */
        public void LoadGameplayTagTables()
        {
            GameplayTagTables.Clear();
        }

        /** 构建整个 tag 树*/
        public void ConstructGameplayTagTree()
        {
            if (!GameplayRootTag.IsValid())return;

            FGameplayTagNode currNode = GameplayRootTag;
            foreach (var item in GameplayTagTables)
            {
                var subTags = item.SubTags;
                for (int i = 0; i < subTags.Count; i++)
                {
                    List<FGameplayTagNode> childTags = currNode.GetChildTagNodes();
                    int insertIndex = InsertTagIntoNodeArray(subTags[i], currNode, childTags);
                    currNode = childTags[insertIndex];
                }
            }

        }

        /** 销毁整个 tag 树 */
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
        * 检测 两个tag有多相近, 返回值越好说明越相近
        * 比如A.b.c 和A.b.d 就比 A.b.c和A.c更相近
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
        //public bool ShouldUseFastReplication()
        //{
        //    return bUseFastReplication;
        //}

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

       // public List<FGameplayTagNode> GetNetworkGameplayTagNodeIndex()
        //{
        //    return NetworkGameplayTagNodeIndex;
        //}

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
         * @return Index of the node of the tag
         */
        private int InsertTagIntoNodeArray(string Tag, FGameplayTagNode ParentNode, List<FGameplayTagNode> NodeArray)
        {
            int InsertionIdx = -1;
            int WhereToInsert = -1;

            // See if the tag is already in the array
            for (int CurIdx = 0; CurIdx < NodeArray.Count; ++CurIdx)
            {
                var node = NodeArray[CurIdx];
                if (!node.IsValid()) continue;

                if (node.GetSimpleTagName() == Tag)
                {
                    InsertionIdx = CurIdx;
                    break;
                }

                if (WhereToInsert == -1)
                {
                    int v = string.CompareOrdinal(node.GetSimpleTagName(), Tag);
                    // Insert new node before this
                    if (v > 0) WhereToInsert = CurIdx;
                }
            }

            if (InsertionIdx != -1) return InsertionIdx;

            // Insert at end
            if (WhereToInsert == -1)WhereToInsert = NodeArray.Count;

            // Don't add the root node as parent
            FGameplayTagNode TagNode = new FGameplayTagNode(Tag, ParentNode != GameplayRootTag ? ParentNode : null);

            // Add at the sorted location
            NodeArray.Insert(WhereToInsert, TagNode);
            InsertionIdx = WhereToInsert;

            FGameplayTag GameplayTag = TagNode.GetCompleteTag();
            GameplayTagNodeMap.Add(GameplayTag.GetTagName(), TagNode);
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