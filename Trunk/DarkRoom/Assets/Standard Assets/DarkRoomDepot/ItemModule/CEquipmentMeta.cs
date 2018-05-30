using System.Collections.Generic;
using DarkRoom.Game;

namespace DarkRoom.Item {
	#region class
	public class CEquipmentMeta : CBaseMeta{
		//Slot 和 type是多对多的关系, 所以不能通过slot获得type, 反之亦然
		public enum EquipSlot{
			Head,//头盔
			Neck,//项链
			Chest,//盔甲
			Waist,//腰带
			Hand, //手套
			LeftRing,
			RightRing,
			Leg,
			Feet,//鞋
			PrimaryWeapon,
			SecondaryWeapon,
			Count,
		}

		public enum EquipType{
			Null,
			Cap,
			Necklace,
			Armor,
			Belt,
			Glove, //手套
			Ring,
			Trouser,
			Shoe,
			Weapon,
			Sheild,
			Count
		}

		public enum EquipMethod {
			Null,
			LinkAsChild,
			ReplaceSkinMesh,
			UseSelfAnimator,
		}

		//prefab命名规范
		//Preb_Armor_001_0(1)(2)
		//001是id, 最后的0/1/2是对应的标准骨骼数
		//比如所有参与换装的角色总共有10种, 那么就是0-9
		//比如火炬之光4职业, 2个性别. 因为他的所有的职业身高一样, 所以就两套骨骼, 一套男的, 一套女的
		//那么就0/1
		public string Prefab;
		public string NameKey;
		public EquipType Type;
		public EquipMethod Method;

		public CEquipmentMeta(int id) : base(id){}

		public bool CanEquipToSlot(EquipSlot slot){
			bool value = false;
			switch(slot){
				case EquipSlot.Head:
					if (isCap)value = true;
					break;
				case EquipSlot.Neck:
					if (isNecklace)value = true;
					break;
				case EquipSlot.Chest:
					if (isArmor)value = true;
					break;
				case EquipSlot.Waist:
					if (isBelt)value = true;
					break;
				case EquipSlot.Hand:
					if (isGlove)value = true;
					break;
				case EquipSlot.LeftRing:
				case EquipSlot.RightRing:
					if (isRing)value = true;
					break;
				case EquipSlot.Feet:
					if (isShoe)value = true;
					break;
				case EquipSlot.PrimaryWeapon:
				case EquipSlot.SecondaryWeapon:
					if (isWeapon || isShield)value = true;
					break;
			}

			return value;
		}

		public bool isCap{
			get{ return  Type == EquipType.Cap; }
		}

		public bool isNecklace{
			get{ return  Type == EquipType.Necklace; }
		}

		public bool isArmor{
			get{ return  Type == EquipType.Armor; }
		}

		public bool isBelt{
			get{ return  Type == EquipType.Belt; }
		}

		public bool isGlove{
			get{ return  Type == EquipType.Glove; }
		}

		public bool isRing{
			get{ return  Type == EquipType.Ring; }
		}

		public bool isShoe{
			get{ return  Type == EquipType.Shoe; }
		}

		public bool isWeapon{
			get{ return  Type == EquipType.Weapon; }
		}

		public bool isShield{
			get{ return  Type == EquipType.Sheild; }
		}

	}
	#endregion

	#region class
	public class CEquipmentMetaManager{
		private static Dictionary<int, CEquipmentMeta> m_dict = 
			new Dictionary<int, CEquipmentMeta>();
		public CEquipmentMetaManager (){}

		public static Dictionary<int, CEquipmentMeta> data {
			get { return m_dict; }
		}

		public static void AddMeta(CEquipmentMeta meta){
			m_dict.Add(meta.Id, meta);
		}

		public static CEquipmentMeta GetMeta(int id){
			return m_dict[id];
		}

		public static CWeaponMeta GetWeapon(int id){
			CEquipmentMeta meta = null;

			bool b = m_dict.TryGetValue(id, out meta);
			if (b) return meta as CWeaponMeta;

			UnityEngine.Debug.Log("get weapon " + id);
			return null;
		}
	}
	#endregion

	#region parser
	public class CEquipmentMetaParser : CMetaParser {
		public override void Execute(string content) {
			base.Execute(content);

			for (int i = 0; i < m_reader.row; ++i) {
				m_reader.MarkRow(i);

				CEquipmentMeta meta = new CEquipmentMeta(m_reader.ReadInt());
				meta.NameKey = m_reader.ReadString();
				meta.Prefab = m_reader.ReadString();
				meta.Type = (CEquipmentMeta.EquipType)m_reader.ReadInt();
				meta.Method = (CEquipmentMeta.EquipMethod)m_reader.ReadInt();

				CEquipmentMetaManager.AddMeta(meta);
			}
		}
	}

	#endregion
}