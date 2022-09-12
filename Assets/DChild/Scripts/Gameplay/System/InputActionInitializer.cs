using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.Systems
{
    public class InputActionInitializer : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput m_input;

        [SerializeField]
        private InputHandle m_gameplayInput;
        [SerializeField]
        private InputHandle[] m_uiInputs;

        private void Awake()
        {
            var gameplayActionMap = m_input.actions.FindActionMap("Gameplay");
            m_gameplayInput.SetActionMap(gameplayActionMap);

            var uiActionMaps = m_input.actions.FindActionMap("UI");
            for (int i = 0; i < m_uiInputs.Length; i++)
            {
                m_uiInputs[i].SetActionMap(uiActionMaps);
            }
        }
    }
}