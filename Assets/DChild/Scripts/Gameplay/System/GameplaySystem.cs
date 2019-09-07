using DChild.Configurations;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Cinematics;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Databases;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.SoulEssence;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Systems.Serialization;
using DChild.Gameplay.VFX;
using DChild.Inputs;
using DChild.Serialization;
using Holysoft.Gameplay.UI;
using Sirenix.OdinInspector;
using System;
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
        private static DatabaseManager m_databaseManager;

        private static IGameplaySystemModule[] m_modules;
        private static IGameplayActivatable[] m_activatableModules;
        private static CombatManager m_combatManager;

        private static FXManager m_fxManager;
        private static Cinema m_cinema;
        private static World m_world;
        private static SimulationHandler m_simulation;
        private static PlayerManager m_playerManager;
        private static LootHandler m_lootHandler;
        private static GameplayModifiers m_modifiers;
        private static ZoneMoverHandle m_zoneMover;

        public static ICombatManager combatManager => m_combatManager;

        public static IFXManager fXManager => m_fxManager;
        public static IDatabaseManager databaseManager => m_databaseManager;
        public static ICinema cinema => m_cinema;
        public static IWorld world => m_world;
        public static ITime time => m_world;
        public static IPlayerManager playerManager => m_playerManager;
        public static ISimulationHandler simulationHandler => m_simulation;
        public static ILootHandler lootHandler => m_lootHandler;
        public static GameplayModifiers modifiers { get => m_modifiers; }

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
            GameSystem.SetCursorVisibility(false);
        }

        public static void PauseGame()
        {
            Time.timeScale = 0;
            m_playerManager?.DisableInput();
            isGamePaused = true;
            GameSystem.SetCursorVisibility(true);
        }

        public static void LoadGame(CampaignSlot campaignSlot)
        {

        }

        public static void MovePlayerToLocation(Character character, LocationData location, TravelDirection entranceType)
        {
            m_zoneMover.MoveCharacterToLocation(character, location, entranceType);
        }

        public static CampaignSlot SaveGame()
        {
            return null;
        }

        private void AssignModules()
        {
            AssignModule(out m_combatManager);
            AssignModule(out m_fxManager);
            AssignModule(out m_databaseManager);
            AssignModule(out m_lootHandler);
            AssignModule(out m_cinema);
            AssignModule(out m_world);
            AssignModule(out m_simulation);
            AssignModule(out m_playerManager);
            AssignModule(out m_zoneMover);
            //Debug.Log("Modules Assigned");
        }

        private void AssignModule<T>(out T module) where T : MonoBehaviour, IGameplaySystemModule => module = GetComponentInChildren<T>();

#if UNITY_EDITOR
        private void ValidateModule<T>() where T : MonoBehaviour, IGameplaySystemModule => this.ValidateChildComponent<T>();
#endif

        protected void Awake()
        {
            AssignModules();
            m_modules = GetComponentsInChildren<IGameplaySystemModule>();
            m_activatableModules = GetComponentsInChildren<IGameplayActivatable>();

            var initializables = GetComponentsInChildren<IGameplayInitializable>();
            for (int i = 0; i < initializables.Length; i++)
            {
                initializables[i].Initialize();
            }
            //m_fxManager.LoadDatabase(m_database);
            //m_fxManager.Initialize(); //Temporary for grasscutFX
        }

        private void Start()
        {
            //m_cinema.SetTrackingTarget(m_player.model);
            m_settings = GameSystem.settings?.gameplay ?? null;
            m_modifiers = new GameplayModifiers();
            isGamePaused = true;
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
            Debug.Log("Quit");
            Time.timeScale = 1;
        }
    }
}