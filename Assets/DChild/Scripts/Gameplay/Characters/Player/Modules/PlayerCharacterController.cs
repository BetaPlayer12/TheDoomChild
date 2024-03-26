using DChild.Gameplay.Characters.Players.BattleAbilityModule;
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
        private CombatArts m_abilities;
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
        private PlayerOneWayPlatformDropHandle m_platformDrop;

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
        private WhipAttackCombo m_whipCombo;
        private ProjectileThrow m_projectileThrow;
        private PlayerBlock m_block;
        private PlayerIntroControlsController m_introController;

        #region Soul Skills
        private ShadowbladeFX m_shadowBladeFX;
        #endregion

        #region Battle Abilities
        private AirLunge m_airLunge;
        private FireFist m_fireFist;
        private ReaperHarvest m_reaperHarvest;
        private KrakenRage m_krakenRage;
        private FinalSlash m_finalSlash;
        private AirSlashCombo m_airSlashCombo;
        private SovereignImpale m_sovereignImpale;
        private HellTrident m_hellTrident;
        private FoolsVerdict m_foolsVerdict;
        private SoulFireBlast m_soulFireBlast;
        private EdgedFury m_edgedFury;
        private NinthCircleSanction m_ninthCircleSanction;
        private DoomsdayKong m_doomsdayKong;
        private BackDiver m_backDiver;
        private Barrier m_barrier;
        private FencerFlash m_fencerFlash;
        private DiagonalSwordDash m_diagonalSwordDash;
        private ChampionsUprising m_championsUprising;
        private Eelecktrick m_eelecktrick;
        private LightningSpear m_lightningSpear;
        private IcarusWings m_icarusWings;
        private TeleportingSkull m_teleportingSkull;
        private AirSlashRange m_airSlashRange;
        #endregion
        #endregion

        private bool m_updateEnabled = true;

        public event EventAction<EventActionArgs> ControllerDisabled;
        public event EventAction<EventActionArgs> ControllerEnabled;

        public void Disable()
        {
            m_updateEnabled = false;
            m_idle?.Execute(false);
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
            m_whipCombo?.Cancel();
            m_projectileThrow?.Cancel();
            m_shadowMorph.Cancel();
            m_block?.Cancel();
            m_shadowGaugeRegen.Enable(true);
            m_airLunge?.Cancel();
            m_fireFist?.Cancel();
            m_reaperHarvest?.Cancel();
            m_krakenRage?.Cancel();
            m_finalSlash?.Cancel();
            m_airSlashCombo?.Cancel();
            m_sovereignImpale?.Cancel();
            m_hellTrident?.Cancel();
            m_foolsVerdict?.Cancel();
            m_soulFireBlast?.Cancel();
            m_edgedFury?.Cancel();
            m_ninthCircleSanction?.Cancel();
            m_doomsdayKong?.Cancel();
            m_backDiver?.Cancel();
            m_barrier?.Cancel();
            m_fencerFlash?.Cancel();
            m_diagonalSwordDash?.Cancel();
            m_championsUprising?.Cancel();
            m_eelecktrick?.Cancel();
            m_lightningSpear?.Cancel();
            m_icarusWings?.Cancel();
            m_airSlashRange?.Cancel();
            m_teleportingSkull?.Cancel();

            if (m_state.isGrounded)
            {
                m_movement?.SwitchConfigTo(Movement.Type.Jog);
            }
            ControllerDisabled?.Invoke(this, EventActionArgs.Empty);
        }

        public void Enable()
        {
            m_updateEnabled = true;
            ControllerEnabled?.Invoke(this, EventActionArgs.Empty);
        }

        private void HasTeleported(object sender, EventActionArgs eventArgs)
        {
            m_flinch?.CancelFlinch();
            m_idle?.Execute(false);
            m_movement?.Cancel();
            m_crouch?.Cancel();
            m_dash?.Cancel();
            m_shadowSlide?.Cancel();
            m_slide?.Cancel();
            m_wallStick?.Cancel();
            m_devilWings?.Cancel();
            m_shadowDash?.Cancel();
            m_basicSlashes?.Cancel();
            m_slashCombo?.Cancel();
            m_swordThrust?.Cancel();
            m_earthShaker?.Cancel();
            m_whip?.Cancel();
            m_whipCombo?.Cancel();
            m_projectileThrow?.Cancel();
            if (m_projectileThrow.willResetProjectile)
                m_projectileThrow.ResetProjectile();
            m_shadowMorph.Cancel();
            m_block?.Cancel();
            m_shadowGaugeRegen.Enable(true);
            m_airLunge?.Cancel();
            m_fireFist?.Cancel();
            m_reaperHarvest?.Cancel();
            m_krakenRage?.Cancel();
            m_finalSlash?.Cancel();
            m_airSlashCombo?.Cancel();
            m_sovereignImpale?.Cancel();
            m_hellTrident?.Cancel();
            m_foolsVerdict?.Cancel();
            m_soulFireBlast?.Cancel();
            m_edgedFury?.Cancel();
            m_ninthCircleSanction?.Cancel();
            m_doomsdayKong?.Cancel();
            m_backDiver?.Cancel();
            m_barrier?.Cancel();
            m_fencerFlash?.Cancel();
            m_diagonalSwordDash?.Cancel();
            m_championsUprising?.Cancel();
            m_eelecktrick?.Cancel();
            m_lightningSpear?.Cancel();
            m_icarusWings?.Cancel();
            m_airSlashRange?.Cancel();
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
                m_objectManipulation?.Cancel();
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
                        m_slashCombo.Cancel();
                        m_whip.Cancel();
                        m_whipCombo.Cancel();
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
                        m_projectileThrow.EndAim();
                        m_projectileThrow.Cancel();
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
            m_teleportingSkull?.DisableTeleport();
        }

        private void OnFlinch(object sender, EventActionArgs eventArgs)
        {
            if (m_teleportingSkull.canTeleport)
            {
                m_teleportingSkull?.Cancel();
                m_teleportingSkull.TeleportToProjectile();
            }
            else
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
                            m_swordThrust?.Cancel();
                            m_basicSlashes?.Cancel();
                            m_whip?.Cancel();
                            m_slashCombo?.Cancel();
                            m_slashCombo?.Reset();
                            m_whipCombo?.Cancel();
                            m_whipCombo?.Reset();
                            m_airLunge?.Cancel();
                            m_fireFist?.Cancel();
                            m_reaperHarvest?.Cancel();
                            m_finalSlash?.Cancel();
                            m_sovereignImpale?.Cancel();
                            m_hellTrident?.Cancel();
                            m_foolsVerdict?.Cancel();
                            m_ninthCircleSanction?.Cancel();
                            m_doomsdayKong?.Cancel();
                            m_backDiver?.Cancel();
                            m_barrier?.Cancel();
                            m_fencerFlash?.Cancel();
                            m_championsUprising?.Cancel();
                            m_eelecktrick?.Cancel();
                            m_icarusWings?.Cancel();
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
                    else if (m_state.isInShadowMode)
                    {
                        m_shadowMorph?.Cancel();
                    }
                    else
                    {
                        m_shadowGaugeRegen?.Enable(true);
                        m_idle?.Cancel();
                        m_movement?.Cancel();
                        m_block?.Cancel();
                        m_shadowSlide.Cancel();
                    }

                    GameplaySystem.cinema.ApplyCameraPeekMode(Cinematics.CameraPeekMode.None);
                }
                else
                {
                    if (m_state.isAttacking)
                    {
                        m_basicSlashes?.Cancel();
                        m_earthShaker?.Cancel();
                        m_whip?.Cancel();
                        if (m_projectileThrow.willResetProjectile)
                            m_projectileThrow.ResetProjectile();
                        m_projectileThrow?.Cancel();
                        m_airSlashCombo?.Cancel();
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
                    else if (m_state.isInShadowMode)
                    {
                        m_shadowMorph?.Cancel();
                    }

                    m_devilWings?.Cancel();
                    m_krakenRage?.Cancel();
                    m_soulFireBlast?.Cancel();
                    m_edgedFury?.Cancel();
                    m_reaperHarvest?.Cancel();
                    m_fencerFlash?.Cancel();
                    m_diagonalSwordDash?.Cancel();
                    m_lightningSpear?.Cancel();
                    m_airSlashRange?.Cancel();
                }
            }
        }

        private void OnProjectileThrowRequest(object sender, EventActionArgs eventArgs)
        {
            m_input.projectileThrowPressed = true;
        }

        private void ResetProjectile(object sender, EventActionArgs eventArgs)
        {
            if (m_projectileThrow.willResetProjectile)
                m_projectileThrow.ResetProjectile();
        }

        private void FlipCharacter()
        {
            var oppositeFacing = m_character.facing == HorizontalDirection.Right ? HorizontalDirection.Left : HorizontalDirection.Right;
            m_character.SetFacing(oppositeFacing);
            //m_basicSlashes.Cancel();
            m_slashCombo.Cancel();
            m_slashCombo.Reset();
            //m_whip.Cancel();
            m_whipCombo.Cancel();
            m_whipCombo.Reset();
            //m_airSlashCombo.Cancel();
            //m_airSlashCombo.Reset();
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
            m_shadowGaugeRegen.Enable(true);
            m_objectManipulation = m_character.GetComponentInChildren<ObjectManipulation>();
            m_platformDrop = m_character.GetComponentInChildren<PlayerOneWayPlatformDropHandle>();

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
            m_whipCombo = m_character.GetComponentInChildren<WhipAttackCombo>();
            m_projectileThrow = m_character.GetComponentInChildren<ProjectileThrow>();
            m_projectileThrow.ExecutionRequested += OnProjectileThrowRequest;
            m_projectileThrow.ProjectileThrown += ResetProjectile;
            m_block = m_character.GetComponentInChildren<PlayerBlock>();

            m_shadowBladeFX = m_character.GetComponentInChildren<ShadowbladeFX>();

            m_airLunge = m_character.GetComponentInChildren<AirLunge>();
            m_fireFist = m_character.GetComponentInChildren<FireFist>();
            m_reaperHarvest = m_character.GetComponentInChildren<ReaperHarvest>();
            m_krakenRage = m_character.GetComponentInChildren<KrakenRage>();
            m_finalSlash = m_character.GetComponentInChildren<FinalSlash>();
            m_airSlashCombo = m_character.GetComponentInChildren<AirSlashCombo>();
            m_sovereignImpale = m_character.GetComponentInChildren<SovereignImpale>();
            m_hellTrident = m_character.GetComponentInChildren<HellTrident>();
            m_foolsVerdict = m_character.GetComponentInChildren<FoolsVerdict>();
            m_soulFireBlast = m_character.GetComponentInChildren<SoulFireBlast>();
            m_edgedFury = m_character.GetComponentInChildren<EdgedFury>();
            m_ninthCircleSanction = m_character.GetComponentInChildren<NinthCircleSanction>();
            m_doomsdayKong = m_character.GetComponentInChildren<DoomsdayKong>();
            m_backDiver = m_character.GetComponentInChildren<BackDiver>();
            m_barrier = m_character.GetComponentInChildren<Barrier>();
            m_fencerFlash = m_character.GetComponentInChildren<FencerFlash>();
            m_diagonalSwordDash = m_character.GetComponentInChildren<DiagonalSwordDash>();
            m_championsUprising = m_character.GetComponentInChildren<ChampionsUprising>();
            m_eelecktrick = m_character.GetComponentInChildren<Eelecktrick>();
            m_lightningSpear = m_character.GetComponentInChildren<LightningSpear>();
            m_icarusWings = m_character.GetComponentInChildren<IcarusWings>();
            m_teleportingSkull = m_character.GetComponentInChildren<TeleportingSkull>();
            m_teleportingSkull.Teleported += HasTeleported;
            m_airSlashRange = m_character.GetComponentInChildren<AirSlashRange>();


            //Intro Controller
            m_introController = GetComponent<PlayerIntroControlsController>();

            //Abilities
            m_abilities = GetComponentInParent<Player>().GetComponentInChildren<CombatArts>();

            m_updateEnabled = true;
        }

        private void FixedUpdate()
        {

            if (m_state.isDead)
                return;

            if (m_introController.IsUsingIntroControls())
            {
                m_introController.HandleIntroControlsFixedUpdate();
                return;
            }

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
                if (m_earthShaker.CanEarthShaker())
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
                        else if ((m_input.horizontalInput > 0 && m_character.facing == HorizontalDirection.Left) || (m_input.horizontalInput < 0 && m_character.facing == HorizontalDirection.Right))
                        {
                            m_wallSlide?.Cancel();
                            m_wallStick?.Cancel();
                        }

                        return;
                    }

                    if (m_state.isShadowBlade && !m_shadowBladeFX.canShadowblade)
                    {
                        m_shadowBladeFX.EnableShadowblade();
                    }
                    else if (!m_state.isShadowBlade && m_shadowBladeFX.canShadowblade)
                    {
                        m_shadowBladeFX.DisableShadowblade();
                    }

                    if ((int)m_character.facing == m_input.horizontalInput /*&& m_earthShaker.CanEarthShaker()*/)
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
                }

                m_initialDescentBoost?.Handle();
                if (m_rigidbody.velocity.y < m_groundedness?.groundCheckOffset)
                {
                    if (m_state.forcedCurrentGroundedness == false)
                    {
                        m_groundedness?.Evaluate();
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

            if (m_introController.IsUsingIntroControls())
            {
                m_introController.HandleIntroControls();
                return;
            }

            if (m_shadowGaugeRegen?.CanRegen() ?? false)
            {
                m_shadowGaugeRegen.Execute();
            }

            m_tracker.Execute(m_input);
            m_platformDrop.HandleDroppablePlatformCollision();

            if (m_state.isInShadowMode)
            {
                if (m_shadowMorph.HaveEnoughSourceForExecution())
                {
                    m_shadowMorph.ConsumeSource();
                }
                else
                {
                    m_shadowMorph?.Cancel();
                    m_shadowGaugeRegen?.Enable(true);
                }
            }

            if (m_state.waitForBehaviour /*|| !m_earthShaker.CanEarthShaker()*/)
                return;

            if (m_state.isCombatReady)
            {
                m_combatReadiness?.HandleDuration();
            }

            if (m_slashCombo.CanSlashCombo() == false)
            {
                m_slashCombo.HandleSlashComboTimer();
            }

            if (m_slashCombo.CanMove() == false)
            {
                m_slashCombo.HandleMovementTimer();
            }

            if (m_whipCombo.CanWhipCombo() == false)
            {
                m_whipCombo.HandleComboTimer();
            }

            if (m_whipCombo.CanMove() == false)
            {
                m_whipCombo.HandleMovementTimer();
            }

            if (m_whip.CanMove() == false)
            {
                m_whip.HandleMovementTimer();
            }
            #region Combat Arts Cooldowns

            if (m_airLunge.CanAirLunge() == false)
            {
                m_airLunge.HandleAttackTimer();
            }

            if (m_fireFist.CanFireFist() == false)
            {
                m_fireFist.HandleAttackTimer();
            }

            if (m_fireFist.CanMove() == false)
            {
                m_fireFist.HandleMovementTimer();
            }

            if (m_finalSlash.CanFinalSlash() == false && !m_state.isChargingFinalSlash)
            {
                m_finalSlash.HandleAttackTimer();
            }

            if (m_finalSlash.CanMove() == false)
            {
                m_finalSlash.HandleMovementTimer();
            }

            if (m_airSlashCombo.CanMove() == false)
            {
                m_airSlashCombo.HandleMovementTimer();
            }

            if (m_sovereignImpale.CanSovereignImpale() == false)
            {
                m_sovereignImpale.HandleAttackTimer();
            }

            if (m_sovereignImpale.CanMove() == false)
            {
                m_sovereignImpale.HandleMovementTimer();
            }

            if (m_hellTrident.CanHellTrident() == false)
            {
                m_hellTrident.HandleAttackTimer();
            }

            if (m_hellTrident.CanMove() == false)
            {
                m_hellTrident.HandleMovementTimer();
            }

            if (m_foolsVerdict.CanFoolsVerdict() == false)
            {
                m_foolsVerdict.HandleAttackTimer();
            }

            if (m_foolsVerdict.CanMove() == false)
            {
                m_foolsVerdict.HandleMovementTimer();
            }

            if (m_soulFireBlast.CanSoulFireBlast() == false)
            {
                m_soulFireBlast.HandleAttackTimer();
            }

            if (m_ninthCircleSanction.CanNinthCircleSanction() == false)
            {
                m_ninthCircleSanction.HandleAttackTimer();
            }

            if (m_ninthCircleSanction.CanMove() == false)
            {
                m_ninthCircleSanction.HandleMovementTimer();
            }

            if (m_doomsdayKong.CanMove() == false)
            {
                m_doomsdayKong.HandleMovementTimer();
            }

            if (m_championsUprising.CanChampionsUprising() == false)
            {
                m_championsUprising.HandleAttackTimer();
            }

            if (m_barrier.CanMove() == false)
            {
                m_barrier.HandleMovementTimer();
            }

            if (m_eelecktrick.CanEelecktrick() == false)
            {
                m_eelecktrick.HandleAttackTimer();
            }

            if (m_eelecktrick.CanMove() == false)
            {
                m_eelecktrick.HandleMovementTimer();
            }

            if (m_lightningSpear.CanReset() == true)
            {
                m_lightningSpear.HandleResetTimer();
            }

            if (m_lightningSpear.CanMove() == false)
            {
                m_lightningSpear.HandleMovementTimer();
            }

            if (m_icarusWings.CanIcarusWings() == false)
            {
                m_icarusWings.HandleAttackTimer();
            }

            if (m_airSlashRange.CanReset() == true)
            {
                m_airSlashRange.HandleResetTimer();
            }

            if (m_airSlashRange.CanMove() == false)
            {
                m_airSlashRange.HandleMovementTimer();
            }
            #endregion

            if (m_state.canAttack == true)
            {
                m_slashCombo.HandleComboResetTimer();
                m_whipCombo.HandleComboResetTimer();
                m_airSlashCombo.HandleComboResetTimer();
            }
            else
            {
                if (m_state.isAttacking == false)
                {
                    m_basicSlashes.HandleNextAttackDelay();
                    m_slashCombo.HandleComboAttackDelay();
                    m_whip.HandleNextAttackDelay();
                    m_whipCombo.HandleComboAttackDelay();
                    m_projectileThrow.HandleNextAttackDelay();
                    m_airSlashCombo.HandleComboAttackDelay();
                }
            }

            if (m_state.isGrounded)
            {
                HandleGroundBehaviour();
                m_basicSlashes?.ResetAerialGravityControl();
                m_basicSlashes?.ResetAirAttacks();
                m_whip?.ResetAerialGravityControl();
                m_whip?.ResetAirAttacks();
                m_devilWings?.EnableLevitate();
                if (!m_airSlashCombo.CanAirSlashCombo())
                {
                    m_airSlashCombo?.ResetAirSlashCombo();
                }
                #region Combat Arts Cooldowns

                if (m_doomsdayKong.CanDoomsdayKong() == false)
                {
                    m_doomsdayKong.HandleAttackTimer();
                }

                if (m_diagonalSwordDash.CanDiagonalSwordDash() == false)
                {
                    m_diagonalSwordDash.HandleAttackTimer();
                }

                if (m_backDiver.CanBackDiver() == false)
                {
                    m_backDiver.HandleAttackTimer();
                }

                if (m_reaperHarvest.CanReaperHarvest() == false)
                {
                    m_reaperHarvest.HandleAttackTimer();
                }

                if (m_fencerFlash.CanFencerFlash() == false)
                {
                    m_fencerFlash.HandleAttackTimer();
                }

                if (m_edgedFury.CanEdgedFury() == false)
                {
                    m_edgedFury.HandleAttackTimer();
                }

                if (m_lightningSpear.CanLightningSpear() == false)
                {
                    m_lightningSpear.HandleAttackTimer();
                }

                if (m_airSlashRange.CanAirSlashRange() == false)
                {
                    m_airSlashRange.HandleAttackTimer();
                }
                #endregion
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
                if (m_state.isDoingSwordThrust)
                {
                    HandleSwordThrust();
                }
                if (m_rigidbody.velocity.y < 0)
                {
                    m_groundedness?.Evaluate();
                }
                //if (m_state.isChargingLightningSpear)
                //{
                //    if (!m_input.lightningSpearHeld && m_lightningSpear.CanMove())
                //    {
                //        m_lightningSpear.ReleaseHold();
                //    }
                //    return;
                //}
                //if (m_input.edgedFuryReleased && !m_state.isChargingLightningSpear)
                //{
                //    m_edgedFury?.EndExecution();
                //    return;
                //}
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
                    else if (m_skills.IsModuleActive(PrimarySkill.WallMovement))
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
                }
                else
                {
                    var verticalInput = m_input.verticalInput;
                    if (verticalInput < 0 && m_state.canWallCrawl == true)
                    {
                        m_wallSlide?.Cancel();
                        m_wallMovement?.Move(verticalInput);

                        m_groundedness?.Evaluate();
                        if (m_state.isGrounded)
                            return;

                        if ((m_wallMovement?.IsThereAWall(WallMovement.SensorType.Body) ?? false) == false)
                        {
                            m_wallMovement?.Cancel();
                            //m_wallStick?.Cancel();
                        }
                    }
                    else if (verticalInput > 0 && m_state.canWallCrawl == true)
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

                if (m_state.canAttack)
                {
                    if (m_input.airSlashComboPressed && m_airSlashCombo.CanAirSlashCombo() /*&& m_abilities.IsAbilityActivated(CombatArt.AirSlashCombo)*/)
                    {
                        m_activeDash?.Cancel();

                        PrepareForMidairAttack();
                        if (m_airSlashCombo.CanAirSlashCombo())
                            m_airSlashCombo.Execute();
                        return;
                    }
                }
            }
            else if (m_state.isSliding)
            {
                HandleSlide();
            }
            else
            {


                if (m_state.canAttack)
                {
                    #region MidAir Attacks
                    if (m_input.slashPressed && m_input.verticalInput < 0 && m_skills.IsModuleActive(PrimarySkill.EarthShaker) && !m_input.diagonalSwordDashPressed)
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            if (m_skills.IsModuleActive(PrimarySkill.EarthShaker))
                            {
                                PrepareForMidairAttack();

                                m_earthShaker?.StartExecution();
                            }

                            return;
                        }
                    }
                    else if (m_input.slashPressed && m_basicSlashes.CanAirAttack() && !m_input.airSlashComboPressed && !m_input.reaperHarvestPressed && !m_input.diagonalSwordDashPressed)
                    {
                        PrepareForMidairAttack();
                        m_devilWings?.EnableLevitate();
                        m_extraJump?.Cancel();
                        //m_airSlashCombo?.Cancel();

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
                    else if (m_input.airSlashComboPressed && m_airSlashCombo.CanAirSlashCombo() && !m_input.reaperHarvestPressed && !m_input.diagonalSwordDashPressed && !m_wallStick.IsThereAWall() /*&& m_abilities.IsAbilityActivated(CombatArt.AirSlashCombo)*/)
                    {
                        m_basicSlashes?.Cancel();
                        m_whip?.Cancel();

                        if (m_state.isInShadowMode == true)
                        {
                            if (m_shadowMorph.IsAttackAllowed() == true)
                            {
                                PrepareForMidairAttack();
                                m_airSlashCombo.Execute();
                                return;
                            }
                        }
                        else
                        {
                            PrepareForMidairAttack();
                            m_airSlashCombo.Execute();
                            return;
                        }
                    }
                    else if (m_input.reaperHarvestPressed && m_reaperHarvest.CanReaperHarvest() && m_abilities.IsAbilityActivated(CombatArt.ReaperHarvest))
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            PrepareForMidairAttack();
                            m_idle?.Cancel();
                            m_movement?.Cancel();
                            m_devilWings?.Cancel();
                            m_extraJump?.Cancel();

                            if (IsFacingInput())
                            {
                                m_reaperHarvest.Execute(ReaperHarvest.ReaperHarvestState.Midair);
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.fencerFlashPressed && m_fencerFlash.CanFencerFlash() && m_abilities.IsAbilityActivated(CombatArt.FencerFlash))
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            PrepareForMidairAttack();
                            m_idle?.Cancel();
                            m_movement?.Cancel();
                            m_devilWings?.Cancel();
                            m_extraJump?.Cancel();

                            if (IsFacingInput())
                            {
                                m_fencerFlash.Execute(FencerFlash.FencerFlashState.Midair);
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.whipPressed && m_whip.CanAirWhip() && !m_input.edgedFuryPressed && !m_input.fencerFlashPressed)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.Whip))
                        {
                            PrepareForMidairAttack();
                            m_devilWings?.EnableLevitate();
                            m_extraJump?.Cancel();

                            if (m_input.verticalInput > 0)
                            {
                                m_whip.Execute(WhipAttack.Type.MidAir_Overhead);
                            }
                            else
                            {
                                m_whip.Execute(WhipAttack.Type.MidAir_Forward);
                            }
                        }

                        return;
                    }
                    else if (m_input.edgedFuryPressed && m_edgedFury.CanEdgedFury() && !m_input.fencerFlashPressed && !m_input.lightningSpearPressed && m_abilities.IsAbilityActivated(CombatArt.EdgedFury))
                    {
                        PrepareForMidairAttack();
                        m_devilWings?.Cancel();
                        m_extraJump?.Cancel();

                        m_edgedFury.Execute();

                        return;
                    }
                    else if (m_input.teleportingSkullPressed && m_teleportingSkull.canTeleport && m_abilities.IsAbilityActivated(CombatArt.TeleportingSkull))
                    {
                        m_teleportingSkull.TeleportToProjectile();
                        return;
                    }
                    else if (m_input.soulFireBlastPressed && !m_input.krakenRagePressed && m_soulFireBlast.CanSoulFireBlast() && m_abilities.IsAbilityActivated(CombatArt.SoulfireBlast))
                    {
                        PrepareForMidairAttack();
                        m_devilWings?.Cancel();
                        m_extraJump?.Cancel();

                        m_soulFireBlast.Execute();

                        return;
                    }
                    //else if (m_input.krakenRagePressed)
                    //{
                    //    PrepareForMidairAttack();
                    //    m_devilWings?.Cancel();
                    //    m_extraJump?.Cancel();
                    //    m_krakenRage.Execute();

                    //    return;
                    //}
                    else if (m_input.diagonalSwordDashPressed && m_diagonalSwordDash.CanDiagonalSwordDash() && m_abilities.IsAbilityActivated(CombatArt.DiagonalSwordDash))
                    {
                        PrepareForMidairAttack();
                        m_devilWings?.Cancel();
                        m_extraJump?.Cancel();

                        m_diagonalSwordDash.Execute();

                        return;
                    }
                    else if (m_input.lightningSpearPressed && m_airSlashRange.CanAirSlashRange() && (m_abilities.IsAbilityActivated(CombatArt.AirSlashCombo) && !m_abilities.IsAbilityActivated(CombatArt.LightningSpear)))
                    {
                        if (m_state.isInShadowMode == false)
                        {

                            PrepareForMidairAttack();
                            if (IsFacingInput())
                            {
                                m_airSlashRange.Execute();
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.lightningSpearPressed && m_lightningSpear.CanLightningSpear() && m_abilities.IsAbilityActivated(CombatArt.LightningSpear))
                    {
                        if (m_state.isInShadowMode == false)
                        {

                            PrepareForMidairAttack();
                            if (IsFacingInput())
                            {
                                m_lightningSpear.Execute();
                            }
                            return;
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
                else if (m_input.jumpPressed && !m_input.backDiverPressed)
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
                else if (((m_input.levitatePressed && m_state.isLevitating == false) || (m_input.levitateHeld && m_state.isLevitating == false)) && m_devilWings.CanLevitate() && !m_input.backDiverPressed)
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
                else if (m_input.backDiverPressed && m_backDiver.CanBackDiver() && m_backDiver.HaveSpacetoExecute() && m_earthShaker.CanEarthShaker() && m_abilities.IsAbilityActivated(CombatArt.BackDiver))
                {
                    if (m_state.isInShadowMode == false)
                    {
                        m_devilWings?.Cancel();
                        m_extraJump?.Cancel();

                        PrepareForGroundAttack();
                        if (IsFacingInput())
                        {
                            m_backDiver.Execute();
                        }
                        return;
                    }

                    return;
                }
                else
                {
                    if (m_state.isInShadowMode == false)
                    {
                        m_movement.Move(m_input.horizontalInput, true);
                    }

                    if (m_input.horizontalInput != 0)
                    {
                        if (m_state.isHighJumping == false && m_state.isLevitating == false)
                        {
                            if (m_state.isInShadowMode == false)
                            {
                                if (m_skills.IsModuleActive(PrimarySkill.WallMovement))
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
                }
                #endregion

                if (m_input.levitateHeld && !m_devilWings.CanDetectWall() && (m_devilWings?.HaveEnoughSourceForMaintainingHeight() ?? true) == true)
                    m_devilWings.EnableLevitate();

                if (m_state.isLevitating)
                {
                    m_devilWings?.MaintainHeight();
                    m_devilWings?.GiveMovementBoost();
                    m_devilWings?.ConsumeSource();
                    if (m_input.levitateHeld == false || (m_devilWings?.HaveEnoughSourceForMaintainingHeight() ?? true) == false || m_devilWings.CanDetectWall())
                    {
                        m_devilWings?.Cancel();
                    }
                }
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

                    m_projectileThrow.MoveAim(m_input.m_mouseDelta.normalized, GameplaySystem.cinema.mainCamera.ScreenToWorldPoint(m_input.m_mousePosition));

                    if (m_projectileThrow?.HasReachedVerticalThreshold() == true)
                    {
                        GameplaySystem.cinema.ApplyCameraPeekMode(Cinematics.CameraPeekMode.Up);
                    }
                    else
                    {
                        GameplaySystem.cinema.ApplyCameraPeekMode(Cinematics.CameraPeekMode.None);
                    }

                    if (m_input.projectileThrowReleased || m_input.projectileThrowHeld == false)
                    {
                        m_projectileThrow.EndAim();
                        m_projectileThrow.StartThrow();
                        GameplaySystem.cinema.ApplyCameraPeekMode(Cinematics.CameraPeekMode.None);

                        //if (m_projectileThrow.willResetProjectile)
                        //    m_projectileThrow.ResetProjectile();
                    }
                }
                else if (m_state.isDoingSwordThrust)
                {
                    HandleSwordThrust();
                    return;
                }
                else
                {
                    m_attackRegistrator?.ResetHitCache();
                }
                if (m_state.isChargingFinalSlash)
                {
                    if (!m_input.finalSlashHeld && m_finalSlash.CanMove() /*|| m_input.slashPressed && !m_finalSlash.CanFinalSlash()*/)
                    {
                        m_finalSlash.ExecuteDash();
                    }
                    //if (m_input.finalSlashReleased && !m_finalSlash.CanFinalSlash() || m_input.slashPressed && !m_finalSlash.CanFinalSlash())
                    //{
                    //    m_finalSlash.ExecuteDash();
                    //}
                    return;
                }
                else if (m_state.isChargingEelecktrick)
                {
                    if (!m_input.eelecktrickHeld && m_eelecktrick.CanMove())
                    {
                        m_eelecktrick.ReleaseHold();
                    }
                    return;
                }
                if (m_barrier.IsDoingBarrier())
                {
                    if (!m_input.barrierHeld && m_barrier.CanMove())
                    {
                        m_barrier?.EndExecution();
                    }
                }
            }
            else if (m_state.isBlocking && m_earthShaker.CanEarthShaker())
            {
                if (m_input.blockHeld == false)
                {
                    m_block.Cancel();
                }
            }
            else if (m_state.isSliding && m_earthShaker.CanEarthShaker())
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
            else if (m_state.isCrouched && m_earthShaker.CanEarthShaker() && !m_input.backDiverPressed && !m_input.sovereignImpalePressed && !m_input.fireFistPressed && !m_input.projectileThrowPressed)
            {
                if (m_state.canAttack)
                {
                    if (m_input.slashPressed)
                    {
                        if (m_state.isInShadowMode == true)
                        {
                            if (m_shadowMorph.IsAttackAllowed() == true)
                            {
                                PrepareForGroundAttack();
                                m_basicSlashes.Execute(BasicSlashes.Type.Crouch);
                                return;
                            }
                        }
                        else
                        {
                            PrepareForGroundAttack();
                            m_basicSlashes.Execute(BasicSlashes.Type.Crouch);
                            return;
                        }
                    }
                    else if (m_input.whipPressed && !m_input.ninthCircleSanctionPressed)
                    {
                        if (m_state.isInShadowMode == true)
                        {
                            if (m_shadowMorph.IsAttackAllowed() == true)
                            {
                                if (m_skills.IsModuleActive(PrimarySkill.Whip))
                                {
                                    PrepareForGroundAttack();
                                    m_whip.Execute(WhipAttack.Type.Crouch_Forward);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (m_skills.IsModuleActive(PrimarySkill.Whip))
                            {
                                PrepareForGroundAttack();
                                m_whip.Execute(WhipAttack.Type.Crouch_Forward);
                                return;
                            }
                        }

                        return;
                    }
                    else if (m_input.ninthCircleSanctionPressed && m_ninthCircleSanction.CanNinthCircleSanction() && m_abilities.IsAbilityActivated(CombatArt.NinthCircleSanction))
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            m_crouch?.Cancel();
                            m_ninthCircleSanction.Reset();
                            PrepareForGroundAttack();
                            m_movement?.SwitchConfigTo(Movement.Type.Jog);
                            if (IsFacingInput())
                            {
                                m_ninthCircleSanction.Execute();
                            }
                            return;
                        }

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

                if (m_input.blockHeld == true)
                {
                    PrepareForGroundAttack();
                    m_block.Execute();
                    return;
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
                            m_whipCombo?.Cancel();
                            m_whipCombo?.Reset();
                            m_earthShaker?.Cancel();
                            m_objectManipulation?.Cancel();
                            ExecuteSlide();
                        }
                    }
                }

                if (CanMove()
                    && m_earthShaker.CanEarthShaker())
                    MoveCharacter(false);

                if (m_input.crouchHeld == false && m_earthShaker.CanEarthShaker())
                {
                    if (m_crouch?.IsThereNoCeiling() ?? true)
                    {
                        m_crouch?.Cancel();
                        m_basicSlashes.ResetAttackDelay();
                        m_movement?.SwitchConfigTo(Movement.Type.Jog);
                        m_basicSlashes.AttackOver();
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

                if (m_state.canAttack)
                {
                    if (m_input.slashPressed && !m_input.airLungeSlashPressed && !m_input.reaperHarvestPressed /*!(m_input.levitateHeld && m_input.slashHeld)*/)
                    {
                        m_activeDash?.Cancel();

                        if (m_input.verticalInput > 0)
                        {
                            PrepareForGroundAttack();
                            m_basicSlashes.Execute(BasicSlashes.Type.Ground_Overhead);
                            return;
                        }
                        else
                        {
                            PrepareForGroundAttack();
                            if (m_slashCombo.CanSlashCombo())
                                m_slashCombo.Execute();
                            return;
                        }
                    }
                }
            }
            else if (m_state.isGrabbing)
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
                            if (m_state.isPushing)
                            {
                                m_movement?.SwitchConfigTo(Movement.Type.Push);
                            }
                            else
                            {
                                m_movement?.SwitchConfigTo(Movement.Type.Pull);
                            }

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

                MoveCharacter(m_state.isGrabbing);
            }
            else
            {

                //From Standing
                if (m_state.canAttack)
                {
                    #region Ground Attacks
                    if (m_input.slashPressed && !m_input.airLungeSlashPressed && !m_input.reaperHarvestPressed && !m_input.finalSlashPressed && !m_input.sovereignImpalePressed)
                    {
                        m_whip.Cancel();
                        m_whipCombo.Cancel();
                        m_whipCombo.Reset();
                        if (m_state.isInShadowMode == true)
                        {
                            if (m_shadowMorph.IsAttackAllowed() == true)
                            {
                                if (m_input.verticalInput > 0)
                                {
                                    PrepareForGroundAttack();
                                    m_basicSlashes.Execute(BasicSlashes.Type.Ground_Overhead);
                                    return;
                                }
                                else
                                {
                                    if (m_slashCombo.CanSlashCombo() == true)
                                    {
                                        PrepareForGroundAttack();
                                        m_slashCombo.Execute();
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (m_input.verticalInput > 0)
                            {
                                PrepareForGroundAttack();
                                m_basicSlashes.Execute(BasicSlashes.Type.Ground_Overhead);
                                return;
                            }
                            else
                            {
                                if (m_slashCombo.CanSlashCombo() == true)
                                {
                                    PrepareForGroundAttack();
                                    m_slashCombo.Execute();
                                    return;
                                }
                            }
                        }
                    }
                    else if (m_input.whipPressed && !m_input.fencerFlashPressed && !m_input.championsUprisingPressed)
                    {
                        if (m_state.isInShadowMode == false)
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
                                    if (m_input.horizontalInput != 0)
                                    {
                                        if (IsFacingInput())
                                        {
                                            if (m_whipCombo.CanWhipCombo())
                                                m_whipCombo.Execute();
                                        }
                                    }
                                    else
                                    {
                                        m_whipCombo.Reset();
                                        m_whip.Execute(WhipAttack.Type.Ground_Forward);
                                    }
                                    return;
                                }
                            }
                        }

                        return;
                    }
                    else if (m_input.airLungeSlashPressed && m_airLunge.CanAirLunge() && m_earthShaker.CanEarthShaker() && m_abilities.IsAbilityActivated(CombatArt.AirLunge))
                    {
                        if (m_state.isInShadowMode == false)
                        {

                            m_crouch?.Cancel();
                            m_airLunge.Reset();
                            PrepareForGroundAttack();
                            m_movement?.SwitchConfigTo(Movement.Type.Jog);
                            if (IsFacingInput())
                            {
                                m_airLunge.Execute();
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.reaperHarvestPressed && m_reaperHarvest.CanReaperHarvest() && m_abilities.IsAbilityActivated(CombatArt.ReaperHarvest))
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            m_crouch?.Cancel();
                            m_reaperHarvest?.Reset();
                            PrepareForGroundAttack();
                            m_movement?.SwitchConfigTo(Movement.Type.Jog);
                            if (IsFacingInput())
                            {
                                m_reaperHarvest.Execute(ReaperHarvest.ReaperHarvestState.Grounded);
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.championsUprisingPressed && m_championsUprising.CanChampionsUprising() && !m_input.fencerFlashPressed && m_abilities.IsAbilityActivated(CombatArt.ChampionsUprising))
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            m_idle?.Cancel();
                            m_movement?.Cancel();

                            PrepareForGroundAttack();
                            if (IsFacingInput())
                            {
                                m_championsUprising.Execute();
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.fencerFlashPressed && m_fencerFlash.CanFencerFlash() && m_abilities.IsAbilityActivated(CombatArt.FencerFlash))
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            m_idle?.Cancel();
                            m_movement?.Cancel();

                            PrepareForGroundAttack();
                            if (IsFacingInput())
                            {
                                m_fencerFlash.Execute(FencerFlash.FencerFlashState.Grounded);
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.finalSlashPressed && m_finalSlash.CanFinalSlash() && !m_input.airLungeSlashPressed && !m_input.sovereignImpalePressed && m_abilities.IsAbilityActivated(CombatArt.FinalSlash))
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            //m_slashCombo?.Cancel();
                            //m_slashCombo?.Reset();

                            PrepareForGroundAttack();
                            if (IsFacingInput())
                            {
                                m_finalSlash.Execute();
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.eelecktrickPressed && m_eelecktrick.CanEelecktrick() && !m_input.airLungeSlashPressed && m_abilities.IsAbilityActivated(CombatArt.Eelecktrick))
                    {
                        if (m_state.isInShadowMode == false)
                        {

                            PrepareForGroundAttack();
                            if (IsFacingInput())
                            {
                                m_eelecktrick.Execute();
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.teleportingSkullPressed && m_teleportingSkull.canTeleport && !m_input.foolsVerdictPressed && !m_input.fireFistPressed && !m_input.hellTridentPressed)
                    {
                        m_teleportingSkull.TeleportToProjectile();
                        return;
                    }
                    else if (m_input.projectileThrowPressed && !m_input.foolsVerdictPressed && !m_input.hellTridentPressed && !m_input.fireFistPressed)
                    {
                        if (m_input.teleportingSkullPressed && m_abilities.IsAbilityActivated(CombatArt.TeleportingSkull))
                        {
                            m_projectileThrow.SetProjectileInfo(m_teleportingSkull.projectile);
                            m_projectileThrow.WillResetProjectile();
                            m_teleportingSkull.Execute();
                        }

                        if (m_skills.IsModuleActive(PrimarySkill.SkullThrow))
                        {
                            m_crouch?.Cancel();
                            m_movement?.SwitchConfigTo(Movement.Type.Jog);
                            PrepareForGroundAttack();
                            m_projectileThrow.StartAim();
                            m_projectileThrow.Execute();
                        }
                        return;
                    }
                    else if (m_input.fireFistPressed && m_fireFist.CanFireFist() && m_abilities.IsAbilityActivated(CombatArt.FireFist))
                    {
                        if (m_state.isInShadowMode == false)
                        {

                            PrepareForGroundAttack();
                            if (IsFacingInput())
                            {
                                m_fireFist.Execute();
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.hellTridentPressed && m_hellTrident.CanHellTrident() && m_abilities.IsAbilityActivated(CombatArt.HellTrident))
                    {
                        if (m_state.isInShadowMode == false)
                        {

                            PrepareForGroundAttack();
                            if (IsFacingInput())
                            {
                                m_hellTrident.Execute();
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.foolsVerdictPressed && m_foolsVerdict.CanFoolsVerdict() && m_abilities.IsAbilityActivated(CombatArt.FoolsVerdict))
                    {
                        if (m_state.isInShadowMode == false)
                        {

                            PrepareForGroundAttack();
                            if (IsFacingInput())
                            {
                                m_foolsVerdict.Execute();
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.doomsdayKongPressed && m_doomsdayKong.CanDoomsdayKong() && m_abilities.IsAbilityActivated(CombatArt.DoomsdayKong))
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            PrepareForGroundAttack();
                            if (IsFacingInput())
                            {
                                m_doomsdayKong.Execute();
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.backDiverPressed && m_backDiver.CanBackDiver() && m_backDiver.HaveSpacetoExecute() && m_earthShaker.CanEarthShaker() && m_abilities.IsAbilityActivated(CombatArt.BackDiver))
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            m_crouch?.Cancel();
                            m_backDiver.Reset();
                            PrepareForGroundAttack();
                            m_movement?.SwitchConfigTo(Movement.Type.Jog);
                            if (IsFacingInput())
                            {
                                m_backDiver.Execute();
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.sovereignImpalePressed && m_sovereignImpale.CanSovereignImpale() && m_abilities.IsAbilityActivated(CombatArt.SovereignImpale))
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            m_crouch?.Cancel();
                            m_sovereignImpale.Reset();
                            PrepareForGroundAttack();
                            m_movement?.SwitchConfigTo(Movement.Type.Jog);
                            if (IsFacingInput())
                            {
                                m_sovereignImpale.Execute();
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.barrierPressed /*&& m_abilities.IsAbilityActivated(CombatArt.Barrier)*/)
                    {
                        if (m_state.isInShadowMode == false)
                        {

                            PrepareForGroundAttack();
                            if (IsFacingInput())
                            {
                                m_barrier.Execute();
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_input.icarusWingsPressed && m_icarusWings.CanIcarusWings() && m_abilities.IsAbilityActivated(CombatArt.IcarusWings))
                    {
                        if (m_state.isInShadowMode == false)
                        {

                            //m_crouch?.Cancel();
                            m_icarusWings.Reset();
                            PrepareForGroundAttack();
                            //m_movement?.SwitchConfigTo(Movement.Type.Jog);
                            if (IsFacingInput())
                            {
                                m_icarusWings.Execute();
                            }
                            return;
                        }

                        return;
                    }
                    else if (m_state.isInShadowMode == false)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.SwordThrust))
                        {
                            if (m_input.slashHeld && !m_input.airLungeSlashPressed && !m_input.reaperHarvestPressed/*!(m_input.levitateHeld && m_input.slashHeld)*/)
                            {
                                PrepareForGroundAttack();
                                m_chargeAttackHandle.Set(m_swordThrust, () => m_input.slashHeld);

                                //Start SwordThrust
                                m_swordThrust?.StartCharge();

                                return;
                            }
                            else
                            {
                                m_swordThrust?.Cancel();
                            }
                        }
                    }
                    #endregion
                }

                if (m_input.blockHeld == true)
                {
                    //PrepareForGroundAttack();
                    //m_block.Execute();
                    return;
                }

                if (m_input.interactPressed && m_state.isInShadowMode == false)
                {
                    m_objectInteraction?.Interact();
                    return;
                }

                if (m_input.shadowMorphPressed)
                {
                    if (m_skills.IsModuleActive(PrimarySkill.ShadowMorph))
                    {
                        m_idle?.Cancel();
                        m_movement?.Cancel();
                        m_objectManipulation?.Cancel();

                        if (m_state.isInShadowMode)
                        {
                            m_shadowMorph.Cancel();
                            m_shadowGaugeRegen?.Enable(true);
                        }
                        else
                        {
                            m_shadowGaugeRegen?.Enable(false);
                            m_shadowMorph.Execute();
                        }

                        return;
                    }
                }

                if (m_input.grabPressed || m_input.grabHeld)
                {
                    if (m_objectManipulation?.IsThereAMovableObject() ?? false)
                    {
                        m_idle?.Cancel();
                        m_objectManipulation?.Execute();
                    }
                }

                #region Non Combat Standing
                if (m_input.crouchHeld && m_earthShaker.CanEarthShaker())
                {
                    m_crouch?.Execute();
                    m_idle?.Cancel();
                    m_movement?.SwitchConfigTo(Movement.Type.Crouch);
                }
                else if (m_input.dashPressed)
                {
                    if (m_state.isInShadowMode == false)
                    {
                        if (m_skills.IsModuleActive(PrimarySkill.Dash) && m_state.canDash)
                        {
                            m_idle?.Cancel();
                            m_movement?.Cancel();
                            m_whipCombo?.Cancel();
                            m_whipCombo?.Reset();
                            m_earthShaker?.Cancel();
                            m_objectManipulation?.Cancel();
                            ExecuteDash();
                        }
                    }
                }
                else if (m_input.jumpPressed && m_earthShaker.CanEarthShaker() && !m_input.backDiverPressed)
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
                    if (CanMove()
                        && m_earthShaker.CanEarthShaker())
                        MoveCharacter(m_state.isGrabbing);

                    if (m_input.horizontalInput != 0)
                    {
                        if (m_stepClimb.CheckForStepClimbableSurface())
                        {
                            m_stepClimb.ClimbSurface();
                        }
                    }
                }
                #endregion
            }
            m_objectManipulation?.LookForMoveableObject();
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

        private void HandleSwordThrust()
        {
            m_swordThrust?.HandleDurationTimer();
            if (m_swordThrust?.IsSwordThrustDurationOver() ?? true)
            {
                m_swordThrust?.EndSwordThrust();
                m_swordThrust?.ResetCooldownTimer();
                m_swordThrust?.ResetDurationTimer();
            }
            else
            {
                m_swordThrust?.Execute();
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
                    if (m_crouch.IsThereNoCeiling() || !m_slide.HasGroundToSlideOn() || !m_shadowSlide.HasGroundToSlideOn())
                    {
                        m_activeSlide?.Cancel();
                        m_activeSlide?.ResetCooldownTimer();
                    }
                    else
                    {
                        if (m_crouch.IsCrouchingPossible() || !m_slide.HasGroundToSlideOn() || !m_shadowSlide.HasGroundToSlideOn())
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
            if (m_skills.IsModuleActive(PrimarySkill.ShadowDash))
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
            if (m_skills.IsModuleActive(PrimarySkill.ShadowSlide))
            {
                if (m_shadowSlide?.HaveEnoughSourceForExecution() ?? false)
                {
                    m_activeSlide = m_shadowSlide;
                    m_shadowSlide.ConsumeSource();

                    m_activeSlide?.ResetDurationTimer();
                    m_activeSlide?.Execute();
                }
                else
                {
                    //m_activeSlide = m_slide;
                }
            }
            else
            {
                //m_activeSlide = m_slide;
            }
        }

        private void MoveCharacter(bool isGrabbing)
        {
            if (!IsFacingInput())
            {
                m_basicSlashes.Cancel();
                m_slashCombo.Cancel();
                m_whip.Cancel();
                m_whipCombo.Cancel();
                m_whipCombo.Reset();
            }

            if (isGrabbing == false)
            {
                if (m_input.horizontalInput == 0)
                {
                    m_idle?.Execute(m_state.allowExtendedIdle);
                }
                else
                {
                    m_idle?.Cancel();
                }

                m_movement?.Move(m_input.horizontalInput, true);
            }
            else
            {
                m_movement?.Move(m_input.horizontalInput, false);
            }
        }

        private bool CanMove()
        {
            return m_whipCombo.CanMove()
                    && m_slashCombo.CanMove()
                    && m_whip.CanMove()
                    //&& m_airLunge.CanMove()
                    && m_fireFist.CanMove()
                    && m_reaperHarvest.CanMove()
                    && m_finalSlash.CanMove()
                    && m_airSlashCombo.CanMove()
                    && m_sovereignImpale.CanMove()
                    && m_hellTrident.CanMove()
                    && m_foolsVerdict.CanMove()
                    && m_ninthCircleSanction.CanMove()
                    && m_doomsdayKong.CanMove()
                    && m_fencerFlash.CanMove()
                    && m_barrier.CanMove()
                    && m_eelecktrick.CanMove();
        }

        private bool IsFacingInput()
        {
            return m_input.horizontalInput > 0 && m_character.facing == HorizontalDirection.Right
                || m_input.horizontalInput < 0 && m_character.facing == HorizontalDirection.Left
                || m_input.horizontalInput == 0;
        }

        private void PrepareForGroundAttack()
        {
            m_combatReadiness?.Execution();
            m_idle?.Cancel();
            m_movement?.Cancel();
            m_objectManipulation?.Cancel();
            m_attackRegistrator?.ResetHitCache();
            m_projectileThrow.EndAim(); //fix for projectile throw delay WIP
            m_projectileThrow?.Cancel(); //fix for projectile throw delay WIP
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
