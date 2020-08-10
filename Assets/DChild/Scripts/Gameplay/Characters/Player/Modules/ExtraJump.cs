using DChild.Gameplay.Characters.Players.Behaviour;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ExtraJump : MonoBehaviour, IResettableBehaviour, IComplexCharacterModule, ICancellableBehaviour
    {
        [SerializeField, MinValue(1)]
        private int m_count;
        [SerializeField, MinValue(0)]
        private float m_power;

        private Rigidbody2D m_rigidbody;
        private Animator m_animator;
        private int m_animationParameter;
        private int m_currentCount;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_rigidbody = info.rigidbody;
            m_currentCount = m_count;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Jump);
        }

        public bool HasExtras() => m_currentCount > 0;

        public void Cancel()
        {
            m_rigidbody.velocity = Vector2.zero;
            m_animator.SetBool(m_animationParameter, false);
        }

        public void EndExecution()
        {
            m_animator.SetBool(m_animationParameter, false);
        }

        public void Reset() => m_currentCount = m_count;

        public void Execute()
        {
            if (m_currentCount > 0)
            {
                m_currentCount--;
                m_rigidbody.velocity = new Vector2(0, m_power);
                m_animator.SetBool(m_animationParameter, true);
            }
        }
    }
}
