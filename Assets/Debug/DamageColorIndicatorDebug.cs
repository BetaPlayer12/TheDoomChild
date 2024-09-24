using DChild.Gameplay;
using DChild.Gameplay.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug
{
    public class DamageColorIndicatorDebug : MonoBehaviour
    {
        [SerializeField]
        private DamageColorIndicator[] m_indicators;
        [SerializeField]
        private float m_simulateDamageIntervals;

        private float m_time;

        private void Update()
        {

            if (m_time <= 0)
            {
                m_time = m_simulateDamageIntervals;
                for (int i = 0; i < m_indicators.Length; i++)
                {
                    m_indicators[i].Execute();
                }
            }
            else
            {
                m_time -= GameplaySystem.time.deltaTime;
            }
        }
    }

}