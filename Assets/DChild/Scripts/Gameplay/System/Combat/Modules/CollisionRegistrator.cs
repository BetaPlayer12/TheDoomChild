/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using UnityEngine;
using System.Collections.Generic;
using Holysoft.Event;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Combat
{
    public class CollisionRegistrator : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        private Dictionary<Hitbox, bool> m_hasHitPair;
        [ShowInInspector, ReadOnly]
        private Dictionary<Collider2D, Hitbox> m_colliderPair;
        private Dictionary<IDamageable, Hitbox> m_damageablePair;
        private Dictionary<Hitbox, List<Collider2D>> m_hitboxToColliderPair;

        public bool HasDamagedHitbox(Hitbox hitbox) => m_hasHitPair.ContainsKey(hitbox) ? m_hasHitPair[hitbox] : false;
        public void RegisterHitboxAs(Hitbox hitbox, bool hasHit)
        {
            if (m_hasHitPair.ContainsKey(hitbox))
            {
                m_hasHitPair[hitbox] = hasHit;
            }
            else
            {
                m_hasHitPair.Add(hitbox, hasHit);
            }
        }

        public Hitbox GetHitbox(Collider2D collider)
        {
            if (m_colliderPair.ContainsKey(collider))
            {
                return m_colliderPair[collider];
            }
            else
            {
                var hitbox = collider.GetComponentInParent<Hitbox>();
                m_colliderPair.Add(collider, hitbox); // include null
                if (hitbox != null)
                {
                    if (m_hitboxToColliderPair.ContainsKey(hitbox) == false)
                    {
                        m_hitboxToColliderPair.Add(hitbox, new List<Collider2D>());
                        m_damageablePair.Add(hitbox.damageable, hitbox);
                        hitbox.damageable.Destroyed += OnHitboxDestroyed;
                    }
                    m_hitboxToColliderPair[hitbox].Add(collider);
                }
                return hitbox;
            }
        }

        private void OnHitboxDestroyed(object sender, EventActionArgs eventArgs)
        {
            var damageable = (IDamageable)sender;
            damageable.Destroyed -= OnHitboxDestroyed;
            var hitbox = m_damageablePair[damageable];
            var colliderList = m_hitboxToColliderPair[hitbox];
            for (int i = 0; i < colliderList.Count; i++)
            {
                m_colliderPair.Remove(colliderList[i]);
            }
            m_hasHitPair.Remove(hitbox);
            m_hitboxToColliderPair.Remove(hitbox);
            m_damageablePair.Remove(damageable);
        }

        public void ResetHitCache() => m_hasHitPair.Clear();

        public void ClearCache()
        {
            m_hasHitPair.Clear();
            m_colliderPair.Clear();
            m_damageablePair.Clear();
            m_hitboxToColliderPair.Clear();
        }

        private void Awake()
        {
            m_hasHitPair = new Dictionary<Hitbox, bool>();
            m_colliderPair = new Dictionary<Collider2D, Hitbox>();
            m_damageablePair = new Dictionary<IDamageable, Hitbox>();
            m_hitboxToColliderPair = new Dictionary<Hitbox, List<Collider2D>>();
        }
    }
}
