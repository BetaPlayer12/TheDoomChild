using DChild.Gameplay.Characters.Players;

namespace DChild.Gameplay.Items
{
    public interface IUsableItemModule
    {
        bool CanBeUse(IPlayer player);
        void Use(IPlayer player);
    }
}
