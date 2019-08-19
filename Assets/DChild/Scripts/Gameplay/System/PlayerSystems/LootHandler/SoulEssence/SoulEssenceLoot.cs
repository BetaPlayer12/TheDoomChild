using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Systems;
using Holysoft;
using Holysoft.Collections;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.SoulEssence
{
    public class SoulEssenceLoot : Loot
    {
        [SerializeField, Min(1)]
        private int m_value;
        private Collider2D m_collider;

        [SerializeField]
        private Rigidbody2D m_rigidbody;
        [SerializeField]
        private RangeFloat m_popVelocityX;
        [SerializeField]
        private RangeFloat m_popVelocityY;

        public int value => m_value;

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
    }
}