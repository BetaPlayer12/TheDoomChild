using DChild.Gameplay.Combat;
using Holysoft.Event;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public enum IntroActions
    {
        Jump,
        Crouch,
        Attack
    }

    public class PlayerIntroControlsController : MonoBehaviour, IMainController
    {
        [SerializeField]
        private InputTranslator m_input;
        [SerializeField]
        private CharacterState m_state;
        [SerializeField]
        private Character m_character;
        [SerializeField]
        private Rigidbody2D m_rigidbody;

        //[SerializeField]
        //private bool m_isUsingIntroControls = false;
        //[SerializeField]
        //private bool m_canJump = false;
        //[SerializeField]
        //private bool m_canCrouch = false;
        //[SerializeField]
        //private bool m_canSlash = false;

        private bool m_isUsingIntroControls = false;
        private bool m_canJump = false;
        private bool m_canCrouch = false;
        private bool m_canSlash = false;

        #region Behaviors
        private PlayerStatisticTracker m_tracker;
        private GroundednessHandle m_groundedness;
        private PlayerPhysicsMatHandle m_physicsMat;
        private InitialDescentBoost m_initialDescentBoost;
        private PlayerOneWayPlatformDropHandle m_platformDrop;
        private ObjectInteraction m_objectInteraction;
        private CombatReadiness m_combatReadiness;
        private IdleHandle m_idle;

        private Movement m_movement;
        private GroundJump m_groundJump;
        private Crouch m_crouch;
        private LedgeGrab m_ledgeGrab;
        private AutoStepClimb m_stepClimb;

        private CollisionRegistrator m_attackRegistrator;
        private BasicSlashes m_basicSlashes;
        private SlashCombo m_slashCombo;
        #endregion

        private bool m_updateEnabled = true;

        public event EventAction<EventActionArgs> ControllerDisabled;
        public event EventAction<EventActionArgs> ControllerEnabled;

        public void Enable()
        {
            m_updateEnabled = true;
        }

        public void Disable()
        {
            m_updateEnabled = false;
        }

        public bool IsUsingIntroControls() => m_isUsingIntroControls;

        public void EnableIntroControls()
        {
            if (m_isUsingIntroControls == false)
            {
                m_isUsingIntroControls = true;
            }
        }

        public void DisableIntroControls()
        {
            if (m_isUsingIntroControls == true)
            {
                m_isUsingIntroControls = false;
            }
        }

        public void EnableJump()
        {
            m_canJump = true;
        }

        public void EnableCrouch()
        {
            m_canCrouch = true;
        }

        public void EnableAttacks()
        {
            m_canSlash = true;
        }

        public void EnableIntroAction(List<IntroActions> actions)
        {
            foreach (var introAction in actions)
            {
                switch (introAction)
                {
                    case IntroActions.Jump:
                        m_canJump = true;
                        break;
                    case IntroActions.Crouch:
                        m_canCrouch = true;
                        break;
                    case IntroActions.Attack:
                        m_canSlash = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public void HandleIntroControls()
        {
            if (m_updateEnabled == false)
                return;

            if (m_state.isDoingCombo)
            {
                if (m_state.waitForBehaviour == false)
                {
                    if (m_state.canAttack == false)
                    {
                        m_slashCombo.HandleComboAttackDelay();
                    }
                }

                HandleGroundBehaviour();
                return;
            }

            if (m_state.waitForBehaviour)
                return;

            if (m_state.isCombatReady)
            {
                m_combatReadiness?.HandleDuration();
            }

            if (m_state.canAttack == true)
            {
                m_slashCombo.HandleComboResetTimer();
            }
            else
            {
                m_basicSlashes.HandleNextAttackDelay();
                m_slashCombo.HandleComboAttackDelay();
            }

            m_tracker.Execute(m_input);
            m_platformDrop.HandleDroppablePlatformCollision();

            if (m_state.isGrounded)
            {
                HandleGroundBehaviour();
                m_basicSlashes?.ResetAerialGravityControl();
            }
            else
            {
                HandleAirBehaviour();
            }
        }

        public void HandleIntroControlsFixedUpdate()
        {
            if (m_state.isGrounded)
            {
                if (m_state.forcedCurrentGroundedness == false)
                {
                    m_groundedness?.Evaluate();
                }

                if (m_groundedness?.isUsingCoyote ?? false)
                {
                    m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Midair);
                }
                else
                {
                    m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Ground);
                }
            }
            else
            {
                if ((int)m_character.facing == m_input.horizontalInput)
                {
                    if (m_state.waitForBehaviour)
                        return;

                    if (m_ledgeGrab?.IsDoable() ?? false)
                    {
                        if (m_state.isAttacking == false)
                        {
                            m_ledgeGrab?.Execute();
                        }
                    }
                }

                m_initialDescentBoost?.Handle();
                if (m_rigidbody.velocity.y < m_groundedness?.groundCheckOffset)
                {
                    if (m_state.forcedCurrentGroundedness == false)
                    {
                        m_groundedness?.Evaluate();
                    }
                }
            }
        }

        private void HandleGroundBehaviour()
        {
            if (m_state.isDoingCombo == true)
            {
                if (m_state.canAttack)
                {
                    if (m_input.slashPressed)
                    {
                        PrepareForGroundAttack();
                        m_slashCombo.Execute();
                        return;
                    }
                }

                return;
            }

            if (m_state.isAttacking)
            {
                m_attackRegistrator?.ResetHitCache();
            }

            if (m_state.isCrouched)
            {
                if (m_state.canAttack)
                {
                    if (m_input.slashPressed && m_canSlash)
                    {
                        PrepareForGroundAttack();
                        m_basicSlashes.Execute(BasicSlashes.Type.Crouch);
                        return;
                    }
                }

                if (m_input.jumpPressed == true)
                {
                    if (m_platformDrop?.IsThereADroppablePlatform() == true)
                    {
                        m_platformDrop.Execute();

                        return;
                    }
                }

                MoveCharacter();

                if (m_input.crouchHeld == false)
                {
                    if (m_crouch?.IsThereNoCeiling() ?? true)
                    {
                        m_crouch?.Cancel();
                        m_movement?.SwitchConfigTo(Movement.Type.Jog);
                    }
                }
            }
            else
            {
                if (m_state.canAttack)
                {
                    #region Ground Attacks
                    if (m_input.slashPressed && m_canSlash)
                    {
                        if (m_input.verticalInput > 0)
                        {
                            PrepareForGroundAttack();
                            m_basicSlashes.Execute(BasicSlashes.Type.Ground_Overhead);
                            return;
                        }
                        else
                        {
                            PrepareForGroundAttack();
                            m_slashCombo.Execute();
                            return;
                        }
                    }
                    #endregion
                }

                if (m_input.interactPressed)
                {
                    m_objectInteraction?.Interact();
                    return;
                }

                if (m_input.crouchHeld && m_canCrouch)
                {
                    m_crouch?.Execute();
                    m_idle?.Cancel();
                    m_movement?.SwitchConfigTo(Movement.Type.Crouch);
                }
                else if (m_input.jumpPressed && m_canJump)
                {
                    m_idle?.Cancel();
                    m_movement?.SwitchConfigTo(Movement.Type.MidAir);
                    m_groundedness?.ChangeValue(false);
                    m_groundJump?.Execute();
                }
                else
                {
                    MoveCharacter();
                    if (m_input.horizontalInput != 0)
                    {
                        if (m_stepClimb.CheckForStepClimbableSurface())
                        {
                            m_stepClimb.ClimbSurface();
                        }
                    }
                }
            }
        }

        private void HandleAirBehaviour()
        {
            if (m_state.isAttacking)
            {
                if (m_rigidbody.velocity.y < 0)
                {
                    m_groundedness?.Evaluate();
                }
            }
            else
            {
                if (m_state.canAttack)
                {
                    #region MidAir Attacks
                    if (m_input.slashPressed && m_canSlash)
                    {
                        PrepareForMidairAttack();

                        if (m_input.verticalInput > 0)
                        {
                            m_basicSlashes.Execute(BasicSlashes.Type.MidAir_Overhead);
                        }
                        else if (m_input.verticalInput == 0)
                        {
                            m_basicSlashes.Execute(BasicSlashes.Type.MidAir_Forward);
                        }
                        return;
                    }
                    #endregion
                }

                if (m_state.isHighJumping)
                {
                    if (m_groundJump?.CanCutoffJump() ?? true)
                    {
                        if (m_input.jumpHeld == false)
                        {
                            m_groundJump?.CutOffJump();
                        }
                    }

                    if (m_rigidbody.velocity.y <= (m_groundJump?.highJumpCutoffThreshold ?? 0f))
                    {
                        m_groundJump?.EndExecution();
                    }
                    m_groundJump?.HandleCutoffTimer();
                }

                m_movement.AirMove(m_input.horizontalInput, false);
            }
        }

        private void MoveCharacter()
        {
            if (m_input.horizontalInput == 0)
            {
                m_idle?.Execute(m_state.allowExtendedIdle);
            }
            else
            {
                m_idle?.Cancel();
            }

            if (m_state.isGrounded)
                m_movement?.GroundMove(m_input.horizontalInput, true);
            else
                m_movement?.AirMove(m_input.horizontalInput, false);
        }

        private void OnGroundednessStateChange(object sender, EventActionArgs eventArgs)
        {
            if (m_state.isDead)
            {
                //Then you need to git gud.
            }
            else
            {
                #region Groundedness Switch
                if (m_state.isGrounded)
                {
                    m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Ground);

                    m_initialDescentBoost?.Reset();
                    m_movement?.SwitchConfigTo(Movement.Type.Jog);

                    if (m_state.isAttacking)
                    {
                        m_basicSlashes.Cancel();
                    }
                }
                else
                {
                    m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Midair);

                    if (m_state.isCrouched)
                    {
                        m_crouch?.Cancel();
                    }

                    m_idle?.Cancel();
                    m_movement?.SwitchConfigTo(Movement.Type.MidAir);
                }
                #endregion
            }
        }

        private void PrepareForGroundAttack()
        {
            m_combatReadiness?.Execution();
            m_idle?.Cancel();
            m_movement?.Cancel();
            m_attackRegistrator?.ResetHitCache();
        }

        private void PrepareForMidairAttack()
        {
            if (m_state.isHighJumping == true)
            {
                m_groundJump?.CutOffJump();
            }

            m_combatReadiness?.Execution();
            m_attackRegistrator?.ResetHitCache();
        }

        private void Awake()
        {
            m_tracker = m_character.GetComponentInChildren<PlayerStatisticTracker>();
            m_physicsMat = m_character.GetComponentInChildren<PlayerPhysicsMatHandle>();
            m_groundedness = m_character.GetComponentInChildren<GroundednessHandle>();
            m_groundedness.StateChange += OnGroundednessStateChange;
            m_initialDescentBoost = m_character.GetComponentInChildren<InitialDescentBoost>();
            m_platformDrop = m_character.GetComponentInChildren<PlayerOneWayPlatformDropHandle>();
            m_objectInteraction = m_character.GetComponentInChildren<ObjectInteraction>();
            m_idle = m_character.GetComponentInChildren<IdleHandle>();
            m_combatReadiness = m_character.GetComponentInChildren<CombatReadiness>();

            m_movement = m_character.GetComponentInChildren<Movement>();
            m_groundJump = m_character.GetComponentInChildren<GroundJump>();
            m_crouch = m_character.GetComponentInChildren<Crouch>();
            m_stepClimb = m_character.GetComponentInChildren<AutoStepClimb>();
            m_ledgeGrab = m_character.GetComponentInChildren<LedgeGrab>();

            m_attackRegistrator = m_character.GetComponentInChildren<CollisionRegistrator>();
            m_basicSlashes = m_character.GetComponentInChildren<BasicSlashes>();
            m_slashCombo = m_character.GetComponentInChildren<SlashCombo>();

            m_updateEnabled = true;
        }
    }
}