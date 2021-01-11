using DChild.Gameplay.Items;
using Holysoft.Event;
using Holysoft.UI;
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
        private SingleFocusHandler m_highlighter;
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

        private void OnItemUsedConsumed(object sender, EventActionArgs eventArgs)
        {
            m_info.SetInfo(null);
            m_highlighter.DontFocusOnAnything();
            m_containerUI.UpdateUI();
        }

        private void Awake()
        {
            m_containerUI.NewPage += OnNewPage;
            m_usableItemMenuHandle.AllItemCountConsumed += OnItemUsedConsumed;
        }


    }
}
