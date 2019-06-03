using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    [RequireComponent(typeof(DamagingExplosion))]
    public abstract class ExplodingObject : DestructableObject, IFragileObject
    {
        [SerializeField]
        private GameObject m_explosionFX;
        private DamagingExplosion m_explosion;

        public override Vector2 position => transform.position;

        public override IAttackResistance attackResistance => null;

        public void Break()
        {
            Explode();
        }

        public override void Heal(int health)
        {
        }

        public override void TakeDamage(int totalDamage, AttackType type)
        {
            Explode();
        }

        private void Explode()
        {
            m_explosion.Explode();
            GameplaySystem.fXManager.InstantiateFX(m_explosionFX, transform.position);
            Destroy(gameObject);
        }

        protected virtual void Start()
        {
            m_explosion = GetComponent<DamagingExplosion>();
        }
    }
}