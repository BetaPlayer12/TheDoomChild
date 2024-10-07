using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public abstract class ArmyBattleScenarioHandle : MonoBehaviour
    {
        private Army m_playerArmy;
        private Army m_enemyArmy;

        protected Army player => m_playerArmy;
        protected Army enemy => m_enemyArmy;
        public abstract void StartScenario();
        public abstract void UpdateScenario(int turnIndex);
        public abstract void EndScenario(bool playerWon);

        public void Initialize(Army player, Army enemy)
        {
            m_playerArmy = player;
            m_enemyArmy = enemy;
        }

    }
}