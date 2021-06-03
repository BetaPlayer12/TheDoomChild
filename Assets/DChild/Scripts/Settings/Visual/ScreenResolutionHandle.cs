using UnityEngine;

namespace DChild.Configurations.Visuals
{
    public class ScreenResolutionHandle : MonoBehaviour
    {
        [SerializeField]
        private int m_resolutionIndex;
        [SerializeField]
        private bool m_fullScreen;

        private SupportedResolutions m_supportedResolutions;
        private Resolution m_currentResolution;

        public int resolutionIndex => m_resolutionIndex;

        public bool fullScreen => m_fullScreen;

        public void SetSupportedResolutions(SupportedResolutions supportedResolutions)
        {
            m_supportedResolutions = supportedResolutions;
            m_currentResolution = m_supportedResolutions.GetResolution(m_resolutionIndex);
        }

        public void SetResolution(int resolutionIndex)
        {
            m_resolutionIndex = resolutionIndex;
            m_currentResolution = m_supportedResolutions.GetResolution(resolutionIndex);
        }

        public void SetFullscreen(bool fullscreen)
        {
            m_fullScreen = fullscreen;
        }


        public void Apply()
        {
            //Screen.fullScreenMode = m_fullScreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
            Screen.fullScreen = m_fullScreen;
            Screen.SetResolution(m_currentResolution.width, m_currentResolution.height, m_fullScreen);
        }
    }
}