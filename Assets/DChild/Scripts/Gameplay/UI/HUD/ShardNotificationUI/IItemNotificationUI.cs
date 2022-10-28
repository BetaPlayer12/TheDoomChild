using DChild.Gameplay.Items;

namespace DChild.Gameplay.UI
{

    public interface IItemNotificationUI
    {
        bool IsNotificationFor(ItemData itemData);
        void ShowNotificationFor(ItemData itemData);
    }

}