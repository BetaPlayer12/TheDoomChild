using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class BlackBloodImmunity : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        private bool m_isActive;
        private StatusEffectResistance m_resistance;

        public bool isActive => m_isActive;

        public void EnableImmunity(bool isEnable)
        {
            if(m_resistance == null)
            {
                m_resistance = GetComponentInChildren<StatusEffectResistance>();
            }

            if (m_isActive != isEnable)
            {
                if (isEnable)
                {
                    m_resistance.SetResistance(StatusEffectType.Cursed, 100);
                    m_resistance.ResistanceChange += OnResistanceChange;
                }
                else
                {
                    m_resistance.ResistanceChange -= OnResistanceChange;
                    //Set it to 0 since there might not be a chance for Black Blood Immunity to be disable after obtaining it
                    m_resistance.SetResistance(StatusEffectType.Cursed, 0);
                }
                m_isActive = isEnable;
            }
        }

        private void OnResistanceChange(object sender, StatusEffectResistance.ResistanceEventArgs eventArgs)
        {
            // unsubscribe to event so that this function will not be called everytime this forces the resistance value;
            m_resistance.ResistanceChange -= OnResistanceChange;
            m_resistance.SetResistance(StatusEffectType.Cursed, 100);
            m_resistance.ResistanceChange += OnResistanceChange;
        }

        private void Awake()
        {
            if (m_resistance == null)
            {
                m_resistance = GetComponentInChildren<StatusEffectResistance>();
            }
        }
    }
}