using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public abstract class Loot : PoolableObject
    {
        [SerializeField, MinValue(0.1f), BoxGroup("Basic Loot Info")]
        private float m_pickUpVelocity;
        [SerializeField, BoxGroup("Basic Loot Info")]
        protected Rigidbody2D m_rigidbody;
        [SerializeField, BoxGroup("Basic Loot Info")]
        private Animator m_animator;
        private float m_originalDrag;
        private bool m_isPopping;
        private bool m_hasBeenPickUp;

        public static string objectTag => "Loot";
        private Collider2D m_collider;
        protected IPlayer m_pickedBy;

        public void DisableEnvironmentCollider() => m_collider.isTrigger = true;
        public void EnableEnvironmentCollider() => m_collider.isTrigger = false;

        public virtual void PickUp(IPlayer player)
        {
            m_pickedBy = player;
            m_hasBeenPickUp = true;
            DisableEnvironmentCollider();
            m_animator.SetBool("PickedUp", true);
        }

        protected virtual void ApplyPickUp()
        {
            m_animator.SetTrigger("Apply");
        }

        public void Pop(Vector2 force)
        {
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        public override void SpawnAt(Vector2 position, Quaternion rotation)
        {
            base.SpawnAt(position, rotation);
            m_rigidbody.drag = m_originalDrag;
            EnableEnvironmentCollider();
            m_animator.SetBool("PickedUp", false);
            m_hasBeenPickUp = false;
            m_isPopping = true;
            m_pickedBy = null;
        }

        protected virtual void Awake()
        {
            m_originalDrag = m_rigidbody.drag;
            m_collider = GetComponentInChildren<Collider2D>();
        }

        protected virtual void FixedUpdate()
        {
            if (m_isPopping)
            {
                if (m_rigidbody.velocity.magnitude <= 1)
                {
                    m_rigidbody.drag = 0;
                    m_isPopping = false;
                }
            }
            else if (m_hasBeenPickUp)
            {
                var toPLayer = (m_pickedBy.damageableModule.position - m_rigidbody.position).normalized;
                m_rigidbody.velocity = toPLayer * m_pickUpVelocity;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Sensor" && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                ApplyPickUp();
                CallPoolRequest();
                m_pickedBy = null;
            }
        }
    }
}