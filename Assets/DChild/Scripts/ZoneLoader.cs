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