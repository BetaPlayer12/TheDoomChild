using DChild.Menu;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild
{
    public class ZoneLoader : MonoBehaviour
    {
        [SerializeField]
        private SceneInfo m_loadingScene;
        [SerializeField]
        private SceneInfo m_gameplayScene;

        private bool m_isZoneLoaded;

        public void LoadZone(string sceneName, bool withLoadingScene)
        {
            if (withLoadingScene)
            {
                LoadingHandle.sceneToLoad = sceneName;
                SceneManager.LoadScene(m_loadingScene.sceneName, LoadSceneMode.Additive);
                if (SceneManager.GetSceneByName(m_gameplayScene.sceneName).isLoaded == false)
                {
                    var gameplaySystem = SceneManager.LoadSceneAsync(m_gameplayScene.sceneName, LoadSceneMode.Additive);
                    StartCoroutine(WaitTillZoneDone(gameplaySystem));
                }
            }
            else
            {
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                SceneManager.LoadSceneAsync(m_gameplayScene.sceneName, LoadSceneMode.Additive);
            }
        }

        private IEnumerator WaitTillZoneDone(AsyncOperation operation)
        {
            m_isZoneLoaded = false;
            operation.allowSceneActivation = false;
            while (m_isZoneLoaded == false)
            {
                yield return null;
            }
            operation.allowSceneActivation = true;
        }

        private void OnSceneDone(object sender, EventActionArgs eventArgs)
        {
            m_isZoneLoaded = true;
        }

        private void Awake()
        {
            LoadingHandle.SceneDone = OnSceneDone;
        }
    }

}