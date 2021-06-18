using UnityEngine;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.AI;

namespace DChild.Gameplay.Combat
{
    [RequireComponent(typeof(ColliderDamage))]
    public class KnockBackCollider : MonoBehaviour
    {
        private enum Type
        {
            AwayFromCenter,
            ToOneDirection,
        }

        [SerializeField]
        private bool m_isEnabled = true;
        [SerializeField, MinValue(0)]
        private float m_force;
        [SerializeField, MinValue(0)]
        private float m_duration;
        [SerializeField]
        private Type m_type;

        [SerializeField, ShowIf("@m_type == Type.AwayFromCenter")]
        private Vector2 m_centerOffset;
        [SerializeField, ShowIf("@m_type == Type.AwayFromCenter")]
        private bool m_affectX = true;
        [SerializeField, ShowIf("@m_type == Type.AwayFromCenter")]
        private bool m_affectY = true;

        [SerializeField, ShowIf("@m_type == Type.ToOneDirection"), Wrap(-1f, 1f)]
        private Vector2 m_direction;

        private Vector2 center => (Vector2)transform.position + m_centerOffset;
        private Vector2 relativeDirection
        {
            get
            {
                var scale = transform.lossyScale;
                var relativeDirection = m_direction;
                relativeDirection.x *= Mathf.Sign(scale.x);
                relativeDirection.y *= Mathf.Sign(scale.y);
                return relativeDirection;
            }
        }

        public void SetForce(float force)
        {
            m_force = Mathf.Max(0, force);
        }

        public void SetDuration(float duration)
        {
            m_duration = Mathf.Max(0, duration);
        }

        private void OnDamageableDetected(TargetInfo arg1, Collider2D arg2)
        {
            if (arg1.isCharacter)
            {
                var target = arg1.instance.transform;
                if (target.TryGetComponent(out IKnockbackableAI ai))
                {
                    ai.HandleKnockback(m_duration);
                    if (target.TryGetComponent(out IsolatedPhysics2D physics))
                    {
                        Execute(physics, arg1.instance.position);
                    }
                }

            }
        }

        private void Execute(IsolatedPhysics2D physics, Vector3 physicsCenter)
        {
            physics.SetVelocity(Vector2.zero);
            switch (m_type)
            {
                case Type.AwayFromCenter:
                    var direction = (Vector2)physicsCenter - center;
                    if (m_affectX == false)
                    {
                        direction.x = 0;
                    }
                    if (m_affectY == false)
                    {
                        direction.y = 0;
                    }

                    physics.AddForce(direction.normalized * m_force, ForceMode2D.Impulse);
                    break;
                case Type.ToOneDirection:
                    physics.AddForce(relativeDirection * m_force, ForceMode2D.Impulse);
                    break;
            }
        }

        private void Start()
        {
            GetComponent<ColliderDamage>().DamageableDetected += OnDamageableDetected;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            switch (m_type)
            {
                case Type.AwayFromCenter:
                    Gizmos.DrawSphere(center, 0.5f);
                    break;
                case Type.ToOneDirection:
                    var position = (Vector2)transform.position;
                    var lineLength = 4f;
                    var lineEndPos = position + (relativeDirection * lineLength);
                    Gizmos.DrawSphere(position, 0.25f);
                    Gizmos.DrawLine(position, lineEndPos);
                    break;
            }
        }
    }
}