using DChild.Gameplay.ArmyBattle.Visualizer;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleSystem : MonoBehaviour
    {
        public static ArmyBattleScenario BattleScenario;
        public static RecruitedCharacterList DebugPlayerRecruitedCharacters;

        [SerializeField]
        private ArmyBattleLocationInstantiator m_locationInstantiator;
        [SerializeField]
        private ArmyGenerator m_generator;

        [SerializeField]
        private ArmyBattleTurnHandle m_turnHandle;
        [SerializeField]
        private ArmyFightManager m_fightManager;

        [SerializeField]
        private ArmyController m_player;
        [SerializeField]
        private ArmyAI m_enemy;

        [Button]
        public void StartBattle()
        {
            m_turnHandle.TurnStart();
        }

        private void Awake()
        {
            m_turnHandle.SetParticipants(m_player, m_enemy);
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

    }
}