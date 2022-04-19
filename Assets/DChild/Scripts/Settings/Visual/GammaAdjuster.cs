using DChild;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace DChild.Configurations.Visuals
{
    public class GammaAdjuster : MonoBehaviour
    {
        public static GammaAdjuster instance;

        [HideInInspector] public Volume renderingVolume;
        LiftGammaGain liftGammaGain;



        [Button]
        public void SetGammaAlpha(float gammaAlpha)
        {
            liftGammaGain.gamma.Override(new Vector4(1f, 1f, 1f, gammaAlpha));
        }

        public float GetGammaAlpha() { return liftGammaGain.gamma.value.w; }
        private void Awake()
        {
            instance = this;
            renderingVolume = GetComponent<Volume>();
            if (!renderingVolume.profile.TryGet(out liftGammaGain)) throw new System.NullReferenceException(nameof(liftGammaGain));
        }

        private void OnEnable()
        {
            SetGammaAlpha(GameSystem.settings.configuration.visualConfiguration.brightness);
        }
    }
}
