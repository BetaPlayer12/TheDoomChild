using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Narrative
{
    public class DialogueVariableTracker : MonoBehaviour
    {
        [SerializeField, LuaScriptWizard]
        private string m_condition;
        [SerializeField, LuaScriptWizard]
        private string m_script;

        protected void RunLua()
        {
            if (string.IsNullOrEmpty(m_condition) == false)
            {
                if (Lua.IsTrue(m_condition) == false)
                    return;
            }
            Lua.Run(m_script);
        }
    }
}