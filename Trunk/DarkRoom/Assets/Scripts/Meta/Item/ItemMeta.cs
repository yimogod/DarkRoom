using System.Collections.Generic;
using DarkRoom.Game;

namespace Sword
{
	public class ItemMeta : CBaseMeta{
		//玩家命名, 理论上应该可以拼出prefab, 和icon texture的名称
		public string Prefab;

		public ItemType ItemType;

		/*从商店购买的价格*/
		public int Cost;

		/*是否可以直接使用, 比如血瓶, 增加属性的丹药等*/
		public bool CanUse;

		//如果是装备, 那么就会引用到真实的装备配置数据meta
		public int RelatedId = 0;

		//使用的时候产生的能力id
		public int abilityId = 0;

		public ItemMeta (int id) : base(id){}
	}

	public class ItemMetaManager{
		private static Dictionary<string, ItemMeta> m_itemDict = new Dictionary<string, ItemMeta>();
		public ItemMetaManager (){}

		public static void AddMeta(ItemMeta meta){
			m_itemDict.Add(meta.sId, meta);
		}

		public static ItemMeta GetMeta(string id){
			return m_itemDict[id];
		}
	}

	public class ItemMetaParser : CMetaParser{
        protected override void Parse(){
			for(int i = 0; i < m_reader.Row; ++i){
				m_reader.MarkRow(i);

				ItemMeta meta = new ItemMeta(m_reader.ReadInt());
				meta.NameKey = m_reader.ReadString();
				meta.Prefab = m_reader.ReadString();
				meta.Cost = m_reader.ReadInt();
				meta.CanUse = m_reader.ReadBool();
				meta.RelatedId = m_reader.ReadInt();
				meta.abilityId = m_reader.ReadInt();

				ItemMetaManager.AddMeta(meta);
			}

		}
	}
}