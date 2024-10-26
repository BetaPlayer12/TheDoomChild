using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    [CreateAssetMenu(fileName = "PlayerCharacterModuleConfiguration", menuName = "DChild/Gameplay/Character/Player Character Module Configuration")]
    public class PlayerCharacterModuleConfiguration : ScriptableObject
    {
        [Header("Basic")]
        [Title("Movement")]
        [SerializeField, HideLabel]
        private MovementStatsInfo m_movementInfo;
        public MovementStatsInfo movementStatsInfo => m_movementInfo;

        [Title("Ground Jump")]
        [SerializeField, HideLabel]
        private GroundJumpStatsInfo m_groundJumpInfo;
        public GroundJumpStatsInfo groundJumpInfo => m_groundJumpInfo;

        [Title("Object Manipulation")]
        [SerializeField, HideLabel]
        private ObjectManipulationStatsInfo m_objectManipulationInfo;
        public ObjectManipulationStatsInfo objectManipulationInfo => m_objectManipulationInfo;

        [Title("Ledge Grab")]
        [SerializeField, HideLabel]
        private LedgeGrabStatsInfo m_ledgeGrabStatsInfo;
        public LedgeGrabStatsInfo ledgeGrabStatsInfo => m_ledgeGrabStatsInfo;

        [Title("Shadow Morph")]
        [SerializeField, HideLabel]
        private ShadowMorphStatsInfo m_shadowMorphInfo;
        public ShadowMorphStatsInfo shadowMorphStatsInfo => m_shadowMorphInfo;

        [Title("Auto Step Climb")]
        [SerializeField, HideLabel]
        private AutoStepClimbStatsInfo m_autoStepClimbInfo;
        public AutoStepClimbStatsInfo autoStepClimbStatsInfo => m_autoStepClimbInfo;

        [Header("Combat")]
        [Title("Basic Slashes")]
        [SerializeField, HideLabel]
        private BasicSlashesStatsInfo m_basicSlashesStatsInfo;
        public BasicSlashesStatsInfo basicSlashesStatsInfo => m_basicSlashesStatsInfo;

        [Title("Slash Combo")]
        [SerializeField, HideLabel]
        private SlashComboStatsInfo m_slashComboStatsInfo;
        public SlashComboStatsInfo slashComboStatsInfo => m_slashComboStatsInfo;

        [Title("Player Flinch")]
        [SerializeField, HideLabel]
        private FlinchStatsInfo m_flinchStatsInfo;
        public FlinchStatsInfo flinchStatsInfo => m_flinchStatsInfo;

        [Header("Skills")]
        [Title("Dash")]
        [SerializeField, HideLabel]
        private DashStatsInfo m_dashStatsInfo;
        public DashStatsInfo dashStatsInfo => m_dashStatsInfo;

        [Title("Devil Wings")]
        [SerializeField, HideLabel]
        private DevilWingsStatsInfo m_devilWingsInfo;
        public DevilWingsStatsInfo devilWingsInfo => m_devilWingsInfo;

        [Title("Extra Jump")]
        [SerializeField, HideLabel]
        private ExtraJumpStatsInfo m_extraJumpInfo;
        public ExtraJumpStatsInfo extraJumpInfo => m_extraJumpInfo;

        [Title("Shadow Dash")]
        [SerializeField, HideLabel]
        private ShadowDashStatsInfo m_shadowDashInfo;
        public ShadowDashStatsInfo shadowDashInfo => m_shadowDashInfo;

        [Title("Shadow Slide")]
        [SerializeField, HideLabel]
        private ShadowSlideStatsInfo m_shadowSlideInfo;
        public ShadowSlideStatsInfo shadowSlideInfo => m_shadowSlideInfo;

        [Title("Slide")]
        [SerializeField, HideLabel]
        private SlideStatsInfo m_slideInfo;
        public SlideStatsInfo slideInfo => m_slideInfo;

        [Header("Movement Skills")]
        [Title("Wall Jump")]
        [SerializeField, HideLabel]
        private WallJumpStatsInfo m_wallJumpStatsInfo;
        public WallJumpStatsInfo wallJumpInfo => m_wallJumpStatsInfo;

        [Title("Wall Movement")]
        [SerializeField, HideLabel]
        private WallMovementStatsInfo m_wallMovementStatsInfo;
        public WallMovementStatsInfo wallMovementInfo => m_wallMovementStatsInfo;

        [Title("Wall Slide")]
        [SerializeField, HideLabel]
        private WallSlideStatsInfo m_wallSlideStatsInfo;
        public WallSlideStatsInfo wallSlideInfo => m_wallSlideStatsInfo;

        [Title("Wall Stick")]
        [SerializeField, HideLabel]
        private WallStickStatsInfo m_wallStickStatsInfo;
        public WallStickStatsInfo wallStickInfo => m_wallStickStatsInfo;

        [Header("Combat Skills")]
        [Title("Earth Shaker")]
        [SerializeField, HideLabel]
        private EarthShakerStatsInfo m_earthShakerStatsInfo;
        public EarthShakerStatsInfo earthShakerInfo => m_earthShakerStatsInfo;
        [Title("Sword Thrust")]
        [SerializeField, HideLabel]
        private SwordThrustStatsInfo m_swordThrustStatsInfo;
        public SwordThrustStatsInfo swordThrustInfo => m_swordThrustStatsInfo;
        [Title("Whip Attack")]
        [SerializeField, HideLabel]
        private WhipAttackStatsInfo m_whipAttackStatsInfo;
        public WhipAttackStatsInfo whipAttackInfo => m_whipAttackStatsInfo;
        [Title("Whip Attack Combo")]
        [SerializeField, HideLabel]
        private WhipAttackComboStatsInfo m_whipAttackComboStatsInfo;
        public WhipAttackComboStatsInfo whipAttackComboInfo => m_whipAttackComboStatsInfo;
        [Title("Projectile Throw")]
        [SerializeField, HideLabel]
        private ProjectileThrowStatsInfo m_projectileThrowStatsInfo;
        public ProjectileThrowStatsInfo projectileThrowInfo => m_projectileThrowStatsInfo;
        [Title("Block")]
        [SerializeField, HideLabel]
        private BlockStatsInfo m_blockStatsInfo;
        public BlockStatsInfo blockInfo => m_blockStatsInfo;

        [Header("Misc")]
        [Title("Combat Readiness")]
        [SerializeField, HideLabel]
        private CombatReadinessStatsInfo m_combatReadinessStatsInfo;
        public CombatReadinessStatsInfo combatReadinessInfo => m_combatReadinessStatsInfo;

        [Title("Idle Handle")]
        [SerializeField, HideLabel]
        private IdleHandleStatsInfo m_idleHandleStatsInfo;
        public IdleHandleStatsInfo idleHandleInfo => m_idleHandleStatsInfo;
    }
}