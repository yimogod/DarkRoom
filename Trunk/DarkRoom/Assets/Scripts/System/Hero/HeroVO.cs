using System;
using System.Collections.Generic;

namespace Sword
{
    public class HeroVO : ActorVO
    {
        public string Name;
        public int Level;
        public int Class;
        public int Race;

        /// <summary>
        /// 还没有使用的属性点
        /// </summary>
        public int AttributePoint;

        /// <summary>
        /// 还没有使用的技能点
        /// </summary>
        public int SkillPoint;

        public HeroEntity Entity;
    }
}
