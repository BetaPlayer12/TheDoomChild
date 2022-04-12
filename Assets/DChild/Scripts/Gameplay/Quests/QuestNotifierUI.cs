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

        public void OnQuestStateChange(string questName)
        {
            if (QuestLog.IsQuestTrackingAvailable(questName) == false || QuestLog.GetQuestState(questName) != QuestState.Active)
                return;

            m_questTitle.text = FormattedText.Parse(questName).text;
            m_questEntry.text = "";
            ShowNotifier();
        }

        public void OnQuestEntryStateChange(QuestEntryArgs questInfo)
        {
            if (QuestLog.IsQuestTrackingAvailable(questInfo.questName) == false || QuestLog.GetQuestEntryState(questInfo.questName, questInfo.entryNumber) != QuestState.Active)
                return;

            m_questTitle.text = FormattedText.Parse(questInfo.questName).text;
            var entryName = QuestLog.GetQuestEntry(questInfo.questName, questInfo.entryNumber);
            m_questEntry.text = FormattedText.Parse(entryName).text;
            ShowNotifier();
        }

        public void ShowNotifier()
        {
            GameEventMessage.SendEvent("Quest Notify");
        }
    }
}