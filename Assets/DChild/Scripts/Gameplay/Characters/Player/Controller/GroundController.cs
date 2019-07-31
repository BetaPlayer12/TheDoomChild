using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class GroundController : MonoBehaviour
    {
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private GroundMovement m_groundMovement;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private Crouch m_crouch;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private CrouchMovement m_crouchMovement;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private GroundJumpHandler m_groundJump;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private GroundDash m_groundDash;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private PlatformDrop m_platformDrop;

        private SkillResetRequester m_skillRequester;
        public void Initialize(GameObject behaviours, SkillResetRequester skillRequester)
        {
            m_skillRequester = skillRequester;
            m_groundMovement = behaviours.GetComponentInChildren<GroundMovement>();
            m_crouch = behaviours.GetComponentInChildren<Crouch>();
            m_crouchMovement = behaviours.GetComponentInChildren<CrouchMovement>();
            m_groundJump = behaviours.GetComponentInChildren<GroundJumpHandler>();
            m_groundDash = behaviours.GetComponentInChildren<GroundDash>();
            m_platformDrop = behaviours.GetComponentInChildren<PlatformDrop>();
        }

        public void CallFixedUpdate(IPlayerState state, IPrimarySkills skills, ControllerEventArgs callArgs)
        {
            if (state.isDashing)
            {

            }
            else
            {
                if (state.isCrouched)
                {
                    m_crouchMovement?.Move(callArgs.input.direction.horizontalInput);
                }
                else
                {
                    if (state.hasJumped == false)
                    {
                        m_groundMovement.Move(callArgs.input.direction.horizontalInput);
                    }
                }
            }
        }

        public void CallUpdate(IPlayerState state, IPrimarySkills skills, ControllerEventArgs callArgs)
        {
            if (state.isCrouched)
            {
                m_crouch?.HandleCrouch(callArgs.input.direction.isDownHeld);
                if (state.canPlatformDrop && callArgs.input.isJumpPressed)
                {
                    m_crouch?.StopCrouch();
                    m_platformDrop?.DropFromPlatform();
                }
            }
            else if (state.isDashing)
            {

            }
            else
            {
                if (callArgs.input.isJumpPressed)
                {
                    m_groundJump?.HandleJump();
                }
                else if (callArgs.input.direction.isDownHeld)
                {
                    m_crouch?.StartCrouch();
                }
                else if (skills.IsEnabled(PrimarySkill.Dash))
                {
                    if (callArgs.input.skillInput.isDashPressed)
                    {
                        m_groundDash?.StartDash();
                    }
                }
            }
        }
    }
}