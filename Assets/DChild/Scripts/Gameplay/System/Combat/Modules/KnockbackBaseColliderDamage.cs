/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [AddComponentMenu("DChild/Gameplay/Combat/Knockback Base Collider Damage")]
    public class KnockbackBaseColliderDamage : BaseColliderDamage
    {
        [SerializeField]
        private Vector2 m_knockBackForce;

        protected override void OnValidCollider(Collider2D collision, Hitbox hitbox)
        {
            base.OnValidCollider(collision, hitbox);
            var rigidBody = hitbox.GetComponentInParent<Rigidbody2D>();
            var directionToRigidBody = ((Vector2)rigidBody.transform.position - m_damageDealer.position).normalized;
            var knockBackForce = m_knockBackForce;
            knockBackForce.x *= Mathf.Sign(directionToRigidBody.x);
            rigidBody.velocity += knockBackForce;
        }
    }
}