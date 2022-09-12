using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.NavigationMap
{
#if UNITY_EDITOR
    [System.Serializable]
    public class VariableToObjectEditorList<T>
    {
        [System.Serializable, HideReferenceObjectPicker]
        public class Info
        {
            [SerializeField, HideInInspector]
            public DialogueDatabase m_reference;
            [SerializeField, ValueDropdown("GetVariableNames")]
            private string m_varName;
            [SerializeField]
            private T m_gameObject;

            public Info(DialogueDatabase reference)
            {
                m_reference = reference;
            }

            public string varName => m_varName;
            public T gameObject => m_gameObject;

            private IEnumerable GetVariableNames() => m_reference.variables.Select(x => x.Name);
        }

        [SerializeField, OnValueChanged("UpdateListDatabase")]
        private DialogueDatabase m_database;
        [SerializeField,OnValueChanged("UpdateListDatabase"),TableList]
        private List<Info> m_infoList;

        public Dictionary<string, T> ToDictionary()
        {
            var dictionary = new Dictionary<string, T>();
            foreach (var info in m_infoList)
            {
                dictionary.Add(info.varName, info.gameObject);
            }
            return dictionary;
        }

        private void UpdateListDatabase()
        {
            for (int i = 0; i < m_infoList.Count; i++)
            {
                m_infoList[i].m_reference = m_database;
            }
        }
    }
#endif
}