using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [CreateAssetMenu(fileName = "AttackResistanceData", menuName = "DChild/Gameplay/Combat/Attack Resistance Data")]
    public class AttackResistanceData : SerializedScriptableObject
    {
        [PropertySpace]
        [OdinSerialize, HideReferenceObjectPicker, PropertyOrder(2), ReadOnly]
        private Dictionary<DamageType, float> m_resistance = new Dictionary<DamageType, float>();
        public Dictionary<DamageType, float> resistance => m_resistance;

        private void OnEnable()
        {
#if UNITY_EDITOR
            CopyResistance();
#endif
        }

#if UNITY_EDITOR
        [System.Serializable]
        public struct Info
        {
            [ShowInInspector, MinValue(-1), MaxValue(2), OnValueChanged("UpdateData")]
            private float m_value;
            [ShowInInspector, ReadOnly]
            private AttackResistanceType m_type;

            public Info(AttackResistanceType m_type) : this()
            {
                this.m_type = m_type;
                m_value = ConvertToFloat(m_type);
            }

            public Info(float m_value) : this()
            {
                this.m_value = m_value;
                m_type = GetClosestType(m_value);
            }

            public float value
            {
                get => m_value; set
                {
                    m_value = value;
                    m_type = GetClosestType(m_value);
                }
            }

            public AttackResistanceType type => m_type;
        }

        [SerializeField, OnValueChanged("UpdateResistanceLists"), FoldoutGroup("Edit Section")]
        private bool m_useType;

        [ShowInInspector, HideReferenceObjectPicker, PropertyOrder(1), ShowIf("m_useType"), OnValueChanged("Validate", true), FoldoutGroup("Edit Section")]
        protected Dictionary<DamageType, AttackResistanceType> m_resistantType = new Dictionary<DamageType, AttackResistanceType>();
        [ShowInInspector, HideReferenceObjectPicker, PropertyOrder(1), HideIf("m_useType"), OnValueChanged("Validate", true)]
        protected Dictionary<DamageType, Info> m_resistantInfo = new Dictionary<DamageType, Info>();

        private void Validate()
        {
            m_resistance.Clear();
            if (m_useType)
            {
                foreach (var key in m_resistantType.Keys)
                {
                    var type = m_resistantType[key];
                    if (type == AttackResistanceType.None)
                    {
                        m_resistantType.Remove(key);
                    }
                    else
                    {
                        m_resistance.Add(key, ConvertToFloat(m_resistantType[key]));
                    }
                }
            }
            else
            {
                foreach (var key in m_resistantInfo.Keys)
                {
                    var info = m_resistantInfo[key];
                    if (info.type == AttackResistanceType.None)
                    {
                        m_resistantInfo.Remove(key);
                    }
                    else
                    {
                        m_resistance.Add(key, info.value);
                    }
                }
            }
        }

        private void UpdateResistanceLists()
        {
            if (m_useType)
            {
                m_resistantType.Clear();
                foreach (var key in m_resistantInfo.Keys)
                {
                    m_resistantType.Add(key, m_resistantInfo[key].type);
                }
            }
            else
            {
                m_resistantInfo.Clear();
                foreach (var key in m_resistantType.Keys)
                {
                    m_resistantInfo.Add(key, new Info(m_resistantType[key]));
                }
            }
            Validate();
        }

        private void CopyResistance()
        {
            m_resistantInfo.Clear();
            m_resistantType.Clear();
            foreach (var key in m_resistance.Keys)
            {
                var value = m_resistance[key];
                m_resistantInfo.Add(key, new Info(value));
                m_resistantType.Add(key, GetClosestType(value));
            }
        }

        protected static AttackResistanceType GetClosestType(float value)
        {
            if (value > 1)
            {
                return AttackResistanceType.Absorb;
            }
            else if (value == 0f)
            {
                return AttackResistanceType.None;
            }
            else if (value == 1f)
            {
                return AttackResistanceType.Immune;
            }
            else if (value < 0)
            {
                return AttackResistanceType.Weak;
            }
            else
            {
                return AttackResistanceType.Strong;
            }
        }

        protected static float ConvertToFloat(AttackResistanceType type)
        {
            switch (type)
            {
                case AttackResistanceType.None:
                    return 0;
                case AttackResistanceType.Weak:
                    return -1;
                case AttackResistanceType.Strong:
                    return 0.5f;
                case AttackResistanceType.Immune:
                    return 1;
                case AttackResistanceType.Absorb:
                    return 2;
                default:
                    return 1;
            }
        }
#endif

    }
}