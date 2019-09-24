using DChild.Gameplay.Characters.Players.State;
using Holysoft.Collections;
using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    [System.Serializable]
    public class FallHandle : MonoBehaviour
    {
        [SerializeField, MaxValue(0)]
        private float m_velocityToConsiderAsFall;

        [SerializeField, Tooltip("Scale when Velocity Reaches TreshHold")]
        private float m_gravityScale;
        [SerializeField, MaxValue(0)]
        private float m_velocityThreshold;
        [SerializeField, MinValue(0)]
        private float m_durationBeforeLongFall;

        private CountdownTimer m_timer;
        private IsolatedPhysics2D m_physics;
        private Animator m_animator;
        private string m_animationParameter;
        private IGroundednessState m_state;
        private int m_speedY;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedY);
            m_timer = new CountdownTimer(m_durationBeforeLongFall);
            m_timer.CountdownEnd += OnCountdownEnd;
            m_state = info.state;
            m_physics = info.physics;
        }

        private void OnCountdownEnd(object sender, EventActionArgs eventArgs)
        {
            m_animator.SetInteger(m_animationParameter, -2);
        }

        public void StartFall()
        {
            if (!m_state.isGrounded) {
                m_timer.Reset();
                m_animator.SetInteger(m_animationParameter, -1);
                m_state.isFalling = true;
            }
            

        }

        public void ResetValue()
        {
            
            m_animator.SetInteger(m_animationParameter, 0);
            m_state.isFalling = false;
        }

        public void Execute(float deltaTime)
        {
            m_timer.Tick(deltaTime);
            if (m_physics.velocity.y < m_velocityThreshold)
            {
                m_physics.gravity.gravityScale = m_gravityScale;
            }
        }
       
        public bool isFalling(CharacterPhysics2D physics) => physics.velocity.y < m_velocityToConsiderAsFall;
      
    }

}