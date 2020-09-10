using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class Levitation : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private ParticleSystem m_wingsFX;

        private ILevitateState m_state;
        private Rigidbody2D m_rigidbody;
        private float m_cacheGravity;
        private Animator m_animator;
        private int m_animationParameter;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_state = info.state;
            m_rigidbody = info.rigidbody;
            m_cacheGravity = m_rigidbody.gravityScale;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsLevitating);
        }

        public void Cancel()
        {
            m_wingsFX.Stop();
            m_wingsFX.Clear();
            m_state.isLevitating = false;
            m_rigidbody.gravityScale = m_cacheGravity;
            m_rigidbody.velocity = Vector2.zero;
            m_animator.SetBool(m_animationParameter, false);
        }

        public void Execute()
        {
            m_wingsFX.Play();
            m_state.isLevitating = true;
            m_cacheGravity = m_rigidbody.gravityScale;
            m_rigidbody.gravityScale = 0;
            m_rigidbody.velocity = Vector2.zero;
            m_animator.SetBool(m_animationParameter, true);
        }

        public void MaintainHeight()
        {
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0);
        }
    }
}
