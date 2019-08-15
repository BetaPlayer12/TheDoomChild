using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Systems;
using Holysoft;
using Holysoft.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.SoulEssence
{
    public class SoulEssenceLoot : Loot
    {
#if UNITY_EDITOR
        [SerializeField, Min(1)]
#endif
        private int m_value;
        private Collider2D m_collider;

        [SerializeField, HideInInspector]
        private Rigidbody2D m_rigidbody;
        [SerializeField]
        private RangeFloat m_popVelocityX;
        [SerializeField]
        private RangeFloat m_popVelocityY;


        public int value
        {
            get => m_value; set
            {
                m_value = Mathf.Max(value, 1);
            }
        }

        public void DisableEnvironmentCollider() => m_collider.isTrigger = true;
        public void EnableEnvironmentCollider() => m_collider.isTrigger = false;

        public override void PickUp(IPlayer player)
        {
            player.inventory.AddSoulEssence(m_value);
            CallPoolRequest();
        }

        public override void SpawnAt(Vector2 position, Quaternion rotation)
        {
            base.SpawnAt(position, rotation);
            EnableEnvironmentCollider();
            var xVelocity = transform.right * MathfExt.RandomSign() * m_popVelocityX.GenerateRandomValue();
            var yVelocity = transform.up * m_popVelocityY.GenerateRandomValue();
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.AddForce(xVelocity + yVelocity, ForceMode2D.Impulse);
            m_collider.isTrigger = false;
        }

        private void Awake()
        {
            m_collider = GetComponentInChildren<Collider2D>();
        }

        private void OnValidate()
        {
            ComponentUtility.AssignNullComponent(this, ref m_rigidbody);
        }
    }
}