using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DChild.Configurations.Visuals
{
    [System.Serializable]
    public class GammaAdjuster : IPostProcessAdjusterModule
    {
        [SerializeField]
        private float m_gamma;
        private LiftGammaGain m_liftGammaGain = null;

        public void ApplyConfiguration(PostProcessConfiguration configuration)
        {
            if (m_liftGammaGain != null)
            {
                m_liftGammaGain.gamma.Override(new Vector4(1f, 1f, 1f, configuration.gamma));
                m_gamma = configuration.gamma;
            }
        }

        public void ModifyConfiguration(ref PostProcessConfiguration configuration)
        {
            configuration.gamma = m_gamma;
        }

        public bool ValidatePostProcess(Volume volume, ref string message)
        {
            var result = volume.profile.TryGet(out LiftGammaGain liftGammaGain);
            if (result)
            {
                m_liftGammaGain = liftGammaGain;
            }
            else
            {
                message += $"Missing Gamma \n";
            }
            return result;
        }
    }
}