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


        private void OnModuleValueChange()
        {
            var configuration = new PostProcessConfiguration();
            for (int i = 0; i < m_modules.Length; i++)
            {
                var module = m_modules[i];
                module.ModifyConfiguration(ref configuration);
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

    }
}
