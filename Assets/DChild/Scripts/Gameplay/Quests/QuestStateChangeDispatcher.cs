using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;

namespace DChild.Gameplay.Quests
{
    public class QuestStateChangeDispatcher : MonoBehaviour
    {
        public static event Action<string, QuestState> QuestStateChange;
        public static event Action<QuestEntryArgs, QuestState> QuestEntryStateChange;


        public void OnQuestStateChange(string questName)
        {
            QuestStateChange?.Invoke(questName, QuestLog.GetQuestState(questName));
        }

        public void OnQuestEntryStateChange(QuestEntryArgs questInfo)
        {
            QuestEntryStateChange?.Invoke(questInfo, QuestLog.GetQuestEntryState(questInfo.questName, questInfo.entryNumber));
        }
    }
}