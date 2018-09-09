using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class StartupCommand : MacroCommand
    {
        public const string NAME = "StartupCommand";

        public override void Execute(INotification note)
        {
            AddSubCommand(typeof(CmdInitCommand));
            AddSubCommand(typeof(ProxyInitCommand));
            AddSubCommand(typeof(MediatorInitCommand));
            AddSubCommand(typeof(SystemInitCommand));
            base.Execute(note);
        }
    }
}