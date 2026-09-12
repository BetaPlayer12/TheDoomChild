using DChild.Gameplay.EquipmentSystem;
using Doozy.Runtime.UIManager.Components;
using Holysoft.Event;
using NSubstitute.Exceptions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Equipment.UI
{
    public class EquipmentGridItemUI : MonoBehaviour
    {
        [SerializeField] private EquipmentSelectionUI m_selectionUI;
        [SerializeField] private Image m_itemIcon;
        [SerializeField] private TextMeshProUGUI m_questionMark;
        [SerializeField] private Image m_equippedIcon;

        private UIToggle m_toggle;

        private SoulEquipmentItem m_attachedItem;
        public event EventAction<EventActionArgs> OnGridItemSelected;

        private void SetGridItemUIState(bool hasItem)
        {
            m_questionMark.gameObject.SetActive(!hasItem);
            m_itemIcon.gameObject.SetActive(hasItem);

            m_toggle.interactable = hasItem;
        }

        private void OnItemEquipped(object sender, ItemEquipEventArgs eventArgs) => m_equippedIcon.gameObject.SetActive(eventArgs.equipmentItem == m_attachedItem);
        private void OnItemRemoved(object sender, ItemEquipEventArgs eventArgs)
        {
            if (eventArgs.equipmentItem == m_attachedItem)
                m_equippedIcon.gameObject.SetActive(false);
        }

        public void Display(SoulEquipmentItem item = null)
        {
            m_attachedItem = item;
            bool hasItem = item != null;

            SetGridItemUIState(hasItem);
            m_equippedIcon.gameObject.SetActive(false);

            if (hasItem)
                m_itemIcon.sprite = item.slotIcon;
        }

        [Button]
        public void GetEquippedStatus(EquipmentCurrentItemUI currentItem)
        {
            m_equippedIcon.gameObject.SetActive(currentItem.currentItem == m_attachedItem);
        }

        public void PrepareAttachedItem()
        {
            if (m_attachedItem == null)
                return;
            m_selectionUI.equipButtonUI.SetSelectedItem(m_attachedItem);
            m_selectionUI.SetItemDetails(m_attachedItem);
            OnGridItemSelected?.Invoke(this, EventActionArgs.Empty);
        }

        public void Select() => m_toggle.Select();

        public void ResetSelection() => m_toggle.SetIsOn(false, false, false);

        private void Awake()
        {
            m_toggle = GetComponent<UIToggle>();
        }

        private void OnEnable()
        {
            m_selectionUI.equipButtonUI.OnItemEquipped += OnItemEquipped;
            m_selectionUI.equipButtonUI.OnItemRemoved += OnItemRemoved;
        }


        private void OnDisable()
        {
            m_selectionUI.equipButtonUI.OnItemEquipped -= OnItemEquipped;
            m_selectionUI.equipButtonUI.OnItemRemoved -= OnItemRemoved;

        }
    }

}
