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
        private GroundMovement m_movement;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private GroundSpeedTransistor m_speedTransistor;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private Crouch m_crouch;
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

            m_crouch = behaviours.GetComponentInChildren<Crouch>();
            m_groundJump = behaviours.GetComponentInChildren<GroundJumpHandler>();
            m_groundDash = behaviours.GetComponentInChildren<GroundDash>();
            m_platformDrop = behaviours.GetComponentInChildren<PlatformDrop>();
            m_movement = behaviours.GetComponentInChildren<GroundMovement>();
            m_speedTransistor = behaviours.GetComponentInChildren<GroundSpeedTransistor>();
        }

        public void CallFixedUpdate(IPlayerState state, IPrimarySkills skills, ControllerEventArgs callArgs)
        {
            if (state.isDashing)
            {

            }
            else
            {
               
                if (state.hasJumped == false)
                {
                    m_movement?.Move(callArgs.input.direction.horizontalInput);
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
                    m_speedTransistor?.SwitchToJogSpeed();
                    m_crouch?.StopCrouch();
                    m_platformDrop?.DropFromPlatform();
                }
            }
            else if (state.isDashing)
            {

            }
            else
            {
                if (state.isCrouched == false)
                {
                    if (state.isMoving)
                    {
                        m_speedTransistor?.HandleSprintTransistion();
                    }
                    else
                    {
                        m_speedTransistor?.SwitchToJogSpeed();
                    }
                }

                if (callArgs.input.isJumpPressed)
                {
                    m_groundJump?.HandleJump();
                }
                else if (callArgs.input.direction.isDownHeld)
                {
                    m_speedTransistor.SwitchToCrouchSpeed();
                    m_crouch?.StartCrouch();
                }
                else if (skills.IsEnabled(PrimarySkill.Dash))
                {
                    if (callArgs.input.skillInput.isDashPressed && state.canDash)
                    {
                        m_groundDash?.StartDash();
                    }
                }
            }
        }
    }
}