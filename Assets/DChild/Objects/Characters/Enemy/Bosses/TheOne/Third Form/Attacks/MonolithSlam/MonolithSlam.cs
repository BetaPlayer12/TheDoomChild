using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MonolithSlam : PoolableObject
    {
        [SerializeField, TabGroup("Reference")]
        protected SpineRootAnimation m_animation;
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_anticipationLoopAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_attackDestroyAftermathAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_attackPlatformAftermathAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_emergeAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_platformDestroyAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_platformPersistAnimation;

        [SerializeField]
        private BoxCollider2D m_impactCollider;
        [SerializeField]
        private BoxCollider2D m_obstacleCollider;
        [SerializeField]
        private RaySensor m_floorSensor;

        public bool keepMonolith;
        public bool smashMonolith;
        public bool monolithGrounded;

        // Start is called before the first frame update
        void Start()
        {
            m_impactCollider.enabled = true;
            m_obstacleCollider.enabled = false;
            smashMonolith = false;
            keepMonolith = false;
            StartCoroutine(EmergeTentacle());
        }

        // Update is called once per frame
        void Update()
        {
            if (smashMonolith)
            {
                StartCoroutine(Smash());
                smashMonolith = false;
            }
        }

        private IEnumerator EmergeTentacle()
        {
            m_animation.SetAnimation(0, m_emergeAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_emergeAnimation);
            yield return AnticipationLoop();
        }

        private IEnumerator AnticipationLoop()
        {
            m_animation.SetAnimation(0, m_anticipationLoopAnimation, true);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_anticipationLoopAnimation);
        }

        private IEnumerator AttackWithDestroyMonolith()
        {
            m_animation.SetAnimation(0, m_attackDestroyAftermathAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_attackDestroyAftermathAnimation);
        }

        private IEnumerator AttackWithKeepMonolith()
        {
            m_animation.SetAnimation(0, m_attackPlatformAftermathAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_attackPlatformAftermathAnimation);
        }

        private IEnumerator DestroyMonolith()
        {
            m_animation.SetAnimation(0, m_platformDestroyAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_platformDestroyAnimation);
            monolithGrounded = true;
            m_impactCollider.enabled = false;
            m_obstacleCollider.enabled = false;
            DestroyInstance();
        }

        private IEnumerator MonolithPersist()
        {
            m_impactCollider.enabled = false;
            m_obstacleCollider.enabled = true;
            m_animation.SetAnimation(0, m_platformPersistAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_platformPersistAnimation);
            monolithGrounded = true;
        }

        private IEnumerator DoAttackWithMonolithPersist()
        {
            yield return AttackWithKeepMonolith();
            yield return MonolithPersist();
        }

        private IEnumerator DoAttackWithoutMonolithPersist()
        {
            yield return AttackWithDestroyMonolith();
            yield return DestroyMonolith();
        }

        [Button]
        private void AttackWithMonolith()
        {
            StartCoroutine(DoAttackWithMonolithPersist());
        }

        [Button]
        private void AttackDestroyMonolith()
        {
            StartCoroutine(DoAttackWithoutMonolithPersist());
        }

        private IEnumerator Smash()
        {
            if (keepMonolith)
            {
                AttackWithMonolith();
            }
            else
            {
                AttackDestroyMonolith();
            }
            yield return null;
        }
    }
}

