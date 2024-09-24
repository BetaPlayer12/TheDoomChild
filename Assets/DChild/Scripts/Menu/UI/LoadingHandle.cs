using DChild.Gameplay;
using Doozy.Runtime.Nody;
using Doozy.Runtime.Signals;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        private FlowController m_flow;
        [SerializeField]
        private SignalSender m_loadStartSignal;
        [SerializeField]
        private SignalSender m_loadDoneSignal;
        [SerializeField]
        private LoadingAnimation m_animation;
        [SerializeField]
        private SceneInfo m_loadingScene;
        [SerializeField]
        private Image m_loadingImages;
        [SerializeField]
        private Sprite[] m_loadingSceneImages;

        public static LoadType loadType { get; private set; }

        #region SceneManager Load
        private static List<string> scenesToLoad;
        private static List<string> scenesToUnload;
        public static event EventAction<EventActionArgs> LoadingDone;
        public static event EventAction<EventActionArgs> SceneDone;

        private static List<AsyncOperation> m_loadOperations;
        private static List<AsyncOperation> m_unloadOperations;
        private static bool m_isInitialized;
        #endregion

        #region Addressable Load
        private static List<string> addressableScenesToLoad;
        private static List<string> addressableScenesToUnload;
        #endregion

        private bool m_unloadThis;
        private bool m_canTransferScene;

        public static bool isLoading { get; private set; }

        public static void SetLoadType(LoadType value) => loadType = value;

        public static void LoadScenes(params SceneInfo[] scenes)
        {
            if (scenesToLoad == null)
            {
                scenesToLoad = new List<string>();
                addressableScenesToLoad = new List<string>();
            }

            for (int i = 0; i < scenes.Length; i++)
            {
                var scene = scenes[i];
                if (scene.isAddressables)
                {
                    if (GameSystem.sceneManager.IsSceneLoaded(scene.sceneName) == false)
                        addressableScenesToLoad.Add(scene.sceneName);
                }
                else
                {
                    scenesToLoad.Add(scene.sceneName);
                }
            }
        }

        public static void UnloadScenes(params string[] scenes)
        {
            if (scenesToUnload == null)
            {
                scenesToUnload = new List<string>();
                addressableScenesToUnload = new List<string>();
            }

            for (int i = 0; i < scenes.Length; i++)
            {
                var scene = scenes[i];
                if (GameSystem.sceneManager.IsSceneLoaded(scene))
                {
                    addressableScenesToUnload.Add(scene);
                }
                else
                {
                    scenesToUnload.Add(scene);
                }
            }
        }

        public static void UnloadScenes(params SceneInfo[] scenes)
        {
            if (scenesToUnload == null)
            {
                scenesToUnload = new List<string>();
                addressableScenesToUnload = new List<string>();
            }

            for (int i = 0; i < scenes.Length; i++)
            {
                var scene = scenes[i];
                if (scene.isAddressables)
                {
                    addressableScenesToUnload.Add(scene.sceneName);
                }
                else
                {
                    scenesToUnload.Add(scene.sceneName);
                }
            }
        }

        public void DoLoad()
        {
            Debug.LogError("False Positive: Loading Start");
            StartCoroutine(ExecuteLoadUnloadScene());
        }

        public void SendEvents()
        {
            switch (loadType)
            {
                case LoadType.Force:
                    m_loadStartSignal.Payload.booleanValue = true;
                    break;
                case LoadType.Smart:
                    m_loadStartSignal.Payload.booleanValue = false;
                    break;
            }
            m_loadStartSignal.SendSignal();
        }

        public void AllowSceneTransfer() => m_canTransferScene = true;

        private void OnAnimationEnd(object sender, EventActionArgs eventArgs)
        {
            SceneManager.UnloadSceneAsync(gameObject.scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }

        private void ExecuteAddressableSceneLoading()
        {
            for (int i = 0; i < addressableScenesToLoad.Count; i++)
            {
                GameSystem.sceneManager.LoadSceneAsync(addressableScenesToLoad[i], false);
            }
        }

        private void ExecuteAddressableSceneUnloading()
        {
            for (int i = 0; i < addressableScenesToUnload.Count; i++)
            {
                GameSystem.sceneManager.UnloadSceneAsync(addressableScenesToUnload[i]);
            }
            addressableScenesToUnload.Clear();
        }

        private IEnumerator WaitForHandlingSceneOperations()
        {
            while (GameSystem.sceneManager.isHandlingSceneOperations)
                yield return null;
        }

        private IEnumerator ActivateLoadedAddressableScenes()
        {
            yield return GameSystem.sceneManager.ActivateLoadedScenesInOrder();
            addressableScenesToLoad.Clear();
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

            ExecuteAddressableSceneUnloading();
            yield return WaitForHandlingSceneOperations();
            ExecuteAddressableSceneLoading();
            yield return WaitForHandlingSceneOperations();
            yield return ActivateLoadedAddressableScenes();


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

            bool allScenesDoneLoading = false;
            while (allScenesDoneLoading)
            {
                allScenesDoneLoading = true;
                for (int i = 0; i < m_loadOperations.Count; i++)
                {
                    if (m_loadOperations[i].isDone == false)
                    {
                        allScenesDoneLoading = false;
                        break;
                    }
                }
                yield return endOfFrame;
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
                m_loadDoneSignal.SendSignal();
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
            isLoading = true;
            m_loadingImages.sprite = m_loadingSceneImages[UnityEngine.Random.Range(0, m_loadingSceneImages.Length)];

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

        private IEnumerator Start()
        {
            while (m_flow.initialized == false)
                yield return null;

            SendEvents();
        }

        private void Update()
        {
            if (m_unloadThis)
            {
                GameSystem.sceneManager.UnloadSceneAsync(m_loadingScene.sceneName);
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
            isLoading = false;
        }
    }
}