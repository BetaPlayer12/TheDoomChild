using UnityEngine;

namespace DChild.Configurations.Visuals
{
    public class ScreenResolution : MonoBehaviour
    {
        [SerializeField]
        private int m_resolutionIndex;
        [SerializeField]
        private bool m_fullScreen;

        private Resolution m_currentResolution;

        public int resolutionIndex => m_resolutionIndex;

        public bool fullScreen => m_fullScreen;

        public void SetResolution(int resolutionIndex)
        {
            m_resolutionIndex = resolutionIndex;
            m_currentResolution = GameSystem.settings.supportedResolutions.GetResolution(m_resolutionIndex);
        }

        public void SetFullscreen(bool fullscreen) => m_fullScreen = fullscreen;

        public void Apply()
        {
            Screen.fullScreen = m_fullScreen;
            Screen.SetResolution(m_currentResolution.width, m_currentResolution.height, Screen.fullScreen);
        }

        private void Start()
        {
            m_currentResolution = GameSystem.settings.supportedResolutions.GetResolution(m_resolutionIndex);
        }
    }
}