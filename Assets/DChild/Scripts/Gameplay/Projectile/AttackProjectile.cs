
using DChild.Gameplay.Combat;
using Holysoft.Pooling;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public abstract class AttackProjectile : Projectile
    {
        [SerializeField]
        private AttackProjectileData m_data;

        protected bool m_collidedWithEnvironment;
        private static Hitbox m_cacheToDamage;

        protected override ProjectileData projectileData => m_data;

        public override void ResetState()
        {
            base.ResetState();
            m_collidedWithEnvironment = false;
        }

        protected abstract void Collide();

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Environment")
            {
                if (m_data.canPassThroughEnvironment == false)
                {
                    m_collidedWithEnvironment = true;
                    Collide();
                }
            }
            else if (collision.CompareTag("Hitbox"))
            {
                var m_cacheToDamage = collision.GetComponent<Hitbox>();
                if (m_cacheToDamage != null && m_cacheToDamage.isInvulnerable == false)
                {
                    var damage = m_data.damage;
                    for (int i = 0; i < damage.Length; i++)
                    {
                        AttackerCombatInfo info = new AttackerCombatInfo(transform.position, 0, 1, damage[i]);
                        var targetInfo = new TargetInfo(m_cacheToDamage.damageable, m_cacheToDamage.defense.damageReduction);
                        var result = GameplaySystem.combatManager.ResolveConflict(info, targetInfo);
                        CallAttackerAttacked(new CombatConclusionEventArgs(info, targetInfo, result));
                    }
                }
                if (m_data.isPiercing == false)
                {
                    m_collidedWithEnvironment = false;
                    Collide();
                }
            }
        }

        private void OnValidate()
        {
            if (m_data != null)
            {
                var physics = GetComponent<IsolatedPhysics2D>();
                if (physics.simulateGravity != !m_data.hasConstantSpeed)
                {
                    physics.simulateGravity = !m_data.hasConstantSpeed;
                }
            }
        }
    }
}
