using System;
using System.Collections.Generic;
using DarkRoom.Game;

namespace Sword
{
    public class ActorEntity : CPawnEntity
    {
        /// <summary>
        /// 配表id
        /// </summary>
        public int MetaId;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 图标. 可以通过一些规则得到其他类型的图标
        /// </summary>
        public string Icon;

        /// <summary>
        /// actor的属性,包含蓝,红,攻,防
        /// </summary>
        public SwordAttributeSet AttributeSet = new SwordAttributeSet();

        protected CWorld m_world => CWorld.Instance;

        protected SwordGameMode m_gameMode => m_world.GetGameMode<SwordGameMode>();
    }
}
