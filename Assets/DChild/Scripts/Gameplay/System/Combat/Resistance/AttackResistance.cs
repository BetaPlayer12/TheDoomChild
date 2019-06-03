using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public abstract class AttackResistance : SerializedMonoBehaviour, IAttackResistance
    {
        [OdinSerialize, HideReferenceObjectPicker, ReadOnly, PropertyOrder(2)]
        protected Dictionary<AttackType, float> m_resistanceInfo = new Dictionary<AttackType, float>();

        public abstract float GetResistance(AttackType type);

        public void SetResistance(AttackType type, AttackResistanceType resistanceType)
        {
            if (m_resistanceInfo.ContainsKey(type))
            {
                m_resistanceInfo[type] = ConvertToFloat(resistanceType);
            }
            else
            {
                m_resistanceInfo.Add(type, ConvertToFloat(resistanceType));
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

#if UNITY_EDITOR
        [HideLabel]
        public struct ResistanceValue
        {
            [SerializeField, ValueDropdown("GetValue")]
            private float m_value;
            public float value;

            private ValueDropdownItem<float> CreateItem(AttackResistanceType type) => new ValueDropdownItem<float>(type.ToString(), ConvertToFloat(type));

            private IEnumerable GetValue()
            {
                return new ValueDropdownList<float>
                {
                    CreateItem(AttackResistanceType.None),
                    CreateItem(AttackResistanceType.Weak),
                    CreateItem(AttackResistanceType.Strong),
                    CreateItem(AttackResistanceType.Immune),
                    CreateItem(AttackResistanceType.Absorb)
                };
            }
        }

        [OdinSerialize, HideReferenceObjectPicker, PropertyOrder(1)]
        protected Dictionary<AttackType, ResistanceValue> m_resistance = new Dictionary<AttackType, ResistanceValue>();

        [Button, PropertyOrder(0)]
        private void Validate()
        {
            m_resistanceInfo.Clear();
            foreach (var key in m_resistance.Keys)
            {
                m_resistanceInfo.Add(key, m_resistance[key].value);
            }
        }
#endif

        /* Calculation will be::
        Resisted Damage = Damage * ResistanceValue;
        Actual Damage = Damage - ResistedDamage;
    */
    }
}