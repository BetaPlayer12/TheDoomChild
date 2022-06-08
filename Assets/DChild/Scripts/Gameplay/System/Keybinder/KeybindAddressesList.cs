using UnityEngine;

namespace DChild.CustomInput.Keybind
{
    [CreateAssetMenu(fileName = "KeybindSelectionAddresses", menuName = "DChild/System/Keybind Selection Addresses")]
    public class KeybindAddressesList : ScriptableObject
    {
        [SerializeField]
        private KeybindAddress[] m_address;

        public int count => m_address.Length;
        public KeybindAddress GetAddress(int index) => m_address[index];
    }
}