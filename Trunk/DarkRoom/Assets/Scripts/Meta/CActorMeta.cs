using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using DarkRoom.Game;

namespace Sword{
	#region class
	//actor除了初始化角色的时候用到, 在角色升级也要用到
	public class CActorMeta : CBaseMeta{
		//角色职业
		public enum ActorClass{
			None,
			Warrior,
			Magic,
			Robber,
			Ranger,
		}

		/* name display */
		public string nameKey;

		//基于秒的单位为米(m)--一般人类的速度是10km/1h
		//意味着1s是2.7m
		public float speed = 3f;

		/* warrior magic or ranger */
		public ActorClass job = ActorClass.Warrior;

		//骨骼类型, 根据具体的标准骨骼数量来定义
		//具体对应的是CEquipMtea 的prefab 命名解释
		public int boneType = 0;

		/*击杀获取的经验*/
		public int deadExp;

		//视野, field of view
		public int fov;

		/* resource key, which indeed  is prefab name */
		public string prefab;

		public int atkBase;
		public int atkStep;

		public int defBase;
		public int defStep;

		public int hpBase;
		public int hpStep;

		public int mpBase;
		public int mpStep;

		/* base on 1000 */
		public int critBase;

		//创建角色给的武器
		public int initWeapon = 0;
		public int initSecondaryWeapon = 0;
		//头盔
		public int initCapId = 0;
		//护身符
		public int initNecklaceId = 0;
		//盔甲
		public int initArmorId = 0;
		//腰带
		public int initBeltId = 0;
		//手套
		public int initGloveId = 0;
		//戒指
		public int initLeftRingId = 0;
		//戒指
		public int initRightRingId = 0;
		//裤子
		public int initTrouserId = 0;
		//鞋
		public int initShoeId = 0;

		//主动技能, x代表ability id， y代表ability lv
		public List<Vector2> activeAbilityList = new List<Vector2>();

		//talent, 出生自带的buff
		public List<Vector2> talentList = new List<Vector2>();

		public CActorMeta (string id):base(id){}

		public string name{
			get { return nameKey; }
		}

		public int GetATK(int level){
			return atkBase + level * atkStep;
		}
		
		public int GetDEF(int level){
			return defBase + level * defStep;
		}

		public int GetHP(int level){
			return hpBase + level * hpStep;
		}

		public int GetMP(int level){
			return mpBase + level * mpBase;
		}

		public int GetCrit(int level){
			return critBase;
		}
	}
	#endregion

	#region manager
	public class CActorMetaManager{
		public static Dictionary<string, CActorMeta> _unitDict = new Dictionary<string, CActorMeta>();

		public CActorMetaManager (){}

		public static Dictionary<string, CActorMeta> data {
			get { return _unitDict; }
		}

		public static void AddUnitMeta(CActorMeta meta){
			_unitDict.Add(meta.Id, meta);
		}

		public static CActorMeta GetUnitMeta(string id){
			if(!_unitDict.ContainsKey(id)){
				Debug.LogError(string.Format("actor id -- {0} not found ", id));
				return null;
			}
			return _unitDict[id];
		}
	}
	#endregion

	#region parser
	public class CActorMetaParser : CMetaParser{
		public CActorMetaParser() : base(true){}


		public override void Execute (string content){
			base.Execute(content);
			m_xreader.ReadRootNode();
			foreach (XmlElement node in m_xreader.rootChildNodes){
				CActorMeta meta = new CActorMeta(node.GetAttribute("id"));
				meta.nameKey = node.GetAttribute("name");
				m_xreader.TryReadChildNodeAttr(node, "Style", "prefab", ref meta.prefab);
				m_xreader.TryReadChildNodeAttr(node, "Style", "bone", ref meta.boneType);

				m_xreader.TryReadChildNodeAttr(node, "Hp", "value", ref meta.hpBase);
				m_xreader.TryReadChildNodeAttr(node, "Hp", "step", ref meta.hpStep);

				m_xreader.TryReadChildNodeAttr(node, "Mp", "value", ref meta.mpBase);
				m_xreader.TryReadChildNodeAttr(node, "Mp", "step", ref meta.mpStep);

				m_xreader.TryReadChildNodeAttr(node, "Atk", "value", ref meta.atkBase);
				m_xreader.TryReadChildNodeAttr(node, "Atk", "step", ref meta.atkStep);

				m_xreader.TryReadChildNodeAttr(node, "Def", "value", ref meta.defBase);
				m_xreader.TryReadChildNodeAttr(node, "Def", "step", ref meta.defStep);

				m_xreader.TryReadChildNodeAttr(node, "Porperty", "speed", ref meta.speed);

				int job = 0;
				m_xreader.TryReadChildNodeAttr(node, "Porperty", "job", ref job);
				meta.job = (CActorMeta.ActorClass)job;

				m_xreader.TryReadChildNodeAttr(node, "Porperty", "dead_exp", ref meta.deadExp);
				m_xreader.TryReadChildNodeAttr(node, "Porperty", "fov", ref meta.fov);

				m_xreader.TryReadChildNodeAttr(node, "Equip", "left_weapon", ref meta.initWeapon);
				m_xreader.TryReadChildNodeAttr(node, "Equip", "right_weapon", ref meta.initSecondaryWeapon);
				m_xreader.TryReadChildNodeAttr(node, "Equip", "cap", ref meta.initCapId);
				m_xreader.TryReadChildNodeAttr(node, "Equip", "neck", ref meta.initNecklaceId);
				m_xreader.TryReadChildNodeAttr(node, "Equip", "armor", ref meta.initArmorId);
				m_xreader.TryReadChildNodeAttr(node, "Equip", "belt", ref meta.initBeltId);
				m_xreader.TryReadChildNodeAttr(node, "Equip", "glove", ref meta.initGloveId);
				m_xreader.TryReadChildNodeAttr(node, "Equip", "left_ring", ref meta.initLeftRingId);
				m_xreader.TryReadChildNodeAttr(node, "Equip", "right_ring", ref meta.initRightRingId);
				m_xreader.TryReadChildNodeAttr(node, "Equip", "trouser", ref meta.initTrouserId);
				m_xreader.TryReadChildNodeAttr(node, "Equip", "shoe", ref meta.initShoeId);

				//解析ability
				XmlNode abilityRoot =  node.SelectSingleNode("Ability");
				foreach(XmlElement ability in abilityRoot.ChildNodes){
					Vector2 v = Vector2.zero;
					v.x = int.Parse(ability.GetAttribute("id"));
					v.y = int.Parse(ability.GetAttribute("lv"));
					meta.activeAbilityList.Add(v);
				}

				//解析talent
				XmlNode talentRoot =  node.SelectSingleNode("Talent");
				foreach(XmlElement talent in talentRoot.ChildNodes){
					Vector2 v = Vector2.zero;
					v.x = int.Parse(talent.GetAttribute("id"));
					v.y = int.Parse(talent.GetAttribute("lv"));
					meta.talentList.Add(v);
				}

				CActorMetaManager.AddUnitMeta(meta);
			}
		}
	}
	#endregion
}