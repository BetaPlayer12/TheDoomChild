using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace DChild.Menu
{
    public class AddressableSceneManager
    {
        private List<SceneInstance> m_loadedscenes;
        private List<AsyncOperationHandle<SceneInstance>> m_ongoingOperation;

        public AddressableSceneManager()
        {
            m_loadedscenes = new List<SceneInstance>();
            m_ongoingOperation = new List<AsyncOperationHandle<SceneInstance>>();
        }

        public bool isHandlingSceneOperations => m_ongoingOperation.Count > 0;

        public bool IsSceneLoaded(string sceneName)
        {
            for (int i = 0; i < m_loadedscenes.Count; i++)
            {
                if (m_loadedscenes[i].Scene.name == sceneName)
                    return true;
            }

            return false;
        }

        public void ActivateLoadedScenes()
        {
            for (int i = 0; i < m_loadedscenes.Count; i++)
            {
                m_loadedscenes[i].ActivateAsync();
            }
        }

        public IEnumerator ActivateLoadedScenesInOrder()
        {

            for (int i = 0; i < m_loadedscenes.Count; i++)
            {
                var loadedScene = m_loadedscenes[i];
                loadedScene.ActivateAsync();
                while (loadedScene.Scene.isLoaded == false)
                    yield return null;
            }
        }

        public void UnloadAllLoadedScenes()
        {
            for (int i = m_loadedscenes.Count - 1; i >= 0; i--)
            {
                UnloadScene(m_loadedscenes[i]);
            }
        }

        public void UnloadSceneAsync(string scene)
        {
            for (int i = m_loadedscenes.Count - 1; i >= 0; i--)
            {
                var instance = m_loadedscenes[i];
                if (instance.Scene.name == scene)
                {
                    UnloadScene(instance);
                    continue;
                }
            }
        }

        public AsyncOperationHandle<SceneInstance> LoadSceneAsync(string scene, bool activateOnLoad = true)
        {
            var operation = Addressables.LoadSceneAsync(scene, LoadSceneMode.Additive, activateOnLoad);
            operation.Completed += OSceneLoad;
            m_ongoingOperation.Add(operation);
            return operation;
        }

        private void UnloadScene(SceneInstance instance)
        {
            var operation = Addressables.UnloadSceneAsync(instance);
            m_ongoingOperation.Add(operation);
            operation.Completed += OnSceneUnload;
            m_loadedscenes.Remove(instance);
        }

        private void OnSceneUnload(AsyncOperationHandle<SceneInstance> obj)
        {
            obj.Completed -= OnSceneUnload;
            m_ongoingOperation.Remove(obj);
        }

        private void OSceneLoad(AsyncOperationHandle<SceneInstance> obj)
        {
            obj.Completed -= OSceneLoad;
            m_loadedscenes.Add(obj.Result);
            m_ongoingOperation.Remove(obj);
        }
    }
}