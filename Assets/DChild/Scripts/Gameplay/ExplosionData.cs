using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    [CreateAssetMenu(fileName = "ExplosionData", menuName ="DChild/Gameplay/Explosion Data")]
    public class ExplosionData : ScriptableObject
    {
        [SerializeField, MinValue(1f), Tooltip("Radius on how far objects react to the explosion")]
        private float m_explosiveRadius;
        [SerializeField]
        private float m_explosivePower;

        public float explosiveRadius => m_explosiveRadius;
        public float explosivePower => m_explosivePower;
    }
}
