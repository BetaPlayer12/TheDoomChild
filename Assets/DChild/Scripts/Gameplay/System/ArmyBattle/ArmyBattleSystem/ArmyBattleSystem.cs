using DChild.Gameplay.ArmyBattle.SpecialSkills;
using DChild.Gameplay.ArmyBattle.UI;
using DChild.Gameplay.ArmyBattle.Visualizer;
using Doozy.Runtime.Signals;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleSystem : MonoBehaviour
    {
        public static ArmyBattleScenario BattleScenario;
        public static RecruitedCharacterList DebugPlayerRecruitedCharacters;

        private static ArmyBattleSystem Instance;

        [SerializeField]
        private ArmyBattleLocationInstantiator m_locationInstantiator;
        [SerializeField]
        private ArmyGenerator m_generator;

        [SerializeField]
        private ArmyBattleTurnHandle m_turnHandle;
        [SerializeField]
        private ArmyBattleSpecialSkillHandle m_specialSkillHandle;
        [SerializeField]
        private ArmyFightManager m_fightManager;
        [SerializeField]
        private ArmyBattleUIManager m_uiManager;

        [SerializeField]
        private PlayerArmyController m_player;
        [SerializeField]
        private ArmyAI m_enemy;

        [SerializeField, FoldoutGroup("Signals")]
        private SignalSender m_battleStartSignal;
        [SerializeField, FoldoutGroup("Signals")]
        private SignalSender m_battleEndSignal;
        [SerializeField, FoldoutGroup("Signals")]
        private SignalSender m_turnStartSignal;
        [SerializeField, FoldoutGroup("Signals")]
        private SignalSender m_turnEndSignal;

        private bool m_hasBattleStarted;
        private bool m_hasViableBattleSetup;
        private ArmyBattleScenarioHandle m_scenarioHandle;

        private bool canBattleBeStarted => m_hasViableBattleSetup && m_hasBattleStarted == false;

        public ArmyController player => m_player;
        public ArmyController enemy => m_enemy;

        public ArmyFightManager fightManager => m_fightManager;
        public ArmyBattleTurnHandle turnHandle => m_turnHandle;

        public static int GetCurrentTurnNumber() => Instance.turnHandle.currentTurn;
        public static ArmyController GetPlayer() => Instance.player;
        public static ArmyController GetEnemy() => Instance.enemy;

        //Feels Like A Hack Solution ATM
        public static ArmyController GetTargetOf(ArmyController reference)
        {
            if (reference == Instance.m_player)
                return Instance.m_enemy;

            return Instance.m_player;
        }

        public static Vector3 GetBattlationPositionOf(ArmyController reference)
        {
            if (reference == Instance.m_player)
                return Instance.fightManager.GetPlayerBattalionPosition();

            return Instance.fightManager.GetEnemyBattalionPosition();
        }

        public static void StartBattleGameplay() => Instance.StartBattle();
        public static void StartNewTurn() => Instance.StartTurn();

        [Button, ShowIf("@canBattleBeStarted == true")]
        public void StartBattle()
        {
            if (m_hasBattleStarted)
                return;

            m_battleStartSignal.SendSignal();
            StartTurn();
            m_scenarioHandle.UpdateScenario(); //For Trackers to Be Updated at Turn 1
            m_hasBattleStarted = true;
        }

        public void StartTurn()
        {
            m_uiManager.UpdatePlayerOptions();
            m_turnStartSignal.SendSignal();
            m_turnHandle.TurnStart();
        }

        private void OnTurnEnd(object sender, EventActionArgs eventArgs)
        {
            bool endBattle = false;
            if (m_player.controlledArmy.troopCount <= 0)
            {
                endBattle = true;
            }
            else if (m_enemy.controlledArmy.troopCount <= 0)
            {
                endBattle = true;
            }
            else if (m_player.HasViableTurnOptions() == false)
            {
                endBattle = true;
            }

            if (endBattle == false)
            {
                m_specialSkillHandle.ResolveActiveSkills();
                m_specialSkillHandle.ReinstanteActivateEffects();
                m_turnEndSignal.SendSignal();
                m_scenarioHandle.UpdateScenario();
            }
            else
            {
                m_battleEndSignal.Payload.booleanValue = m_enemy.controlledArmy.troopCount <= 0;
                m_battleEndSignal.SendSignal();
                StartCoroutine(EndScenarioRoutine());
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            m_turnHandle.SetParticipants(m_player, m_enemy);
            m_turnHandle.OnTurnEnd += OnTurnEnd;
        }

        private void Start()
        {
            if (BattleScenario == null)
            {
                //throw new Exception();
            }

            m_locationInstantiator.InstantiateLocation(BattleScenario.location);

            CreateParticipatingArmies();

            m_uiManager.Initialize(m_player, m_enemy);
            m_fightManager.Initialize(m_player.controlledArmy, m_enemy.controlledArmy);

            m_hasViableBattleSetup = true;

            InitializeBattleScenario();
        }

        private void InitializeBattleScenario()
        {
            var scenarioHandleInstance = Instantiate(BattleScenario.scenarioHandle) as GameObject;
            m_scenarioHandle = scenarioHandleInstance.GetComponent<ArmyBattleScenarioHandle>();
            m_scenarioHandle.Initialize(m_player.controlledArmy, m_enemy.controlledArmy);
            if (canBattleBeStarted)
            {
                StartCoroutine(StartScenarioRoutine());
            }
        }

        private IEnumerator StartScenarioRoutine()
        {
            yield return new WaitForSeconds(1.5f);
            m_scenarioHandle.StartScenario();
        }

        private IEnumerator EndScenarioRoutine()
        {
            yield return new WaitForSeconds(1.5f);
            m_scenarioHandle.EndScenario(m_player.controlledArmy.troopCount > 0);
        }

        private void CreateParticipatingArmies()
        {
            //Create Player Army
            if (GameplaySystem.campaignSerializer != null)
            {

            }
            else if (DebugPlayerRecruitedCharacters != null)
            {
                var playerArmy = m_generator.GenerateArmy(DebugPlayerRecruitedCharacters);
                m_player.SetArmyToControl(playerArmy);
            }
            Debug.Log("Player Army Created");

            //Create Enemy Army
            if (BattleScenario.enemyToBattle != null)
            {
                var enemyArmy = m_generator.GenerateArmy(BattleScenario.enemyToBattle.armyData);
                m_enemy.SetArmyToControl(enemyArmy);
                m_enemy.SetAI(BattleScenario.enemyToBattle);
                Debug.Log("Enemy Army Created");
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}