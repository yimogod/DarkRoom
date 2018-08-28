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

        private UICharacterEntry m_view => ViewComponent as UICharacterEntry;

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
                    OpenPanel<UICharacterEntry>();
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