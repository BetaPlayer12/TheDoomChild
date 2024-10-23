using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyBattleScenario", menuName = "DChild/Gameplay/Army/BattleScenario")]
    public class ArmyBattleScenario : ScriptableObject
    {
        [SerializeField]
        private ArmyAIData m_enemyToBattle;
        [SerializeField]
        private ArmyBattleLocation m_location;
        [SerializeField]
        private GameObject m_scenarioHandle;

        public ArmyAIData enemyToBattle => m_enemyToBattle;
        public ArmyBattleLocation location => m_location;

        public GameObject scenarioHandle => m_scenarioHandle;
    }
}