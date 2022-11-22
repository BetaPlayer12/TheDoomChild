using Holysoft.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Doozy.Runtime.Signals;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

using System.Collections;

namespace DChild
{
    public class GameIntro : MonoBehaviour
    {
        public static SceneInstance currentIntroScene;

        [SerializeField]
        private VideoPlayer m_intro;
        [SerializeField]
        private SceneInfo m_mainMenu;
        [SerializeField]
        private SignalSender m_stopIntro;

        private SceneInstance m_loadedMainMenu;
        private bool m_isMainMenuLoaded;
        private bool m_unloadScene;

        public void ActivateNextScene()
        {
            if (m_isMainMenuLoaded)
            {
                m_loadedMainMenu.ActivateAsync();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(ActivateNextSceneAsync());
            }
            m_unloadScene = true;
        }

        public void LoadScene()
        {
            Addressables.LoadSceneAsync(m_mainMenu.sceneName, LoadSceneMode.Additive);
            Addressables.UnloadSceneAsync(currentIntroScene);
        }

        public void LoadSceneAsync()
        {
            m_isMainMenuLoaded = false;
            Addressables.LoadSceneAsync(m_mainMenu.sceneName, LoadSceneMode.Additive, false).Completed += OnMainMenuLoaded;
        }

        private void OnMainMenuLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            m_loadedMainMenu = obj.Result;
            obj.Completed -= OnMainMenuLoaded;
            m_isMainMenuLoaded = true;
        }

        private IEnumerator ActivateNextSceneAsync()
        {
            while (m_isMainMenuLoaded == false)
                yield return null;

            m_loadedMainMenu.ActivateAsync();
        }

        private void Awake()
        {
            m_intro.loopPointReached += OnIntroEnd;
        }

        private void OnIntroEnd(VideoPlayer source)
        {
            m_stopIntro.SendSignal();
        }

        private void Update()
        {
            if (m_unloadScene)
            {
                Addressables.UnloadSceneAsync(currentIntroScene);
            }
        }
    }
}