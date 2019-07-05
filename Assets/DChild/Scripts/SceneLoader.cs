using DChild.Menu;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private SceneInfo m_loadingScene;
        [SerializeField]
        private SceneInfo m_gameplayScene;
        [SerializeField]
        private SceneInfo m_mainMenu;

        private string m_activeZone;

        private bool m_isZoneLoaded;

        public void LoadZone(string sceneName, bool withLoadingScene)
        {
            if (withLoadingScene)
            {
                if (SceneManager.GetSceneByName(m_gameplayScene.sceneName).isLoaded == false)
                {
                    LoadingHandle.LoadScenes(m_gameplayScene.sceneName);
                }
                LoadingHandle.LoadScenes(sceneName);
                SceneManager.LoadScene(m_loadingScene.sceneName, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                SceneManager.LoadSceneAsync(m_gameplayScene.sceneName, LoadSceneMode.Additive);
            }
            m_activeZone = sceneName;
        }

        public void LoadMainMenu()
        {
            if (m_activeZone != null)
            {
                LoadingHandle.UnLoadScenes(m_gameplayScene.sceneName, m_activeZone);
                LoadingHandle.LoadScenes(m_mainMenu.sceneName);
                SceneManager.LoadScene(m_loadingScene.sceneName, LoadSceneMode.Additive);
            }
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