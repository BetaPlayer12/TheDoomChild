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

        public void InitializeModuleConfigurations()
        {
            m_movement.SetConfiguration(m_playerBasicBehaviourConfiguration.movementStatsInfo);
            m_groundJump.SetConfiguration(m_playerBasicBehaviourConfiguration.groundJumpInfo);
            m_objectManipulation.SetConfiguration(m_playerBasicBehaviourConfiguration.objectManipulationInfo);
            m_ledgeGrab.SetConfiguration(m_playerBasicBehaviourConfiguration.ledgeGrabStatsInfo);
            m_shadowMorph.SetConfiguration(m_playerBasicBehaviourConfiguration.shadowMorphStatsInfo);
            m_autoStepClimb.SetConfiguration(m_playerBasicBehaviourConfiguration.autoStepClimbStatsInfo);
            m_flinch.SetConfiguration(m_playerBasicBehaviourConfiguration.flinchStatsInfo);
        }
    }
}

