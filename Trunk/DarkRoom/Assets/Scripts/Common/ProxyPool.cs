using System;

namespace Sword
{
    public class ProxyPool
    {
        private static ApplicationFacade m_facade = ApplicationFacade.instance;

        public static ApplicationFacade Facade
        {
            get { return m_facade; }
        }

        public static UserProxy UserProxy
        {
            get { return m_facade.RetrieveProxy(UserProxy.NAME) as UserProxy; }
        }
    }
}