using Doozy.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay.Systems
{
    public class ZoneLoader : MonoBehaviour, IGameplaySystemModule
    {
        private AsyncOperation m_sceneLoadOperation;
        private AsyncOperation m_sceneUnloadOperation;
        private string m_activeZone;

        private string m_zoneToLoad;
        private bool m_unloadCurrentZone;
        private Action m_callback;
       

        public void SetAsActiveZone(string sceneName) => m_activeZone = sceneName;

        public void LoadZoneAsync(string zone, float delay = 0, Action CallbackAfterZoneLoad = null)
        {
            m_unloadCurrentZone = false;
            if (m_activeZone != zone)
            {
                m_sceneLoadOperation = SceneManager.LoadSceneAsync(zone, LoadSceneMode.Additive);
                m_sceneLoadOperation.allowSceneActivation = false;
                m_unloadCurrentZone = true;
            }
            m_zoneToLoad = zone;
            m_callback = CallbackAfterZoneLoad;

            StopAllCoroutines();
            StartCoroutine(DelayRoutine(delay));
        }

        public void StartZoneTransfer()
        {
            m_sceneUnloadOperation = SceneManager.UnloadSceneAsync(m_activeZone);
            Time.timeScale = 0;
            StartCoroutine((ZoneTransferRoutine()));
        }

        private IEnumerator DelayRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            GameplaySystem.gamplayUIHandle.ShowGameplayUI(false);
        }

        private IEnumerator ZoneTransferRoutine()
        {
            var time = 0f;
            var endOfFrame = new WaitForEndOfFrame();

            if (m_unloadCurrentZone)
            {
                while (m_sceneLoadOperation.progress < 0.9f)
                {
                    yield return endOfFrame;
                    time += Time.unscaledDeltaTime;
                }
                m_sceneLoadOperation.allowSceneActivation = true;
                while (m_sceneUnloadOperation.isDone == false)
                {
                    yield return endOfFrame;
                    time += Time.unscaledDeltaTime;
                }

            }
            m_callback?.Invoke();
            GameplaySystem.gamplayUIHandle.ShowGameplayUI(true);
            m_activeZone = m_zoneToLoad;
            Time.timeScale = 1;
            Debug.Log($"Loading Time: {time}");
        }
    }

}