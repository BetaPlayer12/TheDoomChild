using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using Spine.Unity;

namespace DChild.Gameplay.Characters.Enemies
{
    public class TentacleCeiling : MonoBehaviour
    {
        private BoxCollider2D m_tentacleHitBox;

        [SerializeField, TabGroup("Reference")]
        protected SpineRootAnimation m_animation;
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_anticipationLoopAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_attackAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_extendedAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_retractAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_startAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_waitForInputAnimation;

        public IEnumerator AnticipateAttack()
        {
            m_animation.SetAnimation(0, m_anticipationLoopAnimation, true);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_anticipationLoopAnimation);
        }

        public IEnumerator Attack(float duration)
        {
            m_animation.SetAnimation(0, m_attackAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_attackAnimation);

            yield return Extended(duration);
        }

        public IEnumerator Extended(float duration)
        {
            m_animation.SetAnimation(0, m_extendedAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_extendedAnimation);
            yield return new WaitForSeconds(duration);
            yield return Retract();
        }

        public IEnumerator Retract()
        {
            m_animation.SetAnimation(0, m_retractAnimation, false);
            m_tentacleHitBox.enabled = false;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_retractAnimation);
        }

        private void Start()
        {
            m_animation.SetAnimation(0, m_waitForInputAnimation, true);
            m_tentacleHitBox = this.GetComponent<BoxCollider2D>();
        }

        [Button]
        private void DoAttack()
        {
            StartCoroutine(Attack(3.5f));
            //StartCoroutine(Retract());
        }
    }
}

