using System;
using System.Collections.Generic;
using DarkRoom.Game;


namespace Sword
{
	/// <summary>
	/// 武器配表, 从目前看来跟abilityeffect有相当一部分重合
	/// 先这么干着. 然后再看如何合并
	/// </summary>
	public class CWeaponMeta : EquipmentMeta
	{
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

	public class WeaponMetaParser : CMetaParser
	{
		public override void Execute(string content)
		{
			base.Execute(content);

			for (int i = 0; i < m_reader.Row; ++i)
			{
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

				EquipmentMetaManager.AddMeta(meta);
			}

		}
	}
}
