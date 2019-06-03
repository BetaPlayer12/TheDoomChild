
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public abstract class AttackProjectile : Projectile
    {
        [SerializeField]
        private bool m_canPassThroughEnvironment;
        [SerializeField]
        private bool m_isPiercing;

        protected bool m_collidedWithEnvironment;
        private static Hitbox m_cacheToDamage;
        private ITarget m_toDamage;
        protected abstract void Collide();

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Environment")
            {
                if (m_canPassThroughEnvironment == false)
                {
                    m_collidedWithEnvironment = true;
                    Collide();
                }
            }
            else if (collision.CompareTag("Hitbox"))
            {
                var m_cacheToDamage = collision.GetComponent<Hitbox>();
                if (m_cacheToDamage != null)
                {
                    for (int i = 0; i < m_damage.Length; i++)
                    {
                        AttackInfo info = new AttackInfo(transform.position, 0, 1, m_damage[i]);
                        var result = GameplaySystem.combatManager.ResolveConflict(info, new TargetInfo(m_cacheToDamage.damageable, m_cacheToDamage.defense.damageReduction));
                        CallAttackerAttacked(new CombatConclusionEventArgs(info, m_toDamage, result));
                    }
                }
                if (m_isPiercing == false)
                {
                    m_collidedWithEnvironment = false;
                    Collide();
                }
            }
        }
    }
}
