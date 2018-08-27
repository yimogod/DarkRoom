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