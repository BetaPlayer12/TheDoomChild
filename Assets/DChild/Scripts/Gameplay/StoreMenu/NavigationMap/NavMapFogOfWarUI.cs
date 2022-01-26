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
        private VariableToObjectEditorList<Image> m_editorList; 
#endif
        [SerializeField, HideInEditorMode]
        private Dictionary<string,Image> m_list;

        public void UpdateFogOfWarUI()
        {
            foreach (var varName in m_list.Keys)
            {
                var isHidden = DialogueLua.GetVariable(varName).asBool;
                m_list[varName].enabled = isHidden;
            }
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            m_list = m_editorList.ToDictionary();
#endif
    }
}
}