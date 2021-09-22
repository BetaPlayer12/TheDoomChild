using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Spine.Unity.Modules;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public abstract class Mummy : Minion, IFlinch
    {
        [SerializeField]
        private Damage m_damage;

        [SerializeField]
        [MinValue(0)]
        private float m_moveSpeed;

        protected abstract MummyAnimation m_animation { get; }
        protected PhysicsMovementHandler2D m_movement;
        protected SpineRootMotion m_rootMotion;
        protected EnemyFacingOnStart m_enemyFacing;

        protected override Damage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        private ISensorFaceRotation[] m_sensorRotater;

        public void MoveTo(Vector2 targetPos)
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            EnableRootMotion(true, true, false);
            m_animation.DoMove();
            m_movement.MoveOnGround(targetPos, m_moveSpeed);
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

        public void WhipAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }
            m_animation.DoWhip();
            m_behaviour.SetActiveBehaviour(StartCoroutine(WhipAttackRoutine()));
        }

        public void Turn()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));          
        }

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, MummyAnimation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, MummyAnimation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            RotateSensor();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator WhipAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoWhip();
            yield return new WaitForAnimationComplete(m_animation.animationState, MummyAnimation.ANIMATION_WHIP_ATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, MummyAnimation.ANIMATION_DEATH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
            gameObject.SetActive(false);
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        private void RotateSensor()
        {
            for (int x = 0; x < m_sensorRotater.Length; x++)
            {
                m_sensorRotater[x].AlignRotationToFacing(m_facing);
            }
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

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
            m_rootMotion = GetComponentInChildren<SpineRootMotion>();
            m_enemyFacing = GetComponent<EnemyFacingOnStart>();
            m_sensorRotater = GetComponentsInChildren<ISensorFaceRotation>();
        }

        protected override void Start()
        {
            base.Start();
            m_enemyFacing.enabled = true;
            RotateSensor();
        }
    }
}
