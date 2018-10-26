using UnityEngine;
using DarkRoom.Game;

namespace Sword
{
	/// <summary>
	/// 武器是绑定在武器GameObject身上的.
	///	他的位置是actor骨骼的手部骨骼.
	///
	///	对于近战武器，我们有个约定。
	///	如果我的攻击动画到了trigger atk的点， 那么我就认为必然攻击了目标
	///	不会再进行攻击距离和攻击bound碰撞测试
	///	之后会在effect dmage中进行是否miss或者cirt等判定
	/// 
	///	但这次攻击时一定有效的。 即使这个瞬间target已经后撤出了攻击范围
	///	但如果在武器前摇阶段之前进行移动取消这个攻击，这不会认为攻击了目标
	///
	///	这个武器目前是绑在人身上的
	/// 
	/// 另外这个武器不一定会有真实的可显示
	/// </summary>
	public class WeaponEntity : MonoBehaviour
	{
		public DamageTypePacket MinDamage;
		public DamageTypePacket MaxDamage;

		private int m_meta;

		//武器自带的效果, 只会被创建一次
		//private CEffect m_effect = null;

		public WeaponMeta Meta{
			//get{ return CEquipmentMetaManager.GetWeapon(m_meta); }
			get { return null; }
		}

		public void SetMeta(int id){
			m_meta = id;
		}

		/// <summary>
		/// TODO 目前没有调用
		/// 完善技能系统后再说
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="target"></param>
		public void Attack(CAIController owner, CController target){
			/*if (m_effect == null){
				m_effect = CEffectHelper.CreateCEffect(m_meta.Effect, gameObject);
			}
			if(m_effect != null)m_effect.Apply(owner, target);*/
		}
	}
}