using DChild.Gameplay.ArmyBattle.Visualizer;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
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
        private ArmyController m_player;
        [SerializeField]
        private ArmyAI m_enemy;

        public ArmyController player => m_player;
        public ArmyController enemy => m_enemy;

        //Feels Like A Hack Solution ATM
        public static ArmyController GetTargetOf(ArmyController reference)
        {
            if (reference == Instance.m_player)
                return Instance.m_enemy;

            return Instance.m_player;
        }

        [Button]
        public void StartBattle()
        {
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

            if (endBattle == false)
            {
                m_turnHandle.TurnStart();
                m_specialSkillHandle.ResolveActiveSkills();
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

            //Create Player Army
            if (GameplaySystem.campaignSerializer != null)
            {

            }
            else if (DebugPlayerRecruitedCharacters != null)
            {
                var playerArmy = m_generator.GenerateArmy(DebugPlayerRecruitedCharacters);
                m_player.SetArmyToControl(playerArmy);
            }

            //Create Enemy Army
            if (BattleScenario.enemyToBattle != null)
            {
                var enemyArmy = m_generator.GenerateArmy(BattleScenario.enemyToBattle.armyData);
                m_enemy.SetArmyToControl(enemyArmy);
                m_enemy.SetAI(BattleScenario.enemyToBattle);
            }

            m_fightManager.Initialize(m_player.controlledArmy, m_enemy.controlledArmy);
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