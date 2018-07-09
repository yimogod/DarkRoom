using System;

namespace Sword
{
    public class MediatorPool
    {
        private static ApplicationFacade m_facade = ApplicationFacade.instance;

        public static ApplicationFacade Facade
        {
            get { return m_facade; }
        }

        /*public static LoginMediator loginMediator{
            get{
                return _facade.RetrieveMediator(LoginMediator.NAME) as LoginMediator;
            }
        }*/
    }
}