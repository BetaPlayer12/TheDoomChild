using DChild;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class ReaperHarvestAnimation : MonoBehaviour
    {
        [SerializeField]
        private SpineRootAnimation m_spine;
#if UNITY_EDITOR
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;

        public void InitializeField(SpineRootAnimation spineRoot)
        {
            m_spine = spineRoot;
        }
#endif
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), BoxGroup("Grounded")]
        private string m_startGroundedAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), BoxGroup("Grounded")]
        private string m_impactGroundedAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), BoxGroup("Midair")]
        private string m_startMidairAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation"), BoxGroup("Midair")]
        private string m_impactMidairAnimation;

        public void StartGrounded()
        {
            m_spine.SetAnimation(0, m_startGroundedAnimation, false);
        }

        public void ImpactGrounded()
        {
            m_spine.SetAnimation(0, m_impactGroundedAnimation, false);
        }

        public void StartMidair()
        {
            m_spine.SetAnimation(0, m_startMidairAnimation, false);
        }

        public void ImpactMidair()
        {
            m_spine.SetAnimation(0, m_impactMidairAnimation, false);
        }
    }
}
