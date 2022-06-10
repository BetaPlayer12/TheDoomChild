using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.CustomInput.Keybind
{
    public class KeyboardBindUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_inputText;

        public void UpdateVisual(InputBinding binding)
        {
            m_inputText.text = binding.effectivePath;
        }
    }
}
