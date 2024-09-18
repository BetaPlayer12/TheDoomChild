using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleDebugger : MonoBehaviour
    {
        [SerializeField, AssetSelector]
        private ArmyCharacterData[] m_playerRecruitedCharacters;
        [SerializeField]
        public ArmyBattleScenario m_battleScenario;

        private void Awake()
        {
            if (ArmyBattleSystem.BattleScenario == null)
            {
                ArmyBattleSystem.BattleScenario = m_battleScenario;
            }
            ArmyBattleSystem.DebugPlayerRecruitedCharacters = new RecruitedCharacterList(m_playerRecruitedCharacters);
        }
    }
}