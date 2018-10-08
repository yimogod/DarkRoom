using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class CharacterCreateMediator : SwordBaseMediator
    {
        public new const string NAME = "CharacterCreateMediator";

        private UI_CharacterCreate m_view => ViewComponent as UI_CharacterCreate;

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
                    OpenPanel<UI_CharacterCreate>();
                    break;
            }
        }
    }
}