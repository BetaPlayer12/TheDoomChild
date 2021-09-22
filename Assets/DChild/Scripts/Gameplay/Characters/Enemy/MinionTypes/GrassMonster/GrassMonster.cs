using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class GrassMonster : Minion, IFlinch
    {
        [SerializeField]
        [TabGroup("Attack")]
        private float m_attackSpeed;

        [SerializeField]
        private Damage m_damage;

        private GrassMonsterAnimation m_animation;
        private PhysicsMovementHandler2D m_movement;
        protected override CombatCharacterAnimation animation => this.m_animation;
        private ITurnHandler m_turn;

        protected override Damage startDamage => m_damage;

        public void MoveTo(Vector2 position)
        {
            m_movement.MoveOnGround(position, m_attackSpeed);
            m_animation.DoMove();
        }

        public void Stay()
        {
            m_movement.Stop();
            m_animation.DoIdle();
        }

        public void TurnLeft()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnLeftRoutine()));
        }

        public void TurnRight()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(TurnRightRoutine()));
        }

        public void DoAttack1()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(Attack1Routine()));
        }

        public void DoAttack2()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(Attack2Routine()));
        }

        public void PlayerDetected()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(PlayerDetectedRoutine()));
        }

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(FlinchRoutine()));
        }

        private IEnumerator Attack1Routine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoAttack1();
            yield return new WaitForAnimationComplete(m_animation.animationState, GrassMonsterAnimation.ANIMATION_ATTACK1);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator Attack2Routine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoAttack2();
            yield return new WaitForAnimationComplete(m_animation.animationState, GrassMonsterAnimation.ANIMATION_ATTACK2);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator PlayerDetectedRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoPlayerDetected();
            yield return new WaitForAnimationComplete(m_animation.animationState, GrassMonsterAnimation.ANIMATION_DETECT_PLAYER);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnLeftRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoTurnLeft();
            yield return new WaitForAnimationComplete(m_animation.animationState, GrassMonsterAnimation.ANIMATION_TURN_LEFT);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator TurnRightRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoTurnRight();
            yield return new WaitForAnimationComplete(m_animation.animationState, GrassMonsterAnimation.ANIMATION_TURN_RIGHT);
            m_animation.DoIdle();
            yield return null;
            TurnCharacter();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        protected override void OnDeath()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DeathRoutine()));
        }

        private IEnumerator FlinchRoutine()
        {
            Debug.Log("Hurt");
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoFlinch();
            yield return new WaitForAnimationComplete(m_animation.animationState, GrassMonsterAnimation.ANIMATION_HURT);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        protected IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, GrassMonsterAnimation.ANIMATION_DEATH);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        protected override void Awake()
        {
            base.Awake();
            m_animation = GetComponentInChildren<GrassMonsterAnimation>();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedCharacterPhysics2D>(), transform);
            m_turn = new SimpleTurnHandler(this);
        }

        protected override void Start()
        {
            base.Start();
            //m_turn.LookAt(GameplaySystem.playerManager.player.position);
        }
    }
}