using DChild.Gameplay.Quests;
using Holysoft.Event;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Narrative
{
    public class DialogueVariableListener : MonoBehaviour
    {
        [System.Serializable]
        private class Reaction
        {
            [Flags]
            private enum Frequency
            {
                OnUpdate = 1 << 0,
                OnStart = 1 << 1
            }

            [SerializeField]
            private Frequency m_frequency;
#if UNITY_EDITOR
            private bool m_lastCheck;
            [InfoBox("@\"Last Check:\" + m_lastCheck", InfoMessageType = InfoMessageType.None)]
#endif 
            [SerializeField, LuaScriptWizard(true), FoldoutGroup("Condition")]
            private string m_condition;
            [SerializeField, LuaScriptWizard, FoldoutGroup("Script")]
            private string m_script;
            [SerializeField]
            private UnityEvent m_event;

            public void Execute(bool isOnStart = false)
            {
                if (isOnStart && m_frequency.HasFlag(Frequency.OnStart) == false)
                    return;

                if (string.IsNullOrEmpty(m_condition) == false)
                {
                    if (Lua.IsTrue(m_condition) == false)
                    {
                        m_lastCheck = false;
                        return;
                    }
                }
                m_lastCheck = true;
                if (string.IsNullOrEmpty(m_script) == false)
                {
                    Lua.Run(m_script);
                }

                m_event.Invoke();
            }
        }

        [SerializeField]
        private Reaction[] m_reactions;

        private void OnVariableChange(object sender, EventActionArgs eventArgs)
        {
            UpdateReaction(false);
        }

        private void UpdateReaction(bool isOnStart)
        {
            for (int i = 0; i < m_reactions.Length; i++)
            {
                m_reactions[i].Execute(isOnStart);
            }
        }

        private void Start()
        {
            UpdateReaction(true);
        }

        private void OnEnable()
        {
            DialogueVariableUtility.OnVariableChange += OnVariableChange;
        }

        private void OnDisable()
        {
            DialogueVariableUtility.OnVariableChange += OnVariableChange;
        }
    }
}