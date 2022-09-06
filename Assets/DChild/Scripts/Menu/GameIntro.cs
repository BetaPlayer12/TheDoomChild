using Holysoft.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using DChild.Temp;

namespace DChild
{
    public class GameIntro : MonoBehaviour
    {
        [SerializeField]
        private VideoPlayer m_intro;
        [SerializeField]
        private SceneInfo m_mainMenu;

        private AsyncOperation m_toMainMenu;
        private bool m_unloadScene;

        public void ActivateNextScene()
        {
            if (m_toMainMenu != null)
            {
                m_toMainMenu.allowSceneActivation = true;
            }
            m_unloadScene = true;
        }

        public void LoadScene()
        {
            SceneManager.LoadSceneAsync(m_mainMenu.sceneName, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(gameObject.scene);
        }

        public void LoadSceneAsync()
        {
            m_toMainMenu = SceneManager.LoadSceneAsync(m_mainMenu.sceneName, LoadSceneMode.Additive);
            m_toMainMenu.allowSceneActivation = false;
        }

        private void Awake()
        {
            m_intro.loopPointReached += OnIntroEnd;
        }

        private void OnIntroEnd(VideoPlayer source)
        {
            GameEventMessage.SendEvent("Stop Intro");
        }

        private void Update()
        {
            if (m_unloadScene)
            {
                SceneManager.UnloadSceneAsync(gameObject.scene);
            }
        }
    }
}