using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyBattleScenario
    {
        [SerializeField]
        private ArmyAIData m_enemyToBattle;
        [SerializeField]
        private ArmyBattleLocation m_location;

        public ArmyAIData enemyToBattle => m_enemyToBattle;
        public ArmyBattleLocation location => m_location;
    }
}