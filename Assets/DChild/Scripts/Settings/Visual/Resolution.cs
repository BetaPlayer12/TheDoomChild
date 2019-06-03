using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Configurations.Visuals
{
    [System.Serializable]
    public struct Resolution
    {
        [SerializeField]
        [MinValue(0f)]
        private int m_width;
        [SerializeField]
        [MinValue(0f)]
        private int m_height;
        [SerializeField]
        [MinValue(0f)]
        private int m_widthAspect;
        [SerializeField]
        [MinValue(0f)]
        private int m_heightAspect;

        public int width => m_width;
        public int height => m_height;
        public int widthAspect => m_widthAspect;
        public int heightAspect => m_heightAspect;

    }
}
