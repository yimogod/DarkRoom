using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 技能的定义
	/// 
	/// 有个问题, 记录一下
	/// 如果target无敌, 我们在启动ability时, 可以通过目标进行测试判断.
	/// 但如果传入的是坐标, 怎么办? 这个时候需要在Effect有同样的数据进行测试判断了.
	/// 这样的话Ability和Effect的数据就有冗余. 我又不愿意在Effect上记录Ability的实例, 解耦
	/// 暂时这样决定. Effect没有Tag, 只要调用就执行. 
	/// Effect是否能起作用, 在Ability, Buff, SpawnActor上做判断
	/// </summary>
	public class CAbilityMeta : CBaseMeta
	{
		/// <summary>
		/// 技能类型的枚举定义
		/// </summary>
		public enum AbilityType
		{
			Attack, //攻击
			EffectTarget, //需要选定目标的effect
            EffectPosition, //对于目标点或者方向实施的技能
		}

		/// <summary>
		/// 技能施放7阶段
		/// 
		/// TODO 这每个阶段的含义我需要明确知道
		/// </summary>
		public enum AbilityStage {
			Approach, //接近
			Prep, //准备
			Cast, //施放
			Channel, //引导
			Finish, //结束
			Bail, //保释
			Wait, //等待
		}

		/// <summary>
		/// 技能类型
		/// </summary>
		public AbilityType Type = AbilityType.Attack;

		/// <summary>
		/// 技能使用的间隔, cd时间, 基于秒
		/// </summary>
		public float Period = 0.5f;

		/// <summary>
		/// 技能消耗的资源. index是CostType, 值是数量
		/// </summary>
		public List<Vector2Int> CostList = new List<Vector2Int>();

	    /// <summary>
	    /// 技能作用目标的团队关系
	    /// </summary>
	    public AbilityTargetTeam TargetTeamRequire = AbilityTargetTeam.All;

		/// <summary>
		/// 技能配合的动作clip
		/// </summary>
		public string Clip;

		/// <summary>
		/// 技能编辑器的表演的路径
		/// </summary>
		public string AbilityShowConfPath;

		/// <summary>
		/// 整个技能持续的时间, 项目需要这个字段
		/// 
		/// 回头应该用Montage来做技能完成
		/// </summary>
		public float ShowTime = 2f;

        /// <summary>
        /// 技能感兴趣的位置.
        /// 比如冲锋是方向
        /// 射箭是目标, 项目需要这个字段
        /// TODO 目前不知道啥用
        /// </summary>
        //public CAbilityEnum.Location TargetLocation;


        //--------------------------------- Tag Related -------------------------

        /// <summary>
        /// 本技能自身的tag
        /// </summary>
        public CGameplayTagContainer AbilityTags;

        /// <summary>
        /// 如果本技能成功启动, 则owner有下列tag的技能需要取消
        /// </summary>
	    public CGameplayTagContainer CancelAbilityWithTags;


        /// <summary>
        /// 本技能运行过程中, 给owner身上添加的tag
        /// 目前没想到实际的技能实例
        /// </summary>
	    public CGameplayTagContainer AbilityTagsApplyToOwner;

        /// <summary>
        /// 本技能active需要owner身上有下列tag
        /// 得满足所有的tag
        /// </summary>
	    public CGameplayTagContainer AbilityTagsRequiredToActive;

	    /// <summary>
	    /// owner有下列tag的任意一个, 本技能则不能运行
	    /// 可以实现技能Set的功能
	    /// </summary>
	    public CGameplayTagContainer AbilityTagsToBlockActive;

	    /// <summary>
	    /// 本技能active需要target身上有下列tag, 比如目标身上有油, 你的点火技能才能用
	    /// 得满足所有的tag
	    /// </summary>
	    public CGameplayTagContainer TargetAbilityTagsRequiredToActive;

	    /// <summary>
	    /// target有下列tag的任意一个, 本技能则不能运行, 比如目标的无敌
	    /// </summary>
	    public CGameplayTagContainer TargetAbilityTagsToBlockActive;
        //--------------------------------- Tag Related End-------------------------

        //TODO 此处添加Trigger, 用于被动触发. 要是有事件被派发
        //TODO 就执行本技能, 问题: 我们是否建立被动技能这个类型?
	    public List<CAbilityTriggerData> AbilityTriggers;

        public CAbilityMeta(string idKey) : base(idKey) {}
	}


	public class CAbilityMetaManager
	{
		private static readonly Dictionary<string, CAbilityMeta> m_dict = new Dictionary<string, CAbilityMeta>();

		public static void AddMeta(CAbilityMeta meta)
		{
			if(m_dict.ContainsKey(meta.Id)) {
				Debug.LogError(string.Format("CAbilityMetaManager ALREADY CONTAIN the ability with id -- {0} ", meta.Id));
			}

			m_dict[meta.Id] = meta;
		}

		public static CAbilityMeta GetMeta(string id)
		{
			CAbilityMeta meta = null;
			bool v = m_dict.TryGetValue(id, out meta);
			if(!v) Debug.LogError(string.Format("CAbilityMetaManager DO NOT CONTAIN the ability with id -- {0} ", id));
            return meta;
		}
	}


	public class CAbilityParser : CMetaParser
	{
		public const string CAbilType_Attack = "CAbilAttack";
		public const string CAbilType_Target = "CAbilEffectTarget";
	    public const string CAbilType_Position = "CAbilEffectPosition";

        public CAbilityParser() : base() {
		}

		public CAbilityParser(bool useXml) : base(useXml) {
		}

		public override void Execute(string content)
		{
			base.Execute(content);
			m_xreader.ReadRootNode();

			foreach (XmlElement node in m_xreader.rootChildNodes) {
				switch (node.LocalName) {
					case CAbilType_Attack:
						Parse_Attack(node);
						break;
					case CAbilType_Target:
						Parse_Target(node);
						break;
                    case CAbilType_Position:
                        Parse_Position(node);
                        break;
                }
			}
		}

		/// <summary>
		/// 解析目标类技能
		/// </summary>
		private void Parse_Target(XmlElement root) {
			var meta = new CAbilityEffectTargetMeta("null");
			Parse_Base(root, meta);

			m_xreader.TryReadChildNodeAttr(root, "Effect", "value", ref meta.Effect);
			m_xreader.TryReadChildNodeAttr(root, "Range", "value", ref meta.Range);
			m_xreader.TryReadChildNodeAttr(root, "ShowTime", "value", ref meta.ShowTime);
			m_xreader.TryReadChildNodeAttr(root, "Period", "value", ref meta.Period);
			string str = String.Empty;
			m_xreader.TryReadChildNodeAttr(root, "TargetLocation", "value", ref str);
			//meta.TargetLocation = CAbilityUtil.GetLocation(str);
		}

		/// <summary>
		/// 解析普攻的技能
		/// </summary>
		/// <param name="root"></param>
		private void Parse_Attack(XmlElement root) {
			var meta = new CAbilityAttackMeta("null");
			Parse_Base(root, meta);

			m_xreader.TryReadChildNodeAttr(root, "MinAttackSpeedMultiplier", "value", ref meta.MinAttackSpeedMultiplier);
			m_xreader.TryReadChildNodeAttr(root, "MinAttackSpeedMultiplier", "value", ref meta.MinAttackSpeedMultiplier);
		}

	    private void Parse_Position(XmlElement root)
	    {

	    }

	    private void Parse_Base(XmlElement root, CAbilityMeta meta) {
			meta.Id = root.GetAttribute("id");
			m_xreader.TryReadChildNodeAttr(root, "Clip", "value", ref meta.Clip);

			CAbilityMetaManager.AddMeta(meta);
		}
	}
}