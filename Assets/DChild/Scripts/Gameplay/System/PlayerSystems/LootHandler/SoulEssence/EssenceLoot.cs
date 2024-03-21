using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Systems;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Essence
{
    public abstract class EssenceLoot : Loot
    {
        [SerializeField]
        private Collider2D m_collision;
        [SerializeField, MinValue(0.1f)]
        private float m_pickUpVelocity;
        [SerializeField]
        private float m_floatGravity;
        [SerializeField, MinValue(0.1f)]
        private float m_floatDuration;
        [SerializeField]
        private float m_fallGravity;
        [SerializeField, MinValue(0.1f)]
        private float m_fadeAfterRestingDuration;

        protected bool m_isFloating;
        protected bool m_hasFallen;
        protected bool m_isFading;
        private float m_gravity;
        private bool m_hasBeenPickUp;
        private float m_timer;
        private bool m_hasReachedCenterMass; // testing , checking if the object has reach the player's center mass
        protected abstract void OnApplyPickup(IPlayer player);

        public override void PickUp(IPlayer player)
        {
            base.PickUp(player);
            m_hasBeenPickUp = true;
            m_collision.isTrigger = true;
        }


        public override void SpawnAt(Vector2 position, Quaternion rotation)
        {
            base.SpawnAt(position, rotation);
            m_isFloating = false;
            m_hasFallen = false;
            m_isFading = false;
            m_rigidbody.gravityScale = m_gravity;
            m_hasBeenPickUp = false;
            m_timer = 0;
            m_collision.enabled = false;
            m_collision.isTrigger = false;
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
            OnApplyPickup(m_pickedBy);
            //CallPoolRequest();
        }


        protected override void OnPopDurationEnd(object sender, EventActionArgs eventArgs)
        {
            base.OnPopDurationEnd(sender, eventArgs);
            m_collision.enabled = true;
            m_collision.isTrigger = true;
            if (m_isPopping == false && m_hasBeenPickUp)
            {
                m_animator?.SetBool("PickedUp", true);
            }
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.gravityScale = 0;
            m_timer = m_floatDuration;
        }

        protected override void Awake()
        {
            base.Awake();
            m_gravity = m_rigidbody.gravityScale;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (m_isPopping == false)
            {
                if (m_hasBeenPickUp)
                {
                    Vector2 toPLayer;
                    if(m_hasReachedCenterMass)
                    {
                        toPLayer = ((Vector2)m_pickedBy.character.GetBodyPart(BodyReference.BodyPart.Feet).position - m_rigidbody.position).normalized; // because the body reference as of now does not move with the model, i am using the body reference feet to make sure the player gets the loot
                    }
                    else
                    {
                        toPLayer = (m_pickedBy.damageableModule.position - m_rigidbody.position).normalized;
                    }

                    //Added to check if the loot has already reached the player's center mass
                    if(Vector2.Distance(transform.position, m_pickedBy.damageableModule.position)<0.5f&&!m_hasReachedCenterMass)
                    {
                        m_hasReachedCenterMass = true;
                        
                        Debug.Log("____1111aaa " + toPLayer);
                    }
                    
                    m_rigidbody.velocity = toPLayer * m_pickUpVelocity;
                }
                else if (m_isFloating)
                {
                    m_timer -= GameplaySystem.time.fixedDeltaTime;
                    if (m_timer <= 0)
                    {
                        m_isFloating = false;
                        m_hasFallen = true;
                        m_rigidbody.gravityScale = m_fallGravity;
                        m_collision.enabled = true;
                    }
                }
                else if (m_isFading)
                {
                    m_timer -= GameplaySystem.time.fixedDeltaTime;
                    if (m_timer <= 0)
                    {
                        CallPoolRequest();
                    }
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_hasFallen && m_isFading == false)
            {
                m_isFading = true;
                m_timer = m_fadeAfterRestingDuration;
            }

        }
    }
}