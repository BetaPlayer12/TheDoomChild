using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class DPSZone : MonoBehaviour
    {
        private DPSObstacle m_obstacle;

        private void Awake()
        {
            m_obstacle = GetComponentInParent<DPSObstacle>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var hitbox = GetComponent<Hitbox>();
            if(hitbox != null){
                m_obstacle.Add(hitbox.damageable);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var hitbox = GetComponent<Hitbox>();
            if (hitbox != null)
            {
                m_obstacle.Remove(hitbox.damageable);
            }
        }
    }
}

