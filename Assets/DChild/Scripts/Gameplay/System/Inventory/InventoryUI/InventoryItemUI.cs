using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories.UI
{
    public class InventoryItemUI : ItemUI
    {
        private UIToggle m_button;

        public override void Hide()
        {
            m_button.SetIsOn(false);
            m_button.interactable = false;
        }

        public override void Show()
        {
            m_button.interactable = true;
        }

        protected override void ShowDetailsOf(IStoredItem reference)
        {
            if (reference == null)
            {
                Hide();
            }
            else
            {
                Show();
                base.ShowDetailsOf(reference);
            }
        }

        private void Awake()
        {
            m_button = GetComponent<UIToggle>();
        }
    }
}