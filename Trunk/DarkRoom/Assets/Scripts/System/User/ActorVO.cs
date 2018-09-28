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

		/// <summary>
		/// 角色名称
		/// </summary>
		public string Name;

		/// <summary>
		/// 角色等级
		/// </summary>
		public int Level;

		/// <summary>
		/// 只做存储用, 读取请用emun字段
		/// </summary>
		public int Class;

		/// <summary>
		/// 只做存储用, 读取请用emun字段
		/// </summary>
		public int Race;

		/// <summary>
		/// 角色模型的地址
		/// 其他相关的prefab可以通过拼接出来
		/// </summary>
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

		/// <summary>
		/// 职业枚举
		/// </summary>
		public ActorClass ClassEnum => (ActorClass)Class;

		/// <summary>
		/// 种族枚举
		/// </summary>
		public ActorRace RaceEnum => (ActorRace)Race;

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
	}
}