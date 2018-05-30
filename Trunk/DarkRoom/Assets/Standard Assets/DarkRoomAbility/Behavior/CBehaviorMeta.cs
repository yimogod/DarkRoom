using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using DarkRoom.Core;
using DarkRoom.Game;

namespace DarkRoom.GamePlayAbility {
	public class CBehaviorMeta : CBaseMeta
	{
		public enum BehaviorType {
			ApplyBuff, //附着buff
		}

		/// <summary>
		/// 行为的类型
		/// </summary>
		public BehaviorType Type = BehaviorType.ApplyBuff;

		public CBehaviorMeta(string idKey) : base(idKey) { }
	}

	public class CBehaviorMetaManager {
		private static Dictionary<string, CBehaviorMeta> m_dict = 
			new Dictionary<string, CBehaviorMeta>();

		public static void AddMeta(CBehaviorMeta meta)
		{
			if(m_dict.ContainsKey(meta.IdKey)) {
				Debug.LogError("CBehaviorMetaManager ALREADY CONTAIN the behavior with id -- " +  meta.IdKey);
			}

			m_dict[meta.IdKey] = meta;
		}

		public static CBehaviorMeta GetMeta(string id)
		{
			CBehaviorMeta meta = null;
			bool v = m_dict.TryGetValue(id, out meta);
			if(!v)Debug.LogError("CBehaviorMetaManager DO NOT CONTAIN the behavior with id -- " + id);

			return meta;
		}
	}

	#region parser
	public class CBehaviorMetaParser : CMetaParser {
		public const string CBehType_Buff = "CBehaviorBuff";

		public CBehaviorMetaParser() : base() { }
		public CBehaviorMetaParser(bool useXml) : base(useXml) { }

		public override void Execute(string content) {
			base.Execute(content);

			m_xreader.ReadRootNode();
			foreach (XmlElement node in m_xreader.rootChildNodes) {
				switch (node.LocalName) {
					case CBehType_Buff:
						ParseBeh_Buff(node);
						break;
				}
			}
		}


		//解析 buff行为
		private void ParseBeh_Buff(XmlElement root) {
			string str = string.Empty;
			var meta = new CBehaviorBuffMeta(root.GetAttribute("id"));

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

			CBehaviorMetaManager.AddMeta(meta);
		}
	}
	#endregion
}