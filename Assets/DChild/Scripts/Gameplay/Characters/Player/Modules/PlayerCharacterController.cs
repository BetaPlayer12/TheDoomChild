using DChild.Gameplay.Combat;
using Holysoft.Event;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerCharacterController : MonoBehaviour, IMainController
    {
        [SerializeField]
        private InputTranslator m_input;
        [SerializeField]
        private PlayerSkills m_skills;
        [SerializeField]
        private CharacterState m_state;
        [SerializeField]
        private Character m_character;
        [SerializeField]
        private Rigidbody2D m_rigidbody;

        private ChargeAttackHandle m_chargeAttackHandle;

        #region Behaviours
        private PlayerStatisticTracker m_tracker;
        private GroundednessHandle m_groundedness;
        private IdleHandle m_idle;
        private CombatReadiness m_combatReadiness;
        private PlayerFlinch m_flinch;
        private InitialDescentBoost m_initialDescentBoost;

        private Movement m_movement;
        private Crouch m_crouch;
        private Dash m_dash;
        private GroundJump m_groundJump;
        private ExtraJump m_extraJump;

        private WallStick m_wallStick;
        private WallMovement m_wallMovement;
        private WallSlide m_wallSlide;
        private WallJump m_wallJump;

        private CollisionRegistrator m_attackRegistrator;
        private BasicSlashes m_basicSlashes;
        private SlashCombo m_slashCombo;
        private SwordThrust m_swordThrust;
        private EarthShaker m_earthShaker;
        #endregion

        public event EventAction<EventActionArgs> ControllerDisabled;

        public void Disable()
        {
            enabled = false;
            m_idle.Execute();
            m_movement.Cancel();
            m_crouch.Cancel();
            m_dash.Cancel();
            m_wallStick.Cancel();
        }

        public void Enable()
        {
            enabled = true;
        }

        private void OnGroundednessStateChange(object sender, EventActionArgs eventArgs)
        {
            m_dash.Reset();
            if (m_state.isGrounded)
            {
                if (m_state.isStickingToWall)
                {
                    m_wallMovement?.Cancel();
                    m_wallStick?.Cancel();
                }
                m_initialDescentBoost?.Reset();
                m_extraJump?.Reset();
                m_movement?.SwitchConfigTo(Movement.Type.Jog);

                if (m_state.isAttacking)
                {
                    m_basicSlashes.Cancel();
                }
            }
            else
            {
                if (m_state.isCrouched)
                {
                    m_crouch?.Cancel();
                }
                m_idle?.Cancel();
                m_movement?.SwitchConfigTo(Movement.Type.MidAir);
            }
        }

        private void OnFlinch(object sender, EventActionArgs eventArgs)
        {
            m_combatReadiness?.Execution();
            if (m_state.isGrounded)
            {
                if (m_state.isAttacking)
                {
                    if (m_state.isChargingAttack)
                    {
                        m_swordThrust?.Cancel();
                    }
                    else
                    {
                        m_basicSlashes?.Cancel();
                    }
                }

                if (m_state.isCrouched)
                {
                    m_idle?.Cancel();
                    m_crouch.Cancel();
                }
                else if (m_state.isDashing)
                {
                    m_dash.Cancel();
                }
                else
                {
                    m_idle?.Cancel();
                    m_movement?.Cancel();
                }
            }
            else
            {
                if (m_state.isAttacking)
                {
                    m_basicSlashes?.Cancel();
                }

                if (m_state.isStickingToWall)
                {
                    m_wallMovement?.Cancel();
                    m_wallSlide?.Cancel();
                    m_wallStick?.Cancel();
                }
                else if (m_state.isDashing)
                {

                }
            }
        }

        private void FlipCharacter()
        {
            var oppositeFacing = m_character.facing == HorizontalDirection.Right ? HorizontalDirection.Left : HorizontalDirection.Right;
            m_character.SetFacing(oppositeFacing);
        }

        private void Awake()
        {
            m_chargeAttackHandle = new ChargeAttackHandle();

            m_tracker = m_character.GetComponentInChildren<PlayerStatisticTracker>();
            m_groundedness = m_character.GetComponentInChildren<GroundednessHandle>();
            m_groundedness.StateChange += OnGroundednessStateChange;
            m_idle = m_character.GetComponentInChildren<IdleHandle>();
            m_combatReadiness = m_character.GetComponentInChildren<CombatReadiness>();
            m_flinch = m_character.GetComponentInChildren<PlayerFlinch>();
            m_flinch.OnExecute += OnFlinch;
            m_initialDescentBoost = m_character.GetComponentInChildren<InitialDescentBoost>();

            m_movement = m_character.GetComponentInChildren<Movement>();
            m_crouch = m_character.GetComponentInChildren<Crouch>();
            m_dash = m_character.GetComponentInChildren<Dash>();
            m_groundJump = m_character.GetComponentInChildren<GroundJump>();
            m_extraJump = m_character.GetComponentInChildren<ExtraJump>();
            m_wallStick = m_character.GetComponentInChildren<WallStick>();
            m_wallMovement = m_character.GetComponentInChildren<WallMovement>();
            m_wallSlide = m_character.GetComponentInChildren<WallSlide>();
            m_wallJump = m_character.GetComponentInChildren<WallJump>();

            m_attackRegistrator = m_character.GetComponentInChildren<CollisionRegistrator>();
            m_basicSlashes = m_character.GetComponentInChildren<BasicSlashes>();
            m_slashCombo = m_character.GetComponentInChildren<SlashCombo>();
            m_swordThrust = m_character.GetComponentInChildren<SwordThrust>();
            m_earthShaker = m_character.GetComponentInChildren<EarthShaker>();
        }

        private void FixedUpdate()
        {
            //if (m_state.waitForBehaviour)
            //    return;

            if (m_state.isGrounded)
            {
                m_groundedness?.Evaluate();
            }
            else
            {
                if (m_state.isStickingToWall)
                    return;

                m_initialDescentBoost?.Handle();
                if (m_rigidbody.velocity.y <= 0)
                {
                    m_groundedness?.Evaluate();

                }
            }
        }


        private void Update()
        {
            m_tracker.Execute(m_input);

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

            if (m_state.isGrounded)
            {
                HandleGroundBehaviour();
            }
            else
            {
                HandleAirBehaviour();
            }
        }

        private void HandleAirBehaviour()
        {
            if (m_state.isAttacking)
            {

                if(m_rigidbody.velocity.y < 0)
                {
                    m_groundedness?.Evaluate();
                }
            }
            else if (m_state.isStickingToWall)
            {
                #region WallStick Behaviour
                if (m_input.jumpPressed)
                {
                    if (m_input.verticalInput < 0)
                    {
                        m_wallStick?.Cancel();
                        m_wallMovement?.Cancel();
                    }
                    else if (m_skills.IsEnabled(PrimarySkill.WallJump))
                    {
                        m_wallStick?.Cancel();
                        m_wallMovement?.Cancel();
                        FlipCharacter();
                        m_wallJump?.JumpAway();
                    }
                }
                else if (m_input.dashPressed)
                {
                    m_wallStick?.Cancel();
                    FlipCharacter();
                    m_dash?.ResetDurationTimer();
                    m_dash?.Execute();
                    m_dash?.Reset();
                }
                else
                {
                    var verticalInput = m_input.verticalInput;
                    if (verticalInput < 0)
                    {
                        m_wallSlide?.Cancel();
                        m_wallMovement?.Move(verticalInput);
                        m_groundedness?.Evaluate();
                        if (m_state.isGrounded)
                            return;

                        if ((m_wallMovement?.IsThereAWall(WallMovement.SensorType.Body) ?? false) == false)
                        {
                            m_wallMovement?.Cancel();
                            m_wallStick?.Cancel();
                        }
                    }
                    else if (verticalInput > 0)
                    {
                        m_wallSlide?.Cancel();
                        m_wallMovement?.Move(verticalInput);
                        if ((m_wallMovement?.IsThereAWall(WallMovement.SensorType.Overhead) ?? false) == false)
                        {
                            m_wallMovement?.Cancel();
                        }
                    }
                    else
                    {
                        m_wallMovement?.Cancel();
                        var horizontalInput = m_input.horizontalInput;
                        if (horizontalInput != 0 && Mathf.Sign(horizontalInput) == (float)m_character.facing)
                        {
                            m_wallSlide?.Cancel();
                        }
                        else
                        {
                            if (m_wallSlide.IsThereAWall())
                            {
                                m_wallSlide?.Execute();
                                m_groundedness?.Evaluate();
                                if (m_state.isGrounded)
                                    return;
                            }
                            else
                            {
                                m_wallSlide?.Cancel();
                                m_wallStick?.Cancel();
                            }
                        }
                    }
                }
                #endregion
            }
            else if (m_state.isDashing)
            {
                HandleDash();
                if (m_extraJump?.HasExtras() ?? false)
                {
                    if (m_input.jumpPressed)
                    {
                        m_dash?.Cancel();
                        m_extraJump?.Execute();
                    }
                }
            }
            else
            {
                if (m_state.canAttack)
                {
                    #region MidAir Attacks
                    if (m_input.slashPressed)
                    {
                        m_combatReadiness?.Execution();
                        m_attackRegistrator?.ResetHitCache();
                        if (m_input.verticalInput > 0)
                        {
                            m_basicSlashes.Execute(BasicSlashes.Type.MidAir_Overhead);
                        }
                        else
                        {
                            m_basicSlashes.Execute(BasicSlashes.Type.MidAir_Forward);
                        }
                        return;
                    }
                    else if (m_input.earthShakerPressed)
                    {
                        m_combatReadiness?.Execution();
                        m_attackRegistrator?.ResetHitCache();
                        m_earthShaker?.StartExecution();

                    }
                    #endregion
                }

                #region NonCombat Air Behaviour
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

                if (m_input.dashPressed)
                {
                    if (m_skills.IsEnabled(PrimarySkill.Dash) && m_state.canDash)
                    {

                        m_groundJump?.Cancel();
                        m_dash?.ResetDurationTimer();
                        m_dash?.Execute();
                    }
                }
                else if (m_input.jumpPressed)
                {
                    if (m_skills.IsEnabled(PrimarySkill.DoubleJump))
                    {
                        if (m_extraJump?.HasExtras() ?? false)
                        {
                            m_extraJump?.Execute();
                        }
                    }
                }
                else
                {
                    m_movement.Move(m_input.horizontalInput);
                    if (m_input.horizontalInput != 0)
                    {
                        if (m_wallStick?.IsThereAWall() ?? false)
                        {
                            m_dash?.Reset();
                            m_extraJump?.Reset();
                            m_wallStick?.Execute();
                        }
                    }
                }
                #endregion
            }
        }

        private void HandleGroundBehaviour()
        {
            if (m_state.isDashing == false && m_state.canDash == false)
            {
                m_dash?.HandleCooldown();
            }

            if (m_state.isAttacking)
            {
                if (m_state.isChargingAttack)
                {
                    m_chargeAttackHandle?.Execute();
                }
            }
            else if (m_state.isCrouched)
            {
                if (m_state.canAttack)
                {
                    //if (m_input.slashPressed)
                    //{
                    //    PrepareForAttack();
                    //    m_basicSlashes.Execute(BasicSlashes.Type.Crouch);
                    //    return;
                    //}
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
            else if (m_state.isDashing)
            {
                HandleDash();
                if (m_input.jumpPressed)
                {
                    m_dash?.Cancel();
                    m_movement?.SwitchConfigTo(Movement.Type.MidAir);
                    m_groundedness?.ChangeValue(false);
                    m_groundJump?.Execute();
                }
            }
            else
            {
                //From Standing
                if (m_state.canAttack)
                {
                    #region Ground Attacks
                    if (m_input.slashPressed)
                    {
                        if (m_input.verticalInput > 0)
                        {
                            //PrepareForAttack();
                            //m_basicSlashes.Execute(BasicSlashes.Type.Ground_Overhead);
                            //return;
                        }
                        else
                        {
                            PrepareForAttack();
                            m_slashCombo.Execute();
                            return;
                        }
                    }
                    else if (m_input.slashHeld)
                    {
                        PrepareForAttack();
                        m_chargeAttackHandle.Set(m_swordThrust, () => m_input.slashHeld);
                        //Start SwordThrust
                        m_swordThrust?.StartCharge();
                        return;
                    }
                    #endregion
                }

                #region Non Combat Standing
                if (m_input.crouchHeld)
                {
                    m_crouch?.Execute();
                    m_movement?.SwitchConfigTo(Movement.Type.Crouch);
                }
                else if (m_input.dashPressed)
                {
                    if (m_skills.IsEnabled(PrimarySkill.Dash) && m_state.canDash)
                    {
                        m_idle?.Cancel();
                        m_movement?.Cancel();
                        m_dash?.ResetDurationTimer();
                        m_dash?.Execute();
                    }
                }
                else if (m_input.jumpPressed)
                {
                    m_idle?.Cancel();
                    m_movement?.SwitchConfigTo(Movement.Type.MidAir);
                    m_groundedness?.ChangeValue(false);
                    m_groundJump?.Execute();
                }
                else
                {
                    MoveCharacter();
                }
                #endregion
            }
        }

        //ModuleHandling
        #region Module Handling
        private void HandleExtraJump()
        {
            if (m_extraJump?.HasExtras() ?? false)
            {
                if (m_input.jumpPressed)
                {
                    m_extraJump?.Execute();
                }
            }
        }

        private void HandleDash()
        {
            m_dash?.HandleDurationTimer();
            if (m_dash?.IsDashDurationOver() ?? true)
            {
                m_dash?.Cancel();
                m_dash?.ResetCooldownTimer();
            }
            else
            {
                if (m_input.horizontalInput != 0)
                {
                    var signInput = Mathf.Sign(m_input.horizontalInput);
                    if (signInput != (float)m_character.facing)
                    {
                        FlipCharacter();
                    }
                }
                m_dash?.Execute();
            }
        }

        private void MoveCharacter()
        {
            m_movement?.Move(m_input.horizontalInput);
            if (m_input.horizontalInput == 0)
            {
                m_idle?.Execute();
            }
            else
            {
                m_idle?.Cancel();
            }
        }

        private void PrepareForAttack()
        {
            m_combatReadiness?.Execution();
            m_idle?.Cancel();
            m_movement?.Cancel();
            m_attackRegistrator?.ResetHitCache();
        }
        #endregion
    }
}
