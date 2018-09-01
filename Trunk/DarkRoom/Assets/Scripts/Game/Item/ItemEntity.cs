using UnityEngine;
using System.Collections;
namespace Sword
{
	//挂在物品身上, 比如说掉落在地上的物品
	public class ItemEntity : MonoBehaviour{
		protected ItemMeta _meta;

		void Start() {
		
		}

		public void Init(string id){
			_meta = ItemMetaManager.GetMeta(id);
		}

		void Update() {
		
		}
	}
}