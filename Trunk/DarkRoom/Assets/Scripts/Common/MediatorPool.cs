using System;
using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class MediatorPool
    {
        public static CharacterEntryMediator CharacterEntryMediator =>
                Facade.instance.RetrieveMediator(CharacterEntryMediator.NAME) as CharacterEntryMediator;
    }
}