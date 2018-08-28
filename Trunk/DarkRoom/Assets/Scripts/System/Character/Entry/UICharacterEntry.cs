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
            CApplicationManager.Instance.ChangeProcedure(PlayerCastle_Procedure.NAME);
        }

        private void OnClickChangeBtn()
        {
            CApplicationManager.Instance.ChangeProcedure(CharacterChoose_Procedure.NAME);
        }

        private void OnClickCreateBtn()
        {
            CApplicationManager.Instance.ChangeProcedure(CharacterCreate_Procedure.NAME);
        }
    }
}
