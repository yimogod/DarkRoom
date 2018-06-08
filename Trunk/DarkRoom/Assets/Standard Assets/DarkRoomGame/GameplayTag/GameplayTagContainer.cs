using System.Collections.Generic;
using System.Linq;
using DarkRoom.Core;

namespace DarkRoom.Game
{
    public enum EGameplayContainerMatchType
    {
        Any,	//	Means the filter is populated by any tag matches in this container.
        All     //	Means the filter is only populated if all of the tags in this container match.
    }

    /// <summary>
    /// FGameplayTagContainer保存了GameplayTag列表
    /// 明确的保存了通过add方法添加的gameplaytag
    /// 且隐含的, 保存了gameplaytag的child tag
    /// </summary>
    public class FGameplayTagContainer
    {
        /** Array of gameplay tags */
        public List<FGameplayTag> GameplayTags = new List<FGameplayTag>();

        /** Array of expanded parent tags, in addition to GameplayTags. Used to accelerate parent searches. May contain duplicates in some cases */
        public List<FGameplayTag> ParentTags = new List<FGameplayTag>();

        public FGameplayTagContainer(){}

        public FGameplayTagContainer(List<FGameplayTag> InGameplayTags)
        {
            GameplayTags.AddRange(InGameplayTags);
        }

        /// <summary>
        /// 根据传入的tag列表, 创建container
        /// </summary>
        public static FGameplayTagContainer CreateFromArray(List<FGameplayTag> SourceTags)
        {
            FGameplayTagContainer container = new FGameplayTagContainer();
            container.GameplayTags.AddRange(SourceTags);
            container.FillParentTags();
            return container;
        }

        /// <summary>
        /// TagToCheck是否在本container中. 也会检查ParentTags
        /// {"A.1"}.HasTag("A") will return True, {"A"}.HasTag("A.1") will return False
        /// </summary>
        public bool HasTag(FGameplayTag TagToCheck)
        {
            if (!TagToCheck.IsValid())return false;
            
            // 自己或者父亲有就可以
            return GameplayTags.Contains(TagToCheck) || ParentTags.Contains(TagToCheck);
        }

        /**
         * 精确包含. 只判断GameplayTags列表
         *{"A.1"}.HasTagExact("A") will return False
         */
        public bool HasTagExact(FGameplayTag TagToCheck)
        {
            if (!TagToCheck.IsValid())return false;

            // Only check check explicit tag list
            return GameplayTags.Contains(TagToCheck);
        }

        /**
         * 本container是否包含ContainerToCheck的任意一个tag, 连ParentTags都会比较
         * {"A.1"}.HasAny({"A","B"}) will return True, {"A"}.HasAny({"A.1","B"}) will return False
         */
        public bool HasAny(FGameplayTagContainer ContainerToCheck)
        {
            if (ContainerToCheck.IsEmpty())return false;

            foreach (FGameplayTag otherTag in ContainerToCheck.GameplayTags)
            {
                if (GameplayTags.Contains(otherTag) || ParentTags.Contains(otherTag))
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * 本container是否  精确  包含ContainerToCheck的任意一个tag, 不会比较ParentTags
         * {"A.1"}.HasAny({"A","B"}) will return False
         */
        public bool HasAnyExact(FGameplayTagContainer ContainerToCheck)
        {
            if (ContainerToCheck.IsEmpty())return false;

            foreach (FGameplayTag otherTag in ContainerToCheck.GameplayTags)
            {
                if (GameplayTags.Contains(otherTag))return true;
            }

            return false;
        }

        /**
         * GameplayTags 和 ParentTags 是否包含ContainerToCheck
         * * {"A.1","B.1"}.HasAll({"A","B"}) will return True, {"A","B"}.HasAll({"A.1","B.1"}) will return False
         */
        public bool HasAll(FGameplayTagContainer ContainerToCheck)
        {
            //ContainerToCheck什么都没有,所以肯定返回true
            if (ContainerToCheck.IsEmpty())return true;

            foreach (FGameplayTag OtherTag in ContainerToCheck.GameplayTags)
            {
                if (!GameplayTags.Contains(OtherTag) && !ParentTags.Contains(OtherTag))
                {
                    return false;
                }
            }

            return true;
        }

        /**
         * GameplayTags 是否包含ContainerToCheck
         * {"A.1","B.1"}.HasAll({"A","B"}) will return False
         */
        public bool HasAllExact(FGameplayTagContainer ContainerToCheck)
        {
            if (ContainerToCheck.IsEmpty())return true;

            foreach (FGameplayTag OtherTag in ContainerToCheck.GameplayTags)
            {
                if (!GameplayTags.Contains(OtherTag))return false;
            }

            return true;
        }

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
            return GameplayTags.Count == 0;
        }

        /** Returns a new container explicitly containing the tags of this container and all of their parent tags
         * 返回一个新的container, 精确的包含了本container的所有GameplayTags和ParentTags
         */
        public FGameplayTagContainer GetGameplayTagParents()
        {
            FGameplayTagContainer container = new FGameplayTagContainer(GameplayTags);
            foreach (var item in ParentTags)
            {
                container.GameplayTags.AddUnique(item);
            }

            return container;
        }

        /**
         * 返回一个新的container, 这个container包含的gameplaytag满足是本tag和OtherContainer的交集
         */
        public FGameplayTagContainer Filter(FGameplayTagContainer OtherContainer)
        {
            FGameplayTagContainer container = new FGameplayTagContainer();

            foreach (FGameplayTag tag in GameplayTags)
            {
                if (tag.MatchesAny(OtherContainer))container.AddTagFast(tag);
            }

            return container;
        }

        /**
         *返回一个新的container, 这个container包含的gameplaytag满足是本tag和OtherContainer的精确交集
         */
        public FGameplayTagContainer FilterExact(FGameplayTagContainer OtherContainer)
        {
            FGameplayTagContainer container = new FGameplayTagContainer();

            foreach (FGameplayTag tag in GameplayTags)
            {
                if (tag.MatchesAnyExact(OtherContainer)) container.AddTagFast(tag);
            }

            return container;
        }

        /**
         * 本container是否满足参数query
         */
        public bool MatchesQuery(FGameplayTagQuery Query)
        {
            return Query.Matches(this);
        }

        /** 
         * Adds all the tags from one container to this container 
         * NOTE: From set theory, this effectively is the union of the container this is called on with Other.
         *
         * @param Other TagContainer that has the tags you want to add to this container
         *
         * 将other的gameplaytags添加到本gameplaytags列表
         * 将other的parent tags添加到本parenttags列表
         */
        public void AppendTags(FGameplayTagContainer Other)
        {
            foreach (FGameplayTag tag in Other.GameplayTags)
            {
                GameplayTags.AddUnique(tag);
            }

            foreach (FGameplayTag tag in Other.ParentTags)
            {
                ParentTags.AddUnique(tag);
            }
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
         *
         * 将OtherA 匹配 OtherB 的子集添加进来
         */
        public void AppendMatchingTags(FGameplayTagContainer OtherA, FGameplayTagContainer OtherB)
        {
            foreach (FGameplayTag otherATag in OtherA.GameplayTags)
            {
                if (otherATag.MatchesAny(OtherB))AddTag(otherATag);
            }
        }

        /**
         * 添加 TagToAdd 到 container 中
         * 1. 添加TagToAdd 到 GameplayTags列表中
         * 2. 添加TagToAdd所在的container的parent tags到 ParentTags列表中
         */
        public void AddTag(FGameplayTag TagToAdd)
        {
            if (!TagToAdd.IsValid())return;
            GameplayTags.AddUnique(TagToAdd);
            AddParentsForTag(TagToAdd);
        }

        /**
         * 不做唯一性判断, 直接添加tag
         */
        public void AddTagFast(FGameplayTag TagToAdd)
        {
            if (!TagToAdd.IsValid()) return;
            GameplayTags.Add(TagToAdd);
            AddParentsForTag(TagToAdd);
        }

        /**
         * 添加TagToAdd到container, 但会删除gameplaytags列表中TagToAdd相关的parent tag
         * 如果TagToAdd已经在gameplaytags列表中, 就不会被添加
         */
        public bool AddLeafTag(FGameplayTag TagToAdd)
        {
            // Check tag is not already explicitly in container
            if (HasTagExact(TagToAdd))return true;

            // If this tag is parent of explicitly added tag, fail
            if (HasTag(TagToAdd))return false;

            FGameplayTagContainer tagToAddContainer = UGameplayTagsManager.Instance.GetSingleTagContainer(TagToAdd);
            if (tagToAddContainer == null)return false;

            // Remove any tags in the container that are a parent to TagToAdd
            foreach (FGameplayTag ParentTag in tagToAddContainer.ParentTags)
            {
                if (HasTagExact(ParentTag))RemoveTag(ParentTag);
            }

            // Add the tag
            AddTag(TagToAdd);
            return true;
        }

        /**
         * Tag to remove from the container
         */
        public bool RemoveTag(FGameplayTag TagToRemove)
        {
            bool succ = GameplayTags.Remove(TagToRemove);
            if (!succ) return false;

            // Have to recompute parent table from scratch because there could be duplicates providing the same parent tag
            FillParentTags();
            return true;
        }

        /**
         * Removes all tags in TagsToRemove from this container
         *
         * @param TagsToRemove	Tags to remove from the container
         */
        public void RemoveTags(FGameplayTagContainer TagsToRemove)
        {
            bool removed = false;
            foreach (var item in TagsToRemove.GameplayTags)
            {
                bool r = GameplayTags.Remove(item);
                if (r) removed = true;
            }

            if (removed)FillParentTags();
        }

        public void Reset()
        {
            GameplayTags.Clear();
            ParentTags.Clear();
        }

        /** Sets from a ImportText string, used in asset registry */
        public void FromExportString(string ExportString)
        {
        }

        public string ToStringSimple()
        {
            string RetString = "";
            for (int i = 0; i < GameplayTags.Count; ++i)
            {
                RetString += "\"";
                RetString += GameplayTags[i].ToString();
                RetString += "\"";
                if (i < GameplayTags.Count - 1)
                {
                    RetString += ", ";
                }
            }
            return RetString;
        }

        /** Returns human readable description of what match is being looked foreachon the readable tag list. */
        public string ToMatchingText(EGameplayContainerMatchType MatchType, bool bInvertCondition)
        {
            return "";
        }

        /** Gets the explicit list of gameplay tags */
        public void GetGameplayTagArray(List<FGameplayTag> InOutGameplayTags)
        {
            InOutGameplayTags.Clear();
            InOutGameplayTags.AddRange(GameplayTags);
        }

        public bool IsValidIndex(int Index)
        {
            return GameplayTags.IsValidIndex(Index);
        }

        public FGameplayTag GetByIndex(int Index)
        {
            if (IsValidIndex(Index))return GameplayTags[Index];
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

        /**
         * 如果GameplayTags列表中有名字为TagName的tag, 就移除这个tag
         * 不要public这个方法. 内部使用
         */
        protected bool RemoveTagByExplicitName(string TagName)
        {
            foreach (var item in GameplayTags)
            {
                if (item.GetTagName() == TagName)
                {
                    RemoveTag(item);
                    return true;
                }
            }

            return false;
        }

        /** 添加InTag所在的container的parent tags 到 本ParentTags中*/
        protected void AddParentsForTag(FGameplayTag InTag)
        {
            FGameplayTagContainer inContainer = UGameplayTagsManager.Instance.GetSingleTagContainer(InTag);
            if (inContainer == null)return;

            // Add Parent tags from this tag to our own
            foreach (FGameplayTag ParentTag in inContainer.ParentTags)
            {
                ParentTags.AddUnique(ParentTag);
            }
        }

        /** 根据GameplayTags填充ParentTags  */
        protected void FillParentTags()
        {
            ParentTags.Clear();
            foreach (var item in GameplayTags)
            {
                AddParentsForTag(item);
            }
        }
    }
}