using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [AddComponentMenu("DChild/Gameplay/Combat/Extended Attack Resistance")]
    public class ExtendedAttackResistance : AttackResistance
    {
        [HorizontalGroup("Split")]

        [OdinSerialize, HideReferenceObjectPicker, OnValueChanged("SendEvent", true), BoxGroup("Split/Base")]
        protected Dictionary<DamageType, float> m_baseResistance = new Dictionary<DamageType, float>();
        [ShowInInspector, HideReferenceObjectPicker, HideInEditorMode, OnValueChanged("SendEvent", true), BoxGroup("Split/Added")]
        private Dictionary<DamageType, float> m_additionalResistance;
        [ShowInInspector, HideReferenceObjectPicker, HideInEditorMode, ReadOnly, BoxGroup("Total")]
        private Dictionary<DamageType, float> m_combinedResistance;

        protected override Dictionary<DamageType, float> resistance => m_combinedResistance;

        public override void SetResistance(DamageType type, float resistanceValue)
        {
            SetResistance(m_baseResistance, type, resistanceValue);
            CalculateResistance();
            CallResistanceChange(new ResistanceEventArgs(type, GetResistance(type)));
        }

        public override void ClearResistance()
        {
            m_baseResistance.Clear();
            m_additionalResistance.Clear();
            m_combinedResistance.Clear();
        }

        public void AddResistance(DamageType type, float resistance)
        {
            if (m_additionalResistance.ContainsKey(type))
            {
                m_additionalResistance[type] += resistance;
            }
            else
            {
                m_additionalResistance.Add(type, resistance);
            }
            CalculateResistance();
            CallResistanceChange(new ResistanceEventArgs(type, GetResistance(type)));
        }

        public void ReduceResistance(DamageType type, float resistance)
        {
            if (m_additionalResistance.ContainsKey(type))
            {
                m_additionalResistance[type] -= resistance;
            }
            else
            {
                m_additionalResistance.Add(type, resistance);
            }
            CalculateResistance();
            CallResistanceChange(new ResistanceEventArgs(type, GetResistance(type)));
        }


        private void CalculateResistance()
        {
            m_combinedResistance.Clear();
            foreach (var key in m_baseResistance.Keys)
            {
                m_combinedResistance.Add(key, m_baseResistance[key]);
            }

            foreach (var key in m_additionalResistance.Keys)
            {
                if (m_combinedResistance.ContainsKey(key))
                {
                    m_combinedResistance[key] += m_additionalResistance[key];
                }
                else
                {
                    m_combinedResistance.Add(key, m_additionalResistance[key]);
                }
            }
        }

        private void RestrictValue(Dictionary<DamageType, float> resistance)
        {
            foreach (var key in resistance.Keys)
            {
                resistance[key] = Mathf.Clamp(resistance[key], 0, 1);
            }
        }

        public override void SetData(AttackResistanceData data)
        {

        }

        private void Awake()
        {
            m_additionalResistance = new Dictionary<DamageType, float>();
            m_additionalResistance.Clear();
            m_combinedResistance = new Dictionary<DamageType, float>();
            m_combinedResistance.Clear();
            CalculateResistance();
        }

#if UNITY_EDITOR
        private void SendEvent()
        {
            CalculateResistance();
            CallResistanceChange(new ResistanceEventArgs(DamageType.All, 0));
        }
#endif
    }
}