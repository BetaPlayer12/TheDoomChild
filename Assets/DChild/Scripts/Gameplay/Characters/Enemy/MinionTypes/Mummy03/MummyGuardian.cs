using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Spine.Unity.Modules;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MummyGuardian : Minion, IFlinch
    {
        [SerializeField]
        [AttackDamageList(DamageType.Physical)]
        private Damage m_damage;

        [SerializeField]
        private float m_moveSpeed;

        [SerializeField]
        private float m_chargeSpeed;

        private ISensorFaceRotation[] m_sensorRotator;

        private MummyGuardianAnimation m_animation;
        private PhysicsMovementHandler2D m_movement;
        private SpineRootMotion m_rootMotion;
        private EnemyFacingOnStart m_enemyFacing;

        protected override Damage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        public bool m_isCharging;

        public void MoveTo(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            EnableRootMotion(true, true, false);
            m_movement.MoveOnGround(targetPos, m_moveSpeed);
            m_animation.DoMove();    
        }

        public void ChargeAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            
            m_behaviour.SetActiveBehaviour(StartCoroutine(ChargeAttackRoutine()));
        }

        public void ChargeAttackPhase1(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(ChargeAttackPhase1Routine(targetPos)));
        }

        public void ChargeAttackPhase2(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            EnableRootMotion(false, true, false);
            m_movement.MoveOnGround(targetPos, m_chargeSpeed);
            m_animation.DoChargePhase2();
        }

        public void ChargeAttackPhase3()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(ChargeAttackPhase3Routine()));
        }
        public void Stay()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_movement.Stop();
            m_animation.DoIdle();
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
            TurnCharacter();
            RotateSensor();
        }

        private void EnableRootMotion(bool enable, bool useX, bool useY)
        {
            m_rootMotion.enabled = enable;
            if (enable)
            {
                m_rootMotion.useX = useX;
                m_rootMotion.useY = useY;
            }
        }

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_animation.DoDamage();
        }

        private IEnumerator ChargeAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoChargeAttack();
            yield return new WaitForAnimationComplete(m_animation.animationState, MummyGuardianAnimation.ANIMATION_CHARGE_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator ChargeAttackPhase1Routine(Vector2 targetPos)
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoChargePhase1();
            yield return new WaitForAnimationComplete(m_animation.animationState, MummyGuardianAnimation.ANIMATION_CHARGE_PHASE1);          
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
            ChargeAttackPhase2(targetPos);
        }

        private IEnumerator ChargeAttackPhase3Routine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoChargePhase3();
            yield return new WaitForAnimationComplete(m_animation.animationState, MummyGuardianAnimation.ANIMATION_CHARGE_PHASE3);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, MummyGuardianAnimation.ANIMATION_DEATH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        private void RotateSensor()
        {
            for (int x = 0; x < m_sensorRotator.Length; x++)
            {
                m_sensorRotator[x].AlignRotationToFacing(m_facing);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();
            m_animation = GetComponent<MummyGuardianAnimation>();
            m_enemyFacing = GetComponent<EnemyFacingOnStart>();
            m_sensorRotator = GetComponentsInChildren<ISensorFaceRotation>();
        }

        protected override void Start()
        {
            base.Start();
            m_enemyFacing.enabled = true;
            RotateSensor();
        }
    }
}
