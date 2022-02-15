using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.NavigationMap
{
    public class NavMapPointOfInterestUI : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, HideInPlayMode]
        private VariableToObjectEditorList<Image> m_editorList;
#endif

        [SerializeField, HideInEditorMode]
        private Dictionary<string, Image> m_list;

        public void UpdatePointOfInterestUI()
        {
            foreach (var varName in m_list.Keys)
            {
                var isHidden = DialogueLua.GetVariable(varName).asBool;
                m_list[varName].enabled = isHidden;
            }
        }

        public void OnValidate()
        {
#if UNITY_EDITOR
            m_list = m_editorList.ToDictionary(); 
#endif
        }
    }
}

