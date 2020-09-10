using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace DChild
{
    public class AddressableSceneManager : MonoBehaviour
    {
        private Dictionary<AssetReference, AsyncOperationHandle<SceneInstance>> m_activeScenes;
        private List<AssetReference> m_scenesToUnload;
        private List<AssetReference> m_scenesToLoad;
        private List<SceneInstance> m_unactivatedScenes;

        public bool HasScenesYetToLoad() => m_scenesToLoad.Count > 0;

        public void ActivateAllUnactiveScenes()
        {
            for (int i = 0; i < m_unactivatedScenes.Count; i++)
            {
                ActivateScene(m_unactivatedScenes[i]);
            }
        }

        public void LoadScene(AssetReference scene, bool activateOnLoad = true)
        {
            if (m_scenesToUnload.Contains(scene))
            {
                Debug.LogError($"Attempting to Load {scene} while it is Unloading");
            }
            else if (m_activeScenes.ContainsKey(scene) == false)
            {
                var handle = Addressables.LoadSceneAsync(scene, UnityEngine.SceneManagement.LoadSceneMode.Additive, activateOnLoad);
                m_activeScenes.Add(scene, handle);
                m_scenesToLoad.Add(scene);
                handle.Completed += (operation) =>
                {
                    if (operation.Status == AsyncOperationStatus.Succeeded)
                    {
                        if (activateOnLoad == false)
                        {
                            m_unactivatedScenes.Add(operation.Result);
                        }
                    }
                    else
                    {
                        Debug.LogError($"Fail to Load {scene}");
                        Addressables.Release(operation);
                        m_activeScenes.Remove(scene);
                    }
                    m_scenesToLoad.Remove(scene);
                };
            }
        }

        public void UnloadScene(AssetReference scene)
        {
            if (m_activeScenes.ContainsKey(scene))
            {
                if (m_activeScenes[scene].IsDone)
                {
                    m_scenesToUnload.Add(scene);
                    m_activeScenes.Remove(scene);
                    Addressables.UnloadSceneAsync(m_activeScenes[scene], true).Completed += (operation) =>
                    {
                        m_scenesToUnload.Remove(scene);
                    };
                }
                else
                {
                    Debug.LogError($"Attempting to Unload {scene} while it is Loading");
                }
            }
        }

        private void ActivateScene(SceneInstance instance)
        {
            instance.Activate();
            m_unactivatedScenes.Remove(instance);
        }

        private void Awake()
        {
            m_activeScenes = new Dictionary<AssetReference, AsyncOperationHandle<SceneInstance>>();
            m_scenesToUnload = new List<AssetReference>();
            m_scenesToLoad = new List<AssetReference>();
            m_unactivatedScenes = new List<SceneInstance>();
        }
    }
}