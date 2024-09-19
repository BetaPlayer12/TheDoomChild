using DChild.Gameplay.ArmyBattle.Visualizer;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleTurnHandle : MonoBehaviour
    {
        [SerializeField]
        private ArmyBattleCombatSimulator m_combatSimulator;
        [SerializeField]
        private ArmyFightManager m_fightManager;

        private ArmyController m_player;
        private ArmyController m_enemy;

        public event EventAction<EventActionArgs> OnTurnStart;
        public event EventAction<EventActionArgs> OnTurnEnd;

        [ShowInInspector, DisableInPlayMode, HideInEditorMode]
        private int m_turnCount;

        [Button]
        public void TurnStart()
        {
            m_turnCount++;
            OnTurnStart?.Invoke(this, EventActionArgs.Empty);
        }

        [Button]
        public void CommenceTurn()
        {
            var playerTurn = m_player.GetTurnAction(m_turnCount - 1);
            var enemyTurn = m_enemy.GetTurnAction(m_turnCount - 1);

            var result = m_combatSimulator.CalculateCombatResult(playerTurn, enemyTurn);
            m_fightManager.VisualizeCombat(result);
        }

        public void SetParticipants(ArmyController player, ArmyController enemy)
        {
            m_player = player;
            m_enemy = enemy;
        }

        private void OnFightEnd(object sender, EventActionArgs eventArgs)
        {
            EndTurn();
        }

        [Button]
        private void EndTurn()
        {
            OnTurnEnd?.Invoke(this, EventActionArgs.Empty);
        }

        private void Awake()
        {
            m_fightManager.OnFightEnd += OnFightEnd;
        }
    }
}