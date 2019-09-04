using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class GroundController : MonoBehaviour
    {
<<<<<<< HEAD
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private GroundMovement m_groundMovement;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private Crouch m_crouch;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private CrouchMovement m_crouchMovement;
=======

        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private MovementHandle m_movement;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private MoveSpeedTransistor m_speedTransistor;
        [ShowInInspector, ReadOnly, BoxGroup("Modules")]
        private Crouch m_crouch;
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
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
<<<<<<< HEAD
            m_groundMovement = behaviours.GetComponentInChildren<GroundMovement>();
            m_crouch = behaviours.GetComponentInChildren<Crouch>();
            m_crouchMovement = behaviours.GetComponentInChildren<CrouchMovement>();
            m_groundJump = behaviours.GetComponentInChildren<GroundJumpHandler>();
            m_groundDash = behaviours.GetComponentInChildren<GroundDash>();
            m_platformDrop = behaviours.GetComponentInChildren<PlatformDrop>();
=======

            m_crouch = behaviours.GetComponentInChildren<Crouch>();
            m_groundJump = behaviours.GetComponentInChildren<GroundJumpHandler>();
            m_groundDash = behaviours.GetComponentInChildren<GroundDash>();
            m_platformDrop = behaviours.GetComponentInChildren<PlatformDrop>();
            m_movement = behaviours.GetComponentInChildren<MovementHandle>();
            m_speedTransistor = behaviours.GetComponentInChildren<MoveSpeedTransistor>();
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
        }

        public void CallFixedUpdate(IPlayerState state, IPrimarySkills skills, ControllerEventArgs callArgs)
        {
            if (state.isDashing)
            {

            }
            else
            {
<<<<<<< HEAD
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
=======

                if (state.hasJumped == false)
                {
                    m_movement?.Move(callArgs.input.direction.horizontalInput);
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
                }
            }
        }

        public void CallUpdate(IPlayerState state, IPrimarySkills skills, ControllerEventArgs callArgs)
        {
<<<<<<< HEAD
            if (state.isCrouched)
            {
                m_crouch?.HandleCrouch(callArgs.input.direction.isDownHeld);
                if (state.canPlatformDrop && callArgs.input.isJumpPressed)
                {
                    m_crouch?.StopCrouch();
                    m_platformDrop?.DropFromPlatform();
=======


            if (state.isCrouched)
            {
                if (m_crouch?.HandleCrouch(callArgs.input.direction.isDownHeld) ?? false)
                {
                    if (state.canPlatformDrop && callArgs.input.isJumpPressed)
                    {
                        m_platformDrop?.DropFromPlatform();
                        m_speedTransistor?.SwitchToJogSpeed();
                        m_crouch?.StopCrouch();
                    }
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
                }
                else
                {
                    m_speedTransistor?.SwitchToJogSpeed();
                }
               
            }
            else if (state.isDashing)
            {

            }
            else
            {
                if (state.isCrouched == false)
                {
<<<<<<< HEAD
                    m_groundJump?.HandleJump();
                }
                else if (callArgs.input.direction.isDownHeld)
                {
                    m_crouch?.StartCrouch();
                }
=======
                    if (state.isMoving)
                    {
                        m_speedTransistor?.HandleSprintTransistion();
                    }
                    else
                    {
                        if (callArgs.input.direction.horizontalInput != 0)
                        {
                            m_speedTransistor?.SwitchToJogSpeed();
                        }
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
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
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