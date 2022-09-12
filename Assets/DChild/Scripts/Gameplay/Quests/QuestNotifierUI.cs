using UnityEngine;
using PixelCrushers.DialogueSystem;
using TMPro;
using Doozy.Engine;

namespace DChild.Gameplay.Quests
{
    public class QuestNotifierUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_questTitle;
        [SerializeField]
        private TextMeshProUGUI m_questEntry;

        private void OnQuestStateChange(string questName, QuestState state)
        {
            if (QuestLog.IsQuestTrackingAvailable(questName) == false || state != QuestState.Active)
                return;

            m_questTitle.text = FormattedText.Parse(questName).text;
            m_questEntry.text = "";
            //ShowNotifier();
        }

        private void OnQuestEntryStateChange(QuestEntryArgs questInfo, QuestState state)
        {
            if (QuestLog.IsQuestTrackingAvailable(questInfo.questName) == false || state != QuestState.Active)
                return;

            m_questTitle.text = FormattedText.Parse(questInfo.questName).text;
            var entryName = QuestLog.GetQuestEntry(questInfo.questName, questInfo.entryNumber);
            m_questEntry.text = FormattedText.Parse(entryName).text;
            ShowNotifier();
        }

        private void ShowNotifier()
        {
            GameEventMessage.SendEvent("Quest Notify");
        }

        private void Awake()
        {
            QuestStateChangeDispatcher.QuestStateChange += OnQuestStateChange;
            QuestStateChangeDispatcher.QuestEntryStateChange += OnQuestEntryStateChange;
        }
    }
}