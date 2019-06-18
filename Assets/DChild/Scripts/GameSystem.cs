using DChild.Configurations;
using DChild.Gameplay.Pooling;
using DChild.Menu;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild
{
    public class GameSystem : MonoBehaviour
    {
        private static PoolManager m_poolManager;
        private static LoadingHandle m_loadingScreen;
        private static ConfirmationHandler m_confirmationHander;
        public static GameSettings settings { get; private set; }
        public static GameDataManager dataManager { get; private set; }

        public static IPoolManager poolManager => m_poolManager;

        [SerializeField]
        private GameIntroHandler m_introHandler;

        public static void RequestConfirmation(EventAction<EventActionArgs> listener, string message)
        {
            m_confirmationHander.RequestConfirmation(listener, message);
        }

        public static void LoadScene(string sceneName)
        {
            m_loadingScreen.LoadScene(sceneName);
        }

        private void Awake()
        {
            settings = GetComponentInChildren<GameSettings>();
            m_loadingScreen = GetComponentInChildren<LoadingHandle>();
            m_confirmationHander = GetComponentInChildren<ConfirmationHandler>();
            dataManager = GetComponentInChildren<GameDataManager>();
            m_poolManager = GetComponentInChildren<PoolManager>();
        }

        private void Start()
        {
            m_introHandler.Execute();
        }
    }

}