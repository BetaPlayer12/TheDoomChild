using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PossessedHuman : Minion, IFlinch
    {
        [SerializeField]
        private Damage m_damage;
        [SerializeField]
        [MinValue(0f)]
        private float m_moveSpeed;
        [SerializeField]
        [MinValue(0f)]
        private float m_patrolSpeed;

        [SerializeField]
        private float m_explodeDamageRadius;
        [SerializeField]
        private ExplosionInfo m_explodeInfo;

        private IsolatedPhysics2D m_physics;
        private PhysicsMovementHandler2D m_movement;

        protected override CombatCharacterAnimation animation => null;

        protected override Damage startDamage => m_damage;

        public void Explode()
        {
            StopActiveBehaviour();
            m_behaviour.SetActiveBehaviour(StartCoroutine(ExplodeRoutine()));
        }

        public void MoveTo(Vector2 position)
        {
            m_movement.MoveTo(position, m_moveSpeed);
        }

        public void PatrolTo(Vector2 position)
        {
            m_movement.MoveTo(position, m_patrolSpeed);
        }

        public void Turn()
        {
            TurnCharacter();
        }

        public void Idle()
        {
            m_movement.Stop();
        }

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {

        }

        private void ApplyExplosionDamage()
        {
            List<ITarget> alreadyDamaged = new List<ITarget>();
            var affectedColliders = Physics2D.OverlapCircleAll(position, m_explodeDamageRadius, 1 << LayerMask.NameToLayer("Player"));
            for (int i = 0; i < affectedColliders.Length; i++)
            {
                var damageable = affectedColliders[i].GetComponentInParent<ITarget>();
                if (damageable != null)
                {
                    if (alreadyDamaged.Contains(damageable) == false)
                    {
                        //GameplaySystem.combatManager.ResolveConflict(info, damageable);
                        alreadyDamaged.Add(damageable);
                    }
                }
            }
        }

        private IEnumerator ExplodeRoutine()
        {
            m_waitForBehaviourEnd = true;
            m_movement.Stop();
            DisableHitboxes();
            yield return new WaitForSeconds(1f);
            ApplyExplosionDamage();
            m_physics.AddExplosion(m_explodeInfo, position);
            m_health.Empty();
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
            StopActiveBehaviour();
        }

        protected override void Awake()
        {
            base.Awake();
            m_physics = GetComponent<IsolatedPhysics2D>();
            m_movement = new PhysicsMovementHandler2D(m_physics, transform);
        }
    }

}