using DChild.Gameplay.Items;
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
        [SerializeField]
        private UsableItemMenuHandle m_usableItemMenuHandle;

        private static ItemData m_cacheItemData;

        public void Select(ItemSlotUI slot)
        {
            m_cacheItemData = slot.item;
            m_info.SetInfo(m_cacheItemData);
            m_usableItemMenuHandle.SetItem((UsableItemData)m_cacheItemData);
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
