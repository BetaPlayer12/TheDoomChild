using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.CustomInput.Keybind
{
    [CreateAssetMenu(fileName = "KeyboardKeybindSelectionAddresses", menuName = "DChild/System/Keyboard Keybind Selection Addresses")]
    public class KeyboardKeybindSelectionAddresses : ScriptableObject
    {
        [System.Serializable]
        private class Info
        {
            [SerializeField, ReadOnly]
            private KeybindSelection m_selection; 
            [SerializeField]
            private KeybindAddressesList m_addressList;

            public Info(KeybindSelection selection)
            {
                m_selection = selection;
            }

            public KeybindAddressesList addressList => m_addressList;
        }

        [SerializeField]
        private Info[] m_infos;

        public int count => m_infos.Length;
        public KeybindAddressesList GetAddressList(int index) => m_infos[index].addressList;

#if UNITY_EDITOR
        [Button, PropertyOrder(-1)]
        private void Reset()
        {
            var count = (int)KeybindSelection._Count;
            m_infos = new Info[count];
            for (int i = 0; i < count; i++)
            {
                m_infos[i] = new Info((KeybindSelection)i);
            }
        }
#endif

    }

}