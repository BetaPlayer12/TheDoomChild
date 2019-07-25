using Holysoft.Collections;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    [System.Serializable]
    public class FallHandle : MonoBehaviour
    {
        [SerializeField, MaxValue(0)]
        private float m_velocityThreshold;
        [SerializeField, MinValue(0)]
        private float m_durationBeforeLongFall;

        private CountdownTimer m_timer;
        private Animator m_animator;
        private string m_animationParameter;
        private int m_speedY;

        public void Initialize(Animator animator, AnimationParametersData animationParameters)
        {
            m_animator = animator;
            m_animationParameter = animationParameters.GetParameterLabel(AnimationParametersData.Parameter.SpeedY);
            m_timer = new CountdownTimer(m_durationBeforeLongFall);
            m_timer.CountdownEnd += OnCountdownEnd;
        }

        private void OnCountdownEnd(object sender, EventActionArgs eventArgs)
        {
            m_animator.SetInteger(m_animationParameter, -2);
        }

        public void StartFall()
        {
            m_timer.Reset();
            m_animator.SetInteger(m_animationParameter, -1);
        }

        public void ResetValue()
        {
            m_animator.SetInteger(m_animationParameter, 0);
        }

        public void Execute(float deltaTime)
        {
            m_timer.Tick(deltaTime);
        }

        public bool isFalling(CharacterPhysics2D physics) => physics.velocity.y < m_velocityThreshold;
    }

}