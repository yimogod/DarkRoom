using System.Collections.Generic;

using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class CharacterChooseMediator : Mediator
    {
        public new const string NAME = "CharacterChooseMediator";

        public CharacterChooseMediator() : base(NAME)
        {
        }

        public override IList<string> ListNotificationInterests()
        {
            IList<string> list = new List<string>();
            list.Add(NotiConst.Open_CharacterChoose);
            return list;
        }

        public override void HandleNotification(INotification note)
        {
            switch (note.Name)
            {
                case NotiConst.Open_CharacterChoose:
                    break;
            }
        }
    }
}