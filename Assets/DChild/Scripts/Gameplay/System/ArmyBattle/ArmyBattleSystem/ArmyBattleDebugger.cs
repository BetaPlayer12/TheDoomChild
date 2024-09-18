using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleDebugger : MonoBehaviour
    {
        [SerializeField]
        private ArmyCharacterData[] m_playerRecruitedCharacters;
        [SerializeField]
        public ArmyBattleScenario m_battleScenario;

        private void Awake()
        {
            if (ArmyBattleSystem.BattleScenario == null)
            {
                ArmyBattleSystem.BattleScenario = m_battleScenario;
            }
        }
    }
}