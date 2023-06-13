using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace DChild.Gameplay.Quests
{
    public class QuestNotifier : MonoBehaviour
    {
        private bool IsQuestBeingLogged(string questName, QuestState state) => QuestLog.IsQuestTrackingAvailable(questName) == true || state == QuestState.Active;
        private void OnQuestStateChange(string questName, QuestState state)
        {
            if (IsQuestBeingLogged(questName, state) == false)
                return;

            var questInfo = new QuestEntryArgs(questName, -1);
            ShowNotification(questInfo);
        }

        private void OnQuestEntryStateChange(QuestEntryArgs questInfo, QuestState state)
        {
            if (IsQuestBeingLogged(questInfo.questName, state) == false)
                return;
            ShowNotification(questInfo);
        }

        private void ShowNotification(QuestEntryArgs questInfo)
        {
            GameplaySystem.gamplayUIHandle.notificationManager.QueueNotification(questInfo);
        }

        protected void Awake()
        {
            QuestStateChangeDispatcher.QuestStateChange += OnQuestStateChange;
            QuestStateChangeDispatcher.QuestEntryStateChange += OnQuestEntryStateChange;
        }
    }
}