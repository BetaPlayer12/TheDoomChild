using Doozy.Engine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class StoreNotificationHandle : SerializedMonoBehaviour
    {
        [SerializeField]
        private StoreNotificationUI m_ui;
        [SerializeField]
        private Dictionary<StoreNotificationType, StoreNotificationInfo> m_storeNotificationPair;

        public void ShowNotification(StoreNotificationType storeNotificationType)
        {
            if(m_storeNotificationPair.TryGetValue(storeNotificationType, out StoreNotificationInfo info))
            {
                m_ui.Show(info);
                GameEventMessage.SendEvent("Show Store Notification");
            }
        }
    }
}