using DChild.Gameplay.Characters.Players.Attributes;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class PlayerAttributes : IAttributes
    {
        [SerializeField, MinValue(1), Indent, OnValueChanged("VitalityChange")]
        private int m_vitality;
        [SerializeField, MinValue(1), Indent, OnValueChanged("IntelligenceChange")]
        private int m_intelligence;
        [SerializeField, MinValue(1), Indent, OnValueChanged("StrengthChange")]
        private int m_strength;
        [SerializeField, MinValue(1), Indent, OnValueChanged("LuckChange")]
        private int m_luck;

        public event EventAction<AttributeValueEventArgs> ValueChange;

        public void AddValue(PlayerAttribute attribute, int value)
        {
            switch (attribute)
            {
                case PlayerAttribute.Vitality:
                    m_vitality += value;
                    ValueChange?.Invoke(this, new AttributeValueEventArgs(attribute, m_vitality));
                    break;
                case PlayerAttribute.Intelligence:
                    m_intelligence += value;
                    ValueChange?.Invoke(this, new AttributeValueEventArgs(attribute, m_intelligence));
                    break;
                case PlayerAttribute.Strength:
                    m_strength += value;
                    ValueChange?.Invoke(this, new AttributeValueEventArgs(attribute, m_strength));
                    break;
                case PlayerAttribute.Luck:
                    m_luck += value;
                    ValueChange?.Invoke(this, new AttributeValueEventArgs(attribute, m_luck));
                    break;
            }

        }

        public int GetValue(PlayerAttribute attribute)
        {
            switch (attribute)
            {
                case PlayerAttribute.Vitality:
                    return m_vitality;
                case PlayerAttribute.Intelligence:
                    return m_intelligence;
                case PlayerAttribute.Strength:
                    return m_strength;
                case PlayerAttribute.Luck:
                    return m_luck;
                default:
                    return 0;
            }
        }

        public void SetValue(PlayerAttribute attribute, int value)
        {
            switch (attribute)
            {
                case PlayerAttribute.Vitality:
                    m_vitality = value;
                    break;
                case PlayerAttribute.Intelligence:
                    m_intelligence = value;
                    break;
                case PlayerAttribute.Strength:
                    m_strength = value;
                    break;
                case PlayerAttribute.Luck:
                    m_luck = value;
                    break;
            }
            ValueChange?.Invoke(this, new AttributeValueEventArgs(attribute, value));
        }

#if UNITY_EDITOR
        private void VitalityChange() => ValueChange?.Invoke(this, new AttributeValueEventArgs(PlayerAttribute.Vitality, m_vitality));
        private void IntelligenceChange() => ValueChange?.Invoke(this, new AttributeValueEventArgs(PlayerAttribute.Intelligence, m_intelligence));
        private void StrengthChange() => ValueChange?.Invoke(this, new AttributeValueEventArgs(PlayerAttribute.Strength, m_strength));
        private void LuckChange() => ValueChange?.Invoke(this, new AttributeValueEventArgs(PlayerAttribute.Luck, m_luck));
#endif
    }
}