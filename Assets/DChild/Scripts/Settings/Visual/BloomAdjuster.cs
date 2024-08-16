using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DChild.Configurations.Visuals
{
    [System.Serializable]
    public class BloomAdjuster : IPostProcessAdjusterModule
    {
        [SerializeField]
        private bool m_enabled = true;
        private Bloom m_bloom = null;

        public void ApplyConfiguration(PostProcessConfiguration configuration)
        {
            if (m_bloom != null)
            {
                m_bloom.active = configuration.isBloomEnabled;
                m_enabled = configuration.isBloomEnabled;
            }
        }

        public void ModifyConfiguration(ref PostProcessConfiguration configuration)
        {
            configuration.isBloomEnabled = m_enabled;
        }

        public bool ValidatePostProcess(Volume volume, ref string message)
        {
            var result = volume.profile.TryGet(out Bloom bloom);
            if (result)
            {
                m_bloom = bloom;
            }
            else
            {
                message += $"Missing Gamma \n";
            }
            return result;
        }
    }
}