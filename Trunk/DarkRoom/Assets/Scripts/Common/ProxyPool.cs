using System;

namespace Sword
{
    public class ProxyPool
    {
        private static ApplicationFacade m_facade = ApplicationFacade.instance;

        public static ApplicationFacade Facade => m_facade;

        public static UserProxy UserProxy =>
            m_facade.RetrieveProxy(UserProxy.NAME) as UserProxy;
    }
}