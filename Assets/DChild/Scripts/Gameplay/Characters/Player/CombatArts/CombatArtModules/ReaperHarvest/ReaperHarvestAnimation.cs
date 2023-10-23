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
#endif
        [SerializeField]
        private float m_lifetime;

        public void InitializeField(SpineRootAnimation spineRoot)
        {
            m_spine = spineRoot;
        }
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
            StopAllCoroutines();
            m_spine.SetAnimation(0, m_startGroundedAnimation, false);
        }

        public void ImpactGrounded()
        {
            m_spine.SetAnimation(0, m_impactGroundedAnimation, false);
        }

        public void StartMidair()
        {
            StopAllCoroutines();
            m_spine.SetAnimation(0, m_startMidairAnimation, false);
        }

        public void ImpactMidair()
        {
            m_spine.SetAnimation(0, m_impactMidairAnimation, false);
        }

        public void Disable()
        {
            if (this.gameObject.activeSelf)
                StartCoroutine(DisableRoutine());
        }

        private IEnumerator DisableRoutine()
        {
            yield return new WaitForSeconds(m_lifetime);
            this.gameObject.SetActive(false);
            yield return null;
        }
    }
}
