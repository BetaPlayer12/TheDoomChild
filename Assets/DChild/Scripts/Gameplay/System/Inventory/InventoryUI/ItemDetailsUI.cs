using UnityEngine;

namespace DChild.Gameplay.Inventories.UI
{
    public abstract class ItemDetailsUI : MonoBehaviour
    {
        public abstract void ShowDetails(IStoredItem reference);

        public abstract void Show();
        public abstract void Hide();
    }
}