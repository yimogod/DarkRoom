using System;
using System.Collections.Generic;

namespace Sword
{
	/// <summary>
	/// 怪物抽象的数据类
	/// 一般通过actor meta的配表创建出来
	/// </summary>
	public class ActorVO
	{
		public int MetaId;
		public string Name;
		public int Level;
		public int Class;
		public int Race;
		public string Address;



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
		protected float m_speedBase;

		public ActorVO()
		{
		}

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
	}
}