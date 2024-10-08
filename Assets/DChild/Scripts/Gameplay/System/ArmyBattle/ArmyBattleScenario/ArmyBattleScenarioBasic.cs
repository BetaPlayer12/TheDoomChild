using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleScenarioBasic : ArmyBattleScenarioHandle
    {
        [SerializeField]
        private ArmyBattleScenarioIntroHandle m_introHandle;
        [SerializeField]
        private ArmyBattleScenarioUpdateHandle m_updateHandle;

        public void ForceStartBattleGameplay()
        {
            ArmyBattleSystem.StartBattleGameplay();
        }

        public override void EndScenario(bool playerWon)
        {
            if (playerWon)
            {
                Debug.Log("Army Battle Scenario: Player Won");
            }
            else
            {
                Debug.Log("Army Battle Scenario: Player Lost");
            }
        }

        public override void StartScenario()
        {
            m_introHandle.Execute();
        }

        public override void UpdateScenario()
        {
            m_updateHandle.UpdateScenario();
        }
    }
}
