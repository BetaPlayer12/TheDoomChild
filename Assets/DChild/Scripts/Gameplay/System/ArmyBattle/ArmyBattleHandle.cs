using DChildDebug;
using Holysoft.Event;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleHandle : MonoBehaviour
    {
        [SerializeField]
        private ArmyBattleResolver m_battleResolver;
        [SerializeField]
        private ArmyBattleVisuals m_visuals;

        [SerializeField]
        private ArmyController m_player;
        [SerializeField]
        private ArmyAI m_enemy;

        private bool m_playerHasChosenAttackType;
        private bool m_isListeningToEntities;
        private bool m_isPlayerArmyAlive;
        private bool m_isEnemyArmyAlive;

        public event EventAction<EventActionArgs> RoundStart;
        public event EventAction<EventActionArgs> BattleEnd;

        public void InitializeBattle(ArmyComposition playerArmy, ArmyComposition enemyArmy)
        {
            m_player.controlledArmy.SetArmyComposition(playerArmy);
            m_player.controlledArmy.Initialize();
            m_enemy.controlledArmy.SetArmyComposition(enemyArmy);
            m_enemy.controlledArmy.Initialize();
            TrackArmyStates();
            m_visuals.InitializeArmyVisuals(m_player, m_enemy);
        }

        //Temporary Function for Testing Only
        [ContextMenu("Initialize Battle")]
        public void InitializeBattle()
        {
            m_player.controlledArmy.Initialize();
            m_enemy.controlledArmy.Initialize();
            TrackArmyStates();
            m_visuals.InitializeArmyVisuals(m_player, m_enemy);
        }

        [ContextMenu("Start Round")]
        public void StartRound()
        {
            StopAllCoroutines();
            m_playerHasChosenAttackType = false;
            StartCoroutine(RoundRoutine());
        }

        private void TrackArmyStates()
        {
            m_isPlayerArmyAlive = true;
            m_isEnemyArmyAlive = true;

            if (m_isListeningToEntities == false)
            {
                m_player.controlledArmy.troopCount.Death += OnPlayerArmyLose;
                m_enemy.controlledArmy.troopCount.Death += OnEnemyArmyLose;
                m_isListeningToEntities = true;
            }
        }

        private void OnEnemyArmyLose(object sender, EventActionArgs eventArgs)
        {
            m_isEnemyArmyAlive = false;
        }

        private void OnPlayerArmyLose(object sender, EventActionArgs eventArgs)
        {
            m_isPlayerArmyAlive = false;
        }

        private IEnumerator RoundRoutine()
        {
            CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, "New Round Start");
            m_enemy.ChooseAttack();
            RoundStart?.Invoke(this, EventActionArgs.Empty);
            yield return WaitForAttacks();
            m_battleResolver.ResolveBattle(m_player, m_enemy);
            yield return m_visuals.StartBattleVisuals(m_player, m_enemy);

            switch (m_isPlayerArmyAlive, m_isEnemyArmyAlive)
            {
                case (true, true):
                    StartRound();
                    break;
                case (true, false):
                    CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, "Player Win");
                    BattleEnd?.Invoke(this, EventActionArgs.Empty);
                    m_visuals.SetArmyAnimationToCelebrate(m_player.controlledArmy, "Player Win");
                    break;
                case (false, true):
                    CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, "Player Lose");
                    BattleEnd?.Invoke(this, EventActionArgs.Empty);
                    m_visuals.SetArmyAnimationToCelebrate(m_enemy.controlledArmy, "Player Lose");

                    break;
                case (false, false):
                    CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, "It A Draw!!");
                    BattleEnd?.Invoke(this, EventActionArgs.Empty);
                    m_visuals.SetArmyAnimationToCelebrate(null, "DRAW");

                    break;
            }
        }

        private IEnumerator WaitForAttacks()
        {
            do
            {
                yield return null;
            } while (m_playerHasChosenAttackType == false);
        }


        private void OnPlayerAttackChosen(object sender, ArmyAttackEvent eventArgs)
        {
            m_playerHasChosenAttackType = true;
        }
        private void Awake()
        {
            m_player.AttackChosen += OnPlayerAttackChosen;
        }
    }
}