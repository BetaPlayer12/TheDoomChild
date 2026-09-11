using DChild.Gameplay.Combat;
using Holysoft.Event;
using UnityEngine;
using DChild.Gameplay.Characters.Players.BattleAbilityModule;
using DChild.Inputs;
using System;
using UnityEngine.UIElements;
using System.ComponentModel;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Systems;
using DChild.Menu;
using DChild.Gameplay.Characters.Players.State;
using System.Runtime.Remoting.Messaging;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using DChild.Gameplay.Narrative;
using System.Collections;
using DChild.UI;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class UnderworldPlayerController : MonoBehaviour, IMainController
    {
        [SerializeField]
        private InputReader m_inputReader;
        public InputReader inputReader => m_inputReader;
        [SerializeField]
        private PlayerModuleActivator m_skills;
        [SerializeField]
        private Rigidbody2D m_rigidbody;
        [SerializeField]
        private CombatArts m_abilities;
        [SerializeField]
        private Character m_character;
        [SerializeField]
        private CharacterState m_state;

        private IDash m_activeDash;
        private ISlide m_activeSlide;

        [SerializeField]
        private PlayerInput m_playerInput;
        #region Modules
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
        private ChargeAttackHandle m_chargeAttackHandle;
        private ShadowbladeFX m_shadowBladeFX;

        private ReaperHarvest m_reaperHarvest;
        private KrakenRage m_krakenRage;
        private SovereignImpale m_sovereignImpale;
        private HellTrident m_hellTrident;
        private FoolsVerdict m_foolsVerdict;
        private SoulFireBlast m_soulFireBlast;
        private EdgedFury m_edgedFury;
        private BackDiver m_backDiver;
        private Barrier m_barrier;
        private DiagonalSwordDash m_diagonalSwordDash;
        private ChampionsUprising m_championsUprising;
        private LightningSpear m_lightningSpear;
        private IcarusWings m_icarusWings;
        private TeleportingSkull m_teleportingSkull;
        private AirSlashRange m_airSlashRange;
        #endregion

        private IInterruptableCombatArtModule m_currentCombatArt;

        [SerializeField]
        private QuickItemHandle m_quickItemHandle;

        #region Input Variables
        [SerializeField, ReadOnly(true)]
        private Vector2 m_vector2Input;
        [SerializeField, ReadOnly(true)]
        private Vector2 m_mouseDelta;
        private bool m_isGrabbing;
        #endregion

        private bool m_storeHasBeenPickedUp = true;
        private bool m_playerWokeUp = true;

        public event EventAction<EventActionArgs> ControllerDisabled;
        public event EventAction<EventActionArgs> ControllerEnabled;
        public static event Action<string> ActiveControllerChanged;

        #region Usual Unity Stuff
        private void Awake()
        {
            m_chargeAttackHandle = new ChargeAttackHandle();

            m_tracker = m_character.GetComponentInChildren<PlayerStatisticTracker>();
            m_groundedness = m_character.GetComponentInChildren<GroundednessHandle>();
            m_physicsMat = m_character.GetComponentInChildren<PlayerPhysicsMatHandle>();
            m_idle = m_character.GetComponentInChildren<IdleHandle>();
            m_combatReadiness = m_character.GetComponentInChildren<CombatReadiness>();
            m_flinch = m_character.GetComponentInChildren<PlayerFlinch>();
            m_death = m_character.GetComponentInChildren<PlayerDeath>();
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
            m_block = m_character.GetComponentInChildren<PlayerBlock>();

            m_shadowBladeFX = m_character.GetComponentInChildren<ShadowbladeFX>();

            m_reaperHarvest = m_character.GetComponentInChildren<ReaperHarvest>();
            m_krakenRage = m_character.GetComponentInChildren<KrakenRage>();
            m_sovereignImpale = m_character.GetComponentInChildren<SovereignImpale>();
            m_hellTrident = m_character.GetComponentInChildren<HellTrident>();
            m_foolsVerdict = m_character.GetComponentInChildren<FoolsVerdict>();
            m_soulFireBlast = m_character.GetComponentInChildren<SoulFireBlast>();
            m_edgedFury = m_character.GetComponentInChildren<EdgedFury>();
            m_backDiver = m_character.GetComponentInChildren<BackDiver>();
            m_barrier = m_character.GetComponentInChildren<Barrier>();
            m_diagonalSwordDash = m_character.GetComponentInChildren<DiagonalSwordDash>();
            m_championsUprising = m_character.GetComponentInChildren<ChampionsUprising>();
            m_lightningSpear = m_character.GetComponentInChildren<LightningSpear>();
            m_icarusWings = m_character.GetComponentInChildren<IcarusWings>();
            m_teleportingSkull = m_character.GetComponentInChildren<TeleportingSkull>();
            m_airSlashRange = m_character.GetComponentInChildren<AirSlashRange>();

            //Intro Controller
            m_introController = GetComponent<PlayerIntroControlsController>();

            //Abilities
            m_abilities = GetComponentInParent<Player>().GetComponentInChildren<CombatArts>();
        }

        private void OnEnable()
        {
            m_groundedness.StateChange += OnGroundednessStateChange;
            m_flinch.OnExecute += OnFlinch;
            m_death.OnExecute += OnDeath;
            m_projectileThrow.ExecutionRequested += OnProjectileThrowRequest;
            m_projectileThrow.ProjectileThrown += ResetProjectile;
            m_teleportingSkull.Teleported += HasTeleported;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            NewGameIntroEvent.NewGameIntroFinished += OnPickedUpBook;
            NewGameIntroEvent.NewGameIntroStarted += OnNewGameIntroStarted;
            NewGameIntroEvent.NewGamePlayerWokeUp += OnPlayerWokeUp;
            BaseGameplaySystem.gamplayUIHandle.gameplayUIStateObserver.GameplayUIStateChanged += OnUIStateChanged;

            //action handles
            m_inputReader.Vector2InputPerformedEvent += OnVector2PerformedInput;
            m_inputReader.Vector2CancelledInputEvent += OnVector2CancelledInput;
            m_inputReader.JumpPerformedEvent += OnJumpPerformedInput;
            m_inputReader.JumpCancelledEvent += OnJumpCancelledInput;
            m_inputReader.JumpStartedEvent += OnJumpStartedInput;
            m_inputReader.PauseStartedEvent += OnPauseInput;
            m_inputReader.StoreStartedEvent += OnStoreInput;
            m_inputReader.DashStartedEvent += OnDashStartedInput;
            m_inputReader.LevitateStartedEvent += OnLevitateStartedInput;
            m_inputReader.LevitatePerformedEvent += OnLevitateInput;
            m_inputReader.LevitateCancelledEvent += OnLevitateCancelledInput;
            m_inputReader.InteractStartedEvent += OnInteractInput;
            m_inputReader.ShadowMorphStartedEvent += OnShadowMorphStartedInput;
            m_inputReader.SlashStartedEvent += OnSlashStartedInput;
            m_inputReader.SlashTappedEvent += OnSlashTappedInput;
            m_inputReader.SlashPressedEvent += OnSlashPressedInput;
            m_inputReader.SlashCancelledEvent += OnSlashCancelledInput;
            m_inputReader.SlashHeldEvent += OnSlashHeldInput;
            m_inputReader.SwordThrustPerformedEvent += OnSwordThrustPerformedInput;
            m_inputReader.SwordThrustCancelledEvent += OnSwordThrustCancelledInput;
            m_inputReader.WhipPerformedEvent += OnWhipPerformedInput;
            m_inputReader.WhipCancelledEvent += OnWhipCancelledInput;
            m_inputReader.CycleQuickItemsStartedEvent += OnCycleQuickItemsStartedInput;
            m_inputReader.UseQuickItemTappedEvent += OnUseQuickItemsTappedInput;
            m_inputReader.UseQuickItemHeldEvent += OnUseQuickItemsHeldInput;
            m_inputReader.UseQuickItemCancelledEvent += OnUseQuickItemsCancelledInput;
            m_inputReader.ProjectileThrowStartedEvent += OnProjectileThrowStartedInput;
            m_inputReader.ProjectileThrowCancelledEvent += OnProjectileThrowCancelledInput;
            m_inputReader.ProjectileThrowTappedEvent += OnProjectileThrowTappedInput;
            m_inputReader.ProjectileThrowHeldEvent += OnProjectileThrowHeldInput;
            m_inputReader.MouseDeltaPerformedEvent += OnMouseDeltaPerformedInput;
            m_inputReader.GrabStartedEvent += OnGrabStartedInput;
            m_inputReader.GrabCancelledEvent += OnGrabCancelledInput;
            m_inputReader.BarrierStartedEvent += OnBarrierStartedInput;
            m_inputReader.BarrierPerformedEvent += OnBarrierPerformedInput;
            m_inputReader.BarrierCancelledEvent += OnBarrierCancelledInput;
            m_inputReader.EarthShakerStartedEvent += OnEarthShakerStartedInput;
            m_inputReader.EarthShakerPerformedEvent += OnEarthShakerPerformedInput;
            m_inputReader.EarthShakerCancelledEvent += OnEarthShakerCancelledInput;
            m_inputReader.AirSlashStartedEvent += OnAirSlashStartedInput;
            m_inputReader.AirSlashCancelledEvent += OnAirSlashCancelledInput;
            m_inputReader.AirSlashPerformedEvent += OnAirSlashPerformedInput;
            m_inputReader.HellTridentStartedEvent += OnHellTridentStartedInput;
            m_inputReader.HellTridentCancelledEvent += OnHellTridentCancelledInput;
            m_inputReader.HellTridentPerformedEvent += OnHellTridentPerformedInput;
            m_inputReader.SoulFireBlastStartedEvent += OnSoulFireBlastStartedInput;
            m_inputReader.SoulFireBlastCancelledEvent += OnSoulFireBlastCancelledInput;
            m_inputReader.SoulFireBlastPerformedEvent += OnSoulFireBlastPerformedInput;
            m_inputReader.BackDiverStartedEvent += OnBackDiverStartedInput;
            m_inputReader.BackDiverCancelledEvent += OnBackDiverCancelledInput;
            m_inputReader.BackDiverPerformedEvent += OnBackDiverPerformedInput;
            m_inputReader.SovereignImpaleStartedEvent += OnSovereignImpaleStartedInput;
            m_inputReader.SovereignImpaleCancelledEvent += OnSovereignImpaleCancelledInput;
            m_inputReader.SovereignImpalePerformedEvent += OnSovereignImpalePerformedInput;
            m_inputReader.DiagonalSwordDashStartedEvent += OnDiagonalSwordDashStartedInput;
            m_inputReader.DiagonalSwordDashCancelledEvent += OnDiagonalSwordDashCancelledInput;
            m_inputReader.DiagonalSwordDashPerformedEvent += OnDiagonalSwordDashPerformedInput;
            m_inputReader.EdgedFuryStartedEvent += OnEdgedFuryStartedInput;
            m_inputReader.EdgedFuryCancelledEvent += OnEdgedFuryCancelledInput;
            m_inputReader.EdgedFuryPerformedEvent += OnEdgedFuryPerformedInput;
            m_inputReader.ReapersHarvestStartedEvent += OnReapersHarvestStartedInput;
            m_inputReader.ReapersHarvestCancelledEvent += OnReapersHarvestCancelledInput;
            m_inputReader.ReapersHarvestPerformedEvent += OnReapersHarvestPerformedInput;
            m_inputReader.IcarusWingsStartedEvent += OnIcarusWingsStartedInput;
            m_inputReader.IcarusWingsCancelledEvent += OnIcarusWingsCancelledInput;
            m_inputReader.IcarusWingsPerformedEvent += OnIcarusWingsPerformedInput;
            m_inputReader.TeleportingSkullStartedEvent += OnTeleportingSkullStartedInput;
            m_inputReader.TeleportingSkullPerformedEvent += OnTeleportingSkullPerformedInput;
            m_inputReader.TeleportingSkullCancelledEvent += OnTeleportingSkullCancelledInput;
            m_inputReader.TeleportToOverworld += OnTeleportToOverworld;
            m_inputReader.TeleportToOverworldStarted += OnTeleportToOverworldStarted;
            m_inputReader.TeleportToMordenThroneRoom += OnTeleportToMordenThroneRoom;
            m_inputReader.TeleportToMordenThroneRoomStarted += OnTeleportToMordenThroneRoomStarted;
        }

        private void OnDisable()
        {
            m_groundedness.StateChange -= OnGroundednessStateChange;
            m_flinch.OnExecute -= OnFlinch;
            m_death.OnExecute -= OnDeath;
            m_projectileThrow.ExecutionRequested -= OnProjectileThrowRequest;
            m_projectileThrow.ProjectileThrown -= ResetProjectile;
            m_teleportingSkull.Teleported -= HasTeleported;
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            NewGameIntroEvent.NewGameIntroFinished -= OnPickedUpBook;
            NewGameIntroEvent.NewGameIntroStarted -= OnNewGameIntroStarted;
            NewGameIntroEvent.NewGamePlayerWokeUp -= OnPlayerWokeUp;
            BaseGameplaySystem.gamplayUIHandle.gameplayUIStateObserver.GameplayUIStateChanged -= OnUIStateChanged;

            //action handles
            m_inputReader.Vector2InputPerformedEvent -= OnVector2PerformedInput;
            m_inputReader.Vector2CancelledInputEvent -= OnVector2CancelledInput;
            m_inputReader.JumpPerformedEvent -= OnJumpPerformedInput;
            m_inputReader.JumpCancelledEvent -= OnJumpCancelledInput;
            m_inputReader.JumpStartedEvent -= OnJumpStartedInput;
            m_inputReader.PauseStartedEvent -= OnPauseInput;
            m_inputReader.StoreStartedEvent -= OnStoreInput;
            m_inputReader.DashStartedEvent -= OnDashStartedInput;
            m_inputReader.LevitateStartedEvent -= OnLevitateStartedInput;
            m_inputReader.LevitatePerformedEvent -= OnLevitateInput;
            m_inputReader.LevitateCancelledEvent -= OnLevitateCancelledInput;
            m_inputReader.InteractStartedEvent -= OnInteractInput;
            m_inputReader.ShadowMorphStartedEvent -= OnShadowMorphStartedInput;
            m_inputReader.SlashStartedEvent -= OnSlashStartedInput;
            m_inputReader.SlashTappedEvent -= OnSlashTappedInput;
            m_inputReader.SlashPressedEvent -= OnSlashPressedInput;
            m_inputReader.SlashCancelledEvent -= OnSlashCancelledInput;
            m_inputReader.SlashHeldEvent -= OnSlashHeldInput;
            m_inputReader.SwordThrustPerformedEvent -= OnSwordThrustPerformedInput;
            m_inputReader.SwordThrustCancelledEvent -= OnSwordThrustCancelledInput;
            m_inputReader.WhipPerformedEvent -= OnWhipPerformedInput;
            m_inputReader.WhipCancelledEvent -= OnWhipCancelledInput;
            m_inputReader.CycleQuickItemsStartedEvent -= OnCycleQuickItemsStartedInput;
            m_inputReader.UseQuickItemTappedEvent -= OnUseQuickItemsTappedInput;
            m_inputReader.UseQuickItemHeldEvent -= OnUseQuickItemsHeldInput;
            m_inputReader.UseQuickItemCancelledEvent -= OnUseQuickItemsCancelledInput;
            m_inputReader.ProjectileThrowStartedEvent -= OnProjectileThrowStartedInput;
            m_inputReader.ProjectileThrowCancelledEvent -= OnProjectileThrowCancelledInput;
            m_inputReader.ProjectileThrowTappedEvent -= OnProjectileThrowTappedInput;
            m_inputReader.ProjectileThrowHeldEvent -= OnProjectileThrowHeldInput;
            m_inputReader.MouseDeltaPerformedEvent -= OnMouseDeltaPerformedInput;
            m_inputReader.GrabStartedEvent -= OnGrabStartedInput;
            m_inputReader.GrabCancelledEvent -= OnGrabCancelledInput;
            m_inputReader.BarrierStartedEvent -= OnBarrierStartedInput;
            m_inputReader.BarrierPerformedEvent -= OnBarrierPerformedInput;
            m_inputReader.BarrierCancelledEvent -= OnBarrierCancelledInput;
            m_inputReader.EarthShakerStartedEvent -= OnEarthShakerStartedInput;
            m_inputReader.EarthShakerPerformedEvent -= OnEarthShakerPerformedInput;
            m_inputReader.EarthShakerCancelledEvent -= OnEarthShakerCancelledInput;
            m_inputReader.AirSlashStartedEvent -= OnAirSlashStartedInput;
            m_inputReader.AirSlashCancelledEvent -= OnAirSlashCancelledInput;
            m_inputReader.AirSlashPerformedEvent -= OnAirSlashPerformedInput;
            m_inputReader.HellTridentStartedEvent -= OnHellTridentStartedInput;
            m_inputReader.HellTridentCancelledEvent -= OnHellTridentCancelledInput;
            m_inputReader.HellTridentPerformedEvent -= OnHellTridentPerformedInput;
            m_inputReader.SoulFireBlastStartedEvent -= OnSoulFireBlastStartedInput;
            m_inputReader.SoulFireBlastCancelledEvent -= OnSoulFireBlastCancelledInput;
            m_inputReader.SoulFireBlastPerformedEvent -= OnSoulFireBlastPerformedInput;
            m_inputReader.BackDiverStartedEvent -= OnBackDiverStartedInput;
            m_inputReader.BackDiverCancelledEvent -= OnBackDiverCancelledInput;
            m_inputReader.BackDiverPerformedEvent -= OnBackDiverPerformedInput;
            m_inputReader.SovereignImpaleStartedEvent -= OnSovereignImpaleStartedInput;
            m_inputReader.SovereignImpaleCancelledEvent -= OnSovereignImpaleCancelledInput;
            m_inputReader.SovereignImpalePerformedEvent -= OnSovereignImpalePerformedInput;
            m_inputReader.DiagonalSwordDashStartedEvent -= OnDiagonalSwordDashStartedInput;
            m_inputReader.DiagonalSwordDashPerformedEvent -= OnDiagonalSwordDashPerformedInput;
            m_inputReader.DiagonalSwordDashCancelledEvent -= OnDiagonalSwordDashCancelledInput;
            m_inputReader.EdgedFuryStartedEvent -= OnEdgedFuryStartedInput;
            m_inputReader.EdgedFuryCancelledEvent -= OnEdgedFuryCancelledInput;
            m_inputReader.EdgedFuryPerformedEvent -= OnEdgedFuryPerformedInput;
            m_inputReader.ReapersHarvestStartedEvent -= OnReapersHarvestStartedInput;
            m_inputReader.ReapersHarvestCancelledEvent -= OnReapersHarvestCancelledInput;
            m_inputReader.ReapersHarvestPerformedEvent -= OnReapersHarvestPerformedInput;
            m_inputReader.IcarusWingsStartedEvent -= OnIcarusWingsStartedInput;
            m_inputReader.IcarusWingsCancelledEvent -= OnIcarusWingsCancelledInput;
            m_inputReader.IcarusWingsPerformedEvent -= OnIcarusWingsPerformedInput;
            m_inputReader.TeleportingSkullStartedEvent -= OnTeleportingSkullStartedInput;
            m_inputReader.TeleportingSkullPerformedEvent -= OnTeleportingSkullPerformedInput;
            m_inputReader.TeleportingSkullCancelledEvent -= OnTeleportingSkullCancelledInput;
            m_inputReader.TeleportToOverworld -= OnTeleportToOverworld;
            m_inputReader.TeleportToOverworldStarted -= OnTeleportToOverworldStarted;
            m_inputReader.TeleportToMordenThroneRoom -= OnTeleportToMordenThroneRoom;
            m_inputReader.TeleportToMordenThroneRoomStarted -= OnTeleportToMordenThroneRoomStarted;
        }

        private void Start()
        {
            m_inputReader.SetInputModeToUnderworldGameplay();
        }

        private void FixedUpdate()
        {
            if (m_state.isDead)
            {
                Disable();
            }


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

                HandleCrouchMovement();

                if (CanMove())
                {
                    if (m_state.isGrabbing)
                    {
                        GrabMoveAction();
                    }
                    else
                    {
                        MoveAction();
                    }
                }
            }
            else
            {
                if (m_earthShaker.CanEarthShaker())
                {
                    if (m_state.isShadowBlade && !m_shadowBladeFX.canShadowblade)
                    {
                        m_shadowBladeFX.EnableShadowblade();
                    }
                    else if (!m_state.isShadowBlade && m_shadowBladeFX.canShadowblade)
                    {
                        m_shadowBladeFX.DisableShadowblade();
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


                if (CanMove())
                {
                    if (m_state.isGrabbing == false)
                    {
                        MoveAction();
                    }
                }

                if (m_state.isStickingToWall || m_state.isGrounded)
                {
                    m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Ground);
                }
                else
                {
                    m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Midair);
                }

                LevitateAction();
            }
        }

        private void Update()
        {
            if (m_state.isDead)
            {
                //m_groundedness.Evaluate();
                return;
            }
            else
            {
                //m_groundedness.Evaluate();
            }

            if (m_introController.IsUsingIntroControls())
            {
                m_introController.HandleIntroControls();
                return;
            }

            if (m_shadowGaugeRegen?.CanRegen() ?? false)
            {
                m_shadowGaugeRegen.Execute();
            }

            m_platformDrop.HandleDroppablePlatformCollision();

            if (m_state.isInShadowMode)
            {
                if (m_shadowMorph.HaveEnoughSourceToMaintainShadowForm())
                {
                    m_shadowMorph.ConsumeSource();
                }
                else
                {
                    m_shadowMorph?.Cancel();
                    m_shadowGaugeRegen?.Enable(true);
                }
            }

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

            if (m_championsUprising.CanChampionsUprising() == false)
            {
                m_championsUprising.HandleAttackTimer();
            }

            if (m_lightningSpear.CanReset() == true)
            {
                m_lightningSpear.HandleResetTimer();
            }

            if (m_lightningSpear.CanMove() == false)
            {
                m_lightningSpear.HandleMovementTimer();
            }
            if (m_diagonalSwordDash.CanReset() == true)
            {
                m_diagonalSwordDash.HandleResetTimer();
            }

            if (m_diagonalSwordDash.CanMove() == false)
            {
                m_diagonalSwordDash.HandleMovementTimer();
            }

            if (m_reaperHarvest.CanReaperHarvest() == false)
            {
                m_reaperHarvest.HandleAttackTimer();
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

            if (m_barrier.IsDoingBarrier())
            {
                if (m_barrier.HaveEnoughSourceForExecution())
                {
                    m_barrier?.ConsumeSource();
                }
                else
                {
                    m_barrier?.EndExecution();
                    m_barrier?.EnableShield(false);
                    m_shadowGaugeRegen?.Enable(true);
                }
            }
            #endregion

            if (m_state.canAttack == true)
            {
                m_slashCombo.HandleComboResetTimer();
                m_whipCombo.HandleComboResetTimer();
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
                }
            }

            if (m_state.waitForBehaviour)
                return;

            if (m_state.isGrounded)
            {
                HandleGroundBehaviour();
                m_basicSlashes?.ResetAerialGravityControl();
                m_basicSlashes?.ResetAirAttacks();
                m_whip?.ResetAerialGravityControl();
                m_whip?.ResetAirAttacks();
                m_devilWings?.EnableLevitate();

                #region Combat Arts Cooldowns
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

            if (m_state.isStickingToWall)
            {
                WallStickMovementAction();
                return;
            }

            m_objectManipulation?.LookForMoveableObject();

            if (m_state.isHighJumping)
            {
                if (m_rigidbody.velocity.y <= (m_groundJump?.highJumpCutoffThreshold ?? 0f))
                {
                    m_groundJump?.EndExecution();
                }
                m_groundJump?.HandleCutoffTimer();
            }

            if (m_state.isAimingProjectile)
            {
                ProjectileThrowAiming();
            }

            LedgeGrabMovementAction();
            SwordThrustAction();
            HandleWallMovement();
        }
        #endregion

        #region Input Handles
        private void OnVector2PerformedInput(Vector2 vector)
        {
            if (m_playerWokeUp == false)
                return;
            if (m_state.isExecutingCombatArt)
                return;

            //if(m_state.isGrounded == false && vector.y < 0)
            //    m_movement.TriggerFastFall();

            m_vector2Input = vector;
        }

        private void OnVector2CancelledInput(Vector2 vector)
        {
            m_vector2Input = Vector2.zero;

            if (m_state.isChargingAttack || m_state.isDoingSwordThrust)
                return;

            if (m_state.isCrouched)
            {
                if (m_crouch?.IsThereNoCeiling() ?? true)
                {
                    m_crouch?.Cancel();
                    m_movement?.SwitchConfigTo(Movement.Type.Jog);
                }
            }

            if (m_state.isStickingToWall)
            {
                if (m_wallSlide.IsThereAWall())
                {
                    m_wallSlide?.Execute();
                }
            }

            if (m_state.isStickingToWall == false)
            {
                m_movement.Cancel();
                m_state.isAttacking = false;
                m_state.waitForBehaviour = false;
                m_idle?.Execute(m_state.allowExtendedIdle);
            }
        }

        private void OnJumpStartedInput()
        {

        }

        private void OnJumpPerformedInput()
        {
            if (m_playerWokeUp == false)
                return;
            if (m_state.isLedgeGrabbing || m_state.waitForBehaviour || m_state.isInShadowMode
                || m_state.isChargingAttack || m_state.isAimingProjectile || m_state.isDoingSwordThrust || m_state.isExecutingCombatArt)
                return;
            if (m_state.isAimingProjectile)
                return;
            if (m_state.isDoingEarthShaker)
                return;
            if (m_crouch.IsThereNoCeiling() == false)
                return;
            if (m_state.isSliding && m_slide.IsThereACeiling())
                return;

            //moved wall jump out of groundedness check to prevent triggering extra jump so you can jump then double jump 
            if (m_state.isStickingToWall)
            {
                if (m_skills.IsModuleActive(PrimarySkill.WallMovement))
                {
                    if (m_state.canWallCrawl)
                    {
                        m_wallMovement?.Cancel();
                    }
                    m_wallStick?.Cancel();
                    m_wallMovement?.Cancel();
                    FlipCharacter();
                    m_wallJump?.JumpAway();
                    m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Midair);
                    return;
                }
            }

            if (m_state.isGrounded)
            {
                if (m_platformDrop?.IsThereADroppablePlatform() == true && m_vector2Input.y < 0)
                {
                    m_activeDash?.Cancel();
                    m_activeSlide?.Cancel();
                    m_platformDrop.Execute();

                    return;
                }

                m_projectileThrow?.Cancel();

                if (m_skills.IsModuleActive(PlayerBehaviour.Jump))
                {
                    if (m_state.isHighJumping == false)
                    {
                        if (m_state.isDashing)
                        {
                            m_activeDash?.Cancel();
                        }

                        m_activeSlide?.Cancel();
                        m_whipCombo?.Cancel();
                        m_slashCombo?.Cancel();
                        m_groundJump?.Execute();
                        m_physicsMat.SetPhysicsTo(PlayerPhysicsMatHandle.Type.Midair);
                        m_movement?.SwitchConfigTo(Movement.Type.MidAir);
                    }
                }
            }
            else
            {
                if (m_skills.IsModuleActive(PrimarySkill.DoubleJump))
                {
                    if (m_extraJump?.HasExtras() ?? false)
                    {
                        if (m_state.isLevitating)
                        {
                            m_devilWings?.Cancel();
                        }

                        m_basicSlashes?.Cancel();
                        m_whip?.Cancel();
                        m_extraJump?.Execute();
                    }
                }
            }
        }

        private void OnJumpCancelledInput()
        {
            if (m_state.isHighJumping)
            {
                m_groundJump?.CutOffJump();
            }
        }

        private void OnLevitateStartedInput()
        {
            if (m_skills.IsModuleActive(PrimarySkill.DevilWings) == false)
                return;
            if (m_state.isGrounded)
                return;
            if (m_state.isInShadowMode)
                return;
            if (m_state.isDashing)
                return;
            if (m_state.isAttacking)
                return;
            if (m_state.isExecutingCombatArt)
                return;
            if (m_state.isDoingEarthShaker)
                return;
            if ((m_devilWings?.HaveEnoughSourceForExecution() ?? false) == false)
                return;

            if (m_state.isHighJumping)
            {
                m_groundJump?.CutOffJump();
            }
            m_devilWings?.Execute();
        }

        private void OnLevitateInput()
        {

        }


        private void OnLevitateCancelledInput()
        {
            if (m_state.isLevitating == false)
                return;
            m_devilWings.Cancel();
        }

        private void OnDashStartedInput()
        {
            if (m_state.isAttacking || m_state.isLedgeGrabbing)
                return;
            if (m_state.isChargingAttack)
                return;
            if (m_state.isInShadowMode)
                return;
            if (m_state.isDoingSwordThrust)
                return;
            if (m_state.isDoingEarthShaker)
                return;
            if (m_state.waitForBehaviour)
                return;
            if (m_state.isSliding)
                return;
            if (m_state.isAimingProjectile)
                return;
            if (m_state.isDashing)
                return;

            if (m_state.isExecutingCombatArt)
            {
                m_currentCombatArt?.Cancel();
            }

            if (m_state.isGrounded)
            {
                if ((m_skills.IsModuleActive(PrimarySkill.Slide) || m_skills.IsModuleActive(PrimarySkill.ShadowSlide)) && m_state.canSlide)
                {
                    if (m_vector2Input.y < 0 && m_state.canSlide)
                    {
                        m_idle?.Cancel();
                        m_movement?.Cancel();
                        m_whipCombo?.Cancel();
                        m_whipCombo?.Reset();
                        m_earthShaker?.Cancel();
                        m_objectManipulation?.Cancel();

                        ExecuteSlide();
                        return;
                    }
                }

                if ((m_skills.IsModuleActive(PrimarySkill.Dash) || m_skills.IsModuleActive(PrimarySkill.ShadowDash)) && m_state.canDash)
                {
                    m_idle?.Cancel();
                    m_movement?.Cancel();
                    m_whipCombo?.Cancel();
                    m_whipCombo?.Reset();
                    m_earthShaker?.Cancel();
                    m_objectManipulation?.Cancel();

                    ExecuteDash();
                    return;
                }
            }
            else
            {
                if ((m_skills.IsModuleActive(PrimarySkill.Dash) || m_skills.IsModuleActive(PrimarySkill.ShadowDash)) && m_state.canDash)
                {
                    m_idle?.Cancel();
                    m_movement?.Cancel();
                    m_whipCombo?.Cancel();
                    m_whipCombo?.Reset();
                    m_earthShaker?.Cancel();
                    m_objectManipulation?.Cancel();

                    if (m_state.isStickingToWall)
                    {
                        m_wallStick?.Cancel();
                        m_wallMovement?.Cancel();
                        FlipCharacter();
                    }

                    if (m_state.isLevitating)
                    {
                        m_devilWings?.Cancel();
                    }

                    ExecuteDash();
                    return;
                }
            }
        }

        private void OnInteractInput()
        {
            if (m_state.isGrounded)
            {
                m_objectInteraction?.Interact();
            }
        }

        private void OnShadowMorphStartedInput()
        {
            if (m_skills.IsModuleActive(PrimarySkill.ShadowMorph) == false)
                return;
            if (m_state.isGrounded == false)
                return;
            if (m_state.isChargingAttack)
                return;
            if (m_state.isAimingProjectile)
                return;
            if (m_state.isDashing || m_state.isSliding || m_state.isAttacking || m_state.isLedgeGrabbing || m_state.isExecutingCombatArt || m_state.isHighJumping)
                return;
            if ((m_shadowMorph?.HaveEnoughSourceForExecution() ?? false) == false)
                return;

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
        }

        private void OnUseQuickItemsTappedInput()
        {
            if (m_quickItemHandle.IsCoolDownOver() == false)
                return;

            if (m_state.isAimingProjectile == true)
                return;

            m_quickItemHandle.UseCurrentItem();
            if (m_quickItemHandle.IsCurrentItemThrowable())
            {
                StartCoroutine(StraightThrowRoutine());
            }
        }

        private void OnUseQuickItemsHeldInput()
        {
            if (m_quickItemHandle.IsCoolDownOver() == false)
                return;

            if (m_quickItemHandle.IsCurrentItemThrowable() == false)
                return;

            m_quickItemHandle.UseCurrentItem();
            if (m_quickItemHandle.IsCurrentItemThrowable())
            {
                ProjectileThrowStart();
            }
        }

        private void OnUseQuickItemsCancelledInput()
        {
            if (m_quickItemHandle.IsCurrentItemThrowable())
            {
                ProjectileThrowCancel();
            }
        }

        private void OnCycleQuickItemsStartedInput(float obj)
        {
            if (m_state.isAimingProjectile == true)
                return;

            if (obj == -1)
            {
                m_quickItemHandle.Previous();
            }
            else
            {
                m_quickItemHandle.Next();
            }
        }

        private void OnStoreInput()
        {
            if (m_storeHasBeenPickedUp == false)
                return;
            if (BaseGameplaySystem.gamplayUIHandle.GetCurrentUIState() != GameplayUIState.GameplayHUD)
                return;

            GameplaySystem.gamplayUIHandle.OpenStoreAtPage(StorePage.Map);
        }

        private void OnPauseInput()
        {
            if (BaseGameplaySystem.gamplayUIHandle.GetCurrentUIState() != GameplayUIState.GameplayHUD)
                return;

            GameplaySystem.gamplayUIHandle.OpenPauseMenu();
        }

        private void OnTeleportToOverworldStarted(InputAction.CallbackContext context, bool isCanceled)
        {
            if (BaseGameplaySystem.gamplayUIHandle.GetCurrentUIState() != GameplayUIState.GameplayHUD)
                return;

            GameplaySystem.gamplayUIHandle.ShowHoldToTeleportSequence(context, isCanceled);
        }

        private void OnTeleportToOverworld()
        {
            //Note: May need to change with gameplayUIHandle check to do it through confirmation window
            UnderworldGameplaySystem.overworldTeleportHandle.TeleportToOverworld();
        }

        private void OnTeleportToMordenThroneRoomStarted(InputAction.CallbackContext context, bool isCanceled)
        {
            //GameplaySystem.gamplayUIHandle.ShowHoldToTeleportSequence(context, isCanceled);
        }

        private void OnTeleportToMordenThroneRoom()
        {
            //Note: May need to change with gameplayUIHandle check to do it through confirmation window
            //UnderworldGameplaySystem.overworldTeleportHandle.TeleportToThroneRoom();
        }

        private void OnSlashStartedInput()
        {
            if (m_playerWokeUp == false)
                return;
            if (m_state.isSliding || m_state.canAttack == false || m_state.isStickingToWall ||
                m_state.isAttacking || m_state.waitForBehaviour || m_state.isExecutingCombatArt)
                return;
            if (m_state.isAimingProjectile)
                return;

            m_idle?.Cancel();

            if (m_state.isGrounded)
            {
                if (m_state.isDashing)
                {
                    m_activeDash.Cancel();
                }

                if (m_state.isInShadowMode)
                {
                    if (m_shadowMorph.IsAttackAllowed() == false)
                    {
                        return;
                    }
                }

                m_diagonalSwordDash.Cancel();
                PrepareForGroundAttack();
                m_whip.Cancel();
                m_whipCombo.Cancel();
                m_whipCombo.Reset();

                if (m_vector2Input.y > 0)
                {
                    m_basicSlashes.Execute(BasicSlashes.Type.Ground_Overhead);
                    return;
                }

                if (m_state.isCrouched && m_vector2Input.y < 0)
                {
                    m_basicSlashes.Execute(BasicSlashes.Type.Crouch);
                    return;
                }

                if (m_vector2Input.y == 0)
                {
                    m_movement.Cancel();
                    m_slashCombo.Execute();
                    return;
                }
            }
            else
            {
                if (m_state.isDashing)
                {
                    return;
                }

                if (m_basicSlashes.CanAirAttack())
                {
                    m_diagonalSwordDash.Cancel();
                    PrepareForMidairAttack();
                    m_devilWings?.EnableLevitate();
                    m_extraJump?.Cancel();

                    if (m_vector2Input.y > 0)
                    {
                        m_basicSlashes.Execute(BasicSlashes.Type.MidAir_Overhead);
                        return;
                    }

                    if (m_vector2Input.y == 0)
                    {
                        m_basicSlashes.Execute(BasicSlashes.Type.MidAir_Forward);
                        return;
                    }
                }

                /*if (m_vector2Input.y < 0)
                {
                    if (m_skills.IsModuleActive(PrimarySkill.EarthShaker) && m_earthShaker.CanEarthShaker() 
                        && m_state.isExecutingCombatArt == false)
                    {
                        m_earthShaker?.Reset();
                        m_diagonalSwordDash?.Cancel();
                        m_earthShaker?.StartExecution();
                        return;
                    }
                }*/
            }
        }

        private void OnSlashPressedInput()
        {

        }

        private void OnSlashTappedInput()
        {

        }
        private bool m_slashHeld;
        private bool m_ignoreSlashHeldUntilReleased;
        private void OnSlashHeldInput()
        {
            if (m_ignoreSlashHeldUntilReleased)
                return;
            if (m_state.isChargingAttack)
                return;
            if (m_state.isCrouched)
                return;
            if (m_state.isSliding)
                return;
            if (m_state.isGrounded == false)
                return;
            if (m_state.isAimingProjectile)
                return;
            if (m_state.isDoingSwordThrust)
                return;

            if (m_state.isGrounded)
            {
                if (m_skills.IsModuleActive(PrimarySkill.SwordThrust))
                {
                    if (m_state.isGrounded && m_state.isInShadowMode == false)
                    {
                        PrepareForGroundAttack();
                        m_swordThrust.Reset();
                        m_groundJump?.Cancel();
                        m_extraJump?.Cancel();
                        m_devilWings?.Cancel();
                        m_whip?.Cancel();
                        m_whipCombo?.Cancel();
                        m_activeDash?.Cancel();
                        m_activeSlide?.Cancel();
                        m_chargeAttackHandle.Set(m_swordThrust, () => true);
                        m_swordThrust?.StartCharge();
                    }
                }
            }
        }

        private void OnSlashCancelledInput()
        {
            /*m_slashHeld = false;
            m_ignoreSlashHeldUntilReleased = false;*/
            if (m_skills.IsModuleActive(PrimarySkill.SwordThrust) == false)
                return;

            if (m_state.isChargingAttack)
            {
                m_chargeAttackHandle.Set(m_swordThrust, () => false);
                if (m_swordThrust.IsChargeComplete())
                {
                    PrepareForGroundAttack();
                    m_groundJump?.Cancel();
                    m_extraJump?.Cancel();
                    m_devilWings?.Cancel();
                    m_whip?.Cancel();
                    m_whipCombo?.Cancel();
                    //m_swordThrust?.ResetDurationTimer();
                    m_swordThrust?.Execute();
                }
                else
                {
                    m_swordThrust?.EndSwordThrust();
                    m_swordThrust?.ResetCooldownTimer();
                    m_swordThrust?.ResetDurationTimer();
                    m_swordThrust?.Cancel();
                    m_idle?.Execute(m_state.allowExtendedIdle);
                }
            }
        }

        private void OnSwordThrustPerformedInput()
        {

        }

        private void OnSwordThrustCancelledInput()
        {
            if (m_state.isChargingAttack)
            {
                m_swordThrust?.EndSwordThrust();
                m_swordThrust?.ResetCooldownTimer();
                m_swordThrust?.ResetDurationTimer();
                m_swordThrust?.Cancel();
                m_idle?.Execute(m_state.allowExtendedIdle);
            }
        }

        private void OnWhipCancelledInput()
        {

        }

        private void OnWhipPerformedInput()
        {
            if (m_skills.IsModuleActive(PrimarySkill.Whip) == false)
                return;

            if (m_state.isChargingAttack || m_state.isDoingSwordThrust || m_state.isAttacking
                || m_state.waitForBehaviour || m_state.canAttack == false || m_state.isExecutingCombatArt)
                return;

            if (m_state.isDashing || m_state.isSliding || m_state.isLedgeGrabbing || m_state.isStickingToWall)
                return;

            if (m_state.isAimingProjectile)
                return;

            if (m_earthShaker.CanEarthShaker() == false)
                return;

            m_idle?.Cancel();

            if (m_state.isInShadowMode)
            {
                if (m_state.canAttackInShadowMode == false)
                {
                    return;
                }
            }

            if (m_state.isGrounded)
            {
                PrepareForGroundAttack();

                if (m_vector2Input.y > 0)
                {
                    m_whip.Execute(WhipAttack.Type.Ground_Overhead);
                    return;
                }

                if (m_state.isCrouched && m_vector2Input.y < 0)
                {
                    m_whip.Execute(WhipAttack.Type.Crouch_Forward);
                    return;
                }

                if (m_whipCombo.CanWhipCombo())
                {
                    m_movement.Cancel();
                    m_idle.Cancel();
                    m_whipCombo.Execute();
                    return;
                }

                #region Old Forward Whip and Whip Combo
                //if (m_vector2Input.x == 0)
                //{
                //    m_whipCombo.Cancel();

                //    m_whip.Execute(WhipAttack.Type.Ground_Forward);
                //    return;
                //}
                //else if (m_vector2Input.x != 0)
                //{
                //    if (IsFacingInput(m_vector2Input.x))
                //    {
                //        if (m_whipCombo.CanWhipCombo())
                //        {
                //            m_whipCombo.Execute();
                //            return;
                //        }
                //    }
                //}
                #endregion
            }
            else
            {
                PrepareForMidairAttack();
                m_devilWings?.EnableLevitate();

                if (m_vector2Input.y > 0)
                {
                    m_whip.Execute(WhipAttack.Type.MidAir_Overhead);
                    return;
                }

                if (m_vector2Input.y == 0)
                {
                    m_whip.Execute(WhipAttack.Type.MidAir_Forward);
                    return;
                }
            }
        }

        private void OnMouseDeltaPerformedInput(Vector2 vector)
        {
            m_mouseDelta = vector;
        }

        private void OnProjectileThrowStartedInput()
        {

        }

        private void ProjectileThrowStart()
        {
            PrepareForGroundAttack();

            if (m_vector2Input.x != 0)
            {
                m_movement.UpdateFaceDirection(m_vector2Input.x);
            }

            m_projectileThrow.StartAim();
            m_projectileThrow.Execute();
            m_state.isAimingProjectile = true;
        }

        private void ProjectileThrowCancel()
        {
            m_projectileThrow.EndAim();
            m_projectileThrow.StartThrow();
            m_state.isAimingProjectile = false;
            GameplaySystem.cinema.ApplyCameraPeekMode(Cinematics.CameraPeekMode.None);
        }


        private void OnProjectileThrowHeldInput()
        {
            if (m_skills.IsModuleActive(PrimarySkill.SkullThrow) == false)
                return;
            if (m_state.isInShadowMode)
                return;
            if (m_state.isGrounded == false)
                return;
            if (m_state.waitForBehaviour)
                return;
            if (m_state.isDashing || m_state.isStickingToWall || m_state.isAttacking || m_state.isLedgeGrabbing ||
                m_state.isCrouched)
                return;
            if (m_state.isChargingAttack)
                return;
            if (m_state.isAimingProjectile)
                return;

            ProjectileThrowStart();
        }

        private void OnProjectileThrowTappedInput()
        {
            if (m_skills.IsModuleActive(PrimarySkill.SkullThrow) == false)
                return;
            if (m_state.isInShadowMode)
                return;
            if (m_state.isGrounded == false)
                return;
            if (m_state.waitForBehaviour)
                return;
            if (m_state.isDashing || m_state.isStickingToWall || m_state.isAttacking || m_state.isLedgeGrabbing ||
                m_state.isCrouched)
                return;
            if (m_state.isChargingAttack)
                return;
            if (m_state.isAimingProjectile)
                return;

            StartCoroutine(StraightThrowRoutine());
        }

        private IEnumerator StraightThrowRoutine()
        {
            PrepareForGroundAttack();
            m_state.isAimingProjectile = true;
            m_projectileThrow.ThrowStraightStartVisuals();
            yield return new WaitForSeconds(0.3f); //hack way to make sure there's time for animation to play
            m_projectileThrow.ThrowStraightEndVisuals();
            yield return new WaitForSeconds(0.2f);
            //m_projectileThrow.ThrowProjectileStraight();
            m_state.isAimingProjectile = false;
        }


        private void OnProjectileThrowCancelledInput()
        {
            if (m_skills.IsModuleActive(PrimarySkill.SkullThrow) == false)
                return;
            if (m_state.isInShadowMode)
                return;
            if (m_state.isAimingProjectile == false)
                return;

            ProjectileThrowCancel();
        }


        private void OnGrabCancelledInput()
        {
            m_isGrabbing = false;
            m_objectManipulation?.Cancel();

            if (m_state.isGrabbing == false)
                return;

            m_movement?.SwitchConfigTo(Movement.Type.Jog);
        }

        private void OnGrabStartedInput()
        {
            if (m_objectManipulation.IsThereAMovableObject() == false)
                return;
            if (m_state.isCrouched || m_state.isLevitating)
                return;

            m_idle?.Cancel();
            m_objectManipulation?.Execute();
            m_isGrabbing = true;
        }

        #region Combat Arts Input
        private void OnBarrierStartedInput()
        {

        }

        private void OnBarrierPerformedInput()
        {
            if (m_abilities.IsAbilityActivated(CombatArt.Barrier) == false)
                return;
            if (m_state.isInShadowMode)
                return;

            if (m_barrier.IsDoingBarrier())
            {
                m_barrier?.EndExecution();
                m_barrier?.EnableShield(false);
            }
            else
            {
                if (m_barrier.HaveEnoughSourceForExecution() == false)
                    return;
                if (m_state.isExecutingCombatArt)
                    return;

                PrepareForGroundAttack();
                m_currentCombatArt = m_barrier;
                if (m_abilities.GetAbilityLevel(CombatArt.Barrier) == 1)
                {
                    if (m_state.isGrounded == false)
                        return;
                    m_barrier?.Execute();
                    m_barrier?.SetCanMove(false);
                }
                else if (m_abilities.GetAbilityLevel(CombatArt.Barrier) == 2)
                {
                    m_barrier?.EnableShield(true);
                    m_barrier?.SetCanMove(true);
                }
            }
        }

        private void OnBarrierCancelledInput()
        {

        }

        private void OnEarthShakerStartedInput()
        {

        }

        private void OnEarthShakerCancelledInput()
        {

        }

        private void OnEarthShakerPerformedInput()
        {
            if (m_state.isExecutingCombatArt) { return; }
            if (m_state.isInShadowMode) { return; }
            if (m_skills.IsModuleActive(PrimarySkill.EarthShaker) && m_earthShaker.CanEarthShaker())
            {
                if (m_state.isGrounded == false)
                {
                    m_earthShaker?.Reset();
                    PrepareForMidairAttack();
                    m_diagonalSwordDash?.Cancel();
                    m_icarusWings?.Cancel();
                    m_devilWings?.Cancel();
                    m_earthShaker?.StartExecution();
                    return;
                }
            }
        }

        private void OnAirSlashStartedInput()
        {

        }

        private void OnAirSlashCancelledInput()
        {

        }

        private void OnAirSlashPerformedInput()
        {
            if (m_state.isExecutingCombatArt)
                return;
            if (m_state.isAttacking)
                return;
            if (m_abilities.IsAbilityActivated(CombatArt.AirSlashRange) && !(m_abilities.IsAbilityActivated(CombatArt.LightningSpear)))
            {
                if (m_state.isGrounded == false)
                {
                    if (m_airSlashRange.CanAirSlashRange())
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            PrepareForMidairAttack();
                            m_currentCombatArt = m_airSlashRange;
                            m_airSlashRange.Execute();
                            return;
                        }
                    }
                }
            }
            if (m_abilities.IsAbilityActivated(CombatArt.LightningSpear) && (m_abilities.IsAbilityActivated(CombatArt.AirSlashRange)))
            {
                if (m_state.isGrounded == false && m_lightningSpear.CanLightningSpear())
                {
                    PrepareForMidairAttack();
                    m_currentCombatArt = m_lightningSpear;
                    m_lightningSpear.Execute();
                    return;
                }
            }
        }

        private void OnHellTridentStartedInput()
        {

        }

        private void OnHellTridentCancelledInput()
        {

        }

        private void OnHellTridentPerformedInput()
        {
            if (m_state.isExecutingCombatArt)
                return;
            if (m_abilities.IsAbilityActivated(CombatArt.HellTrident))
            {
                if (m_state.isInShadowMode == false)
                {
                    if (m_state.isGrounded)
                    {
                        PrepareForGroundAttack();

                        m_currentCombatArt = m_hellTrident;
                        m_hellTrident.Execute();
                        return;
                    }
                }
            }
        }

        private void OnSoulFireBlastStartedInput()
        {
            if (m_state.isExecutingCombatArt)
                return;
            if (m_abilities.IsAbilityActivated(CombatArt.SoulfireBlast))
            {
                m_devilWings?.Cancel();
                m_extraJump?.Cancel();
            }
        }

        private void OnSoulFireBlastCancelledInput()
        {

        }

        private void OnSoulFireBlastPerformedInput()
        {
            if (m_state.isExecutingCombatArt)
                return;
            if (m_abilities.IsAbilityActivated(CombatArt.SoulfireBlast))
            {
                if (m_state.isGrounded == false)
                {
                    PrepareForMidairAttack();
                    m_currentCombatArt = m_soulFireBlast;
                    m_soulFireBlast.Execute();
                    return;
                }
            }
        }

        private void OnBackDiverStartedInput()
        {

        }

        private void OnBackDiverCancelledInput()
        {

        }

        private void OnBackDiverPerformedInput()
        {
            if (m_state.isExecutingCombatArt)
                return;
            if (m_abilities.IsAbilityActivated(CombatArt.BackDiver))
            {
                if (m_state.isGrounded)
                {
                    if (m_backDiver.CanBackDiver() && m_backDiver.HaveSpacetoExecute())
                    {
                        if (m_state.isInShadowMode == false)
                        {
                            m_crouch?.Cancel();
                            m_backDiver.Reset();
                            PrepareForGroundAttack();
                            m_movement?.SwitchConfigTo(Movement.Type.Jog);
                            m_currentCombatArt = m_backDiver;
                            m_backDiver.Execute();
                            return;
                        }
                    }
                }
            }
        }

        private void OnSovereignImpaleStartedInput()
        {
            if (m_state.isExecutingCombatArt)
                return;
            if (m_abilities.IsAbilityActivated(CombatArt.SovereignImpale))
            {
                if (m_sovereignImpale.CanSovereignImpale())
                {
                    if (m_state.isInShadowMode == false)
                    {
                        if (m_state.isGrounded)
                        {
                            m_crouch?.Cancel();
                            m_sovereignImpale.Reset();
                            PrepareForGroundAttack();
                            m_movement?.SwitchConfigTo(Movement.Type.Jog);
                            m_currentCombatArt = m_sovereignImpale;
                            m_sovereignImpale?.Execute();
                            return;
                        }
                    }
                }
            }
        }

        private void OnSovereignImpaleCancelledInput()
        {

        }

        private void OnSovereignImpalePerformedInput()
        {

        }

        private void OnDiagonalSwordDashStartedInput()
        {

        }

        private void OnDiagonalSwordDashCancelledInput()
        {

        }

        private void OnDiagonalSwordDashPerformedInput()
        {
            if (m_state.isExecutingCombatArt){ return; }
            if (m_state.isAttacking) { return; }
            if (m_state.isDoingEarthShaker){ return; }
            if (m_abilities.IsAbilityActivated(CombatArt.DiagonalSwordDash) && m_diagonalSwordDash.CanDiagonalSwordDash())
            {
                if (m_state.isGrounded == false)
                {
                    //if (m_diagonalSwordDash.CanExecuteDash() == false) { m_diagonalSwordDash?.Cancel(); }
                    m_diagonalSwordDash.Reset();
                    PrepareForMidairAttack();
                    m_basicSlashes?.Cancel();
                    m_earthShaker?.Cancel();
                    m_devilWings?.Cancel();
                    m_extraJump?.Cancel();
                    m_currentCombatArt = m_diagonalSwordDash;
                    m_diagonalSwordDash?.Execute();
                    return;
                }
                else
                {
                    m_currentCombatArt = null;
                }
            }
        }

        private void OnEdgedFuryStartedInput()
        {

        }

        private void OnEdgedFuryCancelledInput()
        {

        }

        private void OnEdgedFuryPerformedInput()
        {
            if (m_state.isExecutingCombatArt)
                return;

            if (m_abilities.IsAbilityActivated(CombatArt.EdgedFury))
            {
                if (m_state.isGrounded == false)
                {
                    m_edgedFury.Reset();
                    PrepareForMidairAttack();
                    m_whipCombo?.Cancel();
                    m_devilWings?.Cancel();
                    m_extraJump?.Cancel();
                    m_whip?.Cancel();
                    m_currentCombatArt = m_edgedFury;
                    m_edgedFury.Execute();
                }

            }
        }

        private void OnReapersHarvestStartedInput()
        {

        }

        private void OnReapersHarvestCancelledInput()
        {

        }

        private void OnReapersHarvestPerformedInput()
        {
            if (m_state.isExecutingCombatArt)
                return;
            if (m_state.isHighJumping) //sometimes you're still grounded while jumping [fast fingers]
                return;
            if (m_abilities.IsAbilityActivated(CombatArt.ReaperHarvest))
            {
                m_state.waitForBehaviour = true;
                m_state.isHighJumping = false;
                if (m_state.isGrounded)
                {
                    m_reaperHarvest.Reset();
                    PrepareForGroundAttack();
                    m_currentCombatArt = m_reaperHarvest;
                    m_reaperHarvest.Execute(ReaperHarvest.ReaperHarvestState.Grounded);
                    m_state.waitForBehaviour = false;

                }
            }

        }

        private void OnIcarusWingsStartedInput()
        {
            //is grounded guard is not viable because jump input is being called before this function is called which automatically makes you ungrounded
            //if (m_state.isGrounded == false)
            //    return;

            if (m_abilities.IsAbilityActivated(CombatArt.IcarusWings) == false || m_icarusWings.CanIcarusWings() == false)
                return;

            if (m_state.isHighJumping)
                m_groundJump.Cancel();

            m_state.isExecutingCombatArt = true;
            //m_extraJump.Cancel();
        }

        private void OnIcarusWingsCancelledInput()
        {
            /*m_state.isExecutingCombatArt = false;*/
        }

        private void OnIcarusWingsPerformedInput()
        {
            //if (m_state.isExecutingCombatArt) { return; }
            if (m_state.isGrounded == false)
                return;
            if (m_state.isChargingAttack)
                return;
            if (m_vector2Input.x != 0)
                return;
            if (m_abilities.IsAbilityActivated(CombatArt.IcarusWings) && m_icarusWings.CanIcarusWings())
            {
                m_basicSlashes.Cancel();
                PrepareForGroundAttack();
                m_earthShaker?.Cancel();
                m_diagonalSwordDash?.Cancel();
                m_currentCombatArt = m_icarusWings;
                m_icarusWings.Execute();
            }
        }

        private void OnTeleportingSkullStartedInput()
        {

        }

        private void OnTeleportingSkullPerformedInput()
        {
            if (m_state.isExecutingCombatArt)
                return;
            if (m_abilities.IsAbilityActivated(CombatArt.TeleportingSkull) == false)
                return;

            if (m_teleportingSkull.canTeleport)
            {
                m_teleportingSkull.TeleportToProjectile();
                return;
            }

            m_projectileThrow.SetProjectileInfo(m_teleportingSkull.projectile);
            m_projectileThrow.WillResetProjectile();
            m_teleportingSkull.Execute();
            return;
        }

        private void OnTeleportingSkullCancelledInput()
        {

        }
        #endregion
        #endregion

        #region Action Functions
        private void HandleWallMovement()
        {
            if (m_skills.IsModuleActive(PrimarySkill.WallMovement) == false)
                return;
            if (m_state.isGrounded)
                return;
            if (m_state.isStickingToWall)
                return;

            var hasIntentionToWallStick = m_vector2Input.x != 0 && (m_state.isDashing == false);

            if (hasIntentionToWallStick)
            {
                var allowedToWallStick = m_state.isHighJumping == false && m_state.isInShadowMode == false;

                if (allowedToWallStick)
                {
                    var isWallStickRequirementAchieved = (m_wallStick?.IsHeightRequirementAchieved() ?? false) && (m_wallStick?.IsThereAWall() ?? false);

                    if (isWallStickRequirementAchieved)
                    {
                        if (m_state.isLevitating)
                        {
                            m_devilWings?.Cancel();
                        }

                        m_basicSlashes.ResetAirAttacks();
                        m_dash?.Reset();
                        m_extraJump?.Reset();
                        m_wallStick.Execute();
                        return;
                    }
                }
            }
        }
        private void ProjectileThrowAiming()
        {
            m_projectileThrow.MoveAim(m_mouseDelta, GameplaySystem.cinema.mainCamera.ScreenToWorldPoint(m_mouseDelta));

            if (m_projectileThrow?.HasReachedVerticalThreshold() == true)
            {
                GameplaySystem.cinema.ApplyCameraPeekMode(Cinematics.CameraPeekMode.Up);
            }
            else
            {
                GameplaySystem.cinema.ApplyCameraPeekMode(Cinematics.CameraPeekMode.None);
            }

            return;
        }

        private void SwordThrustAction()
        {
            if (m_state.isChargingAttack)
            {
                m_swordThrust?.HandleDurationTimer();
            }
        }

        private void LevitateAction()
        {
            if (m_state.isLevitating)
            {
                m_devilWings?.EnableLevitate();
                m_devilWings?.MaintainHeight();
                m_devilWings?.GiveMovementBoost();
                m_devilWings?.ConsumeSource();
                if ((m_devilWings?.HaveEnoughSourceForMaintainingHeight() ?? true) == false)
                {
                    m_devilWings?.Cancel();
                }
            }
        }

        private void WallStickMovementAction()
        {
            if (m_state.isStickingToWall == false)
                return;

            if (m_vector2Input.x != 0 && Mathf.Sign(m_vector2Input.x) != (float)m_character.facing)
            {
                m_wallStick?.Cancel();
                m_wallMovement?.Cancel();
                m_extraJump?.Reset();
                FlipCharacter();
                return;
            }

            if (m_state.canWallCrawl == true)
            {
                m_wallSlide?.Cancel();
                m_wallMovement?.Move(m_vector2Input.y);

                m_groundedness?.Evaluate();

                if ((m_wallMovement?.IsThereAWall(WallMovement.SensorType.Body) ?? false) == false ||
                    (m_wallMovement?.IsThereAWall(WallMovement.SensorType.Overhead) ?? false) == false)
                {
                    m_wallMovement?.Cancel();
                }

                if (m_state.isGrounded)
                {
                    return;
                }
            }
            else
            {
                if (m_vector2Input.x != 0 && Mathf.Sign(m_vector2Input.x) == (float)m_character.facing)
                {
                    m_wallSlide?.Cancel();
                    return;
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
                }
            }
            return;
        }

        private void LedgeGrabMovementAction()
        {
            if (m_state.isGrounded == false)
            {
                if (m_vector2Input.x != 0)
                {
                    if (m_ledgeGrab?.IsDoable() ?? false)
                    {
                        m_wallMovement?.Cancel();
                        m_wallStick?.Cancel();
                        m_ledgeGrab?.Execute();
                    }
                }
            }
        }

        private void GrabMoveAction()
        {
            if (m_isGrabbing == false)
                return;

            if (m_objectManipulation.IsThereAMovableObject())
            {
                if (m_vector2Input.x == 0)
                {
                    m_objectManipulation?.GrabIdle();
                    return;
                }

                if (m_vector2Input.x != 0)
                {
                    if (m_state.isPushing)
                    {
                        m_movement?.SwitchConfigTo(Movement.Type.Push);
                    }
                    else
                    {
                        m_movement?.SwitchConfigTo(Movement.Type.Pull);
                    }

                    m_idle.Cancel();
                    m_objectManipulation?.MoveObject(m_vector2Input.x, m_character.facing);
                }
            }
            else
            {
                m_movement?.SwitchConfigTo(Movement.Type.Jog);
                m_objectManipulation?.Cancel();
            }

            MoveCharacter(m_state.isGrabbing, m_vector2Input.x);
        }

        private void HandleCrouchMovement()
        {
            if (m_state.isAttacking || m_state.waitForBehaviour)
                return;

            if (m_state.isGrounded)
            {
                if (m_vector2Input.y < 0)
                {
                    if (m_state.isGrabbing)
                        return;

                    m_state.isAttacking = false;
                    m_state.waitForBehaviour = false;
                    m_idle?.Cancel();
                    m_crouch?.Execute();
                    m_movement?.SwitchConfigTo(Movement.Type.Crouch);
                }
                else
                {
                    if (m_crouch?.IsThereNoCeiling() ?? true)
                    {
                        m_crouch?.Cancel();
                        m_movement?.SwitchConfigTo(Movement.Type.Jog);
                    }
                }
            }
        }

        private void MoveAction()
        {
            if (m_state.isDashing)
                return;
            if (m_state.isAttacking)
                return;
            if (m_state.waitForBehaviour)
                return;
            if (m_state.isLedgeGrabbing)
                return;
            if (m_state.isDoingSwordThrust)
                return;
            if (m_state.isChargingAttack)
                return;
            if (m_state.isDoingWhipCombo)
                return;

            if (m_vector2Input.x == 0)
            {
                m_movement.Cancel();
                m_state.isAttacking = false;
                m_state.waitForBehaviour = false;
                m_idle?.Execute(m_state.allowExtendedIdle);
                return;
            }

            MoveCharacter(m_state.isGrabbing, m_vector2Input.x);
        }
        #endregion

        #region Utility
        private void OnNewGameIntroStarted()
        {
            m_storeHasBeenPickedUp = false;
            m_playerWokeUp = false;
            NewGameIntroEvent.NewGameIntroStarted -= OnNewGameIntroStarted;
        }

        private void OnPlayerWokeUp()
        {
            m_playerWokeUp = true;
            NewGameIntroEvent.NewGamePlayerWokeUp -= OnPlayerWokeUp;
        }

        private void OnPickedUpBook()
        {
            m_storeHasBeenPickedUp = true;
            NewGameIntroEvent.NewGameIntroFinished -= OnPickedUpBook;
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
                if (m_state.isAimingProjectile)
                {
                    if (m_projectileThrow?.HasReachedVerticalThreshold() == true)
                    {
                        GameplaySystem.cinema.ApplyCameraPeekMode(Cinematics.CameraPeekMode.Up);
                    }
                    else
                    {
                        GameplaySystem.cinema.ApplyCameraPeekMode(Cinematics.CameraPeekMode.None);
                    }
                }
                else
                {
                    m_attackRegistrator?.ResetHitCache();
                }
            }
            else if (m_state.isChargingAttack)
            {
                m_chargeAttackHandle?.Execute();
            }
            else if (m_state.isDashing)
            {
                HandleDash();
            }
            else if (m_state.isSliding)
            {
                HandleSlide(m_vector2Input.x);
            }
        }

        private void HandleAirBehaviour()
        {
            if (m_state.isDashing)
            {
                HandleDash();
            }

            if (m_state.isSliding)
            {
                HandleSlide(m_vector2Input.x);
            }

            if (m_state.isStickingToWall)
            {
                if (m_wallSlide.IsThereAWall())
                {
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

                    m_state.isHighJumping = false;

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
                    m_movement?.ResetGravity();

                    m_basicSlashes.Cancel();
                    m_slashCombo.Cancel();
                    m_whip.Cancel();
                    m_whipCombo.Cancel();
                }
                else
                {
                    m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, m_rigidbody.velocity.y);
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

                if (m_currentCombatArt != null)
                {
                    m_lightningSpear?.Cancel();
                    m_diagonalSwordDash?.Cancel();
                    m_earthShaker?.Cancel();

                    m_currentCombatArt = null;
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
            m_slashHeld = false;
            m_ignoreSlashHeldUntilReleased = false;

            m_swordThrust.Cancel();
            m_state.isDoingSwordThrust = false;
            m_vector2Input = Vector2.zero;

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
                    m_activeSlide?.Cancel();
                    m_activeDash?.Cancel(); //Cancelling here because repeated flinch sometimes cause vfx to stay stuck because it doesn't go into dash/slide state
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
                            m_reaperHarvest?.Cancel();
                            m_sovereignImpale?.Cancel();
                            m_hellTrident?.Cancel();
                            m_foolsVerdict?.Cancel();
                            m_backDiver?.Cancel();
                            m_barrier?.Cancel();
                            m_championsUprising?.Cancel();
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
                        m_activeDash?.Cancel();
                    }
                    else if (m_state.isSliding)
                    {
                        m_activeSlide?.Cancel();
                    }
                    else if (m_state.isGrabbing)
                    {
                        m_objectManipulation.Cancel();
                    }
                    else if (m_state.isInShadowMode)
                    {
                        m_shadowMorph?.Cancel();
                    }
                    else if (m_state.isAimingProjectile)
                    {
                        m_projectileThrow?.Cancel();
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
                    m_diagonalSwordDash?.Cancel();
                    m_lightningSpear?.Cancel();
                    m_airSlashRange?.Cancel();
                }
            }
        }

        private void OnProjectileThrowRequest(object sender, EventActionArgs eventArgs)
        {
            //m_input.projectileThrowPressed = true;
        }

        private void FlipCharacter()
        {
            var oppositeFacing = m_character.facing == HorizontalDirection.Right ? HorizontalDirection.Left : HorizontalDirection.Right;
            m_character.SetFacing(oppositeFacing);
            m_slashCombo.Cancel();
            m_slashCombo.Reset();
            m_whipCombo.Cancel();
            m_whipCombo.Reset();
        }

        private void ResetProjectile(object sender, EventActionArgs eventArgs)
        {
            if (m_projectileThrow.willResetProjectile)
                m_projectileThrow.ResetProjectile();
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
            m_reaperHarvest?.Cancel();
            m_krakenRage?.Cancel();
            m_sovereignImpale?.Cancel();
            m_hellTrident?.Cancel();
            m_foolsVerdict?.Cancel();
            m_soulFireBlast?.Cancel();
            m_edgedFury?.Cancel();
            m_backDiver?.Cancel();
            m_barrier?.Cancel();
            m_diagonalSwordDash?.Cancel();
            m_championsUprising?.Cancel();
            m_lightningSpear?.Cancel();
            m_icarusWings?.Cancel();
            m_airSlashRange?.Cancel();
        }

        public void Enable()
        {
            //m_inputReader.Enable();
            m_playerInput.ActivateInput();
            ControllerEnabled?.Invoke(this, EventActionArgs.Empty);
        }

        public void Disable()
        {
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
            m_reaperHarvest?.Cancel();
            m_krakenRage?.Cancel();
            m_sovereignImpale?.Cancel();
            m_hellTrident?.Cancel();
            m_foolsVerdict?.Cancel();
            m_soulFireBlast?.Cancel();
            m_edgedFury?.Cancel();
            m_backDiver?.Cancel();
            m_barrier?.Cancel();
            m_diagonalSwordDash?.Cancel();
            m_championsUprising?.Cancel();
            m_lightningSpear?.Cancel();
            m_icarusWings?.Cancel();
            m_airSlashRange?.Cancel();
            m_teleportingSkull?.Cancel();

            if (m_state.isGrounded)
            {
                m_movement?.SwitchConfigTo(Movement.Type.Jog);
            }
            //m_inputReader.Disable();
            m_playerInput.DeactivateInput();
            ControllerDisabled?.Invoke(this, EventActionArgs.Empty);
        }

        private void OnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            m_movement?.Cancel();
            m_crouch?.Cancel();
            m_dash?.Cancel();
            m_slide?.Cancel();
            m_activeDash?.Cancel();
            m_activeSlide?.Cancel();
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
            m_reaperHarvest?.Cancel();
            m_krakenRage?.Cancel();
            m_sovereignImpale?.Cancel();
            m_hellTrident?.Cancel();
            m_foolsVerdict?.Cancel();
            m_soulFireBlast?.Cancel();
            m_edgedFury?.Cancel();
            m_backDiver?.Cancel();
            m_barrier?.Cancel();
            m_diagonalSwordDash?.Cancel();
            m_championsUprising?.Cancel();
            m_lightningSpear?.Cancel();
            m_icarusWings?.Cancel();
            m_airSlashRange?.Cancel();
            m_teleportingSkull?.Cancel();

            m_activeSlide?.Clear(); //clear slide vfx because it is still visible in some scene changes
            m_shadowGaugeRegen.Enable(true);
        }

        private void OnUIStateChanged(GameplayUIState state)
        {
            //return guard here assumes PlayerCharacterOverride is moving player during Gameplay 
            if (state == GameplayUIState.GameplayHUD)
                return;

            //reset when interactableUI or Cinematic is current state to prevent running on its own
            m_vector2Input = Vector2.zero;
        }

        public void ControlMovementOverride(float direction)
        {
            m_vector2Input.y = 0;
            m_vector2Input.x = direction;
        }
        #endregion

        #region Module Handling
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
                if (m_vector2Input.x != 0)
                {
                    var signInput = Mathf.Sign(m_vector2Input.x);
                    if (signInput != (float)m_character.facing)
                    {
                        FlipCharacter();
                    }
                }
                m_activeDash?.Execute();
            }
        }

        private void HandleSlide(float horizontalInput)
        {
            m_activeSlide?.HandleDurationTimer();

            if (m_state.isGrounded == false)
            {
                m_activeSlide?.Cancel();
                m_activeSlide?.ResetCooldownTimer();
                return;
            }

            if (m_activeSlide?.IsSlideDurationOver() ?? true)
            {
                if (m_slide.IsThereACeiling() == false)
                {
                    m_activeSlide?.Cancel();
                    m_activeSlide?.ResetCooldownTimer();

                    if (m_crouch.IsCrouchingPossible() || !m_slide.HasGroundToSlideOn() || !m_shadowSlide.HasGroundToSlideOn())
                    {

                        if (m_state.isCrouched == false)
                        {
                            m_crouch?.Execute();
                            m_idle?.Cancel();
                            m_movement?.SwitchConfigTo(Movement.Type.Crouch);
                        }
                    }
                }
                else
                {
                    if (m_slide.IsThereACeiling())
                    {
                        m_activeSlide?.ContinueSlide();
                        return;
                    }
                }

                return;
            }

            if (horizontalInput != 0)
            {
                var signInput = Mathf.Sign(horizontalInput);
                if (signInput != (float)m_character.facing)
                {
                    FlipCharacter();
                }
            }
            m_activeSlide?.Execute();
        }

        private void ExecuteDash()
        {
            if (m_skills.IsModuleActive(PrimarySkill.ShadowDash))
            {
                if (m_skills.IsModuleActive(PrimarySkill.Dash) == false)
                    return;
                if (m_shadowDash?.HaveEnoughSourceForExecution() ?? false)
                {
                    m_activeDash = m_shadowDash;
                    m_shadowDash.ConsumeSource();
                }
                else if (m_skills.IsModuleActive(PrimarySkill.Dash))
                {
                    m_activeDash = m_dash;
                }
            }
            else if (m_skills.IsModuleActive(PrimarySkill.Dash))
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
            }

        }

        private void MoveCharacter(bool isGrabbing, float horizontalInput)
        {
            if (!IsFacingInput(horizontalInput))
            {
                m_basicSlashes.Cancel();
                m_slashCombo.Cancel();
                m_whip.Cancel();
                m_whipCombo.Cancel();
                m_whipCombo.Reset();
            }

            if (isGrabbing == false)
            {
                if (horizontalInput == 0)
                {
                    m_idle?.Execute(m_state.allowExtendedIdle);
                }
                else
                {
                    m_idle?.Cancel();
                }
                if (m_state.isGrounded && m_state.isHighJumping == false)
                {
                    m_movement?.GroundMove(horizontalInput, true);
                    if (m_stepClimb.CheckForStepClimbableSurface())
                    {
                        m_stepClimb.ClimbSurface();
                    }
                }
                else
                {
                    m_movement?.AirMove(horizontalInput, true);
                }
            }
            else
            {
                if (m_state.isGrounded)
                {
                    m_movement?.GroundMove(horizontalInput, false);

                    if (m_stepClimb.CheckForStepClimbableSurface())
                    {
                        m_stepClimb.ClimbSurface();
                    }
                }
                else
                {
                    m_movement?.AirMove(horizontalInput, false);
                }
            }


        }
        private bool CanMove()
        {
            var allowedByCombatArts = m_reaperHarvest.CanMove()
                    && m_sovereignImpale.CanMove()
                    && m_hellTrident.CanMove()
                    && m_foolsVerdict.CanMove()
                    && m_barrier.CanMove();

            var isAllowedByDash = m_activeDash?.IsDashDurationOver() ?? true;
            var isAllowedBySlide = (m_activeSlide?.IsSlideDurationOver() ?? true) && (m_state.isSliding == false);
            var isAllowedBySkills = m_whipCombo.CanMove()
                    && m_whip.CanMove()
                    && isAllowedByDash
                    && isAllowedBySlide
                    && m_state.isDoingSwordThrust == false
                    && m_state.isAimingProjectile == false
                    && m_earthShaker.CanEarthShaker();

            return m_slashCombo.CanMove()
                    && isAllowedBySkills
                    && allowedByCombatArts
                    && m_state.isAttacking == false
                    && m_state.isChargingAttack == false;
        }

        private bool IsFacingInput(float horizontalInput)
        {
            return horizontalInput > 0 && m_character.facing == HorizontalDirection.Right
                || horizontalInput < 0 && m_character.facing == HorizontalDirection.Left
                || horizontalInput == 0;
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

        public void OnDeviceTypeChanged()
        {
            ActiveControllerChanged?.Invoke(m_playerInput.currentControlScheme);
        }
    }
}