using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace Sword
{
    public class MediatorInitCommand : SimpleCommand
    {
        public override void Execute(INotification note)
        {
            Facade.RegisterMediator(new CharacterEntryMediator());
            Facade.RegisterMediator(new CharacterCreateMediator());
            Facade.RegisterMediator(new CharacterChooseMediator());

            /*Facade.RegisterMediator(new LoginMediator());
            Facade.RegisterMediator(new CreateHeroMediator());
            Facade.RegisterMediator(new ItemMediator());
            Facade.RegisterMediator(new GamePlayMediator());
            Facade.RegisterMediator(new HeroMediator());
            Facade.RegisterMediator(new InventoryMediator());
            Facade.RegisterMediator(new PortalMediator());

            Facade.RegisterMediator(new AlchemyMediator());*/
        }
    }
}


