using UnityEngine;
using PixelCrushers.DialogueSystem;

namespace DChild.Gameplay.Quests
{
    [System.Serializable]
    public class ForceQuestUpdateHandle
    {
        [SerializeField, QuestPopup(true)]
        private string m_quest;
        [SerializeField]
        private int m_questEntry;


        public void SendQuestUpdate()
        {
            QuestEntryArgs questEntryArgs = new QuestEntryArgs(m_quest, m_questEntry);
            QuestStateChangeDispatcher.SendQuestEntryStateChangeEvent(questEntryArgs);
        }
    }
}