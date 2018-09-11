using System;
using System.Collections.Generic;

namespace Sword
{
	/// <summary>
	/// mvc中的数据来源
	/// 由controller根据各种情况改变各种数据
	/// </summary>
	public class ActorVO
	{
		public int Level;
		public int Class;
		public int Race;

		/// <summary>
		/// 掌握的技能列表, 除了出生的数据, 也包含后天学习的
		/// </summary>
		public List<int> AbilityIdList = new List<int>();

		/// <summary>
		/// 掌握的技能的等级字典
		/// key是ability id, value是级别
		/// </summary>
		public Dictionary<int, int> AbilityLvDict = new Dictionary<int, int>();

		//角色元数据
		protected ActorMeta m_metaBase;
		protected float m_speedBase;

		public ActorVO()
		{
		}

		public ActorVO(ActorMeta meta)
		{
			m_metaBase = meta;
		}

		/// <summary>
		/// 角色元数据基类
		/// </summary>
		public ActorMeta MetaBase => m_metaBase;

		/// <summary>
		/// 当期武器的攻击距离
		/// </summary>
		public virtual float AtkRange
		{
			get { return 10.0f; }
		}

		/// <summary>
		/// 血量小于0就是死亡
		/// </summary>
		public bool Dead
		{
			get { return false; }
		}

		/// <summary>
		/// 人物的基础速度
		/// </summary>
		public float SpeedBase
		{
			get { return m_speedBase; }
			set { m_speedBase = value; }
		}


		public void SetMeta(ActorMeta meta)
		{
			m_metaBase = meta;
		}
	}
}