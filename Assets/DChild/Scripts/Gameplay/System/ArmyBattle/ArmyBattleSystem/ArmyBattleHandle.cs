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

        private ArmyInstanceCombatHandle m_player;
        private ArmyInstanceCombatHandle m_enemy;
        private ArmyAI m_enemyAI;

        private bool m_playerHasChosenAttackType;
        private bool m_isListeningToEntities;
        private bool m_isPlayerArmyAlive;
        private bool m_isEnemyArmyAlive;
        private bool m_hasOngoingBattle;

        public int m_round;

        public bool hasOngoingBattle => m_hasOngoingBattle;

        public event EventAction<EventActionArgs> BattleStart;
        public event EventAction<EventActionArgs> RoundStart;
        public event EventAction<EventActionArgs> RoundEnd;
        public event EventAction<EventActionArgs> BattleEnd;

       
        public void InitializeParticipants(ArmyInstanceCombatHandle player, ArmyInstanceCombatHandle enemy)
        {
            if (m_player != null)
            {
                m_player.armyController.AttackChosen -= OnPlayerAttackChosen;
                m_player.armyController.AbilityChosen -= OnPlayerAbilityChosen;
            }

            m_player = player;
            m_player.armyController.AttackChosen += OnPlayerAttackChosen;
            m_player.armyController.AbilityChosen += OnPlayerAbilityChosen;
            m_enemy = enemy;
            m_enemyAI = (ArmyAI)enemy.armyController;
        }

        public void InitializeBattle(ArmyComposition playerArmy, ArmyComposition enemyArmy)
        {
            m_player.armyController.controlledArmy.SetArmyComposition(playerArmy);
            m_enemy.armyController.controlledArmy.SetArmyComposition(enemyArmy);
            m_visuals.InitializeArmyVisuals(m_player.armyController, m_enemy.armyController);
        }

        public void StartBattle()
        {
            if (m_hasOngoingBattle == false)
            {
                m_round = 1;
                SubscribeToArmies();
                BattleStart?.Invoke(this, EventActionArgs.Empty);
                StartCoroutine(BattleRoutine());
                m_hasOngoingBattle = true;
            }
        }

        public void EndBattle()
        {
            if (m_hasOngoingBattle)
            {
                m_round = 0;
                m_hasOngoingBattle = false;
                UnsubscribeToArmies();
                StopAllCoroutines();
                BattleEnd?.Invoke(this, EventActionArgs.Empty);
            }
        }

        private void UnsubscribeToArmies()
        {
            if (m_isListeningToEntities)
            {
                //m_player.troopCount.Death -= OnPlayerArmyLose;
                //m_enemy.troopCount.Death -= OnEnemyArmyLose;
                m_isListeningToEntities = false;
            }
        }

        private void SubscribeToArmies()
        {
            if (m_isListeningToEntities == false)
            {
                //m_player.troopCount.Death += OnPlayerArmyLose;
                //m_enemy.troopCount.Death += OnEnemyArmyLose;
                m_isListeningToEntities = true;
            }
        }

        private void InitalizeArmyStates()
        {
            m_player.InitializeArmy();
            m_enemy.InitializeArmy();
            m_isPlayerArmyAlive = true;
            m_isEnemyArmyAlive = true;
        }

        private IEnumerator BattleRoutine()
        {
            CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, "Battle Start");
            InitalizeArmyStates();
            do
            {
                ResetArmiesForNextRound();
                yield return RoundRoutine();
            } while (HasWinner() == false);

            DeclareWinner();
            EndBattle();
            CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, "Battle End");

            void ResetArmiesForNextRound()
            {
                m_player.Reset();
                m_enemy.Reset();
                m_playerHasChosenAttackType = false;
            }
        }

        private IEnumerator RoundRoutine()
        {
            CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, "New Round Start");
            RoundStart?.Invoke(this, EventActionArgs.Empty);

            do
            {
                m_enemyAI.ChooseAttack(m_round);
                if (m_player.canAttack)
                {
                    yield return WaitForAttacks();
                }
                m_battleResolver.ResolveBattle(m_player, m_enemy);

                //Battle Animations should be done here
                yield return m_visuals.StartBattleVisuals(m_player.armyController, m_enemy.armyController);
                m_player.HandleAttackEnd();
                m_enemy.HandleAttackEnd();
            } while (m_enemy.canAttack || m_player.canAttack);



            RoundEnd?.Invoke(this, EventActionArgs.Empty);
            m_round++;
            CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, "Round End");
        }

        private IEnumerator WaitForAttacks()
        {
            do
            {
                yield return null;
            } while (m_playerHasChosenAttackType == false);
        }

        private bool HasWinner() => m_isPlayerArmyAlive == false || m_isPlayerArmyAlive == false;

        private void DeclareWinner()
        {
            switch (m_isPlayerArmyAlive, m_isEnemyArmyAlive)
            {
                case (true, false):
                    CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, "Player Win");
                    m_visuals.SetArmyAnimationToCelebrate(m_player.armyController.controlledArmy, "Player Win");
                    break;
                case (false, true):
                    CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, "Player Lose");
                    m_visuals.SetArmyAnimationToCelebrate(m_enemy.armyController.controlledArmy, "Player Lose");

                    break;
                case (false, false):
                    CustomDebug.Log(CustomDebug.LogType.System_ArmyBattle, "It A Draw!!");
                    m_visuals.SetArmyAnimationToCelebrate(null, "DRAW");
                    break;
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

        private void OnPlayerAttackChosen(object sender, ArmyAttackEvent eventArgs)
        {
            m_playerHasChosenAttackType = true;
        }

        private void OnPlayerAbilityChosen(object sender, ArmyAbilityEvent eventArgs)
        {
            eventArgs.ability.UseAbility(m_player.armyController.controlledArmy, m_enemy.armyController.controlledArmy);
            eventArgs.ability.SetAvailability(false);
        }

        private void Awake()
        {

        }

        //Temporary Function for Testing Only
        [ContextMenu("Initialize Battle")]
        public void InitializeBattle()
        {
            InitalizeArmyStates();
            SubscribeToArmies();
            m_visuals.InitializeArmyVisuals(m_player.armyController, m_enemy.armyController);
        }

        [ContextMenu("Start Round")]
        public void StartRound()
        {
            StopAllCoroutines();
            m_playerHasChosenAttackType = false;
            StartCoroutine(RoundRoutine());
        }
    }
}