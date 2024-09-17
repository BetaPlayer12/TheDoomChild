using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Visualizer
{
    public class ArmyFightManagerDebugger : MonoBehaviour
    {
        [SerializeField]
        private ArmyFightManager m_manager;
        [SerializeField]
        private DamageType[] m_playerUnits;
        [SerializeField]
        private DamageType[] m_enemyUnits;
        [SerializeField]
        private ArmyBattleCombatResult m_result;

        [Button]
        public void Initialize()
        {
            m_manager.Initialize(m_playerUnits, m_enemyUnits);
        }

        [Button]
        public void VisualizeCombat()
        {
            m_manager.VisualizeCombat(m_result);
        }

    }
}