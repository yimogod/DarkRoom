using System;
using System.Collections.Generic;
using DarkRoom.Game;


namespace Sword
{
	/// <summary>
	/// 武器配表, 从目前看来跟abilityeffect有相当一部分重合
	/// 先这么干着. 然后再看如何合并
	/// </summary>
	public class WeaponMeta : EquipmentMeta
	{
		public WeaponType WeaponType;

		/// <summary>
		/// 武器自身提供的伤害值.
		/// 如果武器发射飞行物. 那么这个值会覆盖飞行物效果的伤害
		/// </summary>
		public int Damage = 0;

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
		/// 射程,基于米
		/// TODO 改为根据weapontype来定义
		/// </summary>
		public float AtkRange
		{
			get { return 5f; }
		}

		/// <summary>
		/// 是否射击武器
		/// </summary>
		public bool RangedWeapon
		{
			get { return false; }
		}

		/// <summary>
		/// 是否魔法武器
		/// </summary>
		public bool MagaWeapon
		{
			get { return false; }
		}

		/// <summary>
		/// 是否双手武器
		/// </summary>
		public bool TwoHand
		{
			get { return false; }
		} 


		public WeaponMeta(int id) : base(id) { }
	}

	public class WeaponMetaParser : CMetaParser
	{
        protected override void Parse()
        {
			for (int i = 0; i < m_reader.Row; ++i)
			{
				m_reader.MarkRow(i);
				if (i == 0) continue;

				WeaponMeta meta = new WeaponMeta(m_reader.ReadInt());
				meta.NameKey = m_reader.ReadString();
				meta.WeaponType = (WeaponType)m_reader.ReadInt();
				meta.Prefab = m_reader.ReadString();
				meta.Effect = m_reader.ReadString();
				meta.OnEquipedEffect = m_reader.ReadString();
				meta.Damage = m_reader.ReadInt();

				EquipmentMetaManager.AddMeta(meta);
			}

		}
	}
}
