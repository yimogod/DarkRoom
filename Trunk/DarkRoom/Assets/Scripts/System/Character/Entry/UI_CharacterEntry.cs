using System;
using System.Collections.Generic;
using DarkRoom.Game;
using DarkRoom.UI;
using UnityEngine.UI;

namespace Sword
{
    /// <summary>
    /// 记录下ui的方案, 从ui上面生成一个json文件.挂在ui上, 然后打包到boundle里面. 
    /// </summary>
    public class UI_CharacterEntry : CUIWindowBase
    {
        /// <summary>
        /// 进入游戏
        /// </summary>
        public Button EnterBtn;

        /// <summary>
        /// 切换角色游戏
        /// </summary>
        public Button ChooseCharacterBtn;

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
            EnterBtn.onClick.AddListener(OnClickEnterBtn);
            ChooseCharacterBtn.onClick.AddListener(OnClickChooseBtn);
            CreateCharacterBtn.onClick.AddListener(OnClickCreateBtn);
        }

        protected override void OnUnBindEvent()
        {
            EnterBtn.onClick.RemoveAllListeners();
            ChooseCharacterBtn.onClick.RemoveAllListeners();
            CreateCharacterBtn.onClick.RemoveAllListeners();
        }

        private void OnClickEnterBtn()
        {
            CApplicationManager.Instance.ChangeProcedure(PlayerCastle_Procedure.NAME);
        }

        private void OnClickChooseBtn()
        {
            CApplicationManager.Instance.ChangeProcedure(CharacterChoose_Procedure.NAME);
        }

        private void OnClickCreateBtn()
        {
            CApplicationManager.Instance.ChangeProcedure(CharacterCreate_Procedure.NAME);
        }
    }
}
