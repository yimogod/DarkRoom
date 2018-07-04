using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using DarkRoom.Core;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
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
						ParseBeh_Buff(node);
						break;
				}
			}
		}


		//解析 buff行为
		private void ParseBeh_Buff(XmlElement root) {
			string str = string.Empty;
			var meta = new CBuffStatusMeta(root.GetAttribute("id"));

			m_xreader.TryReadChildNodeAttr(root, "Duration", "value", ref meta.Duration);
			m_xreader.TryReadChildNodeAttr(root, "Period", "value", ref meta.Period);
			m_xreader.TryReadChildNodeAttr(root, "PeriodicEffect", "value", ref meta.PeriodicEffect);


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