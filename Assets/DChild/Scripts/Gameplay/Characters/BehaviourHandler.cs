using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public class BehaviourHandler
    {
        private Coroutine m_activeBehaviour;
        private CombatCharacter m_character;

        public BehaviourHandler(CombatCharacter character)
        {
            m_activeBehaviour = null;
            m_character = character;
        }

        public void SetActiveBehaviour(Coroutine behaviour)
        {
            m_activeBehaviour = behaviour;
        }

        public void StopActiveBehaviour(ref bool waitForBehaviourEnd)
        {
            if (m_activeBehaviour != null)
            {
                waitForBehaviourEnd = false;
                m_character.StopCoroutine(m_activeBehaviour);
                m_activeBehaviour = null;
            }
        }
    }
}