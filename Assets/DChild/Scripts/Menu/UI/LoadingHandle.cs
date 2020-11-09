﻿using Doozy.Engine;
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

            //Wait for Unloading to Be done, Unload scenes one by one
            m_unloadThis = false;
            m_unloadOperations.Clear();
            scenesToUnload?.RemoveAll(x => x == string.Empty);
            for (int i = 0; i < (scenesToUnload?.Count ?? 0); i++)
            {
                var operation = SceneManager.UnloadSceneAsync(scenesToUnload[i]);
                while (operation.isDone == false)
                {
                    yield return endOfFrame;
                    time += Time.unscaledDeltaTime;
                }
            }
            scenesToUnload?.Clear();


            //Wait for Loading to Be done, Load scenes one by one
            m_loadOperations.Clear();
            scenesToLoad?.RemoveAll(x => x == string.Empty);
            for (int i = 0; i < (scenesToLoad?.Count ?? 0); i++)
            {
                var operation = SceneManager.LoadSceneAsync(scenesToLoad[i], LoadSceneMode.Additive);
                m_loadOperations.Add(operation);
                operation.allowSceneActivation = false;
                while (operation.progress < 0.9f)
                {
                    yield return endOfFrame;
                    time += Time.unscaledDeltaTime;
                }
            }
            scenesToLoad?.Clear();
            m_animation.PlayEnd();

            if (loadType == LoadType.Force)
            {
                yield return new WaitForSeconds(2.25f);
                time += 2.25f;
            }
            for (int i = 0; i < m_loadOperations.Count; i++)
            {
                m_loadOperations[i].allowSceneActivation = true;
            }

            yield return endOfFrame;
            while (m_canTransferScene == false)
            {
                time += Time.unscaledDeltaTime;
                yield return endOfFrame;
            }

            SceneDone?.Invoke(this, EventActionArgs.Empty);
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
            LoadingDone?.Invoke(this, EventActionArgs.Empty);
            Debug.Log("Loading Scene Destroyed");
        }
    }
}