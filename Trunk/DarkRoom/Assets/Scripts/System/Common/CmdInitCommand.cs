using UnityEngine;

using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class CmdInitCommand : SimpleCommand
    {
        public override void Execute(INotification note)
        {
            /*Facade.RegisterCommand(CMD10001.NAME, typeof(CMD10001));
            Facade.RegisterCommand(CMD10003.NAME, typeof(CMD10003));
            Facade.RegisterCommand(CMD10005.NAME, typeof(CMD10005));

            Facade.RegisterCommand(CMD20001.NAME, typeof(CMD20001));
            Facade.RegisterCommand(CMD20003.NAME, typeof(CMD20003));
            Facade.RegisterCommand(CMD20005.NAME, typeof(CMD20005));
            Facade.RegisterCommand(CMD30001.NAME, typeof(CMD30001));
            Facade.RegisterCommand(CMD50001.NAME, typeof(CMD50001));*/
        }
    }
}

