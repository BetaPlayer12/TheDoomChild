using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Skill
{
    public class WallJump : Jump, IControllableModule
    {
        [SerializeField]
        private float m_forwardPower;
        [SerializeField]
        private CountdownTimer m_inputDisableDuration;

        private IWallJumpState m_state;
        private IIsolatedTime m_time;
        private Animator m_animator;
        private string m_jumpParamater;

        public void ConnectTo(IMainController controller)
        {
            controller.GetSubController<IWallJumpController>().WallJumpCall += OnWallJumpCall;
        }

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            m_state = info.state;
            m_time = info.character.isolatedObject;
            m_animator = info.animator;
            m_jumpParamater = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Jump);
        }

        public override void HandleJump()
        {
            base.HandleJump();
            m_physics.AddForce(Vector2.up * m_power, ForceMode2D.Impulse);
            var forceDirection = m_character.facing == HorizontalDirection.Left ? 1 : -1;
            m_physics.SetVelocity(forceDirection * m_forwardPower);
            m_physics.simulateGravity = true;
            CallJumpStart();

            m_state.waitForBehaviour = true;
            m_inputDisableDuration.Reset();
            enabled = true;
            m_character.SetFacing(m_character.facing == HorizontalDirection.Left ? HorizontalDirection.Right : HorizontalDirection.Left);
            m_animator.SetTrigger(m_jumpParamater);
        }

        private void OnWallJumpCall(object sender, EventActionArgs eventArgs)
        {
            HandleJump();
        }

        private void OnDisableInputEnd(object sender, EventActionArgs eventArgs)
        {
            m_state.waitForBehaviour = false;
        }

        private void Awake()
        {
            m_inputDisableDuration.CountdownEnd += OnDisableInputEnd;
            enabled = false;
        }

        private void Update()
        {
            m_inputDisableDuration.Tick(m_time.deltaTime);
        }
    }
}