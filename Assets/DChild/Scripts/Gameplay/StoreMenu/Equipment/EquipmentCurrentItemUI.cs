using DChild.Gameplay.EquipmentSystem;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Equipment.UI
{
    public class EquipmentCurrentItemUI : MonoBehaviour
    {

        [BoxGroup("MAIN UI"), SerializeField] private EquipmentUI m_equipmentUI;

        [BoxGroup("ITEM PROPERTIES"), SerializeField] private Image m_itemImage;
        public Image itemImage => m_itemImage;

        [BoxGroup("ITEM PROPERTIES/Canvas Groups"), SerializeField] private CanvasGroup m_itemCG;
        [BoxGroup("ITEM PROPERTIES/Canvas Groups"), SerializeField] private CanvasGroup m_undiscoveredCG;

        [SerializeField] private SoulSlot m_soulSlot;
        public SoulSlot soulSlot => m_soulSlot;

        private SoulEquipmentItem m_currentItem;
        public SoulEquipmentItem currentItem => m_currentItem;

        public void OnGridItemSelected(object sender, EventActionArgs eventArgs)
        {
            m_equipmentUI.selectionUI.equipButtonUI.UpdateButtonLabel(this);
        }

        public bool TryEquipItem(SoulEquipmentItem equipmentItem)
        {
            if (equipmentItem == null || equipmentItem.soulEquipment.Slot != m_soulSlot)
                return false;

            m_currentItem = equipmentItem;
            m_itemImage.sprite = equipmentItem.equippedIcon;
            ToggleItemVisibility(true);

            m_equipmentUI.equipmentHandle.EquipSoulEquipment(equipmentItem);
            return true;
        }

        public bool TryRemoveItem(SoulEquipmentItem equipmentItem)
        {
            if (equipmentItem == null || equipmentItem.soulEquipment.Slot != m_soulSlot || m_currentItem != equipmentItem)
                return false;

            m_equipmentUI.equipmentHandle.UnequipSoulEquipment(equipmentItem);

            m_currentItem = null;
            m_itemImage.sprite = null;
            ToggleItemVisibility(false);
            return true;
        }

        public void Refresh()
        {
            if (m_equipmentUI.equipmentHandle.TryGetEquippedSoulEquipment(m_soulSlot, out SoulEquipmentItem equipmentItem))
            {
                m_currentItem = equipmentItem;
                m_itemImage.sprite = equipmentItem.equippedIcon;
                ToggleItemVisibility(true);
                return;
            }

            m_currentItem = null;
            m_itemImage.sprite = null;
            ToggleItemVisibility(false);
        }

        private void ToggleItemVisibility(bool value)
        {
            m_itemCG.alpha = Convert.ToSingle(value);
            m_undiscoveredCG.alpha = Convert.ToSingle(!value);
        }

    }
}
