/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [AddComponentMenu("DChild/Gameplay/Combat/DPS Collider Damage")]
    public class DPSColliderDamage : ColliderDamage
    {
        private struct Info
        {
            public float damageTimer;
            public bool isOutOfCollider;

            public Info(float damageTimer) : this()
            {
                this.damageTimer = damageTimer;
                isOutOfCollider = false;
            }
        }

        [SerializeField]
        private float m_damageInterval;

        private List<Collider2D> m_affectedColliders;
        private List<Hitbox> m_toDamage;
        private List<Info> m_infos;

        protected override bool IsValidColliderToHit(Collider2D collision) => true;
        protected override bool IsValidHitboxToHit(Collider2D collider2D, Hitbox hitbox) => hitbox.CanBeDamageBy(m_colliders);
        protected override void HandleDamageUniqueHitboxes(Collider2D collider2D)
        {
            var hitbox = m_collisionRegistrator.GetHitbox(collider2D);
            if (hitbox != null)
            {
                if (IsValidHitboxToHit(collider2D, hitbox))
                {
                    OnValidCollider(collider2D, hitbox);

                }
            }
        }

        protected override void OnValidCollider(Collider2D collision, Hitbox hitbox)
        {
            if (m_affectedColliders.Contains(collision))
            {
                var index = m_affectedColliders.FindIndex(x => x == collision);
                var info = m_infos[index];
                info.isOutOfCollider = false;
                m_infos[index] = info;
            }
            else
            {
                m_affectedColliders.Add(collision);
                m_toDamage.Add(hitbox);
                m_infos.Add(new Info(m_damageInterval));
            }
            base.OnValidCollider(collision, hitbox);
            if(m_damageUniqueHitboxesOnly)
                m_collisionRegistrator.RegisterHitboxAs(hitbox, true);
        }

        private void RemoveAffectedIndex(int i)
        {
            m_toDamage.RemoveAt(i);
            m_affectedColliders.RemoveAt(i);
            m_infos.RemoveAt(i);
        }

        private void ChangeHitboxRegisterationStatus(Hitbox hitbox, bool hasHit)
        {
            if (m_damageUniqueHitboxesOnly)
            {
                m_collisionRegistrator.RegisterHitboxAs(hitbox, hasHit);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_affectedColliders = new List<Collider2D>();
            m_toDamage = new List<Hitbox>();
            m_infos = new List<Info>();
        }

        private void LateUpdate()
        {
            for (int i = m_infos.Count - 1; i >= 0; i--)
            {
                var info = m_infos[i];
                info.damageTimer -= GameplaySystem.time.deltaTime;
                if (info.damageTimer <= 0)
                {
                    var toDamage = m_toDamage[i];
                    if (info.isOutOfCollider)
                    {
                        RemoveAffectedIndex(i);
                        ChangeHitboxRegisterationStatus(toDamage, false);
                    }
                    else
                    {
                        info.damageTimer = m_damageInterval;

                        if (CanBypassHitboxInvulnerability(toDamage))
                        {
                            var collision = m_affectedColliders[i];
                            ChangeHitboxRegisterationStatus(toDamage, false);
                            DealDamage(collision, toDamage);
                            if (toDamage.damageable.isAlive == false)
                            {
                                RemoveAffectedIndex(i);
                                ChangeHitboxRegisterationStatus(toDamage, false);
                            }
                        }
                        m_infos[i] = info;
                    }
                }
                else
                {
                    m_infos[i] = info;
                }
            }
        }



        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_affectedColliders.Contains(collision))
            {
                var index = m_affectedColliders.FindIndex(x => x == collision);
                var info = m_infos[index];
                info.isOutOfCollider = true;
                m_infos[index] = info;
            }
        }
    }

}
