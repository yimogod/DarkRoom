using System;
using System.Collections.Generic;
using DarkRoom.Core;
using UnityEngine;

namespace DarkRoom.Game
{
    public enum EGameplayTagQueryExprType
    {
        Undefined = 0,

        // start Tags
        AnyTagsMatch,
        AllTagsMatch,
        NoTagsMatch,

        // start expression
        AnyExprMatch,
        AllExprMatch,
        NoExprMatch,
    }

    public class FGameplayTagQueryExpression
    {
        /** 表达式类型 */
        public EGameplayTagQueryExprType ExprType;

        /** 表达式列表, 形成树状结构 */
        public List<FGameplayTagQueryExpression> ExprSet;

        /** 表达式涉及到的tag */
        public List<FGameplayTag> TagSet;

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

        /** 如果ExprType是tag相关返回true */
        public bool UsesTagSet()
        {
            return ExprType == EGameplayTagQueryExprType.AllTagsMatch ||
                   ExprType == EGameplayTagQueryExprType.AnyTagsMatch ||
                   ExprType == EGameplayTagQueryExprType.NoTagsMatch;
        }

        /** 如果ExprType是表达式相关返回true */
        public bool UsesExprSet()
        {
            return ExprType == EGameplayTagQueryExprType.AllExprMatch ||
                   ExprType == EGameplayTagQueryExprType.AnyExprMatch ||
                   ExprType == EGameplayTagQueryExprType.NoExprMatch;
        }

        /**
         * 将本表达式信息和传入的TagDictionary信息写入TokenStream里面
         */
        public void EmitTokens(List<byte> TokenStream, List<FGameplayTag> TagDictionary)
        {
            // emit exprtype
            TokenStream.Add((byte) ExprType);

            // emit exprdata
            switch (ExprType)
            {
                //tags
                case EGameplayTagQueryExprType.AnyTagsMatch:
                case EGameplayTagQueryExprType.AllTagsMatch:
                case EGameplayTagQueryExprType.NoTagsMatch:
                    // emit tagset
                    byte NumTags = (byte) TagSet.Count;
                    TokenStream.Add(NumTags);
                    foreach (var Tag in TagSet)
                    {
                        int TagIdx = TagDictionary.AddUnique(Tag);
                        TokenStream.Add((byte) TagIdx);
                    }

                    break;

                //expression
                case EGameplayTagQueryExprType.AnyExprMatch:
                case EGameplayTagQueryExprType.AllExprMatch:
                case EGameplayTagQueryExprType.NoExprMatch:
                    // emit tagset
                    byte NumExprs = (byte) ExprSet.Count;
                    TokenStream.Add(NumExprs);
                    foreach (var E in ExprSet)
                    {
                        E.EmitTokens(TokenStream, TagDictionary);
                    }

                    break;
            }
        }
    }


    /**
     * An FGameplayTagQuery is a logical query that can be run against an FGameplayTagContainer.  A query that succeeds is said to "match".
     * Queries are logical expressions that can test the intersection properties of another tag container (all, any, or none), or the matching state of a set of sub-expressions
     * (all, any, or none). This allows queries to be arbitrarily recursive and very expressive.  For instance, if you wanted to test if a given tag container contained tags 
     * ((A && B) || (C)) && (!D), you would construct your query in the form ALL( ANY( ALL(A,B), ALL(C) ), NONE(D) )
     * 
     * 
     * Example of how to build a query via code:
     *	FGameplayTagQuery Q;
     *	Q.BuildQuery(
     *		FGameplayTagQueryExpression()
     * 		.AllTagsMatch()
     *		.AddTag(FGameplayTag::RequestGameplayTag(FName(TEXT("Animal.Mammal.Dog.Corgi"))))
     *		.AddTag(FGameplayTag::RequestGameplayTag(FName(TEXT("Plant.Tree.Spruce"))))
     *		);
     * 
     * Queries are internally represented as a byte stream that is memory-efficient and can be evaluated quickly at runtime.
     * Note: these have an extensive details and graph pin customization for editing, so there is no need to expose the internals to Blueprints.
     */
    /// 为技能设计的查询机制. 查询对象是FGameplayTagContainer. 可以通过tag 类型或者 expression 类型
    /// 组合出复杂的查询方法
    /// 使用方法如上
    public class FGameplayTagQuery
    {
        public static FGameplayTagQuery EmptyQuery;

        private int TokenStreamVersion;

        /** 本查询包含的 tag */
        private List<FGameplayTag> TagDictionary;

        /** 本条查询涉及的数据 */
        public List<byte> QueryTokenStream;

        /** User-provided string describing the query */
        private string UserDescription;

        /** Auto-generated string describing the query */
        private string AutoDescription;

        public FGameplayTagQuery()
        {
        }

        /**
        * 创建 有任何一个tag匹配 的查询
        */
        public static FGameplayTagQuery MakeQuery_MatchAnyTags(FGameplayTagContainer InTags)
        {
            var qe = new FGameplayTagQueryExpression();
            return BuildQuery(qe.AnyTagsMatch().AddTags(InTags));
        }

        /**
        * 创建 有完全匹配tag的查询
        */
        public static FGameplayTagQuery MakeQuery_MatchAllTags(FGameplayTagContainer InTags)
        {
            var qe = new FGameplayTagQueryExpression();
            return BuildQuery(qe.AllTagsMatch().AddTags(InTags));
        }

        /**
        * 创建 有完全没有tag匹配的查询
        */
        public static FGameplayTagQuery MakeQuery_MatchNoTags(FGameplayTagContainer InTags)
        {
            var qe = new FGameplayTagQueryExpression();
            return BuildQuery(qe.NoTagsMatch().AddTags(InTags));
        }

        /** 创建一个query */
        public static FGameplayTagQuery BuildQuery(FGameplayTagQueryExpression RootQueryExpr, string InDescription = "")
        {
            FGameplayTagQuery Q = new FGameplayTagQuery();
            Q.Build(RootQueryExpr, InDescription);
            return Q;
        }

        /**
         * 获取tag
         * TODO 注意对index做包含
         */
        public FGameplayTag GetTagFromIndex(int Index)
        {
            return TagDictionary[Index];
        }

        /**
         * 清理现有tag, 用传入的tag填充
         * Replaces existing tags with passed in tags. Does not modify the tag query expression logic.
         * Useful when you need to cache off and update often used query.
         */
        public void ReplaceTagsFast(FGameplayTagContainer Tags)
        {
            TagDictionary.Clear();
            TagDictionary.AddRange(Tags.GameplayTags);
        }

        /**
         * 清理现有tag, 用传入的tag填充
         * Replaces existing tags with passed in tag. Does not modify the tag query expression logic.
         * Useful when you need to cache off and update often used query.
         */
        public void ReplaceTagFast(FGameplayTag Tag)
        {
            TagDictionary.Clear();
            TagDictionary.Add(Tag);
        }

        /** Tags 是否有匹配的标签. */
        public bool Matches(FGameplayTagContainer Tags)
        {
            FQueryEvaluator eval = new FQueryEvaluator(this);
            return eval.Eval(Tags);
        }

        public bool IsEmpty()
        {
            return QueryTokenStream.Count == 0;
        }

        public void Clear()
        {
            QueryTokenStream.Clear();
            TagDictionary.Clear();
        }

        /** 创建查询byte数据, 根据传入的查询表达式. */
        public void Build(FGameplayTagQueryExpression RootQueryExpr, string InUserDescription)
        {
            UserDescription = InUserDescription;

            QueryTokenStream.Clear();
            TagDictionary.Clear();

            // emit the query
            QueryTokenStream.Add(1); // true to indicate is has a root expression
            RootQueryExpr.EmitTokens(QueryTokenStream, TagDictionary);
        }

        /** Builds a FGameplayTagQueryExpression from this query. */
        public void GetQueryExpr(FGameplayTagQueryExpression OutExpr)
        {
            //根据传入的steam token创建表达式树
            FQueryEvaluator QE = new FQueryEvaluator(this);
            QE.Read(OutExpr);
        }

        /** Returns description string. */
        public string GetDescription()
        {
            return string.IsNullOrEmpty(UserDescription) ? AutoDescription : UserDescription;
        }
    }

    /** 解析评估 query token streams 的实体类 */
    public class FQueryEvaluator
    {
        private FGameplayTagQuery Query;
        private int CurStreamIdx = 0;
        private int Version;
        private bool bReadError = false;

        public FQueryEvaluator(FGameplayTagQuery Q)
        {
            Query = Q;
        }

        /** 评估传入的tag是否满足query的查询条件 */
        public bool Eval(FGameplayTagContainer Tags)
        {
            CurStreamIdx = 0;
            if (bReadError) return false;

            Version = GetToken();
            bool bRet = false;

            bool bHasRootExpression = (GetToken() != 0);
            if (!bReadError && bHasRootExpression)
            {
                bRet = EvalExpr(Tags);
            }

            return bRet;
        }

        /** 解析 the token stream 数据 传入到 FGameplayTagQueryExpression. */
        public void Read(FGameplayTagQueryExpression E)
        {
            CurStreamIdx = 0;

            if (Query.IsEmpty()) return;

            // start parsing the set
            if (!bReadError)
            {
                bool bHasRootExpression = GetToken() != 0;
                if (!bReadError && bHasRootExpression)
                {
                    ReadExpr(E);
                }
            }
        }

        private bool EvalExpr(FGameplayTagContainer Tags, bool bSkip = false)
        {
            EGameplayTagQueryExprType ExprType = (EGameplayTagQueryExprType) GetToken();
            if (bReadError) return false;

            switch (ExprType)
            {
                case EGameplayTagQueryExprType.AnyTagsMatch:
                    return EvalAnyTagsMatch(Tags, bSkip);
                case EGameplayTagQueryExprType.AllTagsMatch:
                    return EvalAllTagsMatch(Tags, bSkip);
                case EGameplayTagQueryExprType.NoTagsMatch:
                    return EvalNoTagsMatch(Tags, bSkip);

                case EGameplayTagQueryExprType.AnyExprMatch:
                    return EvalAnyExprMatch(Tags, bSkip);
                case EGameplayTagQueryExprType.AllExprMatch:
                    return EvalAllExprMatch(Tags, bSkip);
                case EGameplayTagQueryExprType.NoExprMatch:
                    return EvalNoExprMatch(Tags, bSkip);
            }

            return false;
        }

        private void ReadExpr(FGameplayTagQueryExpression E)
        {
            E.ExprType = (EGameplayTagQueryExprType) GetToken();
            if (bReadError) return;

            if (E.UsesTagSet())
            {
                // parse tag set
                byte NumTags = GetToken();
                if (bReadError) return;

                for (byte Idx = 0; Idx < NumTags; ++Idx)
                {
                    byte TagIdx = GetToken();
                    if (bReadError) return;

                    FGameplayTag Tag = Query.GetTagFromIndex(TagIdx);
                    E.AddTag(Tag);
                }
            }
            else
            {
                // parse expr set
                byte NumExprs = GetToken();
                if (bReadError)
                {
                    return;
                }

                for (byte Idx = 0; Idx < NumExprs; ++Idx)
                {
                    FGameplayTagQueryExpression Exp = new FGameplayTagQueryExpression();
                    ReadExpr(Exp);
                    Exp.AddExpr(Exp);
                }
            }
        }

        private bool EvalAnyTagsMatch(FGameplayTagContainer Tags, bool bSkip)
        {
            // parse tagset
            byte NumTags = GetToken();
            if (bReadError) return false;

            // 交叉比较传入的tag和我自带query的tag
            if (bSkip)
            {
                GetTokenNext(NumTags);
                if (bReadError) return false;
            }
            else
            {
                for (byte Idx = 0; Idx < NumTags; ++Idx)
                {
                    byte TagIdx = GetToken();
                    if (bReadError) return false;

                    FGameplayTag Tag = Query.GetTagFromIndex(TagIdx);
                    bool bHasTag = Tags.HasTag(Tag);
                    if (bHasTag) return true;
                }
            }

            return false;
        }

        private bool EvalAllTagsMatch(FGameplayTagContainer Tags, bool bSkip)
        {
            // parse tagset
            byte NumTags = GetToken();
            if (bReadError) return false;

            if (bSkip)
            {
                GetTokenNext(NumTags);
                if (bReadError) return false;
            }
            else
            {
                //交叉比较, 有一个没有, 结果就是false
                for (byte Idx = 0; Idx < NumTags; ++Idx)
                {
                    byte TagIdx = GetToken();
                    if (bReadError) return false;

                    FGameplayTag Tag = Query.GetTagFromIndex(TagIdx);
                    bool bHasTag = Tags.HasTag(Tag);
                    if (bHasTag == false) return false;
                }
            }

            return true;
        }

        private bool EvalNoTagsMatch(FGameplayTagContainer Tags, bool bSkip)
        {
            // parse tagset
            byte NumTags = GetToken();
            if (bReadError) return false;

            if (bSkip)
            {
                GetTokenNext(NumTags);
                if (bReadError) return false;
            }
            else
            {
                //交叉比较, 如果有一个就结果失败
                for (byte Idx = 0; Idx < NumTags; ++Idx)
                {
                    byte TagIdx = GetToken();
                    if (bReadError) return false;

                    FGameplayTag Tag = Query.GetTagFromIndex(TagIdx);
                    bool bHasTag = Tags.HasTag(Tag);
                    if (bHasTag) return false;
                }
            }

            return true;
        }

        private bool EvalAnyExprMatch(FGameplayTagContainer Tags, bool bSkip)
        {
            bool bShortCircuit = bSkip;

            // assume false until proven otherwise
            bool Result = false;

            // parse exprset
            byte NumExprs = GetToken();
            if (bReadError) return false;

            for (byte Idx = 0; Idx < NumExprs; ++Idx)
            {
                bool bExprResult = EvalExpr(Tags, bShortCircuit);
                if (bShortCircuit == false)
                {
                    if (bExprResult == true)
                    {
                        // one match is sufficient for true result
                        Result = true;
                        bShortCircuit = true;
                    }
                }
            }

            return Result;
        }

        private bool EvalAllExprMatch(FGameplayTagContainer Tags, bool bSkip)
        {
            bool bShortCircuit = bSkip;

            // assume true until proven otherwise
            bool Result = true;

            // parse exprset
            byte NumExprs = GetToken();
            if (bReadError) return false;

            for (byte Idx = 0; Idx < NumExprs; ++Idx)
            {
                bool bExprResult = EvalExpr(Tags, bShortCircuit);
                if (bShortCircuit == false)
                {
                    if (bExprResult == false)
                    {
                        // one fail is sufficient for false result
                        Result = false;
                        bShortCircuit = true;
                    }
                }
            }

            return Result;
        }

        private bool EvalNoExprMatch(FGameplayTagContainer Tags, bool bSkip)
        {
            bool bShortCircuit = bSkip;

            // assume true until proven otherwise
            bool Result = true;

            // parse exprset
            byte NumExprs = GetToken();
            if (bReadError) return false;

            for (byte Idx = 0; Idx < NumExprs; ++Idx)
            {
                bool bExprResult = EvalExpr(Tags, bShortCircuit);
                if (bShortCircuit == false)
                {
                    if (bExprResult == true)
                    {
                        // one match is sufficient for fail result
                        Result = false;
                        bShortCircuit = true;
                    }
                }
            }

            return Result;
        }

        /** 获取下一个token,如果出错返回0 */
        public byte GetToken()
        {
            if (Query.QueryTokenStream.IsValidIndex(CurStreamIdx))
            {
                return Query.QueryTokenStream[CurStreamIdx++];
            }

            Debug.LogError("Error parsing FGameplayTagQuery!");
            bReadError = true;
            return 0;
        }

        public byte GetTokenNext(int offset)
        {
            CurStreamIdx += offset;
            if (Query.QueryTokenStream.IsValidIndex(CurStreamIdx))
            {
                return Query.QueryTokenStream[CurStreamIdx];
            }

            CurStreamIdx -= offset;
            Debug.LogError("Error parsing FGameplayTagQuery!");
            bReadError = true;
            return 0;
        }
    }
}