using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using Spine.Unity;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ChasingGroundTentacle : PoolableObject
    {
        [SerializeField]
        private Collider2D m_hitbox;

        [SerializeField, TabGroup("Reference")]
        protected SpineRootAnimation m_animation;
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_anticipationLoopAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_extendedAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_retractAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_waitForInputAnimation;

        // Start is called before the first frame update
        void Start()
        {
            m_hitbox.enabled = false;
            StartCoroutine(WaitForInput());
        }

        private IEnumerator Anticipation()
        {
            m_animation.SetAnimation(0, m_anticipationLoopAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_anticipationLoopAnimation);
            StartCoroutine(Extended());
        }

        private IEnumerator Extended()
        {
            m_hitbox.enabled = true;
            m_animation.SetAnimation(0, m_extendedAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_extendedAnimation);
        }

        private IEnumerator Retract()
        {
            m_animation.SetAnimation(0, m_retractAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_retractAnimation);
        }

        private IEnumerator WaitForInput()
        {
            m_animation.SetAnimation(0, m_waitForInputAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_waitForInputAnimation);
        }

        public void ErectTentacle()
        {
            StartCoroutine(Anticipation());
        }

        public void RetractTentacle()
        {
            StartCoroutine(Retract());
        }

        [Button]
        private void Attack()
        {
            ErectTentacle();
        }

        [Button]
        private void UnAttack()
        {
            RetractTentacle();
        }
    }
}

