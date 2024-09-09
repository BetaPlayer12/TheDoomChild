using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace DChild.Configurations.Visuals
{
    public class GammaAdjusterOLD : MonoBehaviour
    {
        [HideInInspector] public Volume renderingVolume;
        LiftGammaGain liftGammaGain;

        [Button]
        public void SetGammaAlpha(float gammaAlpha)
        {
            liftGammaGain.gamma.Override(new Vector4(1f, 1f, 1f, gammaAlpha));
        }

        public float GetGammaAlpha() { return liftGammaGain.gamma.value.w; }
        private void OnValueChange(object sender, EventActionArgs eventArgs)
        {
            SetGammaAlpha(GameSystem.settings.configuration.visualConfiguration.brightness);
        }

        private void Awake()
        {
            renderingVolume = GetComponent<Volume>();
            if (!renderingVolume.profile.TryGet(out liftGammaGain)) throw new System.NullReferenceException(nameof(liftGammaGain));
        }

        private void OnEnable()
        {
            var settings = GameSystem.settings;
            settings.visual.SceneVisualsChange += OnValueChange;
            SetGammaAlpha(settings.configuration.visualConfiguration.brightness);
        }


        private void OnDisable()
        {
            var settings = GameSystem.settings;
            settings.visual.SceneVisualsChange -= OnValueChange;
        }
    }
}
