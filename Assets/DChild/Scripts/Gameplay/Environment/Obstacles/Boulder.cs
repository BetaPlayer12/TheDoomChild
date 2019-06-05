using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Holysoft.Pooling;
using UnityEngine;


namespace DChild.Gameplay.Environment.Obstacles
{

    [RequireComponent(typeof(ObjectPhysics2D))]
    public class Boulder : Obstacle, IDamageable, ISpawnable
    {
        [SerializeField]
        private PoolableItemData m_poolableItemData;

        public PoolableItemData poolableItemData => m_poolableItemData;

        public event EventAction<PoolItemEventArgs> PoolRequest;
        public event EventAction<PoolItemEventArgs> InstanceDestroyed;

        [SerializeField]
        private AttackDamage m_damage;
        [SerializeField]
        private bool m_isFragile;

        [Space]
        [SerializeField]
        private ParticleFX m_breakFX;
        [SerializeField]
        private GameObject m_model;

        private ObjectPhysics2D m_physics;
        private Hitbox m_hitbox;

        public IAttackResistance attackResistance => null;
        protected override AttackDamage damage => m_damage;

        public bool isAlive => false;
        public Vector2 position => transform.position;

        public void SpawnAt(Vector2 position, Quaternion rotation)
        {
            m_physics.Enable();
            m_hitbox.Enable();
            m_model.SetActive(true);
            m_breakFX.Stop();
        }

        public void ForcePool()
        {
            m_physics.Disable();
            m_hitbox.Disable();
            m_model.SetActive(false);
            m_breakFX.Stop();
            PoolRequest?.Invoke(this, new PoolItemEventArgs(this, transform));
        }

        public void DestroyInstance()
        {
            InstanceDestroyed?.Invoke(this, new PoolItemEventArgs(this, transform));
            Destroy(gameObject);
        }

        public override void Damage(ITarget target, BodyDefense targetDefense)
        {
            base.Damage(target, targetDefense);
            Break();
        }

        private void Break()
        {
            m_physics.Disable();
            m_hitbox.Disable();
            m_model.SetActive(false);
            m_breakFX.Play();
            PoolRequest?.Invoke(this, new PoolItemEventArgs(this, transform));
        }

        private void Awake()
        {
            m_physics = GetComponent<ObjectPhysics2D>();
            m_hitbox = GetComponentInChildren<Hitbox>();
            enabled = m_isFragile;
        }

        public void TakeDamage(int totalDamage, AttackType type)
        {
            Break();
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                Break();
            }
        }
    }

}