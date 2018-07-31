using System.Collections.Generic;
using System.Xml;
using DarkRoom.Game;
using UnityEngine;
using DarkRoom.GamePlayAbility;

namespace DarkRoom.GamePlayAbility {
	#region class
	public class CEffectMeta : CBaseMeta
	{
		public enum EffectType
		{
			Damage, //目标伤害扣血
			LaunchMissle, //发射炸弹
			ApplyBehaviour, //应用行为
			EnumArea, //搜索范围
			IssueOrder, //调用指令
			EffectSet, //效果集合
			CreateUnit, //创建单位
		}

		/// <summary>
		/// 效果的类型
		/// </summary>
		public EffectType Type = EffectType.Damage;

		/// <summary>
		/// 该效果是否能应用的验证器, 必须满足所有的条件
		/// 
		/// 用枚举来记录
		/// </summary>
		public List<string> ValidatorArray = new List<string>();

		/// <summary>
		/// 效果作用到谁身上
		/// </summary>
		public CAbilityEnum.Location WhichUnit;

		/// <summary>
		/// 该效果是否作用于自己身上
		/// </summary>
		public bool AttachSelf{
			get { return WhichUnit == CAbilityEnum.Location.CasterUnit; }
		}

		/// <summary>
		/// 该效果是否作用于目标身上
		/// </summary>
		public bool AttachTarget {
			get { return WhichUnit == CAbilityEnum.Location.TargetUnit; }
		}

		/// <summary>
		/// 根据配置, 效果到底作用于目标还是自身
		/// </summary>
		/*public GameObject DectectGameObjectOwner(GameObject from, GameObject to)
		{
			if (WhichUnit == CAbilityEnum.Location.CasterUnit) {
				return from;
			}
			return to;
		}*/

		public CEffectMeta(string idKey) : base(idKey) { }
	}
	#endregion

	#region manager
	public class CEffectMetaManager
	{
		private static Dictionary<string, CEffectMeta> m_dict = new Dictionary<string, CEffectMeta>();

		public static void AddMeta(CEffectMeta meta)
		{
			if(m_dict.ContainsKey(meta.sId)) {
				Debug.LogError("CEffectMetaManager ALREADY CONTAIN the ability with id -- " +  meta.sId);
			}

			m_dict[meta.sId] = meta;
		}

		public static CEffectMeta GetMeta(string id)
		{
			CEffectMeta meta = null;
			bool v = m_dict.TryGetValue(id, out meta);
			if(!v)Debug.LogError("CEffectMetaManager DO NOT CONTAIN the ability with id -- " + id);

			return meta;
		}
	}
	#endregion

	#region parser
	public class CEffectMetaParser : CMetaParser
	{
		public const string CEffectType_Damage = "CEffectDamage";
		public const string CEffectType_LaunchMissile = "CEffectLaunchMissile";
		public const string CEffectType_Set = "CEffectSet";
		public const string CEffectType_Order = "CEffectIssueOrder";
		public const string CEffectType_Behavior = "CEffectApplyBehavior";
		public const string CEffectType_Area = "CEffectEnumArea";
		

		public CEffectMetaParser() : base() {}

		public CEffectMetaParser(bool useXml) : base(useXml) {}


		public override void Execute (string content)
		{
			base.Execute(content);

			m_xreader.ReadRootNode();
			foreach (XmlElement node in m_xreader.rootChildNodes)
			{
				switch (node.LocalName)
				{
					case CEffectType_Damage:
						ParseEffect_Damage(node);
						break;
					case CEffectType_LaunchMissile:
						ParseEffect_LaunchMissle(node);
						break;
					case CEffectType_Set:
						ParseEffect_Set(node);
						break;
					case CEffectType_Order:
						ParseEffect_Order(node);
						break;
					case CEffectType_Behavior:
						ParseEffect_Behavior(node);
						break;
					case CEffectType_Area:
						ParseEffect_Area(node);
						break;
				}
			}
		}

		
		//解析 查询效果
		private void ParseEffect_Area(XmlElement root) {
			string str = string.Empty;
			var meta = new CEffectEnumAreaMeta(root.GetAttribute("id"));

			m_xreader.TryReadChildNodeAttr(root, "WhichUnit", "value", ref str);
			meta.WhichUnit = CAbilityUtil.GetLocation(str);

			m_xreader.TryReadChildNodeAttr(root, "ImpactLocation", "value", ref str);
			meta.ImpactLocation = CAbilityUtil.GetLocation(str);

			m_xreader.TryReadChildNodeAttr(root, "Radius", "value", ref meta.Area.Radius);
			m_xreader.TryReadChildNodeAttr(root, "ImpactEffect", "value", ref meta.Area.Effect);

			CEffectMetaManager.AddMeta(meta);
		}

		//解析 buff效果
		private void ParseEffect_Behavior(XmlElement root) {
			string str = string.Empty;
			var meta = new CEffectApplyBuffMeta(root.GetAttribute("id"));

			m_xreader.TryReadChildNodeAttr(root, "WhichUnit", "value", ref str);
			meta.WhichUnit = CAbilityUtil.GetLocation(str);
			m_xreader.TryReadChildNodeAttr(root, "Behavior", "value", ref meta.Behavior);

			CEffectMetaManager.AddMeta(meta);
		}

		//解析指令效果
		private void ParseEffect_Order(XmlElement root) {
			string str = string.Empty;
			var meta = new CEffectIssueOrderMeta(root.GetAttribute("id"));

			m_xreader.TryReadChildNodeAttr(root, "WhichUnit", "value", ref str);
			meta.WhichUnit = CAbilityUtil.GetLocation(str);

			m_xreader.TryReadChildNodeAttr(root, "Delay", "value", ref meta.Delay);
			m_xreader.TryReadChildNodeAttr(root, "EffectOrder", "value", ref str);
			meta.EffectOrder = CAbilityUtil.GetOrder(str);

			CEffectMetaManager.AddMeta(meta);
		}

		//解析效果集合
		private void ParseEffect_Set(XmlElement root)
		{
			string str = string.Empty;
			var meta = new CEffectSetMeta(root.GetAttribute("id"));

			m_xreader.TryReadChildNodeAttr(root, "WhichUnit", "value", ref str);
			meta.WhichUnit = CAbilityUtil.GetLocation(str);
			m_xreader.TryReadChildNodesAttr(root, "EffectArray", meta.EffectList);

			CEffectMetaManager.AddMeta(meta);
		}

		//解析伤害效果的数据
		private void ParseEffect_Damage(XmlElement root)
		{
			CEffectDamageMeta meta = new CEffectDamageMeta(root.GetAttribute("id"));
			m_xreader.TryReadChildNodeAttr(root, "Amount", "value", ref meta.Amount);

			CEffectMetaManager.AddMeta(meta);
		}

		//解析发射导弹的数据
		private void ParseEffect_LaunchMissle(XmlElement root)
		{
			string str = string.Empty;
			var meta = new CEffectLaunchMissileMeta(root.GetAttribute("id"));

			m_xreader.TryReadChildNodeAttr(root, "WhichUnit", "value", ref str);
			meta.WhichUnit = CAbilityUtil.GetLocation(str);

			m_xreader.TryReadChildNodeAttr(root, "LaunchLocation", "value", ref str);
		//	meta.LaunchVFX.LocationValue = CAbilityUtil.GetLocation(str);
	//		m_xreader.TryReadChildNodeAttr(root, "LaunchLocation", "prefab", ref meta.LaunchVFX.Prefab);

			m_xreader.TryReadChildNodeAttr(root, "ImpactLocation", "value", ref str);
		//	meta.ImpactVFX.LocationValue = CAbilityUtil.GetLocation(str);
		//	m_xreader.TryReadChildNodeAttr(root, "ImpactLocation", "prefab", ref meta.ImpactVFX.Prefab);

			m_xreader.TryReadChildNodeAttr(root, "Missile", "value", ref meta.MisslePrefab);
			m_xreader.TryReadChildNodeAttr(root, "Movers", "Link", ref meta.Mover);
			m_xreader.TryReadChildNodeAttr(root, "Speed", "value", ref meta.MissileSpeed);
			m_xreader.TryReadChildNodeAttr(root, "Slerp", "value", ref meta.Slerp);

			CEffectMetaManager.AddMeta(meta);
		}

	}
	#endregion
}