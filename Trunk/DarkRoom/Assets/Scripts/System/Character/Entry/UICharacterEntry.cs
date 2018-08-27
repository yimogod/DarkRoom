using System;
using System.Collections.Generic;
using DarkRoom.UI;
using UnityEngine.UI;

namespace Sword
{
    public class UICharacterEntry : CUIWindowBase
    {
        /// <summary>
        /// 进入游戏
        /// </summary>
        public Button EnterBtn;

        /// <summary>
        /// 切换角色游戏
        /// </summary>
        public Button ChangeCharacterBtn;

        /// <summary>
        /// 创建角色游戏
        /// </summary>
        public Button CreateCharacterBtn;

        public override void OnShow()
        {
            base.OnShow();
        }
    }
}
