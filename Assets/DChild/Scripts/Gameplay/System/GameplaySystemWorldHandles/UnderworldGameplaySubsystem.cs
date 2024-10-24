using DChild.Gameplay.Combat;
using DChild.Gameplay.SoulSkills;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.InputSystem.HID.HID;

namespace DChild.Gameplay.Systems
{
    public class UnderworldGameplaySubsystem : MonoBehaviour
    {
        [SerializeField]
        private bool m_doNotTeleportPlayerOnAwake;

        #region Modules
        private static IGameplayActivatable[] m_activatableModules;
        private static MinionManager m_minionManager;
        private static SoulSkillManager m_soulSkillManager;
        private static HealthTracker m_healthTracker;
        private static SimulationHandler m_simulation;
        private static CombatManager m_combatManager;
        private static LootHandler m_lootHandler;
        private static DChild.Gameplay.Systems.PlayerManager m_playerManager;
        private static UnderworldGameplayUIHandle m_gameplayUIHandle;

        public static IMinionManager minionManager => m_minionManager;
        public static ISoulSkillManager soulSkillManager => m_soulSkillManager;
        public static IHealthTracker healthTracker => m_healthTracker;

        public static ISimulationHandler simulationHandler => m_simulation;
        public static ICombatManager combatManager => m_combatManager;
        public static ILootHandler lootHandler => m_lootHandler;

        public static IPlayerManager playerManager => m_playerManager;
        private static UnderworldGameplayUIHandle gameplayUIHandle => m_gameplayUIHandle;

        #endregion
        public static void ResumeGame()
        {
            m_playerManager?.EnableInput();
        }

        public static void ClearCaches()
        {
            m_healthTracker?.RemoveAllTrackers();
            m_playerManager?.ClearCache();
        }

        public static void PauseGame()
        {
            m_playerManager?.DisableInput();
        }
        public static void LoadGame()
        {
            m_healthTracker?.RemoveAllTrackers();
        }

        public static PlayerManager GetUnderworldPlayerManager()
        {
            return m_playerManager;
        }
        private static void LockPlayerToSpawnPosition()
        {
            if (GameplaySystem.campaignSerializer.slot == null)
                return;

            m_playerManager.player.transform.position = GameplaySystem.campaignSerializer.slot.spawnPosition;
            m_playerManager.FreezePlayerPosition(true);
        }

        private void AssignModule<T>(out T module) where T : MonoBehaviour, IGameplaySystemModule => module = GetComponentInChildren<T>();

        private void AssignModules()
        {
            AssignModule(out m_combatManager);
            AssignModule(out m_lootHandler);
            AssignModule(out m_simulation);
            AssignModule(out m_playerManager);
            AssignModule(out m_healthTracker);
            AssignModule(out m_soulSkillManager);
            AssignModule(out m_minionManager);
        }

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

        private void Awake()
        {
            AssignModules();

            if (m_doNotTeleportPlayerOnAwake == false)
            {
                LockPlayerToSpawnPosition();
            }
        }

        private void Start()
        {
            StartCoroutine(DelayedShowGameplay());
        }

        private void OnDestroy()
        {
            m_combatManager = null;
            m_lootHandler = null;
            m_simulation = null;
            m_playerManager = null;
            m_activatableModules = null;
        }
    }
}

