using Holysoft.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Systems
{
    public class RandomParticleEmission : MonoBehaviour
    {
        [SerializeField]
        private RangeFloat m_emissionIntervalRange;

        private ParticleSystem m_particleSystem;
        private float m_particleEmissionDuration;
        private float m_timer;
        private bool m_isEmitting;

        private void Start()
        {
            m_particleSystem = GetComponent<ParticleSystem>();
            m_particleEmissionDuration = 0;
            m_isEmitting = m_particleSystem.main.playOnAwake;
            m_timer = m_isEmitting ? m_particleSystem.main.duration : m_emissionIntervalRange.GenerateRandomValue();
        }

        private void Update()
        {
            m_timer -= GameplaySystem.time.deltaTime;
            if (m_isEmitting)
            {
                if (m_timer <= 0)
                {
                    m_timer = m_emissionIntervalRange.GenerateRandomValue();
                    m_particleSystem.Stop(true);
                    m_isEmitting = false;
                }
            }
            else
            {
                if (m_timer <= 0)
                {
                    m_timer = m_particleSystem.main.duration;
                    m_particleSystem.Play(true);
                    m_isEmitting = true;
                }
            }
        }
    }

}