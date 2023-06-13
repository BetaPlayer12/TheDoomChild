using UnityEngine;
using PixelCrushers.DialogueSystem;
using TMPro;
using DChild.Temp;
using Doozy.Runtime.UIManager.Containers;

namespace DChild.Gameplay.Quests
{
    public class QuestNotificationUI : NotificationUI
    {
        [SerializeField]
        private TextMeshProUGUI m_questTitle;
        [SerializeField]
        private TextMeshProUGUI m_questEntry;

        public void UpdateLog(QuestEntryArgs questInfo)
        {
            m_questTitle.text = FormattedText.Parse(questInfo.questName).text;
            m_questEntry.text = "";
            if (questInfo.entryNumber >= 0)
            {
                var entryName = QuestLog.GetQuestEntry(questInfo.questName, questInfo.entryNumber);
                m_questEntry.text = FormattedText.Parse(entryName).text;
            }
        }
    }
}