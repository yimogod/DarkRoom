using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.Item{
	#region class
	public class CItemMeta : CBaseMeta{
		public const float GOLD_COIN_RATE = 1000;
		public enum ItemType{
			PROPS = 1, //1道具(血瓶, 饼干)
			MATERIAL, //2材料(宝石,毛皮)
			EQUIP,//3装备
		}

		public string nameKey;
		//玩家命名, 理论上应该可以拼出prefab, 和icon texture的名称
		public string style;

		public ItemType type = ItemType.PROPS;

		/*从商店购买的价格*/
		public int costType;
		public int cost;

		/*是否可以直接使用, 比如血瓶, 增加属性的丹药等*/
		public bool canUse;

		//如果是装备, 那么就会引用到真实的装备配置数据meta
		public int relatedId = 0;

		//使用的时候产生的能力id
		public int abilityId = 0;

		public CItemMeta (int id) : base(id){}

		//public string name{
		//	get{
		//		string key = string.Format("{0}_name", nameKey);
		//		return LangManager.GetTxt(key);
		//	}
		//}

		//public string desc{
		//	get{
		//		string key = string.Format("{0}_desc", nameKey);
		//		return LangManager.GetTxt(key); 
		//	}
		//}

		/*出售价格*/
		public int sell{
			get{
				float s = cost;
				//if(needGold)s *= GOLD_COIN_RATE;
				return (int)(s * 0.3f);
			}
		}
	}
	#endregion

	#region manager
	public class CItemMetaManager{
		private static Dictionary<string, CItemMeta> _itemDict = new Dictionary<string, CItemMeta>();
		public CItemMetaManager (){}

		public static void AddMeta(CItemMeta meta){
			_itemDict.Add(meta.sId, meta);
		}

		public static CItemMeta GetMeta(string id){
			return _itemDict[id];
		}
	}
	#endregion

	#region parser
	public class CItemMetaParser : CMetaParser{
		public override void Execute(string content){
			base.Execute(content);

			for(int i = 0; i < m_reader.row; ++i){
				m_reader.MarkRow(i);

				CItemMeta meta = new CItemMeta(m_reader.ReadInt());
				meta.nameKey = m_reader.ReadString();
				meta.style = m_reader.ReadString();
				meta.costType = m_reader.ReadInt();
				meta.cost = m_reader.ReadInt();
				meta.canUse = m_reader.ReadBool();
				meta.relatedId = m_reader.ReadInt();
				meta.abilityId = m_reader.ReadInt();

				CItemMetaManager.AddMeta(meta);
			}

		}
	}

	#endregion
}