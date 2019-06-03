using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChildDebug.Gameplay.Characters.AI
{
    public class AITargetSetter : MonoBehaviour
    {
        [SerializeField]
        private Player m_character;
        [SerializeField] 
        private bool m_targetOnStart;

        private IAITargetingBrain m_brain;

        [Button("Set Target")]
        private void Target() => m_brain?.SetTarget(m_character);

        [Button("Cancel Target")]
        private void Cancel() => m_brain?.SetTarget(null);

        public void SetTarget()
        {
            m_brain = GetComponent<IAITargetingBrain>();
            if (m_targetOnStart)
            {
                m_brain.SetTarget(m_character);
            }
        }
    }
}