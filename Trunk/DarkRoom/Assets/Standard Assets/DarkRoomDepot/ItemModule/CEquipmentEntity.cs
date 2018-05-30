
namespace DarkRoom.Item {
	public class CEquipmentEntity : CItemEntity{
		public CEquipmentMeta equipMeta;

		public static CEquipmentEntity LoadAndCreateEquipEntity(int equipId){
			CEquipmentMeta meta = CEquipmentMetaManager.GetMeta(equipId);
			//GameObject go = AssetManager.LoadEquipPrefab("1");
			//CEquipmentEntity equip = go.AddComponent<CEquipmentEntity>();
			//equip.equipMeta = meta;
			//return equip;
			return null;
		}

		//此装备物品装载角色身上
		/*public bool AttachToSlot(CActorEquipComp character, CEquipmentMeta.EquipSlot slot){
			CActorEquipSet data = character.equipSet;

			switch (slot){
				case CEquipmentMeta.EquipSlot.Head:
					data.head = this;
					break;
				case CEquipmentMeta.EquipSlot.Neck:
					data.neck = this;
					break;
				case CEquipmentMeta.EquipSlot.Chest:
					data.chest = this;
					break;
				case CEquipmentMeta.EquipSlot.Hand:
					data.hand = this;
					break;
				case CEquipmentMeta.EquipSlot.Waist:
					data.waist = this;
					break;
				case CEquipmentMeta.EquipSlot.LeftRing:
					data.leftRing = this;
					break;
				case CEquipmentMeta.EquipSlot.RightRing:
					data.rightRing = this;
					break;
				case CEquipmentMeta.EquipSlot.Leg:
					data.leg = this;
					break;
				case CEquipmentMeta.EquipSlot.Feet:
					data.feet = this;
					break;
				case CEquipmentMeta.EquipSlot.PrimaryWeapon:
					data.primaryWeapon = this;
					break;
				case CEquipmentMeta.EquipSlot.SecondaryWeapon:
					data.secondaryWeapon = this;
					break;
			}

			CActorAvatar avatar = character.gameObject.GetComponent<CActorAvatar>();
			avatar.EquipToSlot(slot, equipMeta.equipMethod, gameObject);
			return true;
		}*/
	}
}