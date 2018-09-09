using System.Collections.Generic;
using Assets.Scripts.System.Common;
using DarkRoom.UI;
using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class CharacterEntryMediator : SwordBaseMediator
    {
        public new const string NAME = "CharacterEntryMediator";

        private UI_CharacterEntry m_view => ViewComponent as UI_CharacterEntry;

        public CharacterEntryMediator() : base(NAME)
        {
        }

        public override IList<string> ListNotificationInterests()
        {
            IList<string> list = new List<string>();
            list.Add(NotiConst.Open_CharacterEntry);
            return list;
        }

        public override void HandleNotification(INotification note)
        {
            switch (note.Name)
            {
                case NotiConst.Open_CharacterEntry:
                    OpenPanel<UI_CharacterEntry>();
                    break;
            }
        }

        private void OnClickEnterBtn()
        {
        }

        private void OnClickChangeBtn()
        {

        }

        private void OnClickCreateBtn()
        {

        }
    }
}