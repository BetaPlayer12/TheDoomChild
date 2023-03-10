using Holysoft.Collections;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.NavigationMap
{

    public class NavMapFogOfWarUI : SerializedMonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, HideInPlayMode]
        private VariableToObjectEditorList<NavMapFogOfWarSegment> m_editorList;
#endif
        [SerializeField, HideInEditorMode]
        private Dictionary<string, NavMapFogOfWarSegment> m_list;



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

        public bool HasID(string varName)
        {
            return m_list.ContainsKey(varName);
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            m_list = m_editorList.ToDictionary();
#endif
        }
    }
}