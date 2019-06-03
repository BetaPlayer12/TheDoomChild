using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class PlayerFallHandler : MonoBehaviour, IPlayerExternalModule, IEventModule
    {
        private PlayerAnimation m_animation;
        private Animator m_animator;

        private IPlayerState m_characterState;
        private IPlayerAnimationState m_animationState;
        private IFacing m_facing;

        public void Initialize(IPlayerModules player)
        {
            m_facing = player;
            m_animation = player.animation;
            m_animationState = player.animationState;
            m_characterState = player.characterState;
            m_animator = player.animation.GetComponentInChildren<Animator>();
        }

        public void ConnectEvents()
        {
            GetComponentInParent<IFallController>().FallCall += OnFallCall;
        }

        private void OnFallCall(object sender, EventActionArgs eventArgs)
        {
            if (m_animationState.hasAttacked)
            {
                m_animation.DoMovingFall(m_facing.currentFacingDirection);
            }

            else if (m_animationState.isFallingFromWallJump)
            {

            }


            else if (m_animationState.transitionToFall2)
            {
                m_animation.DoFallLoop2(m_facing.currentFacingDirection);
            }

            else if (m_characterState.isStickingToWall)
            {

            }

            else if (m_animationState.hasDoubleJumped)
            {
                m_animation.DoDoubleJumpFall(m_facing.currentFacingDirection);

                m_animationState.isFromFall = true;
                m_animationState.isAnticPlayed = false;
            }

            else
            {
                m_animation.DoFallLoop1(m_facing.currentFacingDirection);
            }

            m_animationState.isWallJumping = false;
            m_animationState.isFromFall = true;
            m_animationState.isFromIdle = false;
        }

    }
}
