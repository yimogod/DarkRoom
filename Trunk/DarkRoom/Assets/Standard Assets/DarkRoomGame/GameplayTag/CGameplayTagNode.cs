using System;
using System.Collections.Generic;

namespace DarkRoom.Game
{
    /**
     * 用于gameplaytag的简单树状节点. 保存特殊tag的元数据
     * 每个node都持有一个1对1的conatiner. 该container包含此node对应的完整tag
     */
    public class CGameplayTagNode
    {
        /**
         * 完整tag所在node的tag值
         * 比如a.b.c, 那么这个Tag就是c
         */
        private string Tag;

        /**
         * 辅助存储tag的变量.
         * GameplayTags列表的第一个tag就是完整的tag. a.b.c
         * ParentTags包含了此节点tag的所有的父亲tag
         */
        private CGameplayTagContainer CompleteTagWithParents = new CGameplayTagContainer();

        /** 一级儿子node列表 */
        private List<CGameplayTagNode> ChildTags;

        /** 父亲节点 */
        private CGameplayTagNode ParentNode;

        public CGameplayTagNode()
        {
        }

        public CGameplayTagNode(string InTag, CGameplayTagNode InParentNode)
        {
            Tag = InTag;
            ParentNode = InParentNode;

            List<CGameplayTag> ParentCompleteTags = new List<CGameplayTag>();
            CGameplayTagNode CurNode = InParentNode;
            // 只要有父亲node
            while (CurNode.IsValid())
            {
                ParentCompleteTags.Add(CurNode.GetCompleteTag());
                CurNode = CurNode.GetParentTagNode();
            }

            //完整的tag名称
            string CompleteTagString = InTag;
            if (ParentCompleteTags.Count > 0)
            {
                CompleteTagString = string.Format("{0}.{1}", ParentCompleteTags[0].GetTagName(), InTag);
            }

            CGameplayTag tag = new CGameplayTag(CompleteTagString);
            CompleteTagWithParents.GameplayTags.Add(tag);
            CompleteTagWithParents.ParentTags.AddRange(ParentCompleteTags);
        }

        /** Returns a correctly constructed container with only this tag, useful for doing container queries */
        public CGameplayTagContainer GetSingleTagContainer()
        {
            return CompleteTagWithParents;
        }

        /**
         * 获取完整tag, a.b.c
         */
        public CGameplayTag GetCompleteTag()
        {
            return CompleteTagWithParents.Num() > 0 ? CompleteTagWithParents.GameplayTags[0] : null;
        }

        public string GetCompleteTagName()
        {
            return GetCompleteTag().GetTagName();
        }

        /**
         * 获取简单的tag名称. 不包含父亲的tag
         * a or b or c, 而不是a.b.c
         */
        public string GetSimpleTagName()
        {
            return Tag;
        }

        /**
         * 获取一级儿子列表
         */
        public List<CGameplayTagNode> GetChildTagNodes()
        {
            return ChildTags;
        }

        /**
         * 父节点
         */
        public CGameplayTagNode GetParentTagNode()
        {
            return ParentNode;
        }

        public void ResetNode()
        {
            Tag = string.Empty;
            CompleteTagWithParents.Reset();

            for (int ChildIdx = 0; ChildIdx < ChildTags.Count; ++ChildIdx)
            {
                ChildTags[ChildIdx].ResetNode();
            }

            ChildTags.Clear();
        }


        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Tag);
        }
    }
}