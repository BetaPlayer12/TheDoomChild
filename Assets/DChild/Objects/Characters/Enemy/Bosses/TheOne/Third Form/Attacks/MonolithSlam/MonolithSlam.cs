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
        [SerializeField, TabGroup("Placeholder")]
        private GameObject m_AttackIndicator;
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
        private RaySensor m_playerSensor;
        [SerializeField]
        private bool m_playerHit;
        private float m_TentacleHoldSpeed = 0f;

        private bool m_smashMonolith;
        private bool m_isAlreadyTriggered;
        public bool removeMonolithOnGround;
        public bool keepMonolith;
        public bool monolithGrounded;
        
        // Start is called before the first frame update
        void Start()
        {
            m_impactCollider.enabled = true;
            m_obstacleCollider.enabled = false;
            m_smashMonolith = false;
            keepMonolith = false;
            m_playerHit = false;
            m_playerSensor.enabled = false;
            StartCoroutine(EmergeTentacle());
        }

        // Update is called once per frame
        void Update()
        {
            if (m_smashMonolith)
            {
                if (!m_isAlreadyTriggered)
                {
                    StartCoroutine(Smash());
                }
                m_smashMonolith = false;
            }

            if (!monolithGrounded)
            {
                if (keepMonolith)
                {
                    if (m_playerSensor.isDetecting)
                    {
                        m_playerHit = true;
                    }

                    if (m_playerHit)
                    {
                        StartCoroutine(DestroyMonolith());
                    }
                }
            }            
        }

        public void SetTentacleHoldDuration(float HoldForSeconds)
        {
            m_TentacleHoldSpeed = HoldForSeconds;
        }

        private IEnumerator EmergeTentacle()
        {
            m_impactCollider.enabled = false;
            m_animation.SetAnimation(0, m_emergeAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_emergeAnimation);
            yield return AnticipationLoop();
        }

        private IEnumerator AnticipationLoop()
        {
            m_animation.SetAnimation(0, m_anticipationLoopAnimation, true);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_anticipationLoopAnimation);
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

        private IEnumerator DoAttackWithMonolithPersist()
        {
            m_animation.SetAnimation(0, m_attackPlatformAftermathAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_attackPlatformAftermathAnimation);

            //FindObjectOfType<ObstacleChecker>().RemoveMonolithAtIndex(0);

            m_impactCollider.enabled = false;
            m_obstacleCollider.enabled = true;
            m_animation.SetAnimation(0, m_platformPersistAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_platformPersistAnimation);

            monolithGrounded = true;     
        }

        private IEnumerator DoAttackWithoutMonolithPersist()
        {
            m_animation.SetAnimation(0, m_attackDestroyAftermathAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_attackDestroyAftermathAnimation);

            yield return DestroyMonolith();
        }

        [Button]
        private void AttackKeepMonolith()
        {
            Debug.Log("KEPT MONOLITH");
            StartCoroutine(DoAttackWithMonolithPersist());
        }

        [Button]
        private void AttackDestroyMonolith()
        {
            Debug.Log("Destroy MONOLITH");
            StartCoroutine(DoAttackWithoutMonolithPersist());
        }

        private IEnumerator Smash()
        {
            yield return new WaitForSeconds(m_TentacleHoldSpeed);
            m_AttackIndicator.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            m_playerSensor.enabled = true;
            m_impactCollider.enabled = true;
            if (keepMonolith)
            {
                m_isAlreadyTriggered = true;
                AttackKeepMonolith();
            }
            else if(!keepMonolith)
            {
                AttackDestroyMonolith();
            }
            yield return null;
        }

        private void OnDestroy()
        {
            if (FindObjectOfType<ObstacleChecker>().monolithSlamObstacleList != null)
                FindObjectOfType<ObstacleChecker>().monolithSlamObstacleList.Remove(this);
        }

        public void TriggerSmash()
        {
            m_smashMonolith = true;
        }
    }
}

