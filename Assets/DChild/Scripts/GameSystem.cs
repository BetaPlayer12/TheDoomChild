using DChild.Configurations;
using DChild.Gameplay;
using DChild.Gameplay.Pooling;
using DChild.Menu;
using Holysoft.Event;
using Sirenix.Utilities;
using System;
using UnityEngine;

namespace DChild
{
<<<<<<< HEAD
=======
    public class CameraChangeEventArgs : IEventActionArgs
    {
        public void Initialize(Camera camera) 
        {
            this.camera = camera;
        }

        public Camera camera { get; private set; }
    }

>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
    public class GameSystem : MonoBehaviour
    {
        private static PoolManager m_poolManager;
        private static ConfirmationHandler m_confirmationHander;
        private static SceneLoader m_zoneLoader;
        public static GameSettings settings { get; private set; }
        public static GameDataManager dataManager { get; private set; }

<<<<<<< HEAD
        public static IPoolManager poolManager => m_poolManager;
=======
        private static GameSystem m_instance;
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23

        [SerializeField]
        private GameIntroHandler m_introHandler;

<<<<<<< HEAD
        public static void RequestConfirmation(EventAction<EventActionArgs> listener, string message)
=======

        public static void SetCamera(Camera camera)
        {
            mainCamera = camera;
            using (Cache<CameraChangeEventArgs> cacheEventArgs = Cache<CameraChangeEventArgs>.Claim())
            {
                cacheEventArgs.Value.Initialize(mainCamera);
                CameraChange?.Invoke(null, cacheEventArgs.Value);
                cacheEventArgs.Release();
            }
        }

        public static void SetCursorVisibility(bool isVisible)
        {
            m_cursor?.SetVisibility(isVisible);
        }

        public static bool RequestConfirmation(EventAction<EventActionArgs> listener, string message)
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
        {
            if (m_confirmationHander == null)
            {
                return false;
            }
            else
            {
                m_confirmationHander.RequestConfirmation(listener, message);
                return true;
            }
        }

        public static void LoadZone(string sceneName, bool withLoadingScene)
        {
            m_zoneLoader.LoadZone(sceneName, withLoadingScene);
            GameplaySystem.ClearCaches();
        }

        public static void LoadZone(string sceneName, bool withLoadingScene, Action CallAfterSceneDone)
        {
            m_zoneLoader.LoadZone(sceneName, withLoadingScene,CallAfterSceneDone);
            GameplaySystem.ClearCaches();
        }

        public static bool IsCurrentZone(string sceneName) => m_zoneLoader.activeZone == sceneName;

        public static void LoadMainMenu() => m_zoneLoader.LoadMainMenu();

        private void Awake()
        {
<<<<<<< HEAD
            settings = GetComponentInChildren<GameSettings>();
            m_confirmationHander = GetComponentInChildren<ConfirmationHandler>();
            m_zoneLoader = GetComponentInChildren<SceneLoader>();
            dataManager = GetComponentInChildren<GameDataManager>();
            m_poolManager = GetComponentInChildren<PoolManager>();
=======
            if (m_instance)
            {
                Destroy(gameObject);
            }
            else
            {
                m_instance = this;
                settings = GetComponentInChildren<GameSettings>();
                m_confirmationHander = GetComponentInChildren<ConfirmationHandler>();
                m_zoneLoader = GetComponentInChildren<SceneLoader>();
                dataManager = GetComponentInChildren<GameDataManager>();
                m_poolManager = GetComponentInChildren<PoolManager>();
                m_cursor = m_instanceCursor;
            }
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
        }

        private void Start()
        {
            m_introHandler.Execute();
        }

        private void nDestroy()
        {
            if (this == m_instance)
            {
                settings = null;
                m_confirmationHander = null;
                m_zoneLoader = null;
                dataManager = null;
                m_poolManager = null;
            }
        }
    }
}