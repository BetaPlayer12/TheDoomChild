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

        //Basic
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

        //Combat
        [SerializeField]
        private PlayerFlinch m_flinch;

        //MovementSkills
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

        public void InitializeModuleConfigurations()
        {
            m_movement.SetConfiguration(m_playerBasicBehaviourConfiguration.movementStatsInfo);
            m_groundJump.SetConfiguration(m_playerBasicBehaviourConfiguration.groundJumpInfo);
            m_objectManipulation.SetConfiguration(m_playerBasicBehaviourConfiguration.objectManipulationInfo);
            m_ledgeGrab.SetConfiguration(m_playerBasicBehaviourConfiguration.ledgeGrabStatsInfo);
            m_shadowMorph.SetConfiguration(m_playerBasicBehaviourConfiguration.shadowMorphStatsInfo);
            m_autoStepClimb.SetConfiguration(m_playerBasicBehaviourConfiguration.autoStepClimbStatsInfo);
            m_flinch.SetConfiguration(m_playerBasicBehaviourConfiguration.flinchStatsInfo);
            m_dash.SetConfiguration(m_playerBasicBehaviourConfiguration.dashStatsInfo);
            m_extraJump.SetConfiguration(m_playerBasicBehaviourConfiguration.extraJumpInfo);
            m_devilWings.SetConfiguration(m_playerBasicBehaviourConfiguration.devilWingsInfo);
            m_shadowDash.SetConfiguration(m_playerBasicBehaviourConfiguration.shadowDashInfo);
            m_slide.SetConfiguration(m_playerBasicBehaviourConfiguration.slideInfo);
            m_shadowSlide.SetConfiguration(m_playerBasicBehaviourConfiguration.shadowSlideInfo);
        }
    }
}

