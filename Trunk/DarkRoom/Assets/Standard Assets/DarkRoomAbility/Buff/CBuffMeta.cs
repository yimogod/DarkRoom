using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using DarkRoom.Core;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {

    /// <summary>
    /// 有几个注意点
    /// 1. 我们的buff没有成功几率一说, 是否成功我们移到了Effect中去
    /// </summary>
	public class CBuffMeta : CBaseMeta
	{
		/// <summary>
		/// 行为的类型
		/// </summary>
		public enum BuffType
        {
		    Dot, 
            Status
		}

	    public BuffType Type;

	    /// <summary>
	    /// Buff的目标身上有这些Tag, Buff才会生效
	    /// </summary>
	    public List<CGameplayTagRequirement> TargetRequirements;

        /// <summary>
        /// buff时长
        /// 小于0表明永远有效果, 除非角色死亡
        /// 等于0表明立刻销毁
        /// </summary>
        public float Duration = 0f;

        /// <summary>
        /// 是否永远有效, 比如学习了被动效果
        /// 但能被外部移除, 比如卸载技能
        /// 当然驱散也不能移除永久性的buff
        /// </summary>
        public bool Infinite
        {
            get { return Duration < 0; }
        }

	    /// <summary>
	    /// 该buff可以在同一个单位身上存在几个实例
	    /// 比如英雄联盟的血瓶, 可以吃多个, 加快回血速度
	    /// 也比如第4次技能会有晕眩效果
	    /// </summary>
	    public int MaxStackCount;


	    /// <summary>
	    /// buff初始化时的效果
	    /// </summary>
	    public string InitialEffect;

	    /// <summary>
	    /// buff完成时的效果
	    /// </summary>
	    public string FinalEffect;

	    /// <summary>
	    /// buff失效时产生的效果
	    /// </summary>
	    public string ExpireEffect;

        /// <summary>
        /// 本buff对属性的修改列表
        /// </summary>
	    public List<CBuffModifierInfo> Modifiers;

        /// <summary>
        /// buff修改的单位属性
        /// </summary>
        public CAbilityEnum.BuffModification ModifyProperty =
	        new CAbilityEnum.BuffModification();

        public CBuffMeta(string idKey) : base(idKey) { }
	}

	public class CBuffMetaManager {
		private static Dictionary<string, CBuffMeta> m_dict = 
			new Dictionary<string, CBuffMeta>();

		public static void AddMeta(CBuffMeta meta)
		{
			if(m_dict.ContainsKey(meta.Id)) {
				Debug.LogError("CBehaviorMetaManager ALREADY CONTAIN the behavior with id -- " +  meta.Id);
			}

			m_dict[meta.Id] = meta;
		}

		public static CBuffMeta GetMeta(string id)
		{
			CBuffMeta meta = null;
			bool v = m_dict.TryGetValue(id, out meta);
			if(!v)Debug.LogError("CBehaviorMetaManager DO NOT CONTAIN the behavior with id -- " + id);

			return meta;
		}
	}

	#region parser
	public class CBuffMetaParser : CMetaParser {
        /// <summary>
        /// 持续修改Attribute的一个值
        /// 比如增加100的力量或者进入晕眩的状态
        /// </summary>
		public const string BuffType_Status = "BuffStatus";

        /// <summary>
        /// 以period的间断来修改某个Attribute
        /// 一般比如中毒或者喝血
        /// </summary>
	    public const string BuffType_Dot = "BuffDot";

        public CBuffMetaParser() : base() { }
		public CBuffMetaParser(bool useXml) : base(useXml) { }

		public override void Execute(string content) {
			base.Execute(content);

			m_xreader.ReadRootNode();
			foreach (XmlElement node in m_xreader.rootChildNodes) {
				switch (node.LocalName) {
					case BuffType_Status:
						Parse_Status(node);
						break;
				}
			}
		}


		//解析 buff行为
		private void Parse_Status(XmlElement root) {
			string str = string.Empty;
			var meta = new CBuffStatusMeta(root.GetAttribute("id"));

			m_xreader.TryReadChildNodeAttr(root, "Duration", "value", ref meta.Duration);
			//m_xreader.TryReadChildNodeAttr(root, "Period", "value", ref meta.Period);
			//m_xreader.TryReadChildNodeAttr(root, "PeriodicEffect", "value", ref meta.PeriodicEffect);


			//read modify
			var modifyNode = root.SelectSingleNode("Modification") as XmlElement;
			if (modifyNode != null) {
				var modify = meta.ModifyProperty;
				m_xreader.TryReadChildNodeAttr(modifyNode, "MoveSpeedMultiplier", "value", ref modify.MoveSpeedMultiplier);
			}
			//end modify

			//read state
			var stateNode = root.SelectSingleNode("StateFlags") as XmlElement;
			if (stateNode != null) {
				int state = 0;
				bool b = m_xreader.TryReadChildNodeAttr(stateNode, "HitFly", "value", ref state);
				if (b) meta.StateFlags[CPawnVO.State.HitFly] = state;
			}
			//end state

			CBuffMetaManager.AddMeta(meta);
		}
	}
	#endregion
}