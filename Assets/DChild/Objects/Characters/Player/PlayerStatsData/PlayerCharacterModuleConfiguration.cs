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
    }
}
