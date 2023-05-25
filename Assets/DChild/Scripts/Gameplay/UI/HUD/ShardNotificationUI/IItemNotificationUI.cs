using DChild.Gameplay.Items;
using UnityEngine.Events;

namespace DChild.Gameplay.UI
{

    public interface IItemNotificationUI
    {
        bool IsNotificationFor(ItemData itemData);
        void ShowNotificationFor(ItemData itemData);

        void AddListenerToOnNotificationHidden(UnityAction action);
    }

}