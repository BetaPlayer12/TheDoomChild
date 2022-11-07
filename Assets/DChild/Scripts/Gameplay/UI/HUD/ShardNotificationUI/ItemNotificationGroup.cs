using DChild.Gameplay.Items;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class ItemNotificationGroup : SerializedMonoBehaviour, IItemNotificationUI
    {
        [SerializeField]
        private UIContainer m_container;
        [SerializeField]
        private Dictionary<ItemData, UIContainer> m_itemNotificationPair;

        public bool IsNotificationFor(ItemData itemData)
        {
            return m_itemNotificationPair.ContainsKey(itemData);
        }

        public void ShowNotificationFor(ItemData itemData)
        {
            if (m_itemNotificationPair.TryGetValue(itemData, out UIContainer container))
            {
                m_container.Show();
                container.Show();
            }
        }

        public void CloseAllNotification()
        {
            foreach (var key in m_itemNotificationPair.Keys)
            {
                m_itemNotificationPair[key].InstantHide();
            }
        }
    }

}