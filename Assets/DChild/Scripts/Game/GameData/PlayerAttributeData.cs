using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Serialization
{
    [System.Serializable,HideLabel,Title("Attribute Data")]
    public struct PlayerAttributeData
    {
        [SerializeField,MinValue(0)]
        private int m_vitality;
        [SerializeField, MinValue(0)]
        private int m_intelligence;
        [SerializeField, MinValue(0)]
        private int m_strength;
        [SerializeField, MinValue(0)]
        private int m_luck;

        public int vitality { get => m_vitality; }
        public int intelligence { get => m_intelligence; }
        public int strength { get => m_strength; }
        public int luck { get => m_luck; }
    }

}