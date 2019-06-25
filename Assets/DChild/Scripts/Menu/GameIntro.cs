using Holysoft.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Doozy.Engine;

namespace DChild
{
    public class GameIntro : MonoBehaviour
    {
        [SerializeField]
        private VideoPlayer m_intro;
        [SerializeField]
        private SceneInfo m_mainMenu;

        private AsyncOperation m_toMainMenu;

        public void ActivateNextScene()
        {
            m_toMainMenu.allowSceneActivation = true;
            SceneManager.UnloadSceneAsync(gameObject.scene);
        }

        private void Awake()
        {
            m_intro.loopPointReached += OnIntroEnd;
        }

        private void OnIntroEnd(VideoPlayer source)
        {
            GameEventMessage.SendEvent("Stop Intro");
        }

        private void Start()
        {

            m_toMainMenu = SceneManager.LoadSceneAsync(m_mainMenu.sceneName, LoadSceneMode.Additive);
            m_toMainMenu.allowSceneActivation = false;
        }
    }
}