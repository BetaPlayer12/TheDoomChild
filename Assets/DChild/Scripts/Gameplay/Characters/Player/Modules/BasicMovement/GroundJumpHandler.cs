using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using DChild.Inputs;
using UnityEngine;
using System;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class GroundJumpHandler : Jump, IPlayerExternalModule, IEventModule
    {
        private PlayerAnimation m_animation;
        private PlayerInput m_input;

        private IHighJumpState m_highJumpState;
        private IPlayerAnimationState m_animationState;
        private IPlayerState m_characterState;

        private Action<PlayerAnimation> HandleJumpAnimation = delegate { };

        public override void Initialize(IPlayerModules player)
        {
            base.Initialize(player);
            m_highJumpState = player.characterState;
            m_characterState = player.characterState;
            m_animationState = player.animationState;
            m_animation = player.animation;
        }

        public void ConnectEvents()
        {
            GetComponentInParent<IJumpController>().JumpCall += OnJumpCall;
            GetComponentInParent<IMainController>().ControllerDisabled += OnControllerDisabled;
        }


        public void SetJumpHandler(Action<PlayerAnimation> handle)
        {
            HandleJumpAnimation = handle == null ? BasicJump : handle;
        }

        private void OnControllerDisabled(object sender, EventActionArgs eventArgs)
        {
            m_highJumpState.canHighJump = false;
            m_character.SetVelocity(x: 0);
        }

        private void BasicJump(PlayerAnimation animation)
        {
            if (m_animationState.hasAttacked)
            {

            }

            else if (m_characterState.isMoving)
            {
                m_animation.DoJumpLoop(m_facing.currentFacingDirection);

            }

            else
            {
                //m_animation.DoStaticJump(m_facing.currentFacingDirection);
                m_animation.DoJumpLoop(m_facing.currentFacingDirection);

            }
        }

        public override void HandleJump()
        {
            if (m_character.onWalkableGround)
            {
                m_character.StopCoyoteTime();
                m_character.SetVelocity(x: 0);
                base.HandleJump();
                m_character.AddForce(Vector2.up * (m_power * m_modifier.jumpPower), ForceMode2D.Impulse);
                CallJumpStart();
            }



            HandleJumpAnimation(m_animation);

            m_animationState.hasJumped = true;
            m_animationState.hasDoubleJumped = false;
            m_animationState.isFromJog = false;
            m_animationState.hasAttacked = false;
            m_animationState.isFromIdle = false;
        }

        private void OnJumpCall(object sender, EventActionArgs eventArgs)
        {
            m_highJumpState.canHighJump = true;
            HandleJump();
        }

        private void Awake()
        {
            HandleJumpAnimation = BasicJump;
        }
    }
}