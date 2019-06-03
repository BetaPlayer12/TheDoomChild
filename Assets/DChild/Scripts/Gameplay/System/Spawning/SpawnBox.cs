using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay
{
    public class SpawnBox : SpawnArea
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_widthExtent;
        [SerializeField]
        [MinValue(0f)]
        private float m_lengthExtent;

        public override Vector2 GetRandomPosition()
        {
            var widthOffset = m_widthExtent * RandomNormal();
            var lengthOffset = m_lengthExtent * RandomNormal();

            return (Vector2)transform.position + new Vector2(widthOffset, lengthOffset);
        }

        

#if UNITY_EDITOR
        public float widthExtent => m_widthExtent;
        public float lengthExtent => m_lengthExtent;
#endif
    }
}