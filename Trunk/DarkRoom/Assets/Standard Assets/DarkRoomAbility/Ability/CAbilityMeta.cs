using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	/// <summary>
	/// 技能的定义
	/// </summary>
	public class CAbilityMeta : CBaseMeta
	{
		/// <summary>
		/// 技能消耗的资源种类
		/// 这里有个暂时的考虑. 理论上CD也是资源, 但我们现在假定所有的ability都需要cd
		/// </summary>
		public enum CostType
		{
			MP, //蓝
			HP, //红
			Vital, //活力
			UserDef_0,
			UserDef_1,
			UserDef_2,
			UserDef_3,
			Count,
		}

		/// <summary>
		/// 技能类型的枚举定义
		/// </summary>
		public enum AbilityType
		{
			Attack, //攻击
			EffectTarget, //需要选定目标的effect
			//EffectPassive //被动技能
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
		/// 技能所属于的集合. 一个单位只能有同一个集合内的一个技能
		/// </summary>
		public string SetId;

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
		public int[] CostList = new int[(int)CostType.Count];

		/// <summary>
		/// 技能作用目标的过滤器
		/// </summary>
		public CAbilityTargetFilter TargetFilter = new CAbilityTargetFilter();

		/// <summary>
		/// 技能配合的动作clip
		/// </summary>
		public string Clip;

		/// <summary>
		/// 技能编辑器的表演的路径
		/// </summary>
		public string AbilityShowConfPath;

		/// <summary>
		/// 是否表演
		/// </summary>
		public bool StopGame = false;

		/// <summary>
		/// 整个技能持续的时间, 项目需要这个字段
		/// </summary>
		public float ShowTime = 2f;

		/// <summary>
		/// 技能感兴趣的位置.
		/// 比如冲锋是方向
		/// 射箭是目标, 项目需要这个字段
		/// </summary>
		public CAbilityEnum.Location TargetLocation;

		public CAbilityMeta(string idKey) : base(idKey) {}
	}


	public class CAbilityMetaManager
	{
		private static readonly Dictionary<string, CAbilityMeta> m_dict = new Dictionary<string, CAbilityMeta>();

		public static void AddMeta(CAbilityMeta meta)
		{
			if(m_dict.ContainsKey(meta.IdKey)) {
				Debug.LogError(string.Format("CAbilityMetaManager ALREADY CONTAIN the ability with id -- {0} ", meta.IdKey));
			}

			m_dict[meta.IdKey] = meta;
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
				}
			}
		}

		/// <summary>
		/// 解析目标类技能
		/// </summary>
		private void Parse_Target(XmlElement root) {
			var meta = new CAbilEffectTargetMeta("null");
			Parse_Base(root, meta);

			m_xreader.TryReadChildNodeAttr(root, "Effect", "value", ref meta.Effect);
			m_xreader.TryReadChildNodeAttr(root, "Range", "value", ref meta.Range);
			m_xreader.TryReadChildNodeAttr(root, "ShowTime", "value", ref meta.ShowTime);
			m_xreader.TryReadChildNodeAttr(root, "Period", "value", ref meta.Period);
			string str = String.Empty;
			m_xreader.TryReadChildNodeAttr(root, "TargetLocation", "value", ref str);
			meta.TargetLocation = CAbilityUtil.GetLocation(str);
		}

		/// <summary>
		/// 解析普攻的技能
		/// </summary>
		/// <param name="root"></param>
		private void Parse_Attack(XmlElement root) {
			var meta = new CAbilAttackMeta("null");
			Parse_Base(root, meta);

			m_xreader.TryReadChildNodeAttr(root, "MinAttackSpeedMultiplier", "value", ref meta.MinAttackSpeedMultiplier);
			m_xreader.TryReadChildNodeAttr(root, "MinAttackSpeedMultiplier", "value", ref meta.MinAttackSpeedMultiplier);
		}



		private void Parse_Base(XmlElement root, CAbilityMeta meta) {
			meta.IdKey = root.GetAttribute("id");
			m_xreader.TryReadChildNodeAttr(root, "Clip", "value", ref meta.Clip);

			CAbilityMetaManager.AddMeta(meta);
		}
	}
}