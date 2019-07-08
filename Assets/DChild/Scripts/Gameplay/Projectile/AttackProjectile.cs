
using DChild.Gameplay.Combat;
using Holysoft.Pooling;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    [CreateAssetMenu(fileName = "AttackProjectileData", menuName = "DChild/Gameplay/Attack Projectile Data")]
    public class AttackProjectileData : ProjectileData
    {
        [SerializeField]
        private bool m_canPassThroughEnvironment;
        [SerializeField]
        private bool m_isPiercing;
       

        public bool canPassThroughEnvironment => m_canPassThroughEnvironment;
        public bool isPiercing => m_isPiercing;  
    }

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
                if (m_cacheToDamage != null)
                {
                    var damage = m_data.damage;
                    for (int i = 0; i < damage.Length; i++)
                    {
                        AttackInfo info = new AttackInfo(transform.position, 0, 1, damage[i]);
                        var result = GameplaySystem.combatManager.ResolveConflict(info, new TargetInfo(m_cacheToDamage.damageable, m_cacheToDamage.defense.damageReduction));
                        CallAttackerAttacked(new CombatConclusionEventArgs(info, m_cacheToDamage.damageable, result));
                    }
                }
                if (m_data.isPiercing == false)
                {
                    m_collidedWithEnvironment = false;
                    Collide();
                }
            }
        }
    }
}
