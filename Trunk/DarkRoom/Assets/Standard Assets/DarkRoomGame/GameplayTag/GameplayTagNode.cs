using System;
using System.Collections.Generic;

namespace DarkRoom.Game
{
    /**
     * 用于gameplaytag的简单树状节点. 保存特殊tag的元数据
     * 每个node都持有一个1对1的conatiner. 该container包含此node对应的完整tag
     */
    public class FGameplayTagNode
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
        private FGameplayTagContainer CompleteTagWithParents = new FGameplayTagContainer();

        /** 一级儿子node列表 */
        private List<FGameplayTagNode> ChildTags;

        /** 父亲节点 */
        private FGameplayTagNode ParentNode;

        public FGameplayTagNode()
        {
        }

        public FGameplayTagNode(string InTag, FGameplayTagNode InParentNode)
        {
            Tag = InTag;
            ParentNode = InParentNode;

            List<FGameplayTag> ParentCompleteTags = new List<FGameplayTag>();
            FGameplayTagNode CurNode = InParentNode;
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

            FGameplayTag tag = new FGameplayTag(CompleteTagString);
            CompleteTagWithParents.GameplayTags.Add(tag);
            CompleteTagWithParents.ParentTags.AddRange(ParentCompleteTags);
        }

        /** Returns a correctly constructed container with only this tag, useful for doing container queries */
        public FGameplayTagContainer GetSingleTagContainer()
        {
            return CompleteTagWithParents;
        }

        /**
         * 获取完整tag, a.b.c
         */
        public FGameplayTag GetCompleteTag()
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
        public List<FGameplayTagNode> GetChildTagNodes()
        {
            return ChildTags;
        }

        /**
         * 父节点
         */
        public FGameplayTagNode GetParentTagNode()
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