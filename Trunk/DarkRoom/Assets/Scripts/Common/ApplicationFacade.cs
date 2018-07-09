using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class ApplicationFacade : Facade
    {
        public const string STARTUP = "startup";

        protected ApplicationFacade()
        {
        }

        public new static ApplicationFacade instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_staticSyncRoot)
                    {
                        if (m_instance == null) m_instance = new ApplicationFacade();
                    }
                }

                return m_instance as ApplicationFacade;
            }
        }

        protected override void initializeController()
        {
            base.initializeController();
            RegisterCommand(STARTUP, typeof(StartupCommand));
        }

        public void Startup()
        {
            SendNotification(STARTUP);
        }
    }
}