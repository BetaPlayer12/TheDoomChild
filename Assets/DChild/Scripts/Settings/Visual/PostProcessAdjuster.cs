using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

namespace DChild.Configurations.Visuals
{
    public class PostProcessAdjuster : SerializedMonoBehaviour
    {
        [SerializeField, OnValueChanged("OnModuleValueChange", true)]
        private IPostProcessAdjusterModule[] m_modules = new IPostProcessAdjusterModule[0];

        [HideInInspector] public Volume renderingVolume;

        public void SetConfiguration(PostProcessConfiguration configuration)
        {
            for (int i = 0; i < m_modules.Length; i++)
            {
                m_modules[i].ApplyConfiguration(configuration);
            }
        }

        private void UpdateVisualSettings()
        {

            var bloomValue = GameSystem.settings.visual.bloom;
            var brightnessValue = GameSystem.settings.visual.brightness;


            var configuration = new PostProcessConfiguration()
            {
                gamma = brightnessValue,
                isBloomEnabled = bloomValue,
            };
            SetConfiguration(configuration);

            GameSystem.settings.visual.SceneVisualsChange += OnSettingsChange;
        }

        private void OnSettingsChange(object sender, EventActionArgs eventArgs)
        {
            OnModuleValueChange();
        }

        private void OnModuleValueChange()
        {
            var bloomValue = GameSystem.settings.visual.bloom;
            var brightnessValue = GameSystem.settings.visual.brightness;

            var configuration = new PostProcessConfiguration()
            {
                gamma = brightnessValue,
                isBloomEnabled = bloomValue,
            };

            for (int i = 0; i < m_modules.Length; i++)
            {
                var module = m_modules[i];
                //module.ModifyConfiguration(ref configuration);
                module.ApplyConfiguration(configuration);
            }
        }
               
              
        private void Awake()
        {
            renderingVolume = GetComponent<Volume>();
            string validationMessage = "Volume doesnt have the following for proper adjustments: \n";
            bool hasFailedValidation = false;
            for (int i = 0; i < m_modules.Length; i++)
            {
                var result = m_modules[i].ValidatePostProcess(renderingVolume, ref validationMessage);
                if (hasFailedValidation == false)
                {
                    hasFailedValidation = !result;
                }
            }

            if (hasFailedValidation) throw new System.NullReferenceException(validationMessage);
        }

        private void OnEnable()
        {
            UpdateVisualSettings();
         }
        private void OnDisable()
        {
            GameSystem.settings.visual.SceneVisualsChange -= OnSettingsChange;
        }

    }
}
