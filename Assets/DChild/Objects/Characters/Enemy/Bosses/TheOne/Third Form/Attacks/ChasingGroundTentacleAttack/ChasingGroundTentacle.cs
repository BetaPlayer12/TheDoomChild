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

        public float m_timer;

        private CapsuleCollider2D m_capsuleCollider2D;
        // Start is called before the first frame update
        void Start()
        {
            m_capsuleCollider2D.enabled = false;
            StartCoroutine(Anticipation());
        }

        // Update is called once per frame
        void Update()
        {
            m_timer -= GameplaySystem.time.deltaTime;

            if (m_timer < 0)
            {
                StartCoroutine(Retract());
                DestroyInstance();
            }
        }

        private IEnumerator Anticipation()
        {
            m_animation.SetAnimation(0, m_anticipationLoopAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_anticipationLoopAnimation);

            StartCoroutine(Extended());
        }

        private IEnumerator Extended()
        {
            m_capsuleCollider2D.enabled = true;
            m_animation.SetAnimation(0, m_extendedAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_extendedAnimation);
        }

        private IEnumerator Retract()
        {
            m_animation.SetAnimation(0, m_retractAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_retractAnimation);
        }
    }
}

