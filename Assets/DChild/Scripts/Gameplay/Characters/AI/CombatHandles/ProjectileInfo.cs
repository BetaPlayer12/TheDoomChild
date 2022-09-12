using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters
{
    [System.Serializable]
    public class ProjectileInfo
    {
        [SerializeField]
        private GameObject m_projectile;
        [SerializeField, MinValue(0.001f)]
        private float m_speed;

        public GameObject projectile => m_projectile;
        public float speed => m_speed;
    }
}