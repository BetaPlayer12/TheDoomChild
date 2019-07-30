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
        [SerializeField]
        private float m_velocityTreshold;

        private Animator m_animator;
        private string m_animationParameter;
        private string m_speedXParamater;
        private IGroundednessState m_state;
        private IsolatedPhysics2D m_physics;

        [SerializeField]
        private FXSpawner m_fXSpawner;

        private float m_previousYVelocity;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Land);
            m_speedXParamater = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX);
            m_state = info.state;
            m_physics = info.physics;
        }

        public void RecordVelocity() => m_previousYVelocity = m_physics.velocity.y;
        public void SetRecordedVelocity(Vector2 velocity) => m_previousYVelocity = velocity.y;

        public void Execute()
        {
            if (m_previousYVelocity <= m_velocityTreshold)
            {
                m_animator.SetTrigger(m_animationParameter);
                m_fXSpawner.SpawnFX();
                m_state.waitForBehaviour = true;
                m_animator.SetInteger(m_speedXParamater, 0);
                m_physics.SetVelocity(Vector2.zero);
            }
        }
    }

}