using System;
using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class MediatorPool
    {
        private static IFacade m_facade = ApplicationFacade.instance;

        public static ApplicationFacade Facade => m_facade as ApplicationFacade;
        /*public static LoginMediator loginMediator{
            get{
                return _facade.RetrieveMediator(LoginMediator.NAME) as LoginMediator;
            }
        }*/
    }
}