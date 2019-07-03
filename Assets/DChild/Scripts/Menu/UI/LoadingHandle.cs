using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Menu
{
    public class LoadingHandle : MonoBehaviour
    {
        [SerializeField]
        private LoadingAnimation m_animation;
        [SerializeField]
        private SceneInfo m_loadingScene;

        private static List<string> scenesToLoad;
        private static List<string> scenesToUnload;
        public static EventAction<EventActionArgs> SceneDone;

        private static List<AsyncOperation> m_loadOperations;
        private static List<AsyncOperation> m_unloadOperations;
        private static bool m_isInitialized;

        private bool m_unloadThis;

        public static void LoadScenes(params string[] scenes)
        {
            if (scenesToLoad == null)
            {
                scenesToLoad = new List<string>();
            }
            scenesToLoad.AddRange(scenes);
        }

        public static void UnLoadScenes(params string[] scenes)
        {
            if (scenesToUnload == null)
            {
                scenesToUnload = new List<string>();
            }

            scenesToUnload.AddRange(scenes);
        }

        private void Awake()
        {
            if (m_isInitialized == false)
            {
                m_loadOperations = new List<AsyncOperation>();
                m_unloadOperations = new List<AsyncOperation>();
                m_isInitialized = true;
            }

            m_animation.AnimationEnd += OnAnimationEnd;
        }

        private void OnAnimationEnd(object sender, EventActionArgs eventArgs)
        {
            SceneManager.UnloadSceneAsync(gameObject.scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }

        private IEnumerator MonitorProgess()
        {
            m_animation.PlayStart();
            var endOfFrame = new WaitForEndOfFrame();
            yield return endOfFrame;
            if (m_unloadOperations.Count > 0)
            {
                do
                {
                    for (int i = m_unloadOperations.Count - 1; i >= 0; i--)
                    {
                        if (m_unloadOperations[i].isDone)
                        {
                            m_unloadOperations.RemoveAt(i);
                        }
                    }
                    yield return endOfFrame;
                } while (m_unloadOperations.Count > 0);
                yield return endOfFrame;
            }

            if (m_loadOperations.Count > 0)
            {
                bool isLoading = false;
                do
                {
                    isLoading = false;
                    for (int i = 0; i < m_loadOperations.Count; i++)
                    {
                        if (m_loadOperations[i].progress < 0.9f)
                        {
                            isLoading = true;
                            break;
                        }
                    }
                    yield return endOfFrame;
                } while (isLoading);
            }
            m_animation.PlayEnd();
            yield return new WaitForSeconds(2.25f);
            for (int i = 0; i < m_loadOperations.Count; i++)
            {
                m_loadOperations[i].allowSceneActivation = true;
            }
            yield return endOfFrame;
            //Cant Call Unload Here for some reason, so i have to resort to using a flag to trigger the unloading
            m_unloadThis = true;
        }

        private void Start()
        {
            m_unloadThis = false;
            m_unloadOperations.Clear();
            for (int i = 0; i < (scenesToUnload?.Count ?? 0); i++)
            {
                var operation = SceneManager.UnloadSceneAsync(scenesToUnload[i]);
                m_unloadOperations.Add(operation);
            }
            scenesToUnload?.Clear();

            m_loadOperations.Clear();
            for (int i = 0; i < (scenesToLoad?.Count ?? 0); i++)
            {
                Debug.LogError($"{scenesToLoad[i]}");
                m_loadOperations.Add(SceneManager.LoadSceneAsync(scenesToLoad[i], LoadSceneMode.Additive));
                m_loadOperations[i].allowSceneActivation = false;
            }
            scenesToLoad?.Clear();
            StartCoroutine(MonitorProgess());
        }

        private void Update()
        {
            if (m_unloadThis)
            {
                SceneManager.UnloadSceneAsync(m_loadingScene.sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                m_unloadThis = false;
            }
        }

        private void OnDestroy()
        {
            m_animation.AnimationEnd -= OnAnimationEnd;
            SceneDone?.Invoke(this, EventActionArgs.Empty);
        }
    }
}