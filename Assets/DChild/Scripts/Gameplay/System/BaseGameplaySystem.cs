using DarkTonic.MasterAudio;
using DChild;
using DChild.Configurations;
using DChild.Gameplay;
using DChild.Gameplay.Cinematics;
using DChild.Gameplay.Combat;
using DChild.Gameplay.SoulSkills;
using DChild.Gameplay.Systems;
using DChild.Gameplay.VFX;
using DChild.Menu;
using DChild.Serialization;
using Holysoft.Event;
using PixelCrushers.DialogueSystem;
using System;
using UnityEditor.PackageManager;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class BaseGameplaySystem : MonoBehaviour
    {
        [SerializeField]
        private bool m_doNotDeserializeOnAwake;
        [SerializeField]
        private AudioListenerPositioner m_audioListener;

        private GameplaySettings m_settings;
        private static BaseGameplaySystem m_instance;
        private static CampaignSlot m_campaignToLoad;
        private static GameplayModifiers m_modifiers;
        public static GameplayModifiers modifiers => m_modifiers;

        private static CampaignSerializer m_campaignSerializer;

        public static CampaignSerializer campaignSerializer => m_campaignSerializer;

        public static AudioListenerPositioner audioListener { get; private set; }

        [SerializeField]
        private static WorldTypeManager m_worldTypeManager;

        #region Modules
        private static IGameplayActivatable[] m_activatableModules;
        private static IOptionalGameplaySystemModule[] m_optionalGameplaySystemModules;
        private static FXManager m_fxManager;
        private static Cinema m_cinema;
        private static World m_world;

        private static BaseGameplayUIHandle m_baseGameplayUIHandle;

        public static bool isGamePaused { get; private set; }

        public static BaseGameplayUIHandle gamplayUIHandle => m_baseGameplayUIHandle;
        public static IFXManager fXManager => m_fxManager;
        public static ICinema cinema => m_cinema;
        public static IWorld world => m_world;
        public static ITime time
        {
            get
            {
                if (m_world == null)
                {
                    return new TimeInfo(Time.timeScale, Time.deltaTime, Time.fixedDeltaTime);
                }
                else
                {
                    return m_world;
                }
            }
        }
        #endregion

        private void AssignModules()
        {
            AssignModule(out m_fxManager);
            AssignModule(out m_cinema);
            AssignModule(out m_world);
            AssignModule(out m_campaignSerializer);
            AssignModule(out m_baseGameplayUIHandle);
        }

        private void AssignModule<T>(out T module) where T : MonoBehaviour, IGameplaySystemModule => module = GetComponentInChildren<T>();

        public static WorldType GetCurrentWorldType()
        {
            return WorldType.Underworld;
        }

        public static void ResumeGame()
        {
            GameTime.UnregisterValueChange(m_instance, GameTime.Factor.Multiplication);
            isGamePaused = false;
            GameSystem.SetCursorVisibility(false);

            try
            {
                MasterAudio.UnpauseEverything();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public static void PauseGame()
        {
            GameTime.RegisterValueChange(m_instance, 0, GameTime.Factor.Multiplication);
            isGamePaused = true;
            GameSystem.SetCursorVisibility(true);
            MasterAudio.PauseEverything();
            SkeletonAnimationManager.Instance.PauseAllSpines();
        }

        public static void ClearCaches()
        {
            MasterAudio.StopMixer();
            m_cinema?.ClearLists();
        }

        public static void LoadGame(CampaignSlot campaignSlot, LoadingHandle.LoadType loadType)
        {
            m_campaignToLoad = campaignSlot;
            ClearCaches();
            PersistentDataManager.ApplySaveData(campaignSlot.dialogueSaveData, DatabaseResetOptions.KeepAllLoaded);
            LoadingHandle.SetLoadType(loadType);
            GameSystem.LoadZone(m_campaignToLoad.sceneToLoad, true);
            //Reload Items
            LoadingHandle.SceneDone += LoadGameDone;
        }

        private static void LoadGameDone(object sender, EventActionArgs eventArgs)
        {
            LoadingHandle.SceneDone -= LoadGameDone;


            m_campaignSerializer.SetSlot(m_campaignToLoad);
            //m_baseGameplayUIHandle.ResetGameplayUI();
            m_campaignSerializer.Load(SerializationScope.Gameplay, true);
        }

        public static void ReloadGame()
        {
            LoadGame(campaignSerializer.slot, LoadingHandle.LoadType.Force);
        }

        public static void SetCurrentCampaign(CampaignSlot campaignSlot)
        {
            if (m_instance)
            {
                LoadGame(campaignSlot, LoadingHandle.LoadType.Force);
            }
            else
            {
                m_campaignToLoad = campaignSlot;
            }
        }

        protected void Awake()
        {
            if (m_instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Base Gameplay Awake");

                m_instance = this;
                AssignModules();
                m_activatableModules = GetComponentsInChildren<IGameplayActivatable>();
                var initializables = GetComponentsInChildren<IGameplayInitializable>();
                for (int i = 0; i < initializables.Length; i++)
                {
                    initializables[i].Initialize();
                }
                if (m_campaignToLoad != null)
                {
                    m_campaignSerializer.SetSlot(m_campaignToLoad);
                }

                if (m_doNotDeserializeOnAwake == false)
                {
                    m_campaignSerializer.Load(SerializationScope.Gameplay, true);
                }
                Debug.Log("Base Gameplay Awake Done");
            }
        }

        private void Start()
        {
            Debug.Log("Base Gameplay Start");

            audioListener = m_audioListener;
            m_settings = GameSystem.settings?.gameplay ?? null;
            m_modifiers = new GameplayModifiers();
            isGamePaused = false;
            if (m_campaignToLoad != null)
            {
                m_campaignSerializer.SetSlot(m_campaignToLoad);

                m_campaignToLoad = null;
            }
            Debug.Log("Base Gameplay Start Done");
        }

        private void OnEnable()
        {
            for (int i = 0; i < m_activatableModules.Length; i++)
            {
                m_activatableModules[i].Enable();
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < m_activatableModules.Length; i++)
            {
                m_activatableModules[i].Disable();
            }
        }

        private void OnApplicationQuit()
        {
            Time.timeScale = 1;
        }

        private void OnDestroy()
        {
            if (this == m_instance)
            {
                m_fxManager = null;
                m_cinema = null;
                m_world = null;
                m_activatableModules = null;
                GameTime.UnregisterValueChange(m_instance, GameTime.Factor.Multiplication);
            }
        }
    }
}