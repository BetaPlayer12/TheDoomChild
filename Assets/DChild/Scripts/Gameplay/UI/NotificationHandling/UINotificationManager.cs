using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.Items;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Systems.Lore;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.UI
{
    public class UINotificationManager : SerializedMonoBehaviour, IUINotificationManager
    {
        [SerializeField, MinValue(0.01f)]
        private float m_notificationInterval = 0.01f;
        [SerializeField]
        private FullscreenNotificationHandle m_fullscreenNotificationHandle;
        [SerializeField]
        private PromptNotificationHandle m_promptNotificationHandle;

        private List<INotificationHandle> m_notificationHandles;
        private bool m_hasActiveNotification;
        private Coroutine m_ongoingNotificationRoutine;

        #region Prompts
        [Button, FoldoutGroup("Options"), HideInEditorMode]
        public void ShowJournalUpdateNotification()
        {
            m_fullscreenNotificationHandle.PromptJournalUpdateNotification();
            EnableNotificationRoutine();
        }

        [Button("Primary Skill Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(PrimarySkill primarySkillData)
        {
            m_fullscreenNotificationHandle.QueueNotification(primarySkillData);
            EnableNotificationRoutine();
        }

        [Button("Soul Skill Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(SoulSkill soulSkill)
        {
            m_fullscreenNotificationHandle.QueueNotification(soulSkill);
            EnableNotificationRoutine();
        }

        [Button("Lore Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(LoreData data)
        {
            m_fullscreenNotificationHandle.QueueNotification(data);
            EnableNotificationRoutine();
        }

        [Button("Item Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(ItemData itemData)
        {
            m_fullscreenNotificationHandle.QueueNotification(itemData);
            EnableNotificationRoutine();
        }

        [Button("Quest Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(QuestEntryArgs questInfo)
        {
            m_promptNotificationHandle.QueueNotification(questInfo);
            EnableNotificationRoutine();
        }

        [Button("Loot Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(LootList lootList)
        {
            m_promptNotificationHandle.QueueNotification(lootList);
            EnableNotificationRoutine();
        }

        [Button("Store Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(StoreNotificationType notificationType)
        {
            m_promptNotificationHandle.QueueNotification(notificationType);
            EnableNotificationRoutine();
        }
        #endregion

        public void RemoveAllQueuedNotifications()
        {
            for (int i = 0; i < m_notificationHandles.Count; i++)
            {
                m_notificationHandles[i].RemoveAllQueuedNotifications();
            }
        }

        public void InitializePriorityHandling()
        {
            if (m_notificationHandles == null)
            {
                InitializeNotificationHandles();
            }

            m_notificationHandles.Sort(SortPriorityComparison);
        }

        [ContextMenu("Close Current Notification")]
        public void CloseCurrentNotification()
        {
            m_hasActiveNotification = false;
        }

        private void InitializeNotificationHandles()
        {
            m_notificationHandles = new List<INotificationHandle>();
            m_notificationHandles.Add(m_fullscreenNotificationHandle);
            m_notificationHandles.Add(m_promptNotificationHandle);

            m_fullscreenNotificationHandle.Initialize();
            m_fullscreenNotificationHandle.AddListenerToOnNotificationHidden(CloseCurrentNotification);

            m_promptNotificationHandle.Initialize();
            m_promptNotificationHandle.AddListenerToOnNotificationHidden(CloseCurrentNotification);
        }

        private int SortPriorityComparison(INotificationHandle x, INotificationHandle y)
        {
            if (x.priority == y.priority)
                return 0;
            return x.priority < y.priority ? 1 : -1;
        }

        private void EnableNotificationRoutine()
        {
            if (m_ongoingNotificationRoutine == null)
            {
                m_ongoingNotificationRoutine = StartCoroutine(NotificationRoutine());
            }
        }

        private IEnumerator NotificationRoutine()
        {
            var waitTime = new WaitForSecondsRealtime(m_notificationInterval);
            var endOfFrame = new WaitForEndOfFrame();

            do
            {
                while (DialogueManager.isConversationActive)
                    yield return endOfFrame;

                yield return endOfFrame;
                HandleNextNotification();
                do
                {
                    yield return endOfFrame;
                } while (m_hasActiveNotification);
                yield return waitTime;

            } while (HasNotificationsLeft());

            m_ongoingNotificationRoutine = null;
        }

        private bool HasNotificationsLeft()
        {
            for (int i = 0; i < m_notificationHandles.Count; i++)
            {
                if (m_notificationHandles[i].HasNotifications())
                {
                    return true;
                }
            }

            return false;
        }

        private void HandleNextNotification()
        {
            for (int i = 0; i < m_notificationHandles.Count; i++)
            {
                if (m_notificationHandles[i].HasNotifications())
                {
                    m_hasActiveNotification = true;
                    m_notificationHandles[i].HandleNextNotification();
                    break;
                }
            }
        }


    }
}