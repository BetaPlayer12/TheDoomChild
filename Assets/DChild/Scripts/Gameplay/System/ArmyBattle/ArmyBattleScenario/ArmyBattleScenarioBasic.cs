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
            throw new System.NotImplementedException();
        }

        public override void StartScenario()
        {
            m_introHandle.Execute();
        }

        public override void UpdateScenario(int turnIndex)
        {
            m_updateHandle.UpdateScenario(turnIndex);
        }
    }
}
