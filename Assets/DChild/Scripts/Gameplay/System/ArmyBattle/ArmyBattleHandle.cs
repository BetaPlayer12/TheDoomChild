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

        [SerializeField]
        private ArmyController m_playerArmy;
        [SerializeField]
        private ArmyAI m_enemyArmy;

        private bool m_playerHasChosenAttackType;


        public void StartRound()
        {
            m_playerHasChosenAttackType = false;
            StartCoroutine(RoundRoutine());
        }

        private IEnumerator RoundRoutine()
        {
            yield return WaitForAttacks();
            m_battleResolver.ResolveBattle(m_playerArmy, m_enemyArmy);
            yield return m_visuals.StartBattleVisuals();
        }

        private IEnumerator WaitForAttacks()
        {
            do
            {
                yield return null;
            } while (m_playerHasChosenAttackType == false);
        }
    }
}