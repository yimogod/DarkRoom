﻿namespace DarkRoom.Game
{
    public enum CGameplayTagMatchType
    {
        Explicit, // 仅仅判断指定的tag
        IncludeParentTags, // 连父亲的tag也需要判断
    }

    /**
     * 游戏标签, 在 GameplayTagsManager 注册过. 形式是x.y.z
     */
    public class CGameplayTag
    {
        /**
         * tag的完整名称. a.b.c
         */
        protected string TagName;

        /**
         * 根据传入的TagName创建FGameplayTag
         * 会使用TagName去GameplayTagManager里面搜索看是否存在此tag
         * 如果ErrorIfNotFound为true, 且搜索未发现TagName, 则返回null
         */
        public static CGameplayTag RequestGameplayTag(string TagName, bool ErrorIfNotFound = true)
        {
            return CGameplayTagsManager.Instance.RequestGameplayTag(TagName, ErrorIfNotFound);
        }

        /** 注意, 本类只能在GameplayTagsManager里面实例化 */
        public CGameplayTag(string InTagName)
        {
            TagName = InTagName;
        }

        /**
         * 相当于reset tag, 将tagname置空
         */
        public void EmptyTag()
        {
            TagName = string.Empty;
        }

        /**
         * 本tag是否和TagToCheck匹配
         * TagToCheck如果不合法, 则返回false
         * 另外, A.b 匹配 A, 但A不匹配A.b
         */
        public bool MatchesTag(CGameplayTag TagToCheck)
        {
            var container = CGameplayTagsManager.Instance.GetSingleTagContainer(this);
            if (container == null) return false;

            return container.HasTag(TagToCheck);
        }

        /**
         * 精确匹配, A.b 只能匹配 A.b
         */
        public bool MatchesTagExact(CGameplayTag TagToCheck)
        {
            if (!TagToCheck.IsValid())return false;
            return string.Equals(TagName, TagToCheck.TagName);
        }

        /**
         * 检测 两个tag有多相近, 返回值越小说明越相近
         * 比如A.b.c 和A.b.d 就比 A.b.c和A.c更相近
         */
        public int MatchesTagDepth(CGameplayTag TagToCheck)
        {
            return CGameplayTagsManager.Instance.GameplayTagsMatchDepth(this, TagToCheck);
        }

        /**
         * 本tag所在的container是否和ContainerToCheck里面的任何一个tag匹配. 匹配规则如下
         * "A.1".MatchesAny({"A","B"}) will return True, "A".MatchesAny({"A.1","B"}) will return False
         */
        public bool MatchesAny(CGameplayTagContainer ContainerToCheck)
        {
            var container = CGameplayTagsManager.Instance.GetSingleTagContainer(this);
            if (container == null) return false;

            return container.HasAny(ContainerToCheck);
        }

        /**
         * 精确匹配, 本实例是否在ContainerToCheck中
         */
        public bool MatchesAnyExact(CGameplayTagContainer ContainerToCheck)
        {
            if (ContainerToCheck.IsEmpty()) return false;

            return ContainerToCheck.GameplayTags.Contains(this);
        }

        /**
         * 本tag是否合法. 理论上如果不存在在gametagmanamger中也是不合法的
         */
        public bool IsValid()
        {
            return string.IsNullOrEmpty(TagName);
        }

        /** Returns reference to a GameplayTagContainer containing only this tag */
        public CGameplayTagContainer GetSingleTagContainer()
        {
            var container = CGameplayTagsManager.Instance.GetSingleTagContainer(this);
            return container;
        }

        /**
         * 返回直系的父亲
         * calling on x.y will return x
         */
        public CGameplayTag RequestDirectParent()
        {
            return CGameplayTagsManager.Instance.RequestGameplayTagDirectParent(this);
        }

        /** 返回包含本tag的所有父亲tag的container */
        public CGameplayTagContainer GetGameplayTagParents()
        {
            return CGameplayTagsManager.Instance.RequestGameplayTagParents(this);
        }

        /** Used so we can have a TMap of this struct
         * 我可以用静态id来实现
         */
        public int GetTypeHash(CGameplayTag Tag)
        {
            return 1;
        }

        /** Get the tag represented as a name */
        public string GetTagName()
        {
            return TagName;
        }
    }
}