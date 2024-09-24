
using DChild.Gameplay.Combat;
using Holysoft.Pooling;
using Sirenix.Utilities;
using System;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public abstract class AttackProjectile : Projectile
    {
        [SerializeField]
        private AttackProjectileData m_data;

        protected bool m_collidedWithEnvironment;
        private CollisionRegistrator m_collisionRegistrator;
        private ProjectileCraterHandle m_craterHandle;
        private static Hitbox m_cacheToDamage;

        protected override ProjectileData projectileData => m_data;
        public override void ForceCollision()
        {
            Collide();
        }

        public override void ResetState()
        {
            base.ResetState();
            m_collidedWithEnvironment = false;
            m_collisionRegistrator?.ClearCache();
        }

        protected virtual void Collide()
        {
            if (m_data != null)
            {
                GameplaySystem.cinema.ExecuteCameraShake(m_data.onCollideShake);
            }
        }

        private void OnProjectileDealDamage(object sender, CombatConclusionEventArgs eventArgs)
        {
            CallAttackerAttacked(eventArgs);
            if (m_data.isPiercing == false)
            {
                m_collidedWithEnvironment = false;
                Collide();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            var attacker = GetComponent<Attacker>();
            attacker.TargetDamaged += OnProjectileDealDamage;
            attacker.SetDamage(projectileData.damage);
            if (m_data.impactCrater)
            {
                m_craterHandle = new ProjectileCraterHandle(m_data.impactCrater, GetComponent<Rigidbody2D>(), GetComponentInChildren<Collider2D>());
            }
        }

        protected virtual void FixedUpdate()
        {
            if (m_data.willFaceVelocity)
            {
                var velocity = m_physics.velocity;
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
                if (m_data.isGroundProjectile)
                {
                    var velocityXSign = Mathf.Sign(velocity.x);
                    var localScale = transform.localScale;
                    var localScaleXSign = Mathf.Sign(localScale.x);
                    if (velocityXSign != localScaleXSign)
                    {
                        localScale.x *= -1f;
                        transform.localScale = localScale;
                    }

                    if (velocityXSign < 0)
                    {
                        angle += 180f; //To Align the rotation when flip
                    }
                }
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }


        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Sensor") == false)
            {
                if (DChildUtility.IsAnEnvironmentLayerObject(collision.gameObject) || LayerMask.LayerToName(collision.gameObject.layer) == "Default") //Default is for QueenBee Quickfix
                {
                    if (collision.CompareTag("Droppable"))
                    {
                        if (m_data.canPassThroughDroppables == false)
                        {
                            m_collidedWithEnvironment = true;
                            Collide();
                            m_craterHandle?.GenerateCrater();
                        }
                    }
                    else
                    {
                        if (m_data.canPassThroughEnvironment == false)
                        {
                            m_collidedWithEnvironment = true;
                            Collide();
                            m_craterHandle?.GenerateCrater();
                        }
                    }
                }
            }
        }

        private void OnValidate()
        {
            if (m_data != null)
            {
                var physics = GetComponent<IsolatedPhysics2D>();
                if (physics.simulateGravity != !m_data.hasConstantSpeed)
                {
                    physics.simulateGravity = !m_data.hasConstantSpeed;
                }
            }
        }
    }
}
