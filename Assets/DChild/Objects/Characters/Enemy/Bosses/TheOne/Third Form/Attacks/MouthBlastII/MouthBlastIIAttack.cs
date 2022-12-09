using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using DChild.Gameplay.Characters.AI;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MouthBlastIIAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField, TabGroup("Reference")]
        protected SpineRootAnimation m_animation;
        [SerializeField]
        private SkeletonAnimation m_skeletonAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_waitForInitializeAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_attackLoopAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_attackChargeAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_toGrowAnimation;
        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_afterAttackAnimation;

        [SerializeField, BoxGroup("Laser")]
        private LaserLauncher m_launcher;

        [SerializeField]
        private float m_blastDuration;

        public IEnumerator ExecuteAttack()
        {
            ShootMouthBlast();
            yield return null;
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        [Button]
        private void ShootMouthBlast()
        {
            StartCoroutine(GrowMouth());
        }

        private IEnumerator GrowMouth()
        {
            m_animation.SetAnimation(0, m_toGrowAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_toGrowAnimation);
            yield return ChargeBeam();
        }

        private IEnumerator ChargeBeam()
        {
            m_launcher.SetBeam(true);
            m_launcher.SetAim(false);
            m_animation.SetAnimation(0, m_attackChargeAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_attackChargeAnimation);
            yield return ShootBlast();
        }

        private IEnumerator ShootBlast()
        {
            StartCoroutine(m_launcher.LazerBeamRoutine());
            m_animation.SetAnimation(0, m_attackLoopAnimation, true);
            yield return new WaitForSeconds(m_blastDuration);
            m_launcher.SetBeam(false);
            yield return RetractMouth();
        }

        private IEnumerator RetractMouth()
        {
            m_animation.SetAnimation(0, m_afterAttackAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_afterAttackAnimation);
        }

        private void Start()
        {
            m_animation.SetAnimation(0, m_waitForInitializeAnimation, false);
        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
        {
            throw new System.NotImplementedException();
        }
    }
}

