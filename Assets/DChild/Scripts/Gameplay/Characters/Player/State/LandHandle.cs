using Refactor.DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    /// <summary>
    /// Replacement of PlacementTracker
    /// </summary>
    [System.Serializable]
    public class LandHandle : MonoBehaviour
    {
        private Animator m_animator;
        private string m_animationParameter;

        public void Initialize(Animator animator, AnimationParametersData animationParameters)
        {
            m_animator = animator;
            m_animationParameter = animationParameters.GetParameterLabel(AnimationParametersData.Parameter.Land);
        }

        public void Execute()
        {
            m_animator.SetTrigger(m_animationParameter);
        }
    }

}