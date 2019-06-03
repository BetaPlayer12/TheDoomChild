using DChild.Configurations;
using DChild.UI;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Menu.UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LanguageDropdown : MonoBehaviour, IValueUI, IReferenceUI<GameplaySettings>
    {
        private GameplaySettings m_settings;
        private TMP_Dropdown m_dropDown;

        public void UpdateUI()
        {
            m_dropDown.value = (int)m_settings.language;
        }

        public void SetReference(GameplaySettings reference)
        {
            m_settings = reference;
        }

        private void Awake() => m_dropDown = GetComponent<TMP_Dropdown>();

        private void OnEnable() => m_dropDown.onValueChanged.AddListener(OnValueChange);

        private void OnDisable() => m_dropDown.onValueChanged.RemoveListener(OnValueChange);

        private void OnValueChange(int arg0) => m_settings.language = (GameplaySettings.Language)arg0;

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
            List<string> options = new List<string>();
            var size = (int)GameplaySettings.Language._COUNT;
            for (int i = 0; i < size; i++)
            {
                var language = (GameplaySettings.Language)i;
                options.Add(language.ToString());
            }

            var dropDown = GetComponent<TMP_Dropdown>();
            dropDown.ClearOptions();
            dropDown.AddOptions(options);
        }
#endif
    }
}