using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Characters.Players.Behaviour;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerCharacterModuleConfigurator : MonoBehaviour
    {
        [SerializeField]
        private PlayerCharacterModuleConfiguration m_playerBasicBehaviourConfiguration;
        public PlayerCharacterModuleConfiguration playerBasicBehaviourConfiguration => m_playerBasicBehaviourConfiguration;

        [Title("Basic")]
        [SerializeField]
        private Movement m_movement;
        [SerializeField]
        private GroundJump m_groundJump;
        [SerializeField]
        private ObjectManipulation m_objectManipulation;
        [SerializeField]
        private LedgeGrab m_ledgeGrab;
        [SerializeField]
        private ShadowMorph m_shadowMorph;
        [SerializeField]
        private AutoStepClimb m_autoStepClimb;

        [Title("Combat")]
        [SerializeField]
        private BasicSlashes m_basicSlashes;
        [SerializeField]
        private SlashCombo m_slashCombo;
        [SerializeField]
        private PlayerFlinch m_flinch;

        //MovementSkills
        [Title("Movement Skills")]
        [SerializeField]
        private Dash m_dash;
        [SerializeField]
        private ExtraJump m_extraJump;
        [SerializeField]
        private DevilWings m_devilWings;
        [SerializeField]
        private ShadowDash m_shadowDash;
        [SerializeField]
        private Slide m_slide;
        [SerializeField]
        private ShadowSlide m_shadowSlide;

        [Title("Wall Based Movement")]
        [SerializeField]
        private WallStick m_wallStick;
        [SerializeField]
        private WallMovement m_wallMovement;
        [SerializeField]
        private WallJump m_wallJump;
        [SerializeField]
        private WallSlide m_wallSlide;

        [Title("Combat Skills")]
        [SerializeField]
        private EarthShaker m_earthShaker;
        [SerializeField]
        private SwordThrust m_swordThrust;
        [SerializeField]
        private WhipAttack m_whipAttack;
        [SerializeField]
        private WhipAttackCombo m_whipAttackCombo;
        [SerializeField]
        private ProjectileThrow m_projectileThrow;
        [SerializeField]
        private PlayerBlock m_block;

        [Title("Misc")]
        [SerializeField]
        private IdleHandle m_idleHandle;
        [SerializeField]
        private CombatReadiness m_combatReadiness;

        public void InitializeModuleConfigurations()
        {
            m_movement.SetConfiguration(m_playerBasicBehaviourConfiguration.movementStatsInfo);
            m_groundJump.SetConfiguration(m_playerBasicBehaviourConfiguration.groundJumpInfo);
            m_objectManipulation.SetConfiguration(m_playerBasicBehaviourConfiguration.objectManipulationInfo);
            m_ledgeGrab.SetConfiguration(m_playerBasicBehaviourConfiguration.ledgeGrabStatsInfo);
            m_shadowMorph.SetConfiguration(m_playerBasicBehaviourConfiguration.shadowMorphStatsInfo);
            m_autoStepClimb.SetConfiguration(m_playerBasicBehaviourConfiguration.autoStepClimbStatsInfo);
            m_basicSlashes.SetConfiguration(m_playerBasicBehaviourConfiguration.basicSlashesStatsInfo);
            m_slashCombo.SetConfiguration(m_playerBasicBehaviourConfiguration.slashComboStatsInfo);
            m_flinch.SetConfiguration(m_playerBasicBehaviourConfiguration.flinchStatsInfo);
            m_dash.SetConfiguration(m_playerBasicBehaviourConfiguration.dashStatsInfo);
            m_extraJump.SetConfiguration(m_playerBasicBehaviourConfiguration.extraJumpInfo);
            m_devilWings.SetConfiguration(m_playerBasicBehaviourConfiguration.devilWingsInfo);
            m_shadowDash.SetConfiguration(m_playerBasicBehaviourConfiguration.shadowDashInfo);
            m_slide.SetConfiguration(m_playerBasicBehaviourConfiguration.slideInfo);
            m_shadowSlide.SetConfiguration(m_playerBasicBehaviourConfiguration.shadowSlideInfo);
            m_wallStick.SetConfiguration(m_playerBasicBehaviourConfiguration.wallStickInfo);
            m_wallMovement.SetConfiguration(m_playerBasicBehaviourConfiguration.wallMovementInfo);
            m_wallJump.SetConfiguration(m_playerBasicBehaviourConfiguration.wallJumpInfo);
            m_wallSlide.SetConfiguration(m_playerBasicBehaviourConfiguration.wallSlideInfo);
            m_earthShaker.SetConfiguration(m_playerBasicBehaviourConfiguration.earthShakerInfo);
            m_swordThrust.SetConfiguration(m_playerBasicBehaviourConfiguration.swordThrustInfo);
            m_whipAttack.SetConfiguration(m_playerBasicBehaviourConfiguration.whipAttackInfo);
            m_whipAttackCombo.SetConfiguration(m_playerBasicBehaviourConfiguration.whipAttackComboInfo);
            m_projectileThrow.SetConfiguration(m_playerBasicBehaviourConfiguration.projectileThrowInfo);
            m_block.SetConfiguration(m_playerBasicBehaviourConfiguration.blockInfo);
            m_idleHandle.SetConfiguration(playerBasicBehaviourConfiguration.idleHandleInfo);
            m_combatReadiness.SetConfiguration(m_playerBasicBehaviourConfiguration.combatReadinessInfo);
        }
    }
}
