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

    /// <summary>
    /// tag管理器, 单例.
    /// 保存着所有的tag元数据.
    /// 可以创建单个tag
    /// </summary>
    public class CGameplayTagsManager : CSingleton<CGameplayTagsManager>
    {
        /** Roots of gameplay tag nodes */
        private CGameplayTagNode GameplayRootTag;

        /**
         * key 是tag name, 内部使用
         */
        private Dictionary<string, CGameplayTagNode> GameplayTagNodeMap = new Dictionary<string, CGameplayTagNode>();

        /** Cached runtime value for whether we should warn when loading invalid tags */
        private bool bShouldWarnOnInvalidTags;

        /**
         * 所有可用tag的元数据, 就是所谓metamanamger
         */
        private List<CGameplayTagMeta> GameplayTagTables = new List<CGameplayTagMeta>();

        /** 我们请求创建的不存在的tagname列表 */
        private HashSet<string> MissingTagName = new HashSet<string>();

        /** Initializes the manager */
        private static void InitializeManager()
        {
            m_instance.LoadGameplayTagTables();
            m_instance.ConstructGameplayTagTree();
        }

        /**
         * 根据传入的TagName创建FGameplayTag
         * 会使用TagName去GameplayTagManager里面搜索看是否存在此tag
         * 如果ErrorIfNotFound为true, 且搜索未发现TagName, 则返回null
         */
        public CGameplayTag RequestGameplayTag(string TagName, bool ErrorIfNotFound = true)
        {
            if (GameplayTagNodeMap.ContainsKey(TagName))return new CGameplayTag(TagName);

            if (ErrorIfNotFound && !MissingTagName.Contains(TagName))
            {
                MissingTagName.Add(TagName);
                Debug.LogWarning(string.Format("Requested Tag {0} was not found. Check tag data table.", TagName));
            }
            return null;
        }

        /// <summary>
        /// 返回包含本tag以及所有父亲tag的container
        /// 比如传入的a.b.c 那么 此container会包含a, a.b, a.b.c
        /// </summary>
        public CGameplayTagContainer RequestGameplayTagParents(CGameplayTag GameplayTag)
        {
            CGameplayTagContainer container = GetSingleTagContainer(GameplayTag);
            if (container != null)return container.GetGameplayTagParents();
            return null;
        }

        /// <summary>
        /// 获取传入tag所有的子tag组成的container, 不包含传入tag
        /// 例如传入a.b, 则返回的conatiner包含a.b.c, a.b.d, a.b.c.e
        /// </summary>
        public CGameplayTagContainer RequestGameplayTagChildren(CGameplayTag GameplayTag)
        {
            CGameplayTagContainer TagContainer = new CGameplayTagContainer();

            CGameplayTagNode GameplayTagNode = FindTagNode(GameplayTag);
            if (GameplayTagNode.IsValid())
            {
                AddChildrenTags(TagContainer, GameplayTagNode, true, true);
            }
            return TagContainer;
        }

        /**
         * 返回直系的父亲
         * calling on x.y will return x
         */
        public CGameplayTag RequestGameplayTagDirectParent(CGameplayTag GameplayTag)
        {
            CGameplayTagNode GameplayTagNode = FindTagNode(GameplayTag);
            if (!GameplayTagNode.IsValid()) return null;

            var parent = GameplayTagNode.GetParentTagNode();
            if (parent.IsValid())return parent.GetCompleteTag();
            return null;
        }

        /// <summary>
        /// 在node树中查找传入tag对应的container, 此container仅包含传入的tag
        /// </summary>
        public CGameplayTagContainer GetSingleTagContainer(CGameplayTag GameplayTag)
        {
            CGameplayTagNode node = FindTagNode(GameplayTag);
            if (node.IsValid())
            {
                return node.GetSingleTagContainer();
            }

            return null;
        }

        /// <summary>
        /// 找到tag对应的node
        /// </summary>
        public CGameplayTagNode FindTagNode(CGameplayTag GameplayTag)
        {
            CGameplayTagNode Node = GameplayTagNodeMap[GameplayTag.GetTagName()];
            return Node;
        }

        /// <summary>
        /// 找到tag对应的node
        /// </summary>
        public CGameplayTagNode FindTagNode(string TagName)
        {
            CGameplayTag PossibleTag = new CGameplayTag(TagName);
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

            CGameplayTagNode currNode = GameplayRootTag;
            foreach (var item in GameplayTagTables)
            {
                var subTags = item.SubTags;
                for (int i = 0; i < subTags.Count; i++)
                {
                    List<CGameplayTagNode> childTags = currNode.GetChildTagNodes();
                    int insertIndex = InsertTagIntoNodeArray(subTags[i], currNode, childTags);
                    currNode = childTags[insertIndex];
                }
            }

        }

        /** 销毁整个 tag 树 */
        public void DestroyGameplayTagTree()
        {
            GameplayRootTag.ResetNode();
            GameplayTagNodeMap.Clear();
        }

        /// <summary>
        /// 将tag a.b.c 变成数组[a, b, c]
        /// </summary>
        public void SplitGameplayTagFName(CGameplayTag Tag, List<string> OutNames)
        {
            var list = Tag.GetTagName().Split('.');
            OutNames.AddRange(list);
        }

        /// <summary>
        /// 获取所有的gameplay tag
        /// 如果你只添加了a.b.c那么其实会有3个tag--a, a.b, a.b.c
        /// </summary>
        public void RequestAllGameplayTags(CGameplayTagContainer TagContainer, bool OnlyIncludeDictionaryTags)
        {
            foreach(KeyValuePair<string, CGameplayTagNode> item in GameplayTagNodeMap)
            {
                if (OnlyIncludeDictionaryTags)continue;

                var tag = item.Value.GetCompleteTag();
                TagContainer.AddTagFast(tag);
            }
        }

        /// <summary>
        /// 测试输入的tagname是否合法--即是否在预配置里面
        /// </summary>
        public bool ValidateTagCreation(string TagName)
        {
            return FindTagNode(TagName).IsValid();
        }

        /**
        * 检测 两个tag有多相近, 返回值越好说明越相近
        * 比如A.b.c 和A.b.d 就比 A.b.c和A.c更相近
        */
        public int GameplayTagsMatchDepth(CGameplayTag GameplayTagOne, CGameplayTag GameplayTagTwo)
        {
            return 1;
        }

        /** Should we print loading errors when trying to load invalid tags */
        public bool ShouldWarnOnInvalidTags()
        {
            return bShouldWarnOnInvalidTags;
        }

        /**
         * Helper function to insert a tag into a tag node array
         *
         * @param Tag					Tag to insert
         * @param ParentNode			Parent node, if any, for the tag
         * @param NodeArray				Node array to insert the new node into, if necessary (if the tag already exists, no insertion will occur)
         * @return Index of the node of the tag
         */
        private int InsertTagIntoNodeArray(string Tag, CGameplayTagNode ParentNode, List<CGameplayTagNode> NodeArray)
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
            CGameplayTagNode TagNode = new CGameplayTagNode(Tag, ParentNode != GameplayRootTag ? ParentNode : null);

            // Add at the sorted location
            NodeArray.Insert(WhereToInsert, TagNode);
            InsertionIdx = WhereToInsert;

            CGameplayTag GameplayTag = TagNode.GetCompleteTag();
            GameplayTagNodeMap.Add(GameplayTag.GetTagName(), TagNode);

            return 1;
        }

        /// <summary>
        /// 将GameplayTagNode的children node对应的tag添加到TagContainer中去
        /// RecurseAll表示在树状节点中, 就添加一层child还是循环一直往下找
        /// </summary>
        private void AddChildrenTags(CGameplayTagContainer TagContainer, CGameplayTagNode GameplayTagNode,
            bool RecurseAll = true, bool OnlyIncludeDictionaryTags = false)
        {
            if (!GameplayTagNode.IsValid())return;

            var ChildrenNodes = GameplayTagNode.GetChildTagNodes();
            foreach (CGameplayTagNode ChildNode in ChildrenNodes)
            {
                if (!ChildNode.IsValid())continue;

                TagContainer.AddTag(ChildNode.GetCompleteTag());
                if (RecurseAll)
                {
                    AddChildrenTags(TagContainer, ChildNode, true, OnlyIncludeDictionaryTags);
                }
            }
        }
    }
}