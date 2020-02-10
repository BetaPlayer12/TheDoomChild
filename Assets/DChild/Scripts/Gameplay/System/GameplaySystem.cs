using DChild.Configurations;
<<<<<<< HEAD
using DChild.Gameplay.Characters.Players;
=======
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
using DChild.Gameplay.Cinematics;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Databases;
using DChild.Gameplay.Systems;
using DChild.Gameplay.VFX;
using DChild.Menu;
using DChild.Serialization;
<<<<<<< HEAD
using Holysoft.Gameplay.UI;
using Sirenix.OdinInspector;
=======
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
using UnityEngine;

namespace DChild.Gameplay
{
    public class GameplayModifiers
    {
        public float minionSoulEssenceDrop = 1;
    }

    public class GameplaySystem : MonoBehaviour
    {
        private GameplaySettings m_settings;
        private static GameplaySystem m_instance;
        private static CampaignSlot m_campaignToLoad;
        private static GameplayModifiers m_modifiers;
        public static GameplayModifiers modifiers => m_modifiers;

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
<<<<<<< HEAD
        private static GameplayModifiers m_modifiers;
=======
        private static CampaignSerializer m_campaignSerializer;
        private static ZoneMoverHandle m_zoneMover;
        private static HealthTracker m_healthTracker;
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23


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
        public static CampaignSerializer campaignSerializer => m_campaignSerializer;
        #endregion
        public static bool isGamePaused { get; private set; }

        #region Cinematic
        public static void PlayCutscene(Cutscene cutscene)
        {
            //player.EnableBrain(false);
            cutscene.InitializeScene();
            cutscene.Play();
        }

        public static void StopCutscene(Cutscene cutscene)
        {
            cutscene.Stop();
            cutscene.SetAsComplete();
            //player.EnableBrain(true);
        }
        #endregion

        public static void ResumeGame()
        {
            Time.timeScale = 1;
            m_playerManager?.EnableInput();
            isGamePaused = false;
        }

        public static void PauseGame()
        {
            Time.timeScale = 0;
            m_playerManager?.DisableInput();
            isGamePaused = true;
        }

        public static void ClearCaches()
        {
<<<<<<< HEAD

        }

        //Temp
        public SliderStatUI m_bossHealth;
        private static SliderStatUI bossHealth;

        public static void SetBossHealth(Character character)
=======
            m_cinema?.ClearLists();
            m_healthTracker?.RemoveAllTrackers();
        }

        public static void LoadGame(CampaignSlot campaignSlot)
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
        {
            m_campaignToLoad = campaignSlot;
            ClearCaches();
            m_healthTracker.RemoveAllTrackers();
            LoadingHandle.SetLoadType(LoadingHandle.LoadType.Smart);
            GameSystem.LoadZone(m_campaignToLoad.sceneToLoad.sceneName, true);
            m_playerManager.player.transform.position = m_campaignToLoad.spawnPosition;
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
<<<<<<< HEAD
            //Debug.Log("Modules Assigned");
=======
            AssignModule(out m_zoneMover);
            AssignModule(out m_campaignSerializer);
            AssignModule(out m_healthTracker);
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
        }

        private void AssignModule<T>(out T module) where T : MonoBehaviour, IGameplaySystemModule => module = GetComponentInChildren<T>();

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
                m_instance = this;
                AssignModules();
                m_activatableModules = GetComponentsInChildren<IGameplayActivatable>();

                var initializables = GetComponentsInChildren<IGameplayInitializable>();
                for (int i = 0; i < initializables.Length; i++)
                {
                    initializables[i].Initialize();
                }
            }
        }

        private void Start()
        {
            //m_cinema.SetTrackingTarget(m_player.model);
            m_settings = GameSystem.settings?.gameplay ?? null;
            m_modifiers = new GameplayModifiers();
            isGamePaused = true;
            if (m_campaignToLoad != null)
            {
                m_campaignSerializer.SetSlot(m_campaignToLoad);
                m_campaignToLoad = null;
            }
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
                m_combatManager = null;
                m_fxManager = null;
                m_lootHandler = null;
                m_cinema = null;
                m_world = null;
                m_simulation = null;
                m_playerManager = null;
                m_zoneMover = null;
                m_activatableModules = null;
            }
        }
    }
}