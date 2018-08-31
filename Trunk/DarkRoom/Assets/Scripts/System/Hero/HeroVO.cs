using System;
using System.Collections.Generic;

namespace Sword
{
    public class HeroVO : ActorVO
    {
        public string Name;

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
