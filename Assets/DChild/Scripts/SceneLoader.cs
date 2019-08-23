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

        public string activeZone => m_activeZone;

#if UNITY_EDITOR
        public void SetAsActiveZone(string sceneName) => m_activeZone = sceneName; 
#endif

        public void LoadZone(string sceneName, bool withLoadingScene)
        {
            if (withLoadingScene)
            {
                if (m_activeZone != string.Empty)
                {
                    LoadingHandle.UnloadScenes(m_activeZone);
                    m_activeZone = string.Empty;
                }

                if (SceneManager.GetSceneByName(m_gameplayScene.sceneName).isLoaded == false)
                {
                    LoadingHandle.LoadScenes(m_gameplayScene.sceneName);
                }
                LoadingHandle.LoadScenes(sceneName);
                SceneManager.LoadScene(m_loadingScene.sceneName, LoadSceneMode.Additive);
            }
            else
            {
                if (m_activeZone != string.Empty)
                {
                    LoadingHandle.UnloadScenes(m_activeZone);
                    m_activeZone = string.Empty;
                }
                if (SceneManager.GetSceneByName(m_gameplayScene.sceneName).isLoaded == false)
                {
                    SceneManager.LoadSceneAsync(m_gameplayScene.sceneName, LoadSceneMode.Additive);
                }
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            m_activeZone = sceneName;
        }

        public void LoadMainMenu()
        {
            if (m_activeZone != null)
            {
                LoadingHandle.UnloadScenes(m_activeZone);
                m_activeZone = string.Empty;
            }
            LoadingHandle.UnloadScenes(m_gameplayScene.sceneName);
            LoadingHandle.LoadScenes(m_mainMenu.sceneName);
            Time.timeScale = 1;
            SceneManager.LoadScene(m_loadingScene.sceneName, LoadSceneMode.Additive);
        }
    }

}