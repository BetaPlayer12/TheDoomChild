using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using Spine.Unity;
using Holysoft.Event;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SlidingStoneWall : PoolableObject
    {
        [SerializeField, TabGroup("Reference")]
        protected SpineRootAnimation m_animation;
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_emergeAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_waitForInputAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_attackAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_retractAnimation;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_floorSensor;

        [SerializeField, TabGroup("Colliders")]
        private GameObject m_floorSlamCollider;
        [SerializeField, TabGroup("Colliders")]
        private GameObject m_wallSlamCollider;
        [SerializeField, TabGroup("Colliders")]
        private GameObject m_wallCollider;

        public event EventAction<EventActionArgs> AttackStart;
        public event EventAction<EventActionArgs> AttackDone;

        // Start is called before the first frame update
        void Start()
        {
            m_floorSlamCollider.SetActive(false);
            m_wallSlamCollider.SetActive(false);
            m_wallCollider.SetActive(false);
        }

        private IEnumerator EmergeTentacle()
        {
            m_animation.SetAnimation(0, m_emergeAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_emergeAnimation);
        }

        private IEnumerator AttackTentacle()
        {
            m_animation.SetAnimation(0, m_attackAnimation, false).TimeScale = 0.25f; //use timescale to adjust tentacle attack speed
            yield return new WaitForAnimationComplete(m_animation.animationState, m_attackAnimation);
        }

        private IEnumerator RetractTentacle()
        {
            m_animation.SetAnimation(0, m_retractAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_retractAnimation);
        }

        private IEnumerator MonolithGroundSmashImpact()
        {
            m_floorSlamCollider.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            m_floorSlamCollider.SetActive(false);
            m_wallCollider.SetActive(true);
        }

        private IEnumerator MonolithWallSlamImpact()
        {
            m_wallCollider.SetActive(false);
            m_wallSlamCollider.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            m_wallSlamCollider.SetActive(false);
        }

        public IEnumerator CompleteSlidingWallAttackSequence()
        {
            AttackStart?.Invoke(this, EventActionArgs.Empty);
            yield return EmergeTentacle();
            yield return AttackTentacle();
            yield return RetractTentacle();
            AttackDone?.Invoke(this, EventActionArgs.Empty);
        }

        public void GroundSmashEffect()
        {
            StartCoroutine(MonolithGroundSmashImpact());
        }

        public void WallSlamEffect()
        {
            StartCoroutine(MonolithWallSlamImpact());
        }

       public void SlidingStoneWallAttack()
        {
            StartCoroutine(CompleteSlidingWallAttackSequence());
        }
    }   
}

