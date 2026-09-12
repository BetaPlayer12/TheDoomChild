using DChild.Gameplay.EquipmentSystem;
using Holysoft.Event;
using TMPro;
using UnityEngine;
using Doozy.Runtime.UIManager.Components;

namespace DChild.Menu.Equipment.UI
{
    public class EquipmentCategoryToggleUI : MonoBehaviour
    {
        [SerializeField] private EquipmentSelectionUI m_selectionUI;
        [SerializeField] private SoulSlot m_category;
        public SoulSlot category => m_category;
        [SerializeField] private TextMeshProUGUI m_categoryHeader;

        [SerializeField] private EquipmentCurrentItemUI m_activeSlot;

        private UIToggle m_toggle;
        public UIToggle toggle => m_toggle ??= GetComponent<UIToggle>();

        public void UpdateItemGrid()
        {
            DisplayItemGrid(true);
        }

        public void InitializeItemGrid()
        {
            DisplayItemGrid(false);
        }

        public void ResetSelection()
        {
            toggle.SetIsOn(false, false, false);
        }

        public void Select()
        {
            toggle.Select();
        }

        public void RefreshCurrentItem()
        {
            m_activeSlot.Refresh();
        }

        private void DisplayItemGrid(bool selectFirstItem)
        {
            m_categoryHeader.text = m_category.ToString();

            m_selectionUI.SetFilter(m_category);
            m_selectionUI.UpdateItems(m_activeSlot, selectFirstItem);
        }

        private void Awake()
        {
            m_toggle = GetComponent<UIToggle>();
        }
    }
}
