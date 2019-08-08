using DChild.Gameplay.Characters.Players;

namespace Refactor.DChild.Gameplay.Characters.Players
{
    public interface IModifiableModule
    {
        void ConnectTo(IPlayerModifer modifier);
    }
}