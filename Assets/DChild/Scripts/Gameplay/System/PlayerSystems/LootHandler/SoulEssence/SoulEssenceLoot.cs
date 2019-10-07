using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Systems;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.SoulEssence
{
    public class SoulEssenceLoot : Loot
    {
        [SerializeField, Min(1)]
        private int m_value;
        [SerializeField]
        private float m_floatGravity;

        protected bool m_isFloating;
        private float m_gravity;

        public override void SpawnAt(Vector2 position, Quaternion rotation)
        {
            base.SpawnAt(position, rotation);
            m_isFloating = false;
            m_rigidbody.gravityScale = m_gravity;
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

        protected override void ApplyPickUp()
        {
            base.ApplyPickUp();
            m_pickedBy.inventory.AddSoulEssence(m_value);
            //CallPoolRequest();
        }

        protected override void OnPopDurationEnd(object sender, EventActionArgs eventArgs)
        {
            base.OnPopDurationEnd(sender, eventArgs);
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.gravityScale = 0;
        }

        protected override void Awake()
        {
            base.Awake();
            m_gravity = m_rigidbody.gravityScale;
        }
    }
}