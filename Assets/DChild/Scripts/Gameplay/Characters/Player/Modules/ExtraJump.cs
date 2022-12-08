using DChild.Gameplay.Characters.Players.Behaviour;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ExtraJump : MonoBehaviour, IResettableBehaviour, IComplexCharacterModule, ICancellableBehaviour
    {
        [SerializeField, HideLabel]
        private ExtraJumpStatsInfo m_configuration;

        [SerializeField]
        private ParticleSystem m_doubleJumpFX;
        [SerializeField]
        private Transform m_particleSpawnPosition;

        private Rigidbody2D m_rigidbody;
        private Animator m_animator;
        private int m_animationParameter;
        private int m_currentCount;

        public event EventAction<EventActionArgs> ExecuteModule;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_rigidbody = info.rigidbody;
            m_currentCount = m_configuration.count;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Jump);
        }

        public void SetConfiguration(ExtraJumpStatsInfo info)
        {
            m_configuration.CopyInfo(info);
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

        public void Reset() => m_currentCount = m_configuration.count;

        public void Execute()
        {
            if (m_currentCount > 0)
            {
                m_currentCount--;
                m_rigidbody.velocity = new Vector2(0, m_configuration.power);
                m_animator.SetBool(m_animationParameter, true);
                m_doubleJumpFX.Play();

                ParticleSystem particle = Instantiate(m_doubleJumpFX);
                particle.transform.position = m_particleSpawnPosition.position;

                ExecuteModule?.Invoke(this, EventActionArgs.Empty);
            }
        }
    }
}
