﻿using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleSystem : MonoBehaviour
    {
        private static ArmyBattleParticipantsHandle m_participantsHandle;
        private static ArmyBattleHandle m_battleHandle;

        public static IArmyCombatHandle GetArmyCombatHandleOf(Army army) => m_participantsHandle.GetArmyCombatHandleOf(army);

        public static void StartNewBattle(ArmyComposition playerArmy, ArmyComposition enemyArmy)
        {
            if (m_battleHandle.hasOngoingBattle)
            {
                m_battleHandle.EndBattle();
            }

            m_battleHandle.InitializeBattle(playerArmy, enemyArmy);
            m_battleHandle.StartBattle();
        }

        public static void EndCurrentBattle()
        {
            m_battleHandle.EndBattle();
        }

        private void Awake()
        {
            m_participantsHandle = GetComponentInChildren<ArmyBattleParticipantsHandle>();
            m_participantsHandle.Initialize();
            m_battleHandle = GetComponentInChildren<ArmyBattleHandle>();
            m_battleHandle.InitializeParticipants(m_participantsHandle.player, m_participantsHandle.enemy);
        }
    }
}