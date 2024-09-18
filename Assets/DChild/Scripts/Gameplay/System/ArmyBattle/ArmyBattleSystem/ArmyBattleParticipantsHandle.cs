using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyBattleParticipantsHandle : MonoBehaviour
    {
        [SerializeField]
        private ArmyController m_player;
        [SerializeField]
        private ArmyAI m_enemy;

        private ArmyInstanceCombatHandle m_playerCombatHandle;
        private ArmyInstanceCombatHandle m_enemyCombatHandle;

        public ArmyInstanceCombatHandle player => m_playerCombatHandle;
        public ArmyInstanceCombatHandle enemy => m_enemyCombatHandle;

        public void Initialize()
        {
            m_playerCombatHandle = new ArmyInstanceCombatHandle(m_player);
            m_enemyCombatHandle = new ArmyInstanceCombatHandle(m_enemy);
        }

        public IArmyCombatHandle GetArmyCombatHandleOf(Army army)
        {
            if (m_player.army == army)
                return m_playerCombatHandle;

            if (m_enemy.army == army)
                return m_enemyCombatHandle;

            throw new System.Exception($"{army.ToString()} Does is not a participant in the Battle");
        }
    }
}