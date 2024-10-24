using DarkTonic.MasterAudio;
using DChild.Configurations;
using DChild.Gameplay.Cinematics;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems;
using DChild.Gameplay.VFX;
using DChild.Menu;
using DChild.Serialization;
using Holysoft.Event;
using System;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.Collections;
using DChild.Gameplay.SoulSkills;
using DChild.Gameplay.Systems.Serialization;

namespace DChild.Gameplay
{
    public class GameplayModifiers
    {
        public float minionSoulEssenceDrop = 1;
        public float SoulessenceAbsorption = 1;
    }

    public class GameplaySystem : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private bool m_dontDestroyOnLoad;
#endif
        [SerializeField]
        private bool m_doNotDeserializeOnAwake;
        [SerializeField]
        private bool m_doNotTeleportPlayerOnAwake;

        [SerializeField]
        private AudioListenerPositioner m_audioListener;

        private GameplaySettings m_settings;
        private static GameplaySystem m_instance;
        private static CampaignSlot m_campaignToLoad;
        private static GameplayModifiers m_modifiers;
        public static GameplayModifiers modifiers => m_modifiers;
        public static AudioListenerPositioner audioListener { get; private set; }

        #region Modules
        private static IGameplayActivatable[] m_activatableModules;
        private static IOptionalGameplaySystemModule[] m_optionalGameplaySystemModules;
        private static CombatManager m_combatManager;
        private static FXManager m_fxManager;
        private static Cinema m_cinema;
        private static World m_world;
        private static SimulationHandler m_simulation;
        private static DChild.Gameplay.Systems.PlayerManager m_playerManager;
        private static LootHandler m_lootHandler;
        private static CampaignSerializer m_campaignSerializer;
        private static ZoneMoverHandle m_zoneMover;
        private static HealthTracker m_healthTracker;
        private static GameplayUIHandle m_gameplayUIHandle;
        private static SoulSkillManager m_soulSkillManager;
        private static MinionManager m_minionManager;

        public static ICombatManager combatManager => m_combatManager;
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

        public static IPlayerManager playerManager => m_playerManager;
        public static ISimulationHandler simulationHandler => m_simulation;
        public static ILootHandler lootHandler => m_lootHandler;
        public static IHealthTracker healthTracker => m_healthTracker;
        public static IGameplayUIHandle gamplayUIHandle => m_gameplayUIHandle;
        public static ISoulSkillManager soulSkillManager => m_soulSkillManager;
        public static IMinionManager minionManager => m_minionManager;
        public static CampaignSerializer campaignSerializer => m_campaignSerializer;
        #endregion
        public static bool isGamePaused { get; private set; }

        public static void ResumeGame()
        {
            GameTime.UnregisterValueChange(m_instance, GameTime.Factor.Multiplication);
            m_playerManager?.EnableInput();
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
            SkeletonAnimationManager.Instance.UnpauseAllSpines();
            //PostProcess.SetActiveToAll(true);
        }

        public static void PauseGame()
        {
            GameTime.RegisterValueChange(m_instance, 0, GameTime.Factor.Multiplication);
            m_playerManager?.DisableInput();
            isGamePaused = true;
            GameSystem.SetCursorVisibility(true);
            MasterAudio.PauseEverything();
            SkeletonAnimationManager.Instance.PauseAllSpines();
            //PostProcess.SetActiveToAll(false);
        }

        public static void ClearCaches()
        {
            MasterAudio.StopMixer();
            //MasterAudio.StopAllPlaylists();
            m_cinema?.ClearLists();
            m_healthTracker?.RemoveAllTrackers();
            m_playerManager?.ClearCache();
        }

        public static void LoadGame(CampaignSlot campaignSlot, LoadingHandle.LoadType loadType)
        {
            m_campaignToLoad = campaignSlot;
            ClearCaches();
            PersistentDataManager.ApplySaveData(campaignSlot.dialogueSaveData, DatabaseResetOptions.KeepAllLoaded);
            m_healthTracker?.RemoveAllTrackers();
            LoadingHandle.SetLoadType(loadType);
            if (GameSystem.m_useGameModeValidator)
            {
                var WorldTypeVar = FindObjectOfType<WorldTypeManager>();

                WorldTypeVar.SetCurrentWorldType(m_campaignToLoad.location);

                switch (WorldTypeVar.CurrentWorldType)
                {
                    case WorldType.Underworld:
                        GameSystem.LoadZone(GameMode.Underworld, m_campaignToLoad.sceneToLoad, true);
                        break;
                    case WorldType.Overworld:
                        GameSystem.LoadZone(GameMode.Overworld, m_campaignToLoad.sceneToLoad, true);
                        break;
                    case WorldType.ArmyBattle:
                        GameSystem.LoadZone(GameMode.ArmyBattle, m_campaignToLoad.sceneToLoad, true);
                        break;
                }
            }
            else
            {
                GameSystem.LoadZone(m_campaignToLoad.sceneToLoad, true);
            }
            //Reload Items
            LoadingHandle.SceneDone += LoadGameDone;
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

        public static void SetInputActive(bool isActive)
        {
            //Reverted comment because this is used by
            //system to force player input based on game state i.e. when loading
            if (isActive)
            {
                m_playerManager?.EnableInput();
            }
            else
            {
                m_playerManager?.DisableInput();
            }
        }

        public static void ListenToNextSceneLoad()
        {
            LoadingHandle.LoadingDone += OnLoadingSceneDone;
        }

        private static void OnLoadingSceneDone(object sender, EventActionArgs eventArgs)
        {
            LoadingHandle.LoadingDone -= OnLoadingSceneDone;
            if (m_instance == null)
                return;
            m_playerManager.FreezePlayerPosition(false);
        }

        private static void LoadGameDone(object sender, EventActionArgs eventArgs)
        {
            LoadingHandle.SceneDone -= LoadGameDone;


            m_campaignSerializer.SetSlot(m_campaignToLoad);
            m_gameplayUIHandle.ResetGameplayUI();
            m_campaignSerializer.Load(SerializationScope.Gameplay, true);
            //m_playerManager.player.Revitilize();
            //m_playerManager.player.Reset();
        }

        private static void LockPlayerToSpawnPosition()
        {
            if (m_campaignToLoad == null)
                return;

            m_playerManager.player.transform.position = m_campaignToLoad.spawnPosition;
            m_playerManager.FreezePlayerPosition(true);
        }


        private void AssignModules()
        {
            AssignModule(out m_combatManager);
            AssignModule(out m_fxManager);
            AssignModule(out m_lootHandler);
            AssignModule(out m_cinema);
            AssignModule(out m_world);
            AssignModule(out m_simulation);
            AssignModule(out m_playerManager);
            AssignModule(out m_zoneMover);
            AssignModule(out m_campaignSerializer);
            AssignModule(out m_healthTracker);
            AssignModule(out m_gameplayUIHandle);
            AssignModule(out m_soulSkillManager);
            AssignModule(out m_minionManager);
        }

        private void AssignModule<T>(out T module) where T : MonoBehaviour, IGameplaySystemModule => module = GetComponentInChildren<T>();

        private IEnumerator DelayedShowGameplay()
        {
            int frameCount = 30;
            do
            {
                yield return null;
                frameCount--;
            } while (frameCount > 0);
            m_gameplayUIHandle.ShowGameplayUI(true);
        }

#if UNITY_EDITOR
        private void ValidateModule<T>() where T : MonoBehaviour, IGameplaySystemModule => this.ValidateChildComponent<T>();
#endif

        protected void Awake()
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

                if (m_doNotTeleportPlayerOnAwake == false && m_campaignToLoad != null)
                {
                    LockPlayerToSpawnPosition();
                }
            }
        }


        private void Start()
        {
            //m_cinema.SetTrackingTarget(m_player.model);
            audioListener = m_audioListener;
            m_settings = GameSystem.settings?.gameplay ?? null;
            m_modifiers = new GameplayModifiers();
            isGamePaused = false;
            if (m_campaignToLoad != null)
            {
                m_campaignSerializer.SetSlot(m_campaignToLoad);

                m_campaignToLoad = null;
            }

            StartCoroutine(DelayedShowGameplay());
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

        private void LateUpdate()
        {

        }

        private void OnApplicationQuit()
        {
            Time.timeScale = 1;
        }

        private void OnDestroy()
        {
            if (this == m_instance)
            {
                m_combatManager = null;
                m_fxManager = null;
                m_lootHandler = null;
                m_cinema = null;
                m_world = null;
                m_simulation = null;
                m_playerManager = null;
                m_zoneMover = null;
                m_activatableModules = null;
                GameTime.UnregisterValueChange(m_instance, GameTime.Factor.Multiplication);
            }
        }
    }
}