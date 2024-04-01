using DChild.Gameplay.Combat;
using Holysoft.Event;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Enemies
{
    [System.Serializable]
    public class CinderBoltHeatHandle
    {
        [System.Serializable]
        public class Config
        {
            [SerializeField]
            private Dictionary<DamageType, int> m_damageTypeToHeatIncreasePair;

            public int GetHeatIncreaseValue(DamageType damageType)
            {
                if (m_damageTypeToHeatIncreasePair.TryGetValue(damageType, out int heatIncrease))
                {
                    return heatIncrease;
                }

                return 0;
            }
        }

        [SerializeField]
        private CinderBoltHeatGauge m_gauge;
        [ShowInInspector, HideInEditorMode]
        private Config m_config;

        private bool m_isInitialized;

        public event EventAction<EventActionArgs> HeatFull
        {
            add
            {
                m_gauge.HeatFull += value;
            }

            remove
            {
                m_gauge.HeatFull -= value;
            }
        }

        public void SetConfiguration(Config config)
        {
            m_config = config;
        }

        public void HandleDamageTaken(DamageType damageType)
        {
            var heat = m_config.GetHeatIncreaseValue(damageType);
            if (heat != 0)
            {
                m_gauge.AddHeat(heat);
            }
        }

        public void ResetHeat()
        {
            m_gauge.Reset();
        }
    }
}