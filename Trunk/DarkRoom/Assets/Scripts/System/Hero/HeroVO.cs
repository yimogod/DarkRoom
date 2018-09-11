using System;
using System.Collections.Generic;

namespace Sword
{
	/// <summary>
	/// 玩家角色信息, 通过创建界面创建出来
	/// </summary>
	public class HeroVO : ActorVO
	{
		/// <summary>
		/// 还没有使用的属性点
		/// </summary>
		public int AttributePoint;

		/// <summary>
		/// 还没有使用的技能点
		/// </summary>
		public int SkillPoint;

		/// <summary>
		/// 玩家用属性点添加到7个主属性的值
		/// </summary>
		public int Strength;

		public int Dexterity;
		public int Constitution;
		public int Magic;
		public int Willpower;
		public int Cunning;
		public int Luck;

		public HeroEntity Entity;
	}
}