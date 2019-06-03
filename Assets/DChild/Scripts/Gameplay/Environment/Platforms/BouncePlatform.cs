using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment.Platforms
{
    public class BouncePlatform : MonoBehaviour
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_power;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.relativeVelocity.y < 0f)
            {
                var physics = collision.gameObject.GetComponentInParent<IsolatedPhysics2D>();
                physics?.SetVelocity(0, m_power);
            }
        }
    }
}