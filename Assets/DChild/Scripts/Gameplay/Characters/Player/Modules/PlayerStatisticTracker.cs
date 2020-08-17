using DChild.Gameplay.Characters.Players.Behaviour;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerStatisticTracker : MonoBehaviour, IResettableBehaviour, IComplexCharacterModule
    {
        [System.Serializable]
        private struct Tracker
        {
            [SerializeField]
            private bool m_willTrack;
            [HideInInspector]
            public int animationParameter;

            public void Track(Animator animator, float value)
            {
                if (m_willTrack)
                {
                    animator.SetFloat(animationParameter, value);
                }
            }

            public void ChangeValue(Animator animator, float value)
            {
                animator.SetFloat(animationParameter, value);
            }
        }

        [SerializeField]
        private Tracker m_yVelocity;
        [SerializeField]
        private Tracker m_yInput;

        private Rigidbody2D m_rigidbody;
        private Animator m_animator;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_rigidbody = info.rigidbody;
            m_animator = info.animator;
            m_yVelocity.animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedY);
            m_yInput.animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.YInput);
        }

        public void Reset()
        {
            m_yVelocity.ChangeValue(m_animator, 0);
            m_yInput.ChangeValue(m_animator, 0);
        }

        public void Execute(InputTranslator input)
        {
            m_yVelocity.Track(m_animator, m_rigidbody.velocity.y);
            m_yInput.Track(m_animator, input.verticalInput);
        }
    }
}
