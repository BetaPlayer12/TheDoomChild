using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Quests
{
    public abstract class MultiQuestAdvancer
    {
        //[SerializeField, ToggleGroup("m_runLua", "Run Lua")]
        //private bool m_runLua;
        //[SerializeField, LuaScriptWizard, Indent, ToggleGroup("m_runLua", "Run Lua")]
        //private string m_script;

        protected List<bool> m_changeQuestState;
        protected string[] questName;
        protected QuestState[] m_questState;
        protected bool[] m_setQuestEntryState;
        protected int[] m_questEntryNumber;
        protected QuestState[] m_questEntryState;

        //private void Start()
        //{
        //    for (int i = 0; i < m_setQuestEntryState.Length; i++)
        //    {
        //        m_setQuestEntryState[i] = false;
        //    }
        //    for (int i = 0; i < m_questEntryNumber.Length; i++)
        //    {
        //        m_questEntryNumber[i] = 1;
        //    }
        //}

        //private void Awake()
        //{
        //    questName = new string[m_changeQuestState.Count];
        //    m_questState = new QuestState[m_changeQuestState.Count];
        //    m_setQuestEntryState = new bool[m_changeQuestState.Count];
        //    m_questEntryNumber = new int[m_changeQuestState.Count];
        //    m_questEntryState = new QuestState[m_changeQuestState.Count];
        //}

        //protected void ExecuteLuaScript()
        //{
        //    if (m_runLua)
        //    {
        //        Lua.Run(m_script);
        //    }

        //    for (int i = 0; i < m_changeQuestState.Count; i++)
        //    {
        //        if (m_changeQuestState[i])
        //        {
        //            if (string.IsNullOrEmpty(questName[i])) return;
        //            if (m_changeQuestState[i]) QuestLog.SetQuestState(questName[i], m_questState[i]);
        //            if (m_setQuestEntryState[i]) QuestLog.SetQuestEntryState(questName[i], m_questEntryNumber[i], m_questEntryState[i]);
        //        }
        //    }
        //}
    }
}
