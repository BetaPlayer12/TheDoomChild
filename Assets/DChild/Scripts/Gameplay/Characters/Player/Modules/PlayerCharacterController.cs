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
        private PlayerModuleActivator m_skills;
        [SerializeField]
        private CharacterState m_state;
        [SerializeField]
        private Character m_character;
        [SerializeField]
        private Rigidbody2D m_rigidbody;

        private ChargeAttackHandle m_chargeAttackHandle;
        private IDash m_activeDash;
        private ISlide m_activeSlide;

        #region Behaviours
        private PlayerStatisticTracker m_tracker;
        private GroundednessHandle m_groundedness;
        private PlayerPhysicsMatHandle m_physicsMat;
        private IdleHandle m_idle;
        private CombatReadiness m_combatReadiness;
        private PlayerFlinch m_flinch;
        private PlayerDeath m_death;
        private InitialDescentBoost m_initialDescentBoost;
        private ObjectInteraction m_objectInteraction;
        private ShadowGaugeRegen m_shadowGaugeRegen;
        private ObjectManipulation m_objectManipulation;

        private Movement m_movement;
        private Crouch m_crouch;
        private Dash m_dash;
        private Slide m_slide;
        private LedgeGrab m_ledgeGrab;
        private GroundJump m_groundJump;
        private ExtraJump m_extraJump;
        private DevilWings m_devilWings;
        private ShadowDash m_shadowDash;
        private ShadowSlide m_shadowSlide;
        private ShadowMorph m_shadowMorph;
        private AutoStepClimb m_stepClimb;

        private WallStick m_wallStick;
        private WallMovement m_wallMovement;
        private WallSlide m_wallSlide;
        private WallJump m_wallJump;

        private CollisionRegistrator m_attackRegistrator;
        private BasicSlashes m_basicSlashes;
        private SlashCombo m_slashCombo;
        private SwordThrust m_swordThrust;
        private EarthShaker m_earthShaker;
        private WhipAttack m_whip;
        private ProjectileThrow m_skullThrow;
        #endregion

        private bool m_updateEnabled = true;

        public event EventAction<EventActionArgs> ControllerDisabled;

        public void Disable()
        {
            m_updateEnabled = false;
            m_idle?.Execute();
            m_movement?.Cancel();
            m_crouch?.Cancel();
            m_dash?.Cancel();
            m_slide?.Cancel();
            m_wallStick?.Cancel();
            m_devilWings?.Cancel();
            m_shadowDash?.Cancel();
            m_basicSlashes?.Cancel();
            m_slashCombo?.Cancel();
            m_swordThrust?.Cancel();
            m_earthShaker?.Cancel();
            m_whip?.Cancel();
            m_skullThrow?.Cancel();
            m_shadowMorph.Cancel();

            if (m_state.isGrounded)
            {
                m_movement?.SwitchConfigTo(Movement.Type.Jog);
            }
        }

        public void Enable()
        {
            m_updateEnabled = true;
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
                m_dash.Reset();
                m_slide.Reset();
                if (m_state.isGrounded)
                {
                    m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Ground);

                    if (m_state.isStickingToWall)
                    {
                        m_wallMovement?.Cancel();
                        m_wallStick?.Cancel();
                    }
                    else if (m_state.isLevitating)
                    {
                        m_devilWings?.Cancel();
                    }

                    m_initialDescentBoost?.Reset();
                    m_extraJump?.Reset();
                    m_movement?.SwitchConfigTo(Movement.Type.Jog);

                    if (m_state.isAttacking)
                    {
                        m_basicSlashes.Cancel();
                        m_whip.Cancel();
                    }
                }
                else
                {
                    m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Midair);

                    if (m_state.isCrouched)
                    {
                        m_crouch?.Cancel();
                    }
                    else if (m_state.isAimingProjectile)
                    {
                        m_skullThrow.EndAim();
                        m_skullThrow.Cancel();
                    }
                    m_idle?.Cancel();
                    m_movement?.SwitchConfigTo(Movement.Type.MidAir);
                }
                #endregion
            }
        }

        private void OnDeath(object sender, EventActionArgs eventArgs)
        {
            Disable();
            m_idle?.Cancel();
            m_shadowMorph?.Cancel();
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
                        m_whip?.Cancel();
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
                else if (m_state.isSliding)
                {
                    m_slide.Cancel();
                }
                else if (m_state.isGrabbing)
                {
                    m_objectManipulation.Cancel();
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
                    m_earthShaker?.Cancel();
                    m_whip?.Cancel();
                    m_skullThrow?.Cancel();
                }

                if (m_state.isStickingToWall)
                {
                    m_wallMovement?.Cancel();
                    m_wallSlide?.Cancel();
                    m_wallStick?.Cancel();
                }
                else if (m_state.isDashing)
                {
                    m_activeDash?.Cancel();
                }
                else if (m_state.isLevitating)
                {
                    m_devilWings?.Cancel();
                }
            }
        }

        private void OnProjectileThrowRequest(object sender, EventActionArgs eventArgs)
        {
            m_input.skullThrowPressed = true;
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
            m_physicsMat = m_character.GetComponentInChildren<PlayerPhysicsMatHandle>();
            m_idle = m_character.GetComponentInChildren<IdleHandle>();
            m_combatReadiness = m_character.GetComponentInChildren<CombatReadiness>();
            m_flinch = m_character.GetComponentInChildren<PlayerFlinch>();
            m_flinch.OnExecute += OnFlinch;
            m_death = m_character.GetComponentInChildren<PlayerDeath>();
            m_death.OnExecute += OnDeath;
            m_initialDescentBoost = m_character.GetComponentInChildren<InitialDescentBoost>();
            m_objectInteraction = m_character.GetComponentInChildren<ObjectInteraction>();
            m_shadowGaugeRegen = m_character.GetComponentInChildren<ShadowGaugeRegen>();
            m_objectManipulation = m_character.GetComponentInChildren<ObjectManipulation>();

            m_movement = m_character.GetComponentInChildren<Movement>();
            m_crouch = m_character.GetComponentInChildren<Crouch>();
            m_dash = m_character.GetComponentInChildren<Dash>();
            m_slide = m_character.GetComponentInChildren<Slide>();
            m_ledgeGrab = m_character.GetComponentInChildren<LedgeGrab>();
            m_groundJump = m_character.GetComponentInChildren<GroundJump>();
            m_extraJump = m_character.GetComponentInChildren<ExtraJump>();
            m_devilWings = m_character.GetComponentInChildren<DevilWings>();
            m_shadowDash = m_character.GetComponentInChildren<ShadowDash>();
            m_shadowSlide = m_character.GetComponentInChildren<ShadowSlide>();
            m_shadowMorph = m_character.GetComponentInChildren<ShadowMorph>();
            m_wallStick = m_character.GetComponentInChildren<WallStick>();
            m_wallMovement = m_character.GetComponentInChildren<WallMovement>();
            m_wallSlide = m_character.GetComponentInChildren<WallSlide>();
            m_wallJump = m_character.GetComponentInChildren<WallJump>();
            m_stepClimb = m_character.GetComponentInChildren<AutoStepClimb>();

            m_attackRegistrator = m_character.GetComponentInChildren<CollisionRegistrator>();
            m_basicSlashes = m_character.GetComponentInChildren<BasicSlashes>();
            m_slashCombo = m_character.GetComponentInChildren<SlashCombo>();
            m_swordThrust = m_character.GetComponentInChildren<SwordThrust>();
            m_earthShaker = m_character.GetComponentInChildren<EarthShaker>();
            m_whip = m_character.GetComponentInChildren<WhipAttack>();
            m_skullThrow = m_character.GetComponentInChildren<ProjectileThrow>();
            m_skullThrow.ExecutionRequested += OnProjectileThrowRequest;

            m_updateEnabled = true;
        }

        private void FixedUpdate()
        {
            //if (m_state.waitForBehaviour)
            //    return;

            if (m_state.isDead)
                return;

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
                if (m_state.isStickingToWall)
                {
                    if (m_input.verticalInput > 0)
                    {
                        if (m_ledgeGrab?.IsDoable() ?? false)
                        {
                            m_wallMovement?.Cancel();
                            m_wallStick?.Cancel();
                            m_ledgeGrab?.Execute();
                        }
                    }

                    return;
                }

                if ((int)m_character.facing == m_input.horizontalInput)
                {
                    if (m_ledgeGrab?.IsDoable() ?? false)
                    {
                        m_ledgeGrab?.Execute();
                    }
                }

                m_initialDescentBoost?.Handle();
                if (m_rigidbody.velocity.y < 1f)
                {
                    if (m_state.forcedCurrentGroundedness == false)
                    {
                        m_groundedness?.Evaluate();
                        Debug.Log("Check for ground");
                    }
                    m_extraJump?.EndExecution();
                }
            }
        }


        private void Update()
        {
            if (m_updateEnabled == false)
                return;

            if (m_state.isDead)
                return;

            if (m_shadowGaugeRegen?.CanRegen() ?? false)
            {
                m_shadowGaugeRegen.Execute();
            }

            m_tracker.Execute(m_input);

            if (m_state.waitForBehaviour)
                return;

            if (m_state.isCombatReady)
            {
                m_combatReadiness?.HandleDuration();
            }

            if (m_state.isInShadowMode)
            {
                if (m_shadowMorph.HaveEnoughSourceForExecution())
                {
                    m_shadowMorph.ConsumeSource();
                }
                else
                {
                    m_shadowMorph?.Cancel();
                }
            }

            if (m_state.canAttack == true)
            {
                m_slashCombo.HandleComboResetTimer();
            }
            else
            {
                m_basicSlashes.HandleNextAttackDelay();
                m_slashCombo.HandleComboAttackDelay();
                m_whip.HandleNextAttackDelay();
                m_skullThrow.HandleNextAttackDelay();
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
                if (m_rigidbody.velocity.y < 0)
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
                    else if (m_skills.IsModuleActive(PrimarySkill.WallJump))
                    {
                        m_wallStick?.Cancel();
                        m_wallMovement?.Cancel();
                        FlipCharacter();
                        m_wallJump?.JumpAway();
                    }
                }
                else if (m_input.dashPressed)
                {
                    if (m_state.isInShadowMode == false)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.Dash) && m_state.canDash)
                        {
                            m_wallStick?.Cancel();
                            FlipCharacter();
                            ExecuteDash();
                        }
                    }

                    //if (m_skills.IsModuleActive(PrimarySkill.Dash))
                    //{
                    //    m_wallStick?.Cancel();
                    //    FlipCharacter();
                    //    m_dash?.ResetDurationTimer();
                    //    m_dash?.Execute();
                    //    m_dash?.Reset();
                    //}
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
                        m_activeDash?.Cancel();
                        m_extraJump?.Execute();
                    }
                }
            }
            else if (m_state.isSliding)
            {
                HandleSlide();
            }
            else
            {
                if (m_state.isLevitating)
                {
                    m_devilWings?.MaintainHeight();
                    m_devilWings?.ConsumeSource();
                    if (m_input.levitateHeld == false || (m_devilWings?.HaveEnoughSourceForMaintainingHeight() ?? true) == false)
                    {
                        m_devilWings?.Cancel();
                    }
                }

                if (m_state.canAttack)
                {
                    #region MidAir Attacks
                    if (m_input.earthShakerPressed)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.EarthShaker))
                        {
                            PrepareForMidairAttack();

                            m_earthShaker?.StartExecution();
                        }

                        return;
                    }
                    else if (m_input.slashPressed)
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
                    else if (m_input.whipPressed)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.Whip))
                        {
                            PrepareForMidairAttack();

                            if (m_input.verticalInput > 0)
                            {
                                m_whip.Execute(WhipAttack.Type.MidAir_Overhead);
                            }
                            else if (m_input.verticalInput == 0)
                            {
                                m_whip.Execute(WhipAttack.Type.MidAir_Forward);
                            }
                        }

                        return;
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
                    if (m_state.isInShadowMode == false)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.Dash) && m_state.canDash)
                        {
                            if (m_state.isLevitating)
                            {
                                m_devilWings?.Cancel();
                            }

                            m_groundJump?.Cancel();
                            ExecuteDash();
                        }
                    }
                }
                else if (m_input.jumpPressed)
                {
                    if (m_state.isInShadowMode == false)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.DoubleJump))
                        {
                            if (m_extraJump?.HasExtras() ?? false)
                            {
                                if (m_state.isLevitating)
                                {
                                    m_devilWings?.Cancel();
                                }

                                m_extraJump?.Execute();
                            }
                        }
                    }
                }
                else if (m_input.levitatePressed)
                {
                    if (m_state.isInShadowMode == false)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.DevilWings))
                        {
                            if (m_devilWings?.HaveEnoughSourceForExecution() ?? false)
                            {
                                if (m_state.isHighJumping)
                                {
                                    m_groundJump?.CutOffJump();
                                }

                                m_devilWings?.Execute();
                            }
                        }
                    }
                }
                else
                {
                    m_movement.Move(m_input.horizontalInput, true);
                    if (m_input.horizontalInput != 0)
                    {
                        if (m_state.isHighJumping == false && m_state.isLevitating == false)
                        {
                            if (m_state.isInShadowMode == false)
                            {
                                if (m_wallStick?.IsHeightRequirementAchieved() ?? false)
                                {
                                    if (m_wallStick?.IsThereAWall() ?? false)
                                    {
                                        m_dash?.Reset();
                                        m_extraJump?.Reset();
                                        m_wallStick?.Execute();
                                    }
                                }
                            }
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

            if (m_state.isSliding == false && m_state.canSlide == false)
            {
                m_slide?.HandleCooldown();
            }

            if (m_state.isAttacking)
            {
                if (m_state.isChargingAttack)
                {
                    m_chargeAttackHandle?.Execute();
                }
                else if (m_state.isAimingProjectile)
                {
                    if (m_input.horizontalInput != 0)
                    {
                        m_movement.UpdateFaceDirection(m_input.horizontalInput);
                    }

                    m_skullThrow.MoveAim(m_input.m_mouseDelta.normalized);
                    if (m_input.skullThrowReleased || m_input.skullThrowHeld == false)
                    {
                        m_skullThrow.EndAim();
                        m_skullThrow.StartThrow();
                        //Throw Projectile
                    }
                }
            }
            else if (m_state.isSliding)
            {
                HandleSlide();

                if (m_input.jumpPressed)
                {
                    if (m_crouch?.IsThereNoCeiling() ?? true)
                    {
                        m_activeSlide?.Cancel();
                        m_movement?.SwitchConfigTo(Movement.Type.MidAir);
                        m_groundedness?.ChangeValue(false);
                        m_groundJump?.Execute();
                    }
                }

                if (m_input.crouchHeld == false)
                {
                    if (m_crouch?.IsThereNoCeiling() ?? true)
                    {
                        m_crouch?.Cancel();
                        m_movement?.SwitchConfigTo(Movement.Type.Jog);
                    }
                }
            }
            else if (m_state.isCrouched)
            {
                if (m_state.canAttack)
                {
                    if (m_input.slashPressed)
                    {
                        PrepareForGroundAttack();
                        m_basicSlashes.Execute(BasicSlashes.Type.Crouch);
                        return;
                    }
                    else if (m_input.whipPressed)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.Whip))
                        {
                            PrepareForGroundAttack();
                            m_whip.Execute(WhipAttack.Type.Crouch_Forward);
                            return;
                        }

                        return;
                    }
                }

                if (m_input.interactPressed)
                {
                    m_objectInteraction?.Interact();
                    return;
                }

                if (m_input.dashPressed)
                {
                    if (m_state.isInShadowMode == false)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.Slide) && m_state.canSlide)
                        {
                            m_idle?.Cancel();
                            m_movement?.Cancel();
                            m_objectManipulation?.Cancel();
                            ExecuteSlide();
                        }
                    }
                }

                MoveCharacter(false);

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
                    m_activeDash?.Cancel();
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
                    else if (m_input.slashHeld)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.SwordThrust))
                        {
                            PrepareForGroundAttack();
                            m_chargeAttackHandle.Set(m_swordThrust, () => m_input.slashHeld);

                            //Start SwordThrust
                            m_swordThrust?.StartCharge();
                        }

                        return;
                    }
                    else if (m_input.whipPressed)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.Whip))
                        {
                            if (m_input.verticalInput > 0)
                            {
                                PrepareForGroundAttack();
                                m_whip.Execute(WhipAttack.Type.Ground_Overhead);
                                return;
                            }
                            else
                            {
                                PrepareForGroundAttack();
                                m_whip.Execute(WhipAttack.Type.Ground_Forward);
                                return;
                            }
                        }

                        return;
                    }
                    else if (m_input.skullThrowPressed)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.SkullThrow))
                        {
                            PrepareForGroundAttack();
                            m_skullThrow.StartAim();
                            m_skullThrow.Execute();
                        }
                        return;
                    }
                    #endregion
                }

                if (m_input.interactPressed)
                {
                    m_objectInteraction?.Interact();
                    return;
                }

                if (m_input.shadowMorphPressed)
                {
                    m_idle?.Cancel();
                    m_movement?.Cancel();
                    m_objectManipulation?.Cancel();

                    if (m_state.isInShadowMode)
                    {
                        m_shadowMorph.Cancel();
                    }
                    else
                    {
                        m_shadowMorph.Execute();
                    }

                    return;
                }

                if (m_input.grabPressed || m_input.grabHeld)
                {
                    if (m_objectManipulation?.IsThereAMovableObject() ?? false)
                    {
                        m_idle?.Cancel();
                        m_movement?.SwitchConfigTo(Movement.Type.Grab);
                        m_objectManipulation?.Execute();
                    }
                }

                if (m_state.isGrabbing)
                {
                    if (m_input.grabHeld == false)
                    {
                        m_movement?.SwitchConfigTo(Movement.Type.Jog);
                        m_objectManipulation?.Cancel();
                    }
                    else
                    {
                        if (m_objectManipulation.IsThereAMovableObject())
                        {
                            if (m_input.horizontalInput != 0)
                            {
                                m_objectManipulation?.MoveObject(m_input.horizontalInput, m_character.facing);
                            }
                            else
                            {
                                m_objectManipulation?.GrabIdle();
                            }
                        }
                        else
                        {
                            m_movement?.SwitchConfigTo(Movement.Type.Jog);
                            m_objectManipulation?.Cancel();
                        }
                    }
                }

                #region Non Combat Standing
                if (m_input.crouchHeld)
                {
                    m_crouch?.Execute();
                    m_idle?.Cancel();
                    m_movement?.SwitchConfigTo(Movement.Type.Crouch);
                }
                else if (m_input.dashPressed)
                {
                    if (m_state.isInShadowMode == false)
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            if (m_skills.IsModuleActive(PrimarySkill.Dash) && m_state.canDash)
                            {
                                m_idle?.Cancel();
                                m_movement?.Cancel();
                                m_objectManipulation?.Cancel();
                                ExecuteDash();
                            }
                        }
                    }
                }
                else if (m_input.jumpPressed)
                {
                    if (m_state.isInShadowMode == false)
                    {
                        m_idle?.Cancel();
                        m_movement?.SwitchConfigTo(Movement.Type.MidAir);
                        m_groundedness?.ChangeValue(false);
                        m_groundJump?.Execute();
                    }
                }
                else
                {
                    MoveCharacter(m_state.isGrabbing);
                    if (m_stepClimb.CheckForStepClimbableSurface())
                    {
                        if (m_input.horizontalInput != 0)
                        {
                            m_stepClimb.ClimbSurface();
                        }
                    }
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
            m_activeDash?.HandleDurationTimer();
            if (m_activeDash?.IsDashDurationOver() ?? true)
            {
                m_activeDash?.Cancel();
                m_activeDash?.ResetCooldownTimer();
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
                m_activeDash?.Execute();
            }
        }

        private void HandleSlide()
        {
            m_activeSlide?.HandleDurationTimer();

            if (m_state.isGrounded)
            {
                if (m_activeSlide?.IsSlideDurationOver() ?? true)
                {
                    if (m_crouch.IsThereNoCeiling())
                    {
                        m_activeSlide?.Cancel();
                        m_activeSlide?.ResetCooldownTimer();
                    }
                    else
                    {
                        if (m_crouch.IsCrouchingPossible())
                        {
                            m_activeSlide?.Cancel();
                            m_activeSlide?.ResetCooldownTimer();

                            if (m_state.isCrouched == false)
                            {
                                m_crouch?.Execute();
                                m_idle?.Cancel();
                                m_movement?.SwitchConfigTo(Movement.Type.Crouch);
                            }
                        }
                    }
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
                    m_activeSlide?.Execute();
                }
            }
            else
            {
                m_activeSlide?.Cancel();
                m_activeSlide?.ResetCooldownTimer();
            }
        }

        private void ExecuteDash()
        {
            if (m_shadowDash?.HaveEnoughSourceForExecution() ?? false)
            {
                m_activeDash = m_shadowDash;
                m_shadowDash.ConsumeSource();
            }
            else
            {
                m_activeDash = m_dash;
            }

            m_activeDash?.ResetDurationTimer();
            m_activeDash?.Execute();
        }

        private void ExecuteSlide()
        {
            if (m_shadowSlide?.HaveEnoughSourceForExecution() ?? false)
            {
                m_activeSlide = m_shadowSlide;
                m_shadowSlide.ConsumeSource();
            }
            else
            {
                m_activeSlide = m_slide;
            }

            m_activeSlide?.ResetDurationTimer();
            m_activeSlide?.Execute();
        }

        private void MoveCharacter(bool isGrabbing)
        {
            if (isGrabbing == false)
            {
                if (m_input.horizontalInput == 0)
                {
                    m_idle?.Execute();
                }
                else
                {
                    Debug.Log("Cancel Idle");
                    m_idle?.Cancel();
                }

                m_movement?.Move(m_input.horizontalInput, true);
            }
            else
            {
                m_movement?.Move(m_input.horizontalInput, false);
            }
        }

        private void PrepareForGroundAttack()
        {
            m_combatReadiness?.Execution();
            m_idle?.Cancel();
            m_movement?.Cancel();
            m_objectManipulation?.Cancel();
            m_attackRegistrator?.ResetHitCache();
        }

        private void PrepareForMidairAttack()
        {
            if (m_state.isLevitating)
            {
                m_devilWings?.Cancel();
            }

            if (m_state.isHighJumping == true)
            {
                m_groundJump?.CutOffJump();
            }

            m_combatReadiness?.Execution();
            m_attackRegistrator?.ResetHitCache();
        }
        #endregion
    }
}
