using DChild.Configurations;
using DChild.Gameplay.Pooling;
using DChild.Menu;
using Holysoft.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild
{
    public class GameSystem : MonoBehaviour
    {
        private static PoolManager m_poolManager;
        private static ConfirmationHandler m_confirmationHander;
        private static SceneLoader m_zoneLoader;
        public static GameSettings settings { get; private set; }
        public static GameDataManager dataManager { get; private set; }

        public static IPoolManager poolManager => m_poolManager;

        [SerializeField]
        private GameIntroHandler m_introHandler;

        public static void RequestConfirmation(EventAction<EventActionArgs> listener, string message)
        {
            m_confirmationHander.RequestConfirmation(listener, message);
        }

        public static void LoadZone(string sceneName, bool withLoadingScene)
        {
            m_zoneLoader.LoadZone(sceneName, withLoadingScene);
        }

        public static bool IsCurrentZone(string sceneName) => m_zoneLoader.activeZone == sceneName;

        public static void LoadMainMenu() => m_zoneLoader.LoadMainMenu();

        private void Awake()
        {
            settings = GetComponentInChildren<GameSettings>();
            m_confirmationHander = GetComponentInChildren<ConfirmationHandler>();
            m_zoneLoader = GetComponentInChildren<SceneLoader>();
            dataManager = GetComponentInChildren<GameDataManager>();
            m_poolManager = GetComponentInChildren<PoolManager>();
        }

        private void Start()
        {
            m_introHandler.Execute();
        }
    }

}