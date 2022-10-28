using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters;
using Spine.Unity;

namespace DChild.Gameplay.Projectiles
{
    public class TentacleLifeSpan : PoolableObject
    {
        public float m_timer;

        private BoxCollider2D m_tentacleHitBox;

        [SerializeField]
        private GameObject[] safeZones;

        [SerializeField, TabGroup("Reference")]
        protected SpineRootAnimation m_animation;
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_anticipationLoopAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_anticipationStartAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_attackAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_retractAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_stayAnimation;

        public IEnumerator StabRoutine()
        {
            m_animation.SetAnimation(0, m_anticipationStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_anticipationStartAnimation);

            m_animation.SetAnimation(0, m_attackAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_attackAnimation);

            StartCoroutine(TentacleStay());
        }

        public IEnumerator TentacleStay()
        {
            m_animation.SetAnimation(0, m_stayAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_stayAnimation);
        }

        public IEnumerator Retract()
        {
            foreach (GameObject safeZone in safeZones)
            {
                safeZone.SetActive(false);
            }
            m_animation.SetAnimation(0, m_retractAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_retractAnimation);
            
            DestroyInstance();
        }

        // Start is called before the first frame update
        void Start()
        {
            m_tentacleHitBox = this.GetComponent<BoxCollider2D>();
            m_tentacleHitBox.enabled = false;
            StartCoroutine(StabRoutine());
        }

        // Update is called once per frame
        void Update()
        {
            m_timer -= GameplaySystem.time.deltaTime;

            if (m_timer < 0)
            {
                StartCoroutine(Retract());
            }
        }

    }
}

