using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.Items;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Systems.Journal;
using DChild.Gameplay.Systems.Lore;
using DChild.UI;
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

        private List<INotificationHandle> m_fullscreennotificationHandles;
        private List<INotificationHandle> m_promptnotificationHandles;
        private bool m_hasActiveFullNotification;
        private bool m_hasActivePromptNotification;
        private Coroutine m_ongoingFullNotificationRoutine;
        private Coroutine m_ongoingPromptNotificationRoutine;

        #region Prompts
        [Button, FoldoutGroup("Options"), HideInEditorMode]
        public void ShowJournalUpdateNotification(JournalData journalData)
        {
            m_fullscreenNotificationHandle.PromptJournalUpdateNotification(journalData);
            EnableFullNotificationRoutine();
        }

        [Button("Primary Skill Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(PrimarySkill primarySkillData)
        {
            m_fullscreenNotificationHandle.QueueNotification(primarySkillData);
            EnableFullNotificationRoutine();
        }

        [Button("Soul Skill Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(SoulSkill soulSkill)
        {
            m_fullscreenNotificationHandle.QueueNotification(soulSkill);
            EnableFullNotificationRoutine();
        }

        [Button("Lore Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(LoreData data)
        {
            m_fullscreenNotificationHandle.QueueNotification(data);
            EnableFullNotificationRoutine();
        }

        [Button("Item Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(ItemData itemData)
        {
            m_fullscreenNotificationHandle.QueueNotification(itemData);
            EnableFullNotificationRoutine();
        }

        [Button("Quest Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(QuestEntryArgs questInfo)
        {
            m_promptNotificationHandle.QueueNotification(questInfo);
            EnablePromptNotificationRoutine();
        }

        [Button("Loot Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(LootList lootList)
        {
            m_promptNotificationHandle.QueueNotification(lootList);
            EnablePromptNotificationRoutine();
        }

        [Button("Store Notification"), FoldoutGroup("Options"), HideInEditorMode]
        public void QueueNotification(StoreNotificationType notificationType)
        {
            m_promptNotificationHandle.QueueNotification(notificationType);
            EnablePromptNotificationRoutine();
        }
        #endregion

        public void RemoveAllQueuedNotifications()
        {
            for (int i = 0; i < m_fullscreennotificationHandles.Count; i++)
            {
                m_fullscreennotificationHandles[i].RemoveAllQueuedNotifications();
            }
            for (int i = 0; i < m_promptnotificationHandles.Count; i++)
            {
                m_promptnotificationHandles[i].RemoveAllQueuedNotifications();
            }
        }
        
        public void InitializeFullPriorityHandling()
        {
            if (m_fullscreennotificationHandles == null)
            {
                InitializeFullNotificationHandles();
            }

            m_fullscreennotificationHandles.Sort(SortPriorityComparison);
        }
        public void InitializePromptPriorityHandling()
        {
            if (m_promptnotificationHandles == null)
            {
                InitializePromptNotificationHandles();
            }

            m_promptnotificationHandles.Sort(SortPriorityComparison);
        }
        [ContextMenu("Close Current Notification")]
        public void CloseCurrentFullNotification()
        {
            m_hasActiveFullNotification = false;
        }
        [ContextMenu("Close Current Notification")]
        public void CloseCurrentPromptNotification()
        {
            m_hasActivePromptNotification = false;
        }
        private void InitializeFullNotificationHandles()
        {
            m_fullscreennotificationHandles = new List<INotificationHandle>();
            m_fullscreennotificationHandles.Add(m_fullscreenNotificationHandle);

            m_fullscreenNotificationHandle.Initialize();
            m_fullscreenNotificationHandle.AddListenerToOnNotificationHidden(CloseCurrentFullNotification); 
        }
        private void InitializePromptNotificationHandles()
        {
            m_promptnotificationHandles = new List<INotificationHandle>();
            m_promptnotificationHandles.Add(m_promptNotificationHandle);

            m_promptNotificationHandle.Initialize();
            m_promptNotificationHandle.AddListenerToOnNotificationHidden(CloseCurrentPromptNotification);
        }
        private int SortPriorityComparison(INotificationHandle x, INotificationHandle y)
        {
            if (x.priority == y.priority)
                return 0;
            return x.priority < y.priority ? 1 : -1;
        }

        private void EnableFullNotificationRoutine()
        {
            if (m_ongoingFullNotificationRoutine == null)
            {
                m_ongoingFullNotificationRoutine = StartCoroutine(NotificationFullRoutine());
            }
        }
        private void EnablePromptNotificationRoutine()
        {
            if (m_ongoingPromptNotificationRoutine == null)
            {
                m_ongoingPromptNotificationRoutine = StartCoroutine(NotificationPromptRoutine());
            }
        }
        private IEnumerator NotificationFullRoutine()
        {
            var waitTime = new WaitForSecondsRealtime(m_notificationInterval);
            var endOfFrame = new WaitForEndOfFrame();

            do
            {
                while (DialogueManager.isConversationActive && !DChildStandardDialogueUI.currentConverstionIsABanter)
                    yield return endOfFrame;

                yield return endOfFrame;
                HandleNextFullNotification();
                do
                {
                    yield return endOfFrame;
                } while (m_hasActiveFullNotification);
                yield return waitTime;

            } while (HasFullNotificationsLeft());

            m_ongoingFullNotificationRoutine = null;
        }
        private IEnumerator NotificationPromptRoutine()
        {
            var waitTime = new WaitForSecondsRealtime(m_notificationInterval);
            var endOfFrame = new WaitForEndOfFrame();

            do
            {
                while (DialogueManager.isConversationActive)
                    yield return endOfFrame;

                yield return endOfFrame;
                HandleNextPromptNotification();
                do
                {
                    yield return endOfFrame;
                } while (m_hasActivePromptNotification);
                yield return waitTime;

            } while (HasPromptNotificationsLeft());

            m_ongoingPromptNotificationRoutine = null;
        }
        private bool HasFullNotificationsLeft()
        {
            for (int i = 0; i < m_fullscreennotificationHandles.Count; i++)
            {
                if (m_fullscreennotificationHandles[i].HasNotifications())
                {
                    return true;
                }
            }

            return false;
        }

        private void HandleNextFullNotification()
        {
            for (int i = 0; i < m_fullscreennotificationHandles.Count; i++)
            {
                if (m_fullscreennotificationHandles[i].HasNotifications())
                {
                    m_hasActiveFullNotification = true;
                    m_fullscreennotificationHandles[i].HandleNextNotification();
                    break;
                }
            }
        }
        private bool HasPromptNotificationsLeft()
        {
            for (int i = 0; i < m_promptnotificationHandles.Count; i++)
            {
                if (m_promptnotificationHandles[i].HasNotifications())
                {
                    return true;
                }
            }

            return false;
        }

        private void HandleNextPromptNotification()
        {
            for (int i = 0; i < m_promptnotificationHandles.Count; i++)
            {
                if (m_promptnotificationHandles[i].HasNotifications())
                {
                    m_hasActivePromptNotification = true;
                    m_promptnotificationHandles[i].HandleNextNotification();
                    break;
                }
            }
        }

    }
}