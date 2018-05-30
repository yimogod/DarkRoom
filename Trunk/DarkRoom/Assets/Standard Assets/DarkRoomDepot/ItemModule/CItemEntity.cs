using UnityEngine;
using System.Collections;

namespace DarkRoom.Item {
	//挂在物品身上, 比如说掉落在地上的物品
	public class CItemEntity : MonoBehaviour{
		protected CItemMeta _meta;

		void Start() {
		
		}

		public void Init(int id){
			_meta = CItemMetaManager.GetMeta(id);
		}

		void Update() {
		
		}
	}
}