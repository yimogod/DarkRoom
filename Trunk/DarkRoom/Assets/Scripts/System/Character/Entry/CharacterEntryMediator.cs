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

        public CharacterEntryMediator() : base(NAME)
        {
        }

        private UICharacterEntry m_view => ViewComponent as UICharacterEntry;

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
    }
}