using DChild.Temp;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class StoreNotificationHandle : SerializedMonoBehaviour
    {
        [SerializeField]
        private UIContainer m_container;
        [SerializeField]
        private StoreNotificationUI m_ui;
        [SerializeField]
        private Dictionary<StoreNotificationType, StoreNotificationInfo> m_storeNotificationPair;

        public void ShowNotification(StoreNotificationType storeNotificationType)
        {
            if (m_storeNotificationPair.TryGetValue(storeNotificationType, out StoreNotificationInfo info))
            {
                m_ui.Show(info);
                m_container.Show(true);
            }
        }
    }
}