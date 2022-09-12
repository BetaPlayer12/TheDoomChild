using DChild.Gameplay.Inventories;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.UI
{

    public class ItemNotification : SerializedMonoBehaviour
    {
        private List<string> m_messages;
        [SerializeField]
        private CountdownTimer m_notificationTimer;

        [SerializeField]
        private TextMeshProUGUI m_notification;
        //[SerializeField]
        //private IItemContainer[] m_toListen;


        public void ShowNextNotif()
        {
            if (m_messages.Count > 0)
            {
                m_notification.text = m_messages[0];
                GameplaySystem.gamplayUIHandle.ShowItemAcquired(true);
            }
        }

        private void ItemUpdate(object sender, ItemEventArgs eventArgs)
        {
            var action = eventArgs.countModification < 0 ? "Lost" : "Acquired";
            var message = $"{Mathf.Abs(eventArgs.countModification)} {eventArgs.data.itemName} {action}";
            if (m_messages.Count == 0)
            {
                m_notificationTimer.Reset();
                m_notification.text = message;
                GameplaySystem.gamplayUIHandle.ShowItemAcquired(true);
            }
            m_messages.Add(message);
            enabled = true;
        }


        private void OnNotifEnd(object sender, EventActionArgs eventArgs)
        {
            m_messages.RemoveAt(0);
            GameplaySystem.gamplayUIHandle.ShowItemAcquired(false);
        }

        private void Awake()
        {
            m_messages = new List<string>();
            m_notificationTimer.CountdownEnd += OnNotifEnd;
            //for (int i = 0; i < m_toListen.Length; i++)
            //{
            //    m_toListen[i].ItemUpdate += ItemUpdate;
            //}
            enabled = false;
        }

        private void LateUpdate()
        {
            m_notificationTimer.Tick(Time.unscaledDeltaTime);
        }
    }
}