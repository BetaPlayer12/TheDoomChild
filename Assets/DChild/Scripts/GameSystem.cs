﻿using DChild.Configurations;
using DChild.Gameplay;
using DChild.Gameplay.Pooling;
using DChild.Menu;
using Holysoft.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild
{
    public struct CameraChangeEventArgs : IEventActionArgs
    {
        public CameraChangeEventArgs(Camera camera) : this()
        {
            this.camera = camera;
        }

        public Camera camera { get; }
    }

    public class GameSystem : MonoBehaviour
    {
        private static PoolManager m_poolManager;
        private static ConfirmationHandler m_confirmationHander;
        private static SceneLoader m_zoneLoader;
        private static Cursor m_cursor;
        public static GameSettings settings { get; private set; }
        public static GameDataManager dataManager { get; private set; }
        public static IPoolManager poolManager => m_poolManager;
        public static Camera mainCamera { get; private set; }
        public static event EventAction<CameraChangeEventArgs> CameraChange;


        [SerializeField]
        private Cursor m_instanceCursor;
        [SerializeField]
        private GameIntroHandler m_introHandler;

        public static void SetCamera(Camera camera)
        {
            mainCamera = camera;
            CameraChange?.Invoke(null, new CameraChangeEventArgs(mainCamera));
        }

        public static void SetCursorVisibility(bool isVisible)
        {
            m_cursor?.SetVisibility(isVisible);
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
            m_zoneLoader.LoadZone(sceneName, withLoadingScene);
            GameplaySystem.ClearCaches();
        }

        public static bool IsCurrentZone(string sceneName) => m_zoneLoader.activeZone == sceneName;

#if UNITY_EDITOR
        public static void ForceCurrentZoneName(string sceneName) => m_zoneLoader.SetAsActiveZone(sceneName);
#endif

        public static void LoadMainMenu() => m_zoneLoader.LoadMainMenu();

        private void Awake()
        {
            settings = GetComponentInChildren<GameSettings>();
            m_confirmationHander = GetComponentInChildren<ConfirmationHandler>();
            m_zoneLoader = GetComponentInChildren<SceneLoader>();
            dataManager = GetComponentInChildren<GameDataManager>();
            m_poolManager = GetComponentInChildren<PoolManager>();
            m_cursor = m_instanceCursor;
        }

        private void Start()
        {
            m_introHandler.Execute();
        }
    }

}