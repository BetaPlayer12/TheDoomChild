using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.Items;
using DChild.Gameplay.Systems.Journal;
using DChild.Gameplay.Systems.Lore;
using Doozy.Runtime.Signals;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.UI
{

    [SerializeField]
    public class FullscreenNotificationHandle : NotificationHandle<FullscreenNotificationHandle.NotificationType>, INotificationHandle
    {
        #region SubHandles
        [HideReferenceObjectPicker]
        private class UIContainerSubHandle : SubHandle
        {
            [SerializeField]
            private UIContainer m_container;

            public UIContainer container => m_container;

            public override void AddListenerToOnNotificationHidden(UnityAction action)
            {
                m_container.OnHiddenCallback.Event.AddListener(action);
            }
        }

        [HideReferenceObjectPicker]
        private class ItemNotificationSubHandle : SubHandle
        {
            [SerializeField]
            private IItemNotificationUI[] m_itemNotifications;

            public void ShowItemNotification(ItemData itemData)
            {
                for (int i = 0; i < m_itemNotifications.Length; i++)
                {
                    var notification = m_itemNotifications[i];
                    if (notification.IsNotificationFor(itemData))
                    {
                        notification.ShowNotificationFor(itemData);
                    }
                }
            }

            public override void AddListenerToOnNotificationHidden(UnityAction action)
            {
                for (int i = 0; i < m_itemNotifications.Length; i++)
                {
                    m_itemNotifications[i].AddListenerToOnNotificationHidden(action);
                }
            }
        }
        #endregion

        public enum NotificationType
        {
            PrimarySkill,
            SoulSkill,
            Journal_Detailed,
            Lore_Detailed,
            Items
        }

        [SerializeField]
        private SignalSender m_fullScreenNotifSignal;
        [SerializeField]
        private SubHandle<PrimarySkillNotificationUI> m_primarySkillNotification = new SubHandle<PrimarySkillNotificationUI>();
        [SerializeField]
        private SubHandle<SoulSkillNotificationUI> m_soulSkillNotification = new SubHandle<SoulSkillNotificationUI>();
        [SerializeField]
        private SubHandle<JournalNotificationUI> m_journalDetailedNotification = new SubHandle<JournalNotificationUI>();
        [SerializeField]
        private SubHandle<LoreInfoUI> m_loreNotification = new SubHandle<LoreInfoUI>();
        [SerializeField]
        private ItemNotificationSubHandle m_itemNotification;

        #region Prompts
        [ContextMenu("Prompt/Journal")]
        public void PromptJournalUpdateNotification(JournalData journalData)
        {
            if (HasNotificationFor(journalData) == false)
            {
                AddNotificationRequest(new NotificationRequest<NotificationType>(m_journalDetailedNotification.priority, NotificationType.Journal_Detailed, journalData));
            }
        }

        [ContextMenu("Prompt/Primary Skill")]
        public void QueueNotification(PrimarySkill primarySkillData)
        {
            if (HasNotificationFor(primarySkillData) == false)
            {
                AddNotificationRequest(new NotificationRequest<NotificationType>(m_primarySkillNotification.priority, NotificationType.PrimarySkill, primarySkillData));
            }
        }

        [ContextMenu("Prompt/Soul Skill")]
        public void QueueNotification(SoulSkill soulSkill)
        {
            if (HasNotificationFor(soulSkill) == false)
            {
                AddNotificationRequest(new NotificationRequest<NotificationType>(m_soulSkillNotification.priority, NotificationType.SoulSkill, soulSkill));
            }
            
        }

        public void QueueNotification(LoreData data)
        {
            if (HasNotificationFor(data) == false)
            {
                AddNotificationRequest(new NotificationRequest<NotificationType>(m_soulSkillNotification.priority, NotificationType.Lore_Detailed, data));
            }
            
        }

        public void QueueNotification(ItemData itemData)
        {
            AddNotificationRequest(new NotificationRequest<NotificationType>(m_soulSkillNotification.priority, NotificationType.Items, itemData));
        }
        #endregion

        protected override void InitializeSubHandles(List<SubHandle> subHandles)
        {
            subHandles.Add(m_primarySkillNotification);
            subHandles.Add(m_soulSkillNotification);
            subHandles.Add(m_journalDetailedNotification);
            subHandles.Add(m_loreNotification);
            subHandles.Add(m_itemNotification);
        }

        protected override void HandleNotification(NotificationRequest<NotificationType> notificationRequest)
        {
            m_fullScreenNotifSignal.SendSignal();
            var data = notificationRequest.referenceData;
            switch (notificationRequest.type)
            {
                case NotificationType.PrimarySkill:
                    var primarySkillNotification = m_primarySkillNotification.ui;
                    primarySkillNotification.SetNotifiedSkill((PrimarySkill)data);
                    primarySkillNotification.container.Show(true);
                    break;
                case NotificationType.SoulSkill:
                    var soulSkillNotification = m_soulSkillNotification.ui;
                    soulSkillNotification.SetNotifiedSkill((SoulSkill)data);
                    soulSkillNotification.container.Show(true);
                    break;
                case NotificationType.Journal_Detailed:
                    var journalUI = m_journalDetailedNotification.ui;
                    journalUI.UpdateUI((JournalData)data);
                    journalUI.container.Show(true);
                    break;
                case NotificationType.Lore_Detailed:
                    var loreNorification = m_loreNotification.ui;
                    loreNorification.SetInfo((LoreData)data);
                    loreNorification.Show();
                    break;
                case NotificationType.Items:
                    m_itemNotification.ShowItemNotification((ItemData)data);
                    break;
            }
        }
    }
}