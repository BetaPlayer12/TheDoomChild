using DChild.Gameplay.Items;
using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class ItemNotificationGroup : SerializedMonoBehaviour, IItemNotificationUI
    {
        [SerializeField]
        private Dictionary<ItemData, UIView> m_itemNotificationPair;

        public bool IsNotificationFor(ItemData itemData)
        {
            return m_itemNotificationPair.ContainsKey(itemData);
        }

        public void ShowNotificationFor(ItemData itemData)
        {
            if (m_itemNotificationPair.TryGetValue(itemData, out UIView container))
            {
                container.InstantShow();
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