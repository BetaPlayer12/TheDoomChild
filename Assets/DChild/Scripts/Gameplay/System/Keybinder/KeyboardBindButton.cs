using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DChild.CustomInput.Keybind
{
    public class KeyboardBindButton : MonoBehaviour
    {
        [SerializeField]
        private KeyboardBindUI m_ui;
        [SerializeField]
        private KeybindSelection m_selection;
        [SerializeField, ReadOnly]
        private string m_currentPath;

        public KeybindSelection selection => m_selection;

        public void UpdateUI(InputBinding binding)
        {
            m_ui.UpdateVisual(binding);
            m_currentPath = binding.effectivePath;
        }

#if UNITY_EDITOR
        [Button]
        private void Rebind()
        {
            GetComponent<UIButton>().ExecuteClick();
        }
#endif
    }
}
