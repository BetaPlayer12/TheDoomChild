using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.CustomInput.Keybind
{
    [System.Serializable]
    public class KeyboardKeymapData
    {
        [SerializeField,ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, DraggableItems = false,NumberOfItemsPerPage =1,ShowPaging =true)]
        private KeybindInfo[] m_datas;

        public KeyboardKeymapData()
        {
            var count = (int)KeybindSelection._Count;
            m_datas = new KeybindInfo[count];
            for (int i = 0; i < count; i++)
            {
                m_datas[i] = new KeybindInfo();
            }
        }

        public int count => m_datas.Length;

        public KeybindInfo GetKeybindInfo(int index) => m_datas[index];

#if UNITY_EDITOR
        [Button,PropertyOrder(-1)]
        private void Reset()
        {
            var count = (int)KeybindSelection._Count;
            m_datas = new KeybindInfo[count];
            for (int i = 0; i < count; i++)
            {
                m_datas[i] = new KeybindInfo();
            }
        }
#endif
    }
}
