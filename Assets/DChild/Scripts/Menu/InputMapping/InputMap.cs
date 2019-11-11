using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace DChild.Inputs
{
    [System.Serializable]
    public class InputMap
    {
        [System.Serializable]
        public struct Data
        {
            [SerializeField, ReadOnly]
            private InputKey m_input;
            public KeyCode key;

            public Data(InputKey m_input) : this()
            {
                this.m_input = m_input;
            }

            public InputKey input => m_input;
        }

        [SerializeField, ListDrawerSettings(DraggableItems = false, HideRemoveButton = true, HideAddButton = true, OnTitleBarGUI = "OnTitleBar"), HideLabel]
        private Data[] m_dataList;

        public Data[] dataList => m_dataList;

        public InputMap()
        {
            m_dataList = new Data[(int)InputKey._COUNT];
            for (int i = 0; i < m_dataList.Length; i++)
            {
                m_dataList[i] = new Data((InputKey)i);
            }
        }

        public KeyCode GetKeyCode(InputKey input) => m_dataList[(int)input].key;

        public void SetInputKeyCode(InputKey input, KeyCode code) => m_dataList[(int)input].key = code;

        public void Copy(Data[] list)
        {
            var size = (int)InputKey._COUNT;
            if (m_dataList.Length < size)
            {
                var m_dataList = new Data[size];
                for (int i = 0; i < m_dataList.Length; i++)
                {
                    m_dataList[i] = new Data((InputKey)i);
                }
            }

            for (int i = 0; i < list.Length; i++)
            {
                var inputKey = list[i].input;
                for (int j = 0; j < m_dataList.Length; j++)
                {
                    if (inputKey == m_dataList[j].input)
                    {
                        m_dataList[i].key = list[j].key;
                        break;
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void OnTitleBar()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                var size = (int)InputKey._COUNT;
                var list = new Data[size];
                for (int i = 0; i < list.Length; i++)
                {
                    list[i] = new Data((InputKey)i);
                }
                Copy(list);
            }
        }
#endif
    }
}
