using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [System.Serializable]
    [AddComponentMenu("DChild/Gameplay/Combat/Basic Attack Resistance")]
    public class BasicAttackResistance : AttackResistance
    {
        [SerializeField]
        private AttackResistanceData m_data;
        [ShowInInspector, HideInEditorMode, HideReferenceObjectPicker, PropertyOrder(2), OnValueChanged("SendEvent", true)]
        protected Dictionary<AttackType, float> m_resistance;

        protected override Dictionary<AttackType, float> resistance => m_resistance;

        public override void SetData(AttackResistanceData data)
        {
            m_data = data;
            if (m_resistance != null)
            {
                m_resistance.Clear();
                if (m_data != null)
                {
                    Copy(m_data.resistance, m_resistance);
                }
            }
        }

        public override void ClearResistance()
        {
            m_resistance.Clear();
        }

        public void SetResistance(AttackType type, AttackResistanceType resistanceType) => SetResistance(type, ConvertToFloat(resistanceType));

        public override void SetResistance(AttackType type, float resistanceValue)
        {
            SetResistance(m_resistance, type, resistanceValue);
            CallResistanceChange(new ResistanceEventArgs(type, resistanceValue));
        }

        private void Copy(Dictionary<AttackType, float> source, Dictionary<AttackType, float> destination)
        {
            destination.Clear();
            foreach (var key in source.Keys)
            {
                destination.Add(key, source[key]);
            }
        }

        private void Awake()
        {
            m_resistance = new Dictionary<AttackType, float>();
            if (m_data != null)
            {
                Copy(m_data.resistance, m_resistance);
            }
        }

#if UNITY_EDITOR
        private void SendEvent()
        {
            CallResistanceChange(new ResistanceEventArgs(AttackType._COUNT, 0));
        }

      
#endif
    }
}