using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Projectiles;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class DeformedCultist : Minion, IFlinch
    {
        [SerializeField]
        private Damage m_damage;
        [SerializeField]
        [MinValue(2f)]
        private float m_moveSpeed;

        [SerializeField]
        private ProjectileInfo m_spellProjectile;
        [SerializeField]
        private Transform m_spellSpawnPoint;

        private ProjectileLauncher m_projectileLauncher;

        private PhysicsMovementHandler2D m_movement;
        protected override CombatCharacterAnimation animation => null;
        protected override Damage startDamage => m_damage;

        public void MoveTo(Vector2 position)
        {
            m_movement.MoveTo(position, m_moveSpeed);
        }

        public void Turn()
        {
            m_movement.Stop();
            TurnCharacter();
        }

        public void Taunt()
        {
            m_movement.Stop();
        }

        public void Flinch(RelativeDirection direction, DamageType damageTypeRecieved)
        {
            m_movement.Stop();
        }

        public AttackProjectile CastSpellAt(ITarget target)
        {
            // return m_projectileLauncher.FireProjectileTo(m_spellProjectile.projectile, gameObject.scene, m_spellSpawnPoint.position, target.position, m_spellProjectile.speed);
            return null;
        }

        protected override void Awake()
        {
            base.Awake();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedPhysics2D>(), transform);
           // m_projectileLauncher = new ProjectileLauncher();
        }

        public void Flinch(RelativeDirection damageSource, IReadOnlyCollection<DamageType> damageTypeRecieved)
        {
            throw new System.NotImplementedException();
        }
    }

}