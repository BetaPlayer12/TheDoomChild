using DChild.Gameplay.Characters.Players;

namespace DChild.Gameplay.Characters.Players
{
    public interface IModifiableModule
    {
        void ConnectTo(IPlayerModifer modifier);
    }
}