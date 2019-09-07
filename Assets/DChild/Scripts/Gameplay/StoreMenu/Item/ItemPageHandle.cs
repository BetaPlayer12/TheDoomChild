using System;
using UnityEngine;

namespace DChild.Menu.Item
{
    public class ItemPageHandle : MonoBehaviour
    {
        [SerializeField]
        private ItemInfoPage m_info;
        [SerializeField]
        private ItemContainerUI m_containerUI;

        public void Select(ItemSlotUI slot)
        {
            m_info.SetInfo(slot.item);
        }

        private void OnNewPage(object sender, ItemContainerUI.SlotEventActionArgs eventArgs)
        {
            m_info.SetInfo(eventArgs.firstSlot);
        }

        private void Awake()
        {
           m_containerUI.NewPage += OnNewPage;
        }
        
    }
}
