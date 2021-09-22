using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Spine.Unity.Modules;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class ShellBug : Minion, IFlinch
    {
        [SerializeField,TitleGroup("Stat")]
        private Damage m_damage;

        [SerializeField, TitleGroup("Enemy")]
        private float m_chargeSpeed;
        [SerializeField, TitleGroup("Enemy")]
        private float m_scoutDuration;

        [HideInInspector]
        public bool hasCharged;

        private ShellBugAnimation m_animation;
        private PhysicsMovementHandler2D m_movement;
        private WaitForWorldSeconds m_scoutTime;
        private SpineRootMotion m_rootMotion;

        private ISensorFaceRotation[] m_sensorRotator;

        protected override Damage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        public void Patrol()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_rootMotion.enabled = true;
            m_animation.DoMove();
        }

        public void MoveTo()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_rootMotion.enabled = true;
            m_animation.DoMove();
        }

        public void Scout()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(ScoutRoutine()));
        }

        public void Idle()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_animation.DoIdle();
            m_movement.Stop();
        }

        public void Turn()
        {
            if (m_facing == HorizontalDirection.Left)
            {
                SetFacing(HorizontalDirection.Left);      
            }
            else
            {
                SetFacing(HorizontalDirection.Right);
            }

            m_movement.Stop();
            Idle();
            TurnCharacter();
            RotateSensor();
        }

        public void Charge(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_rootMotion.enabled = false;
            m_behaviour.SetActiveBehaviour(StartCoroutine(ChargeRoutine(targetPos)));
        }

        public void Flinch(RelativeDirection damageSource, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, ShellBugAnimation.ANIMATION_DAMAGE);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator ChargeRoutine(Vector2 targetPos)
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoChargeAnticipation();
            yield return new WaitForAnimationComplete(m_animation.animationState, ShellBugAnimation.ANIMATION_CHARGE_PREP);
            m_movement.MoveOnGround(targetPos, m_chargeSpeed);
            m_animation.DoCharge();
            yield return new WaitForAnimationComplete(m_animation.animationState, ShellBugAnimation.ANIMATION_CHARGE);
            hasCharged = false;
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator ScoutRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoIdle();
            yield return m_scoutTime;
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, CombatCharacterAnimation.ANIMATION_DEATH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private void RotateSensor()
        {
            for (int x = 0; x < m_sensorRotator.Length; x++)
            {
                m_sensorRotator[x].AlignRotationToFacing(m_facing);
            }
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
            m_animation = GetComponent<ShellBugAnimation>();
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();
            m_sensorRotator = GetComponentsInChildren<ISensorFaceRotation>();
        }
        protected override void Start()
        {
            base.Start();
            m_scoutTime = new WaitForWorldSeconds(m_scoutDuration);
        }
    }
}
