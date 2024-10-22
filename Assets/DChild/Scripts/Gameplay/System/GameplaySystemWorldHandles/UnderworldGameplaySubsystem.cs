using DChild.Gameplay.Combat;
using DChild.Gameplay.SoulSkills;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class UnderworldGameplaySubsystem : MonoBehaviour
    {
        #region Modules
        private static MinionManager m_minionManager;
        private static SoulSkillManager m_soulSkillManager;
        private static HealthTracker m_healthTracker;
        private static CombatManager m_combatManager;
        private static LootHandler m_lootHandler;
        private static DChild.Gameplay.Systems.PlayerManager m_playerManager;

        public static IMinionManager minionManager => m_minionManager;
        public static ISoulSkillManager soulSkillManager => m_soulSkillManager;
        public static IHealthTracker healthTracker => m_healthTracker;
        public static ICombatManager combatManager => m_combatManager;
        public static ILootHandler lootHandler => m_lootHandler;

        public static IPlayerManager playerManager => m_playerManager;

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

        private void AssignModule<T>(out T module) where T : MonoBehaviour, IGameplaySystemModule => module = GetComponentInChildren<T>();

        private void AssignModules()
        {
            AssignModule(out m_minionManager);
            AssignModule(out m_soulSkillManager);
            AssignModule(out m_healthTracker);
            AssignModule(out m_combatManager);
        }

        private void Awake()
        {
            AssignModules();
        }

    }
}

