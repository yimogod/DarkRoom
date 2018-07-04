using System;
using System.Collections.Generic;

namespace DarkRoom.Game
{
    public class CGameplayTagRequirement
    {
        /// <summary>
        /// 这里面的tag必须都有
        /// </summary>
        public CGameplayTagContainer RequireTagContainer;

        /// <summary>
        /// 这里面的tag必须都没有
        /// </summary>
        public CGameplayTagContainer IgnoreTagContainer;
    }
}
