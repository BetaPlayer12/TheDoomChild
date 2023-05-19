using System;
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Pooling;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public abstract class Loot : PoolableObject
    {
        [SerializeField]
        private CountdownTimer m_popTimer;
        [SerializeField, BoxGroup("Basic Loot Info")]
        protected Rigidbody2D m_rigidbody;
        [SerializeField, BoxGroup("Basic Loot Info")]
        protected Animator m_animator;
        private float m_originalDrag;
        protected bool m_isPopping;
        private bool m_hasBeenApplied;

        public static string objectTag => "Loot";
        private Collider2D m_collider;
        protected IPlayer m_pickedBy;

        public void DisableEnvironmentCollider() => m_collider.isTrigger = true;
        public void EnableEnvironmentCollider() => m_collider.isTrigger = false;

        public virtual void PickUp(IPlayer player)
        {
            m_pickedBy = player;
            DisableEnvironmentCollider();
            if (m_isPopping == false)
            {
                m_animator?.SetBool("PickedUp", true);
            }
        }

        protected virtual void ApplyPickUp()
        {
            m_animator?.SetTrigger("Apply");
            m_pickedBy.lootPicker.Glow();
            m_rigidbody.velocity = Vector2.zero;
        }

        public void Pop(Vector2 force)
        {
            m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.AddForce(force, ForceMode2D.Impulse);
            Debug.Log("Popped");
        }

        public override void SpawnAt(Vector2 position, Quaternion rotation)
        {
            base.SpawnAt(position, rotation);
            m_rigidbody.drag = m_originalDrag;
            EnableEnvironmentCollider();
            m_animator?.SetBool("PickedUp", false);
            m_popTimer.Reset();
            m_isPopping = true;
            m_hasBeenApplied = false;
            enabled = true;
            m_pickedBy = null;
        }

        protected virtual void ExecutePop(float delta)
        {
            m_popTimer.Tick(delta);
        }

        protected virtual void OnPopDurationEnd(object sender, EventActionArgs eventArgs)
        {
            m_isPopping = false;
        }

        protected virtual void Awake()
        {
            m_originalDrag = m_rigidbody.drag;
            m_collider = GetComponentInChildren<Collider2D>();
            m_popTimer.CountdownEnd += OnPopDurationEnd;
        }


        protected virtual void FixedUpdate()
        {
            if (m_isPopping)
            {
                ExecutePop(Time.deltaTime);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (m_hasBeenApplied == false && m_isPopping == false)
            {
                if (collision.tag != "Sensor" && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    //CallPoolRequest is in the PickUp Animation
                    ApplyPickUp();
                    m_pickedBy = null;
                    m_hasBeenApplied = true;
                    enabled = false;
                }
            }
        }
    }
}