using DChild.Gameplay.Characters.Players;

namespace DChild.Gameplay.Items
{
    public interface IDurationItemEffect
    {
        void StartEffect(IPlayer player);
        void StopEffect(IPlayer player);
    }
}
