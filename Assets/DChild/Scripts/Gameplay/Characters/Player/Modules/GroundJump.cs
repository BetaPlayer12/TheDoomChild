using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class GroundJump : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        [SerializeField, MinValue(0.1f)]
        private float m_power;
        [SerializeField]
        private float m_cutOffPower;
        [SerializeField, MinValue(0f)]
        private float m_allowCutoffAfterDuration;

        private Rigidbody2D m_rigidbody;
        private IHighJumpState m_state;
        private Animator m_animator;
        private int m_animationParameter;
        private float m_timer;

        public float highJumpCutoffThreshold => m_cutOffPower;
        public event EventAction<EventActionArgs> ExecuteModule;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_rigidbody = info.rigidbody;
            m_state = info.state;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Jump);
        }

        public void Cancel()
        {
            m_state.isHighJumping = false;
            m_animator.SetBool(m_animationParameter, false);
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, 0);
        }

        public void EndExecution()
        {
            m_state.isHighJumping = false;
            m_animator.SetBool(m_animationParameter, false);
        }

        public void HandleCutoffTimer()
        {
            m_timer -= GameplaySystem.time.deltaTime;
        }

        public bool CanCutoffJump() => m_timer <= 0;

        public void CutOffJump()
        {
            m_state.isHighJumping = false;
            m_animator.SetBool(m_animationParameter, false);
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, m_cutOffPower);
        }

        public void Execute()
        {
            m_state.isHighJumping = true;
            m_animator.SetBool(m_animationParameter, true);
            m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, m_power);
            //m_rigidbody.sharedMaterial.friction = 0f;
            m_timer = m_allowCutoffAfterDuration;

            ExecuteModule?.Invoke(this, EventActionArgs.Empty);
        }
    }
}
