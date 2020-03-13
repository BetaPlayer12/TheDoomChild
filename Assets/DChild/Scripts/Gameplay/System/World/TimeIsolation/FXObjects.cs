using UnityEngine;


namespace DChild.Gameplay.Systems.WorldComponents
{
    [System.Serializable]
    public class FXObjects : IFXObjects
    {
        [SerializeField]
        private ParticleSystem[] m_particleSystems;

        public FXObjects(ParticleSystem[] m_particleSystems)
        {
            this.m_particleSystems = m_particleSystems;
        }

        public void AlignTime(float timeScale)
        {
            for (int i = 0; i < m_particleSystems.Length; i++)
            {
                if (m_particleSystems[i] != null)
                {
                    var main = m_particleSystems[i].main;
                    //main.simulationSpeed = timeScale;
                }
            }
        }
    }
}