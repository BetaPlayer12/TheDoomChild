using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories.UI
{
    public class InventoryItemUI : ItemUI
    {
        private Button m_button;

        public override void Hide()
        {
            m_button.interactable = false;
            m_detailsUI.Hide();
        }

        public override void Show()
        {
            m_button.interactable = true;
            m_detailsUI.Show();
        }

        protected override void ShowDetailsOf(IStoredItem reference)
        {
            if (reference == null)
            {
                m_detailsUI.Hide();
            }
            else
            {
                m_detailsUI.Show();
                base.ShowDetailsOf(reference);
            }
        }

        private void Awake()
        {
            m_button = GetComponent<Button>();
        }
    }
}