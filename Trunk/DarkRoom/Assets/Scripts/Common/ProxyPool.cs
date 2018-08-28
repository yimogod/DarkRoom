using System;
using PureMVC.Patterns;

namespace Sword
{
    public class ProxyPool
    {
        public static UserProxy UserProxy =>
            Facade.instance.RetrieveProxy(UserProxy.NAME) as UserProxy;

        public static HeroProxy HeroProxy =>
            Facade.instance.RetrieveProxy(HeroProxy.NAME) as HeroProxy;
    }
}