using System.Collections.Generic;

using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class BattleMainMediator : SwordBaseMediator
	{
        public new const string NAME = "BattleMainMediator";

        public BattleMainMediator() : base(NAME)
        {
        }

        public override IList<string> ListNotificationInterests()
        {
            IList<string> list = new List<string>();
            list.Add(NotiConst.Open_BattleMain);
            return list;
        }

        public override void HandleNotification(INotification note)
        {
            switch (note.Name)
            {
                case NotiConst.Open_BattleMain:
					OpenPanel<UI_BattleMain>();
                    break;
            }
        }
    }
}