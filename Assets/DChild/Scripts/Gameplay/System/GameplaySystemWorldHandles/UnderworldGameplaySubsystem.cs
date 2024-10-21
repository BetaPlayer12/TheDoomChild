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

        public static IMinionManager minionManager => m_minionManager;
        public static ISoulSkillManager soulSkillManager => m_soulSkillManager;
        public static IHealthTracker healthTracker => m_healthTracker;
        public static ICombatManager combatManager => m_combatManager;
        public static ILootHandler lootHandler => m_lootHandler;
        #endregion

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

