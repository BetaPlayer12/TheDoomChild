using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildDebug.Window
{
    public class ToggleGameplayUI : MonoBehaviour, IToggleDebugBehaviour
    {
        [SerializeField] private CanvasGroup m_gameplayScreen;
        [SerializeField] private CanvasGroup m_gameplayWorld;

        private bool m_isOn;
        public bool value => m_isOn;

        public void HideGameplayUI(bool value)
        {
            m_gameplayScreen.alpha = value ? 0 : 1;
            m_gameplayScreen.interactable = !value;
            m_gameplayScreen.blocksRaycasts = !value;


            if (m_gameplayWorld == null) return;
            m_gameplayWorld.alpha = value ? 0 : 1;
            m_gameplayWorld.interactable = !value;
            m_gameplayWorld.blocksRaycasts = !value;
        }
    }
}
