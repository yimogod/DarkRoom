using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class ProxyInitCommand : SimpleCommand
    {
        public override void Execute(INotification note)
        {
            Facade.RegisterProxy(new UserProxy());
        }
    }
}

