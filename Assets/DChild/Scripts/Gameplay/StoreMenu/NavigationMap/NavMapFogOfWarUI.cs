﻿using Holysoft.Collections;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.NavigationMap
{

    public class NavMapFogOfWarUI : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, HideInPlayMode]
        private VariableToObjectEditorList<NavMapFogOfWarSegment> m_editorList;
#endif
        [SerializeField, HideInEditorMode]
        private Dictionary<string, NavMapFogOfWarSegment> m_list;

#if UNITY_EDITOR
        public Dictionary<string, NavMapFogOfWarSegment> GetSegments() => m_editorList.ToDictionary();
#endif

        public void UpdateUI()
        {
            foreach (var varName in m_list.Keys)
            {
                var state = DialogueLua.GetVariable(varName).asInt;
                SetUIState(varName, (Flag)state);
            }
        }

        public void SetUIState(string varName, Flag state)
        {
            m_list[varName].SetUIState(state);
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            m_list = m_editorList.ToDictionary();
#endif
        }
    }
}