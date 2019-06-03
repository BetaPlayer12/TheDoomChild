using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    public class SpawnCircle : SpawnArea
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_radius;

        public override Vector2 GetRandomPosition()
        {
            var offset = m_radius * new Vector2(RandomNormal(), RandomNormal());
            return (Vector2)transform.position + offset;
        }

#if UNITY_EDITOR
        public float radius => m_radius;
#endif
    }
}