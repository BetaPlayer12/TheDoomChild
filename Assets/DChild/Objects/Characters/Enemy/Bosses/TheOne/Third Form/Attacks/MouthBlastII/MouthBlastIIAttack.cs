using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using Spine.Unity;
using DChild.Gameplay.Characters.AI;
using Holysoft.Event;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MouthBlastIIAttack : MonoBehaviour, IEyeBossAttacks
    {
        [SerializeField, TabGroup("Reference")]
        protected SpineRootAnimation m_animation;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_HitboxCollider;
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

        public event EventAction<EventActionArgs> AttackStart;
        public event EventAction<EventActionArgs> AttackDone;

        public IEnumerator ExecuteAttack()
        {
            AttackStart?.Invoke(this, EventActionArgs.Empty);
            yield return GrowMouth();

            AttackDone?.Invoke(this, EventActionArgs.Empty);
            yield return null;
        }

        public IEnumerator ExecuteAttack(Vector2 PlayerPosition)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator ExecuteAttack(AITargetInfo Target)
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
            m_HitboxCollider.SetActive(true);
            yield return ChargeBeam();
        }

        private IEnumerator ChargeBeam()
        {
            m_launcher.SetBeam(true);
            m_launcher.SetAim(false);
            m_animation.SetAnimation(0, m_attackChargeAnimation, false);
            m_launcher.TurnOffDamageCollider();
            m_launcher.PlayAnimation("WallMouthBlastAnticipation");
            yield return new WaitForAnimationComplete(m_animation.animationState, m_attackChargeAnimation);
            yield return new WaitForSeconds(2f);
            yield return ShootBlast();
        }

        private IEnumerator ShootBlast()
        {
            m_launcher.TurnOnDamageCollider();
            m_launcher.PlayAnimation("WallMouthBlast","WallMouthBlastAnticipation");
            StartCoroutine(m_launcher.LazerBeamRoutine());
            m_animation.SetAnimation(0, m_attackLoopAnimation, true);
            yield return new WaitForSeconds(m_blastDuration);
            m_launcher.SetBeam(false);
            
            yield return RetractMouth();
        }

        private IEnumerator RetractMouth()
        {
            m_launcher.TurnOffDamageCollider();
            m_launcher.PlayAnimation("TentacleBlastDissipation","WallMouthBlast");
            m_launcher.TurnLazer(false);
            m_animation.SetAnimation(0, m_afterAttackAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_afterAttackAnimation);
            m_HitboxCollider.SetActive(false);
        }

        private void Start()
        {
            m_animation.SetAnimation(0, m_waitForInitializeAnimation, false);
        }
    }
}

