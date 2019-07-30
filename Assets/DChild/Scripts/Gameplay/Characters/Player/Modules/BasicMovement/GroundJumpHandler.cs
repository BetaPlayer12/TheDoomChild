using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using DChild.Inputs;
using UnityEngine;
using System;
using Refactor.DChild.Gameplay.Characters.Players;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class GroundJumpHandler : Jump, IControllableModule
    {
        private IHighJumpState m_highJumpState;

        private Animator m_animator;
        private string m_jumpParamater;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            m_highJumpState = info.state;
            m_animator = info.animator;
            m_jumpParamater = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Jump);
        }

        public void ConnectTo(IMainController controller)
        {
            var jumpController = controller.GetSubController<IJumpController>();
            jumpController.JumpCall += OnJumpCall;
        }

        private void OnControllerDisabled(object sender, EventActionArgs eventArgs)
        {
            m_highJumpState.canHighJump = false;
            m_physics.SetVelocity(x: 0);
        }

        public override void HandleJump()
        {
            if (m_physics.onWalkableGround)
            {
                m_physics.StopCoyoteTime();
                m_physics.SetVelocity(x: 0);
                base.HandleJump();
                m_physics.AddForce(Vector2.up * m_power, ForceMode2D.Impulse);
                m_animator.SetTrigger(m_jumpParamater);
            }
        }

        private void OnJumpCall(object sender, ControllerEventArgs eventArgs)
        {
            m_highJumpState.canHighJump = true;
            HandleJump();
        }
    }
}