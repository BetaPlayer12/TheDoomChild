using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Quests
{
    public abstract class QuestAdvancer : MonoBehaviour
    {
        [SerializeField, ToggleGroup("m_runLua", "Run Lua")]
        private bool m_runLua;
        [SerializeField,LuaScriptWizard, Indent,ToggleGroup("m_runLua","Run Lua")]
        private string m_script;

        [SerializeField, ToggleGroup("m_changeQuestState", "Change Quest")]
        private bool m_changeQuestState;
        [SerializeField,QuestPopup(true), Indent, ToggleGroup("m_changeQuestState", "Change Quest")]
        private string questName;
        [SerializeField,QuestState, Indent, ToggleGroup("m_changeQuestState", "Change Quest")]
        private QuestState m_questState;
        [SerializeField,Tooltip("Set state of a quest entry."), Indent, ToggleGroup("m_setQuestEntryState", "Change Quest Entry")]
        private bool m_setQuestEntryState = false;
        [SerializeField,QuestEntryPopup, Indent, ToggleGroup("m_setQuestEntryState", "Change Quest Entry")]
        private int m_questEntryNumber = 1;
        [SerializeField,QuestState, Indent, ToggleGroup("m_setQuestEntryState", "Change Quest Entry")]
        private QuestState m_questEntryState;

        protected void ExecuteLuaScript()
        {
            if (m_runLua)
            {
                Lua.Run(m_script);
            }

            if (m_changeQuestState)
            {
                if (string.IsNullOrEmpty(questName)) return;
                if (m_changeQuestState) QuestLog.SetQuestState(questName, m_questState);
                if (m_setQuestEntryState) QuestLog.SetQuestEntryState(questName, m_questEntryNumber, m_questEntryState);
            }
        }
    }
}