using System;
using System.Collections.Generic;
using DarkRoom.Game;
using DarkRoom.UI;
using UnityEngine.UI;

namespace Sword
{
    public class UI_CharacterChoose : CUIWindowBase
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

        public override void OnReveal()
        {
            base.OnReveal();
        }

        protected override void OnBindEvent()
        {
            EnterBtn.onClick.AddListener(OnClickEnterBtn);
            ChangeCharacterBtn.onClick.AddListener(OnClickChangeBtn);
            CreateCharacterBtn.onClick.AddListener(OnClickCreateBtn);
        }

        protected override void OnUnBindEvent()
        {
            EnterBtn.onClick.RemoveAllListeners();
            ChangeCharacterBtn.onClick.RemoveAllListeners();
            CreateCharacterBtn.onClick.RemoveAllListeners();
        }

        private void OnClickEnterBtn()
        {
            CApplicationManager.Instance.ChangeProcedure(Procedure_PlayerCastle.NAME);
        }

        private void OnClickChangeBtn()
        {
            CApplicationManager.Instance.ChangeProcedure(Procedure_CharacterChoose.NAME);
        }

        private void OnClickCreateBtn()
        {
            CApplicationManager.Instance.ChangeProcedure(Procedure_CharacterCreate.NAME);
        }
    }
}
