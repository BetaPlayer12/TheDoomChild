using Holysoft.Menu;
using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private bool m_buildMode;
        [SerializeField]
        [ShowIf("m_buildMode")]
        private SplashScreen m_splashScreen;
        [SerializeField]
        [ShowIf("m_buildMode")]
        private MainMenuScreen m_mainMenuScreen;
        [SerializeField]
        [ShowIf("m_buildMode")]
        private WindowNavigation m_windowNavigation;

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                if (m_buildMode)
                {
                    UIUtility.OpenCanvas(m_splashScreen);
                    UIUtility.CloseCanvas(m_mainMenuScreen);
                    m_windowNavigation.CloseAll();
                }
            }
#endif
        }
    }
}