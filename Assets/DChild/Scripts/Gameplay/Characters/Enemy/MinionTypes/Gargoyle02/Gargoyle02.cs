using DChild.Gameplay.Combat;
using Spine.Unity.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class Gargoyle02 : Minion, IFlinch
    {
        [SerializeField]
        private Damage m_damage;

        [SerializeField]
        private float m_moveSpeed;

        [SerializeField]
        private float m_patrolSpeed;

        private Gargoyle02Animation m_animation;
        private PhysicsMovementHandler2D m_movement;
        private EnemyFacingOnStart m_enemyFacing;
        private SpineRootMotion m_spineRoot;

        protected override Damage startDamage => m_damage;
        protected override CombatCharacterAnimation animation => m_animation;

        public void MoveTo(Vector2 targetPos)
        {  
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            EnableSpineRoot(false);
           
            m_animation.DoMove();
            m_movement.MoveTo(targetPos, m_moveSpeed);
        }

        public void Patrol(Vector2 Position)
        {

            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            EnableSpineRoot(false);

            m_animation.DoMove();
            m_movement.MoveTo(Position, m_patrolSpeed);
        }

        public void WingAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            EnableSpineRoot(true);

            m_animation.DoWingAttack();
            m_behaviour.SetActiveBehaviour(StartCoroutine(WingAttackRoutine()));
        }

        public void ClawAttack()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            EnableSpineRoot(true);
     
            m_animation.DoClawAttack();
            m_behaviour.SetActiveBehaviour(StartCoroutine(ClawAttackRoutine()));
        }

        public void PlayerDetected()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            EnableSpineRoot(true);

            m_animation.DoPlayerDetect();
            m_behaviour.SetActiveBehaviour(StartCoroutine(PlayerDetectRoutine()));
        }

        public void Stay()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            EnableSpineRoot(false);

            m_movement.Stop();
            m_animation.DoIdle();
        }

        public void Stone()
        {
            if (m_waitForBehaviourEnd)
            {
                StopActiveBehaviour();
            }

            EnableSpineRoot(false);

            m_movement.Stop();
            m_animation.DoStone();
        }

        public void Turn()
        {
            StopActiveBehaviour();
            EnableSpineRoot(false);
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRoutine()));
        }

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            EnableSpineRoot(true);
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator WingAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoWingAttack();
            yield return new WaitForAnimationComplete(animation.animationState, Gargoyle02Animation.ANIMATION_WINGATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator ClawAttackRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoClawAttack();
            yield return new WaitForAnimationComplete(animation.animationState, Gargoyle02Animation.ANIMATION_CLAWATTACK);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator PlayerDetectRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoPlayerDetect();
            yield return new WaitForAnimationComplete(animation.animationState, Gargoyle02Animation.ANIMATION_PLAYERDETECT);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoTurn();
            yield return new WaitForAnimationComplete(m_animation.animationState, Gargoyle02Animation.ANIMATION_TURN);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator FlinchRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, Gargoyle02Animation.ANIMATION_FLINCH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, Gargoyle02Animation.ANIMATION_DEATH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private void EnableSpineRoot(bool value)
        {
            if (value)
            {
                m_spineRoot.enabled = value;
            }
            else
            {
                m_spineRoot.enabled = value;
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
            m_animation = GetComponent<Gargoyle02Animation>();
            m_enemyFacing = GetComponent<EnemyFacingOnStart>();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
            m_spineRoot = GetComponentInChildren<SpineRootMotion>();
            m_enemyFacing.enabled = false;
        }
        protected override void Start()
        {
            base.Start();
            m_enemyFacing.enabled = true;
        }

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<DamageType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }
}
