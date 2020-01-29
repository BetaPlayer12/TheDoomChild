using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Systems;
<<<<<<< HEAD
using Holysoft;
using Holysoft.Collections;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
=======
using Holysoft.Event;
using Sirenix.OdinInspector;
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
using UnityEngine;

namespace DChild.Gameplay.SoulEssence
{
    public class SoulEssenceLoot : Loot
    {
        [SerializeField, MinValue(0.1f)]
        private float m_pickUpVelocity;
        [SerializeField, Min(1)]
        private int m_value;
<<<<<<< HEAD
        private Collider2D m_collider;
=======
        [SerializeField]
        private float m_floatGravity;

        protected bool m_isFloating;
        private float m_gravity;
        private bool m_hasBeenPickUp;

        public override void PickUp(IPlayer player)
        {
            base.PickUp(player);
            m_hasBeenPickUp = true;
        }

        public override void SpawnAt(Vector2 position, Quaternion rotation)
        {
            base.SpawnAt(position, rotation);
            m_isFloating = false;
            m_rigidbody.gravityScale = m_gravity;
            m_hasBeenPickUp = false;
        }

        protected override void ExecutePop(float delta)
        {
            base.ExecutePop(delta);
            if (m_isFloating == false)
            {
                if (m_rigidbody.velocity.magnitude <= 1)
                {
                    m_isFloating = true;
                    m_rigidbody.gravityScale = m_floatGravity;
                }
            }
        }
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23

        public int value => m_value;

        public void DisableEnvironmentCollider() => m_collider.isTrigger = true;
        public void EnableEnvironmentCollider() => m_collider.isTrigger = false;

        public override void PickUp(IPlayer player)
        {
            player.inventory.AddSoulEssence(m_value);
            CallPoolRequest();
        }

<<<<<<< HEAD
        public override void SpawnAt(Vector2 position, Quaternion rotation)
        {
            base.SpawnAt(position, rotation);
            EnableEnvironmentCollider();
            m_collider.isTrigger = false;
        }

        private void Awake()
        {
            m_collider = GetComponentInChildren<Collider2D>();
=======
        protected override void OnPopDurationEnd(object sender, EventActionArgs eventArgs)
        {
            base.OnPopDurationEnd(sender, eventArgs);
            if (m_isPopping == false && m_hasBeenPickUp)
            {
                m_animator?.SetBool("PickedUp", true);
            }
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.gravityScale = 0;
        }

        protected override void Awake()
        {
            base.Awake();
            m_gravity = m_rigidbody.gravityScale;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (m_isPopping == false && m_hasBeenPickUp)
            {
                var toPLayer = (m_pickedBy.damageableModule.position - m_rigidbody.position).normalized;
                m_rigidbody.velocity = toPLayer * m_pickUpVelocity;
            }
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
        }
    }
}