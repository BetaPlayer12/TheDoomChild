using DChild.Gameplay;
using DChild.Gameplay.Environment;
using DChild.Gameplay.EquipmentSystem;
using DChild.Gameplay.UI;
using Holysoft.Event;
using TMPro;
using UnityEngine;

namespace DChild.Menu.Equipment.UI
{
    public class EquipmentEquipButtonUI : MonoBehaviour
    {
        [SerializeField] private SetTextToTextBox m_labelText;

        private SoulEquipmentItem m_selectedItem;
        private SoulEquipmentItem m_currentEquipped;
        private EquipmentCurrentItemUI m_currentSlot;

        public event EventAction<ItemEquipEventArgs> OnItemEquipped;
        public event EventAction<ItemEquipEventArgs> OnItemRemoved;

        private enum EquipButtonLabel
        {
            Equip,
            Replace,
            Remove
        }

        private void SetLabel(EquipButtonLabel label)
        {
            m_labelText.SetText($"BUTTONPROMPT{label}");
        }

        public void UpdateButtonLabel(EquipmentCurrentItemUI itemSlot)
        {
            m_currentSlot = itemSlot;
            m_currentEquipped = itemSlot.currentItem;

            var label = m_currentEquipped == null
                ? EquipButtonLabel.Equip
                : m_currentEquipped != m_selectedItem
                    ? EquipButtonLabel.Replace
                    : EquipButtonLabel.Remove;

            SetLabel(label);
        }

        public void SetSelectedItem(SoulEquipmentItem item) => m_selectedItem = item;

        public void ClearSelection()
        {
            m_selectedItem = null;
            m_currentEquipped = null;
            m_currentSlot = null;
            SetLabel(EquipButtonLabel.Equip);
        }

        public void EquipItem()
        {
            if (m_selectedItem == null || m_currentSlot == null)
                return;

            if (m_currentEquipped == m_selectedItem)
            {
                var removedItem = m_currentEquipped;
                if (!m_currentSlot.TryRemoveItem(removedItem))
                    return;

                OnItemRemoved?.Invoke(this, new ItemEquipEventArgs(removedItem));
                m_currentEquipped = null;
                SetLabel(EquipButtonLabel.Equip);
                return;
            }

            var replacedItem = m_currentEquipped;
            if (!m_currentSlot.TryEquipItem(m_selectedItem))
                return;

            if (replacedItem != null)
                OnItemRemoved?.Invoke(this, new ItemEquipEventArgs(replacedItem));

            OnItemEquipped?.Invoke(this, new ItemEquipEventArgs(m_selectedItem));
            m_currentEquipped = m_selectedItem;
            SetLabel(EquipButtonLabel.Remove);
        }

    }
}
