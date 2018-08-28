using System.Collections.Generic;
using Assets.Scripts.System.Common;
using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class CharacterCreateMediator : SwordBaseMediator
    {
        public new const string NAME = "CharacterCreateMediator";

        private UICharacterCreate m_view => ViewComponent as UICharacterCreate;

        public CharacterCreateMediator() : base(NAME)
        {
        }

        public override IList<string> ListNotificationInterests()
        {
            IList<string> list = new List<string>();
            list.Add(NotiConst.Open_CharacterCreate);
            return list;
        }

        public override void HandleNotification(INotification note)
        {
            switch (note.Name)
            {
                case NotiConst.Open_CharacterCreate:
                    OpenPanel<UICharacterCreate>();
                    break;
            }
        }
    }
}