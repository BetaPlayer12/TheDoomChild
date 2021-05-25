using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class RendererColorChangeHandle : MonoBehaviour
    {
        [SerializeField, TabGroup("Sprites")]
        private SpriteRenderer[] m_spriteRendererColorHandles;
        [SerializeField, TabGroup("Particles")]
        private ParticleSystem[] m_particleSystemColorHandles;

        public void ApplyColor(Color color)
        {
            for (int i = 0; i < m_spriteRendererColorHandles.Length; i++)
            {
                m_spriteRendererColorHandles[i].color = color;
            }

            foreach (var particles in m_particleSystemColorHandles)
            {
                var main = particles.main;
                main.startColor = color;
            }
        }
    }
}
