using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;

namespace DChild.Gameplay.Quests
{
    public abstract class QuestAdvancer : MonoBehaviour
    {
        [SerializeField,LuaScriptWizard]
        private string m_script;

        protected void ExecuteLuaScript()
        {
            Lua.Run(m_script);
        }
    }
}