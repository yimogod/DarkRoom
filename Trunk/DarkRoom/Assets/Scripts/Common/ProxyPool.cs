using System;

namespace Sword
{
    public class ProxyPool
    {
        private static ApplicationFacade m_facade = ApplicationFacade.instance as ApplicationFacade;

        public static UserProxy UserProxy =>
            m_facade.RetrieveProxy(UserProxy.NAME) as UserProxy;
    }
}