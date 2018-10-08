using System.Collections.Generic;

using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class HeroInfoMediator : SwordBaseMediator
	{
        public new const string NAME = "HeroInfoMediator";

        public HeroInfoMediator() : base(NAME)
        {
        }

        public override IList<string> ListNotificationInterests()
        {
            IList<string> list = new List<string>();
			list.Add(NotiConst.Open_HeroInfo);
            return list;
        }

        public override void HandleNotification(INotification note)
        {
            switch (note.Name)
            {
                case NotiConst.Open_HeroInfo:
					OpenPanel<UI_HeroInfo>(ProxyPool.HeroProxy.Hero);
                    break;
            }
        }
    }
}