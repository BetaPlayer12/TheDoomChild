using System.Collections;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Holysoft.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class HauntingGhost : Minion, ISpawnable, IMovingEnemy
    {
        [SerializeField]
        private PoolableItemData m_poolableItemData;

        public PoolableItemData poolableItemData => m_poolableItemData;


        public event EventAction<PoolItemEventArgs> PoolRequest;
        public event EventAction<PoolItemEventArgs> InstanceDestroyed;

        [SerializeField]
        [TabGroup("References")]
        private Hitbox m_nonAttackHitbox;
        [SerializeField]
        [TabGroup("References")]
        private Hitbox m_attackHitbox;

        [SerializeField]
        private Damage m_damage;
        [SerializeField]
        [MinValue(0f)]
        private float m_moveSpeed;
        [SerializeField]
        [MinValue(0f)]
        private float m_dashSpeed;

        private HauntingGhostAnimation m_animation;
        private PhysicsMovementHandler2D m_movement;

        public float dashSpeed => m_dashSpeed;
        protected override CombatCharacterAnimation animation => m_animation;
        protected override Damage startDamage => m_damage;

        public void ForcePool()
        {
            PoolRequest?.Invoke(this,new PoolItemEventArgs(this, transform));
            gameObject.SetActive(false);
        }

        public void SpawnAt(Vector2 position, Quaternion rotation)
        {
            transform.position = position;
            m_brain.ResetBrain();
            gameObject.SetActive(true);
            m_nonAttackHitbox.Enable();
            m_attackHitbox.Disable();
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(SpawnRoutine()));
        }

        public void MoveTo(Vector2 position)
        {
            m_movement.MoveTo(position, m_moveSpeed);
            if (transform.position.x > position.x)
            {
                if (currentFacingDirection == HorizontalDirection.Right)
                {
                    m_animation.DoMove(HauntingGhostAnimation.MoveDirection.Backward);
                }
                else
                {
                    m_animation.DoMove(HauntingGhostAnimation.MoveDirection.Forward);
                }
            }
            else
            {
                if (currentFacingDirection == HorizontalDirection.Right)
                {
                    m_animation.DoMove(HauntingGhostAnimation.MoveDirection.Forward);
                }
                else
                {
                    m_animation.DoMove(HauntingGhostAnimation.MoveDirection.Backward);
                }
            }
        }

        public void Stay()
        {
            m_movement.Stop();
            m_animation.DoIdle();
        }

        public void Despawn()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DespawnRoutine()));
        }

        public void Dash(ITarget target, float duration)
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(DashRoutine(target, duration)));
        }

        public void DestroyInstance()
        {
            InstanceDestroyed?.Invoke(this, new PoolItemEventArgs(this, transform));
            Destroy(gameObject);
        }

        private IEnumerator SpawnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_animation.DoSpawn();
            yield return new WaitForAnimationStart(m_animation.animationState, HauntingGhostAnimation.ANIMATION_SPAWN);
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private IEnumerator DashRoutine(ITarget target, float duration)
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoAttack();
            var animationState = m_animation.animationState;
            yield return new WaitForAnimationStart(animationState, HauntingGhostAnimation.ANIMATION_ATTACK_LOOP);
            m_nonAttackHitbox.Disable();
            m_attackHitbox.Enable();
            m_movement.MoveTo(target.position, m_dashSpeed);
            yield return new WaitForIsolatedSeconds(duration, time);
            m_animation.DoAttackDespawn();
            yield return new WaitForAnimationComplete(animationState, HauntingGhostAnimation.ANIMATION_ATTACK_DESPAWN);
            ForcePool();
            StopActiveBehaviour();
        }

        private IEnumerator DespawnRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDespawn();
            yield return new WaitForAnimationComplete(m_animation.animationState, HauntingGhostAnimation.ANIMATION_ATTACK_DESPAWN);
            ForcePool();
            StopActiveBehaviour();
        }

        protected IEnumerator DeathRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            m_animation.DoDeath();
            yield return new WaitForAnimationComplete(m_animation.animationState, HauntingGhostAnimation.ANIMATION_DEATH);
            ForcePool();
            m_waitForBehaviourEnd = false;
            m_behaviour.SetActiveBehaviour(null);
        }

        private void LookAt(Vector2 target)
        {
            Vector3 diff = (Vector3)target - transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }


        protected override void ResetValues()
        {
            m_nonAttackHitbox.Enable();
            m_attackHitbox.Disable();
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<ObjectPhysics2D>(), transform);
            m_animation = GetComponent<HauntingGhostAnimation>();
        }
    }
}