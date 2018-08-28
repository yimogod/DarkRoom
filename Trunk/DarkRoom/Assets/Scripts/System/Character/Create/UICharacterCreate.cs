using System;
using System.Collections.Generic;
using DarkRoom.Game;
using DarkRoom.UI;
using UnityEngine.UI;

namespace Sword
{
    public class UICharacterCreate : CUIWindowBase
    {
        /// <summary>
        /// 创建角色游戏
        /// </summary>
        public Button CreateCharacterBtn;

        public override void OnDisplay()
        {
            base.OnDisplay();
        }

        protected override void OnBindEvent()
        {
            CreateCharacterBtn.onClick.AddListener(OnClickCreateBtn);
        }

        protected override void OnUnBindEvent()
        {
            CreateCharacterBtn.onClick.RemoveAllListeners();
        }

        private void OnClickCreateBtn()
        {
            ProxyPool.UserProxy.CreateCharacter("TestWarrier", (int)MetaRace.Human, (int)MetaClass.Warrier);
            ProxyPool.HeroProxy.CreateHero("TestWarrier", (int)MetaRace.Human, (int)MetaClass.Warrier);
            CApplicationManager.Instance.ChangeProcedure(CharacterEntry_Procedure.NAME);
        }
    }
}
