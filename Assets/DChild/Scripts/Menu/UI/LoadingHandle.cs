using DChild.Gameplay;
using Doozy.Engine;
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
        public enum LoadType
        {
            Smart,
            Force
        }

        [SerializeField]
        private LoadingAnimation m_animation;
        [SerializeField]
        private SceneInfo m_loadingScene;

        private static List<string> scenesToLoad;
        private static List<string> scenesToUnload;
        public static event EventAction<EventActionArgs> LoadingDone;
        public static event EventAction<EventActionArgs> SceneDone;
        public static LoadType loadType { get; private set; }

        private static List<AsyncOperation> m_loadOperations;
        private static List<AsyncOperation> m_unloadOperations;
        private static bool m_isInitialized;

        private bool m_unloadThis;
        private bool m_canTransferScene;

        public static void SetLoadType(LoadType value) => loadType = value;

        public static void LoadScenes(params string[] scenes)
        {
            if (scenesToLoad == null)
            {
                scenesToLoad = new List<string>();
            }

            scenesToLoad.AddRange(scenes);
        }

        public static void UnloadScenes(params string[] scenes)
        {
            if (scenesToUnload == null)
            {
                scenesToUnload = new List<string>();
            }

            scenesToUnload.AddRange(scenes);
        }

        public void DoLoad()
        {
            //m_unloadThis = false;
            //m_unloadOperations.Clear();
            //scenesToUnload?.RemoveAll(x => x == string.Empty);
            //for (int i = 0; i < (scenesToUnload?.Count ?? 0); i++)
            //{
            //    var operation = SceneManager.UnloadSceneAsync(scenesToUnload[i]);
            //    m_unloadOperations.Add(operation);
            //}
            //scenesToUnload?.Clear();

            //m_loadOperations.Clear();
            //scenesToLoad?.RemoveAll(x => x == string.Empty);
            //for (int i = 0; i < (scenesToLoad?.Count ?? 0); i++)
            //{
            //    m_loadOperations.Add(SceneManager.LoadSceneAsync(scenesToLoad[i], LoadSceneMode.Additive));
            //    m_loadOperations[i].allowSceneActivation = false;
            //}
            //scenesToLoad?.Clear();
            Debug.LogError("False Positive: Loading Start");
            StartCoroutine(ExecuteLoadUnloadScene());
        }

        public void SendEvents()
        {
            switch (loadType)
            {
                case LoadType.Force:
                    GameEventMessage.SendEvent("Force Load");
                    break;
                case LoadType.Smart:
                    GameEventMessage.SendEvent("Smart Load");
                    break;
            }
        }

        public void AllowSceneTransfer() => m_canTransferScene = true;

        private void OnAnimationEnd(object sender, EventActionArgs eventArgs)
        {
            SceneManager.UnloadSceneAsync(gameObject.scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }

        private IEnumerator ExecuteLoadUnloadScene()
        {
            var time = 0f;
            m_animation.PlayStart();
            var endOfFrame = new WaitForEndOfFrame();

            Debug.LogError("False Positive: Unloading Scenes Start");
            //Wait for Unloading to Be done, Unload scenes one by one
            m_unloadThis = false;
            m_unloadOperations.Clear();
            scenesToUnload?.RemoveAll(x => x == string.Empty);
            AsyncOperation operation = null;
            for (int i = 0; i < (scenesToUnload?.Count ?? 0); i++)
            {
                Debug.LogError($"False Positive: \"{scenesToUnload[i]}\" Unload Start");
                if (scenesToUnload[i] != "")
                {
                    try
                    {
                        operation = SceneManager.UnloadSceneAsync(scenesToUnload[i]);
                    }
                    catch (System.Exception)
                    {
                        operation = null;
                    }

                    if (operation != null)
                    {
                        while (operation.isDone == false)
                        {
                            yield return endOfFrame;
                            time += Time.unscaledDeltaTime;
                        }
                    }
                    else
                    {
                        Debug.LogError("Attempt to unload invalid scene: " + scenesToUnload[i]);
                    }
                }
                Debug.LogError($"False Positive: \"{scenesToUnload[i]}\" Unload Done");
            }
            scenesToUnload?.Clear();
            Debug.LogError("False Positive: Unloading Scenes Done");


            Debug.LogError("False Positive: Loading Scenes Start");
            //Wait for Loading to Be done, Load scenes one by one
            m_loadOperations.Clear();
            scenesToLoad?.RemoveAll(x => x == string.Empty);
            for (int i = 0; i < (scenesToLoad?.Count ?? 0); i++)
            {
                Debug.LogError($"False Positive: \"{scenesToLoad[i]}\" Load Start");

                try
                {
                    operation = SceneManager.LoadSceneAsync(scenesToLoad[i], LoadSceneMode.Additive);
                }
                catch (System.Exception)
                {
                    operation = null;
                }

                if (operation != null)
                {
                    m_loadOperations.Add(operation);
                    operation.allowSceneActivation = false;
                    while (operation.progress < 0.9f)
                    {
                        Debug.LogError($"Progress Tracker: \"{scenesToLoad[i]}\" -- {operation.progress}");
                        yield return endOfFrame;
                        time += Time.unscaledDeltaTime;
                    }
                    Debug.LogError($"False Positive: \"{scenesToLoad[i]}\" Load Done");
                }
                else
                {
                    Debug.LogError("Attempt to load invalid scene: " + scenesToLoad[i]);
                }
            }
            scenesToLoad?.Clear();
            Debug.LogError("False Positive: Loading Scenes Done");
            m_animation.PlayEnd();

            if (loadType == LoadType.Force)
            {
                yield return new WaitForSeconds(2.25f);
                time += 2.25f;
            }

            Debug.LogError("False Positive: Scene Activation Start");
            for (int i = 0; i < m_loadOperations.Count; i++)
            {
                m_loadOperations[i].allowSceneActivation = true;
            }
            Debug.LogError("False Positive: Scene Activation Done");

            yield return endOfFrame;
            while (m_canTransferScene == false)
            {
                time += Time.unscaledDeltaTime;
                yield return endOfFrame;
            }

            Debug.LogError("False Positive: Scene Done Event Sent");
            SceneDone?.Invoke(this, EventActionArgs.Empty);
            Debug.LogError("False Positive: Scene Done Reaction Done");
            if (loadType == LoadType.Smart)
            {
                GameEventMessage.SendEvent("Load Done");
                yield return new WaitForSeconds(1f);
                time += 1f;
            }
            yield return endOfFrame;
            time += Time.unscaledDeltaTime;
            //Cant Call Unload Here for some reason, so i have to resort to using a flag to trigger the unloading
            m_unloadThis = true;
            Debug.Log($"Loading Time: {time}");
        }

        private void Awake()
        {
            if (m_isInitialized == false)
            {
                m_loadOperations = new List<AsyncOperation>();
                m_unloadOperations = new List<AsyncOperation>();
                m_isInitialized = true;
            }

            if (loadType == LoadType.Force)
            {
                m_animation.AnimationEnd += OnAnimationEnd;
            }

            GameplaySystem.SetInputActive(false);
        }

        private void Update()
        {
            if (m_unloadThis)
            {
                SceneManager.UnloadSceneAsync(m_loadingScene.sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                m_unloadThis = false;
            }
            GameplaySystem.SetInputActive(false);
        }

        private void OnDestroy()
        {
            m_animation.AnimationEnd -= OnAnimationEnd;
            LoadingDone?.Invoke(this, EventActionArgs.Empty);
            GameplaySystem.SetInputActive(true);
            Debug.Log("Loading Scene Destroyed");
        }
    }
}