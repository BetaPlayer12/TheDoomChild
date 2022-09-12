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
    public class CameraChangeEventArgs : IEventActionArgs
    {
        public void Initialize(Camera camera)
        {
            this.camera = camera;
        }

        public Camera camera { get; private set; }
    }

    public class GameSystem : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private bool m_dontDestroyOnLoad;
#endif

        private static PoolManager m_poolManager;
        private static ConfirmationHandler m_confirmationHander;
        private static SceneLoader m_zoneLoader;
        private static AddressableSceneManager m_sceneManager;
        private static Cursor m_cursor;
        public static GameSettings settings { get; private set; }
        public static GameDataManager dataManager { get; private set; }
        public static IPoolManager poolManager => m_poolManager;
        public static Camera mainCamera { get; private set; }
        public static event EventAction<CameraChangeEventArgs> CameraChange;

        private static GameSystem m_instance;

        [SerializeField]
        private Cursor m_instanceCursor;
        [SerializeField]
        private GameIntroHandler m_introHandler;


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

        public static void ResetCursorPosition()
        {
            m_cursor.SetLockState(CursorLockMode.Locked);
            m_cursor.SetLockState(CursorLockMode.None);
        }

        public static bool RequestConfirmation(EventAction<EventActionArgs> listener, string message)
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
            GameplaySystem.ListenToNextSceneLoad();
            m_zoneLoader.LoadZone(sceneName, withLoadingScene);
            GameplaySystem.ClearCaches();
        }

        public static void LoadZone(string sceneName, bool withLoadingScene, Action CallAfterSceneDone)
        {
            GameplaySystem.ListenToNextSceneLoad();
            m_zoneLoader.LoadZone(sceneName, withLoadingScene, CallAfterSceneDone);
            GameplaySystem.ClearCaches();
        }

        public static bool IsCurrentZone(string sceneName) => m_zoneLoader.activeZone == sceneName;

#if UNITY_EDITOR
        public static void ForceCurrentZoneName(string sceneName) => m_zoneLoader.SetAsActiveZone(sceneName);
#endif

        public static void LoadMainMenu()
        {
            dataManager.InitializeCampaignSlotList();
            m_zoneLoader.LoadMainMenu();
        }

        private void Awake()
        {
            if (m_instance)
            {
                Destroy(gameObject);
            }
            else
            {
#if UNITY_EDITOR
                if (m_dontDestroyOnLoad)
                {
                    transform.parent = null;
                    DontDestroyOnLoad(this.gameObject);
                }
#endif

                m_instance = this;
                settings = GetComponentInChildren<GameSettings>();
                m_confirmationHander = GetComponentInChildren<ConfirmationHandler>();
                m_zoneLoader = GetComponentInChildren<SceneLoader>();
                dataManager = GetComponentInChildren<GameDataManager>();
                m_poolManager = GetComponentInChildren<PoolManager>();
                m_poolManager.Initialize();
                m_cursor = m_instanceCursor;
            }
        }

        private void Start()
        {
            settings.Initialize();
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