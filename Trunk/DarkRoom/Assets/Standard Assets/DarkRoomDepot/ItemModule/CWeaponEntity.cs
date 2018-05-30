using UnityEngine;
using DarkRoom.Game;

namespace DarkRoom.Item {
	#region class
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
	public class CWeaponEntity : MonoBehaviour{
		private int m_meta;

		//武器自带的效果, 只会被创建一次
		//private CEffect m_effect = null;

		public CWeaponMeta Meta{
			get{ return CEquipmentMetaManager.GetWeapon(m_meta); }
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
	#endregion


	#region meta
	/// <summary>
	/// 武器配表, 从目前看来跟abilityeffect有相当一部分重合
	/// 先这么干着. 然后再看如何合并
	/// </summary>
	public class CWeaponMeta : CEquipmentMeta {
		/// <summary>
		/// 武器的射击间隔, cd时间, 基于秒
		/// </summary>
		public float Period = 0.5f;

		/// <summary>
		/// 射程,基于米
		/// </summary>
		public float Range = 5f;

		/// <summary>
		/// 最短射程. 小于这个距离也不能攻击
		/// 比如箭, 太近就不能射了. 就必须后退.
		/// 但moba里面没这个设定. 我们可以将MinRange设为0
		/// </summary>
		public float MinRange = 0;

		/// <summary>
		/// 武器前摇, 英文是back也是前摇
		/// </summary>
		public float Backswing = 0.2f;

		/// <summary>
		/// 攻击期间是否允许移动
		/// </summary>
		public bool AllowMovement = false;

		/// <summary>
		/// 武器自身提供的伤害值.
		/// 如果武器发射飞行物. 那么这个值会覆盖飞行物效果的伤害
		/// </summary>
		public int Damage = 0;

		/// <summary>
		/// 是否是近战武器. 还是靠发射发射物或者魔法的武器
		/// </summary>
		public bool Melee = false;

		/// <summary>
		/// 武器开火或者使用产生的效果
		/// 比如命中产生伤害
		/// 比如挂在buff, 比如溅射
		/// 如果会产生多个效果, 就用EffectSet来实现
		/// </summary>
		public string Effect;

		/// <summary>-
		/// 装备到身上时产生的效果.
		/// 一般用来添加buff
		/// </summary>
		public string OnEquipedEffect;

		/// <summary>
		/// 武器开始攻击的视觉效果
		/// 可以理解为动作播放第一帧时产生的效果
		/// </summary>
		public string LaunchVFX;

		public CWeaponMeta(int id) : base(id) { }
	}
	#endregion


	#region parser
	public class WeaponMetaParser : CMetaParser {
		public override void Execute(string content) {
			base.Execute(content);

			for (int i = 0; i < m_reader.row; ++i) {
				m_reader.MarkRow(i);
				if (i == 0) continue;

				CWeaponMeta meta = new CWeaponMeta(m_reader.ReadInt());
				meta.NameKey = m_reader.ReadString();
				meta.Prefab = m_reader.ReadString();
				meta.Period = m_reader.ReadInt() * 0.001f;
				meta.Range = m_reader.ReadFloat();
				meta.Backswing = m_reader.ReadFloat();
				meta.AllowMovement = m_reader.ReadBool();
				meta.Effect = m_reader.ReadString();
				meta.OnEquipedEffect = m_reader.ReadString();
				meta.Damage = m_reader.ReadInt();

				CEquipmentMetaManager.AddMeta(meta);
			}

		}
	}
	#endregion
}