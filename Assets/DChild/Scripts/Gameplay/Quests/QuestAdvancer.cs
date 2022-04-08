using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Quests
{
    public abstract class QuestAdvancer : MonoBehaviour
    {
        [SerializeField]
        private bool m_runLua;
        [SerializeField,LuaScriptWizard, Indent,ShowIf("m_runLua")]
        private string m_script;

        [SerializeField]
        private bool m_changeQuestState;
        [SerializeField,QuestPopup(true), Indent, ShowIf("m_changeQuestState")]
        private string m_questName;
        [SerializeField,QuestState, Indent, ShowIf("m_changeQuestState")]
        private QuestState m_questState;
        [SerializeField,Tooltip("Set state of a quest entry."), Indent, ShowIf("m_changeQuestState")]
        private bool m_setQuestEntryState = false;
        [SerializeField,QuestEntryPopup, Indent, ShowIf("m_setQuestEntryState")]
        private int m_questEntryNumber = 1;
        [SerializeField,QuestState, Indent, ShowIf("m_setQuestEntryState")]
        private QuestState m_questEntryState;

        protected void ExecuteLuaScript()
        {
            if (m_runLua)
            {
                Lua.Run(m_script);
            }

            if (m_changeQuestState)
            {
                if (string.IsNullOrEmpty(m_questName)) return;
                if (m_changeQuestState) QuestLog.SetQuestState(m_questName, m_questState);
                if (m_setQuestEntryState) QuestLog.SetQuestEntryState(m_questName, m_questEntryNumber, m_questEntryState);
            }
        }
    }
}