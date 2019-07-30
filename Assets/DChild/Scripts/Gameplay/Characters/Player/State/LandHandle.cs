using DChild.Gameplay.Characters.Players.State;
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
        private IGroundednessState m_state;
        private IsolatedPhysics2D m_physics;

        [SerializeField]
        private FXSpawner m_fXSpawner;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Land);
            m_state = info.state;
            m_physics = info.physics;
        }

        public void Execute()
        {
            m_animator.SetTrigger(m_animationParameter);
            m_fXSpawner.SpawnFX();
            m_state.waitForBehaviour = true;
            m_physics.SetVelocity(Vector2.zero);
        }
    }

}