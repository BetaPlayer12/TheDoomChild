using DChild.Gameplay.Optimizers;
using DChild.Menu;
using Holysoft.Collections;
using Holysoft.Event;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
#if UNITY_EDITOR
        [SerializeField]
#endif
        private bool m_gameplaySceneActive;
        private Action CallAfterSceneDone;

        public string activeZone => m_activeZone;

#if UNITY_EDITOR
        public void SetAsActiveZone(string sceneName) => m_activeZone = sceneName;
#endif

        public void LoadZone(SceneInfo scene, bool withLoadingScene, Action ToCallAfterSceneDone)
        {
            CallAfterSceneDone = ToCallAfterSceneDone;
            LoadingHandle.LoadingDone += AfterSceneDone;
            LoadZone(scene, withLoadingScene);
        }

        public void LoadZone(SceneInfo scene, bool withLoadingScene)
        {
            if (withLoadingScene)
            {
                RoomActivityManager.UnloadAllRooms();
                if (m_activeZone != null && m_activeZone != string.Empty /*&& m_activeZone != sceneName*/)
                {
                    LoadingHandle.UnloadScenes(m_activeZone);
                    m_activeZone = string.Empty;
                }

                if (GameSystem.m_useGameModeValidator == false)
                {
                    if (m_gameplaySceneActive == false)
                    {
                        LoadingHandle.LoadScenes(m_gameplayScene);
                        m_gameplaySceneActive = true;
                    }
                }
                //if (m_activeZone != sceneName)
                //{
                LoadingHandle.LoadScenes(scene);
                //}
                GameSystem.sceneManager.LoadSceneAsync(m_loadingScene.sceneName);
            }
            else
            {
                RoomActivityManager.UnloadAllRooms();
                if (m_activeZone != string.Empty && m_activeZone != scene.sceneName)
                {
                    LoadingHandle.UnloadScenes(m_activeZone);
                    m_activeZone = string.Empty;
                }
                if (GameSystem.sceneManager.IsSceneLoaded(m_gameplayScene.sceneName) == false)
                {
                    GameSystem.sceneManager.LoadSceneAsync(m_gameplayScene.sceneName);
                }

                if (scene.isAddressables)
                {
                    GameSystem.sceneManager.LoadSceneAsync(scene.sceneName);
                }
                else
                {
                    SceneManager.LoadSceneAsync(scene.sceneName, LoadSceneMode.Additive);
                }
            }
            m_activeZone = scene.sceneName;
        }

        public void LoadMainMenu()
        {
            if (m_activeZone != null)
            {
                LoadingHandle.UnloadScenes(m_activeZone);
                m_activeZone = string.Empty;
            }
            LoadingHandle.UnloadScenes(m_gameplayScene);
            m_gameplaySceneActive = false;
            LoadingHandle.LoadScenes(m_mainMenu);
            Time.timeScale = 1;
            GameSystem.sceneManager.LoadSceneAsync(m_loadingScene.sceneName);
        }

        private void AfterSceneDone(object sender, EventActionArgs eventArgs)
        {
            CallAfterSceneDone();
            CallAfterSceneDone = null;
            LoadingHandle.LoadingDone -= AfterSceneDone;
        }
    }

}