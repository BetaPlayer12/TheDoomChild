using DChild.Configurations;
using DChild.Configurations.Visuals;
using DChild.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Menu.UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class ResolutionDropdown : MonoBehaviour, IValueUI, IReferenceUI<VisualSettingsHandle>
    {
        private VisualSettingsHandle m_settings;
        private TMP_Dropdown m_dropDown;

        public void UpdateUI()
        {
            m_dropDown.value = m_settings.resolution;
        }

        public void SetReference(VisualSettingsHandle reference)
        {
            m_settings = reference;
        }

        private void Awake() => m_dropDown = GetComponent<TMP_Dropdown>();

        private void OnEnable() => m_dropDown.onValueChanged.AddListener(OnValueChange);

        private void OnDisable() => m_dropDown.onValueChanged.RemoveListener(OnValueChange);

        private void OnValueChange(int arg0) => m_settings.resolution = arg0;

        private void OnValidate()
        {
#if UNITY_EDITOR
            Validate();
#endif
        }

#if UNITY_EDITOR
        [Button("Validate")]
        private void Validate()
        {
            var settings = GameSystem.settings;
            if (settings)
            {
                var resolutions = settings.visual.supportedResolutions.GetResolutions();
                List<string> options = new List<string>();
                for (int i = 0; i < resolutions.Length; i++)
                {
                    var resolution = resolutions[i];
                    options.Add($"{resolution.width} X {resolution.height} [{resolution.widthAspect}:{resolution.heightAspect}]");
                }

                var dropDown = GetComponent<TMP_Dropdown>();
                dropDown.ClearOptions();
                dropDown.AddOptions(options);
            }
        }
#endif
    }

}