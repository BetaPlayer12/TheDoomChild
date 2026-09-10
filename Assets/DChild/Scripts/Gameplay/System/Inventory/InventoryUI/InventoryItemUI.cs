using DChild.Gameplay.Items;
using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories.UI
{
    public class InventoryItemUI : ItemUI
    {
        private UIToggle m_toggle;

        [SerializeField] private Image m_backgroundFrame;
        [SerializeField] private bool m_isQuickItem;
        public bool isQuickItem => m_isQuickItem;

        public override void Hide()
        {
            m_toggle.SetIsOn(false, true, false);

            if (!m_isQuickItem)
                m_toggle.interactable = false;
        }

        public override void SetIconColor(bool isModified)
        {
            m_detailsUI.AdjustIconColor(isModified);
        }

        public override void SetItemFrame(Sprite value)
        {
            m_backgroundFrame.sprite = value;
        }

        public override void Show()
        {
            m_toggle.interactable = true;
        }

        protected override void ShowDetailsOf(IStoredItem reference)
        {
            if (reference == null || reference.data.category == ItemCategory.SoulEssence)
            {
                Hide();
                base.ShowDetailsOf(null);
                return;
            }

            Show();
            base.ShowDetailsOf(reference);
        }

        private void OnEnable()
        {
            m_toggle = GetComponent<UIToggle>();
        }
    }
}
