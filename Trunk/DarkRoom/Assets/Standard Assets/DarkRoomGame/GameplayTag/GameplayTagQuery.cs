using System;
using System.Collections.Generic;

namespace DarkRoom.GamePlayAbility
{
    public enum EGameplayTagQueryExprType
    {
        Undefined = 0,
        AnyTagsMatch,
        AllTagsMatch,
        NoTagsMatch,
        AnyExprMatch,
        AllExprMatch,
        NoExprMatch,
    }

    public class FGameplayTagQueryExpression
    {
        /** Which type of expression this is. */
        EGameplayTagQueryExprType ExprType;

        /** Expression list, for expression types that need it */
        List<FGameplayTagQueryExpression> ExprSet;

        /** Tag list, for expression types that need it */
        List<FGameplayTag> TagSet;

        /** 
         * Fluid syntax approach for setting the type of this expression. 
         */

        public FGameplayTagQueryExpression AnyTagsMatch()
        {
            ExprType = EGameplayTagQueryExprType.AnyTagsMatch;
            return this;
        }

        public FGameplayTagQueryExpression AllTagsMatch()
        {
            ExprType = EGameplayTagQueryExprType.AllTagsMatch;
            return this;
        }

        public FGameplayTagQueryExpression NoTagsMatch()
        {
            ExprType = EGameplayTagQueryExprType.NoTagsMatch;
            return this;
        }

        public FGameplayTagQueryExpression AnyExprMatch()
        {
            ExprType = EGameplayTagQueryExprType.AnyExprMatch;
            return this;
        }

        public FGameplayTagQueryExpression AllExprMatch()
        {
            ExprType = EGameplayTagQueryExprType.AllExprMatch;
            return this;
        }

        public FGameplayTagQueryExpression NoExprMatch()
        {
            ExprType = EGameplayTagQueryExprType.NoExprMatch;
            return this;
        }

        public FGameplayTagQueryExpression AddTag(string TagName)
        {
            return this;
        }

        public FGameplayTagQueryExpression AddTag(FGameplayTag Tag)
        {
            TagSet.Add(Tag);
            return this;
        }

        public FGameplayTagQueryExpression AddTags(FGameplayTagContainer Tags)
        {
            TagSet.AddRange(Tags.GameplayTags);
            return this;
        }

        public FGameplayTagQueryExpression AddExpr(FGameplayTagQueryExpression Expr)
        {
            ExprSet.Add(Expr);
            return this;
        }

        /** Writes this expression to the given token stream. */
        public void EmitTokens(List<int> TokenStream, List<FGameplayTag> TagDictionary)
        {
        }


        /** Returns true if this expression uses the tag data. */
        public bool UsesTagSet()
        {
            return (ExprType == EGameplayTagQueryExprType.AllTagsMatch) ||
                   (ExprType == EGameplayTagQueryExprType.AnyTagsMatch) ||
                   (ExprType == EGameplayTagQueryExprType.NoTagsMatch);
        }

/** Returns true if this expression uses the expression list data. */
        public bool UsesExprSet()
        {
            return (ExprType == EGameplayTagQueryExprType.AllExprMatch) ||
                   (ExprType == EGameplayTagQueryExprType.AnyExprMatch) ||
                   (ExprType == EGameplayTagQueryExprType.NoExprMatch);
        }
    }

    public class FGameplayTagQuery
    {
        public static FGameplayTagQuery EmptyQuery;

        /** Creates a tag query that will match if there are any common tags between the given tags and the tags being queries against. */
        public static FGameplayTagQuery MakeQuery_MatchAnyTags(FGameplayTagContainer InTags)
        {
            return null;
        }

        public static FGameplayTagQuery MakeQuery_MatchAllTags(FGameplayTagContainer InTags)
        {
            return null;
        }

        public static FGameplayTagQuery MakeQuery_MatchNoTags(FGameplayTagContainer InTags)
        {
            return null;
        }

        public FGameplayTagQuery()
        {
        }


        private int TokenStreamVersion;

        /** List of tags referenced by this entire query. Token stream stored indices into this list. */
        private List<FGameplayTag> TagDictionary;

        /** Stream representation of the actual hierarchical query */
        private List<int> QueryTokenStream;

        /** User-provided string describing the query */
        private string UserDescription;

        /** Auto-generated string describing the query */
        private string AutoDescription;

        /** Returns a gameplay tag from the tag dictionary */
        private FGameplayTag GetTagFromIndex(int TagIdx)
        {
            return TagDictionary[TagIdx];
        }


        /** Replaces existing tags with passed in tags. Does not modify the tag query expression logic. Useful when you need to cache off and update often used query. Must use same sized tag container! */
        public void ReplaceTagsFast(FGameplayTagContainer Tags)
        {
            TagDictionary.Clear();
            TagDictionary.AddRange(Tags.GameplayTags);
        }

        /** Replaces existing tags with passed in tag. Does not modify the tag query expression logic. Useful when you need to cache off and update often used query. */
        public void ReplaceTagFast(FGameplayTag Tag)
        {
            TagDictionary.Clear();
            TagDictionary.Add(Tag);
        }

        /** Returns true if the given tags match this query, or false otherwise. */
        public bool Matches(FGameplayTagContainer Tags)
        {
            return false;
        }

        /** Returns true if this query is empty, false otherwise. */
        public bool IsEmpty()
        {
            return false;
        }

        /** Resets this query to its default empty state. */
        public void Clear()
        {
        }

        /** Creates this query with the given root expression. */
        public void Build(FGameplayTagQueryExpression RootQueryExpr, string InUserDescription)
        {
        }

        /** Static function to assemble and return a query. */
        public static FGameplayTagQuery BuildQuery(FGameplayTagQueryExpression RootQueryExpr, string InDescription)
        {
            return null;
        }

        /** Builds a FGameplayTagQueryExpression from this query. */
        public void GetQueryExpr(FGameplayTagQueryExpression OutExpr)
        {
        }

        /** Returns description string. */
        public string GetDescription()
        {
            return string.IsNullOrEmpty(UserDescription) ? AutoDescription : UserDescription;
        }
    }
}