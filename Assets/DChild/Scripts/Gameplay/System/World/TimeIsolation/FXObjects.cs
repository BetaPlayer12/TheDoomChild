using UnityEngine;


namespace DChild.Gameplay.Systems.WorldComponents
{
    [System.Serializable]
    public class FXObjects : IFXObjects
    {
        [SerializeField]
        private ParticleSystem[] m_particleSystems;
        [SerializeField]
        private float[] m_originalSimulationSpeed;

        public FXObjects(ParticleSystem[] m_particleSystems)
        {
            this.m_particleSystems = m_particleSystems;
            m_originalSimulationSpeed = new float[m_particleSystems.Length];
            for (int i = 0; i < m_originalSimulationSpeed.Length; i++)
            {
                m_originalSimulationSpeed[i] = m_particleSystems[i].main.simulationSpeed;
            }
        }

        public void AlignTime(float timeScale)
        {
            for (int i = 0; i < m_particleSystems.Length; i++)
            {
                if (m_particleSystems[i] != null)
                {
                    var main = m_particleSystems[i].main;
                    main.simulationSpeed = timeScale * m_originalSimulationSpeed[i];
                }
            }
        }
    }
}