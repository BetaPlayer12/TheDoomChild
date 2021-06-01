using DChild.Configurations;
using DChild.UI;
using Holysoft.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu.Settings
{
    public class VisualAudioFieldInitializer : MonoBehaviour
    {
        private IValueUI[] m_fields;

        private void Awake()
        {
            m_fields = GetComponentsInChildren<IValueUI>();

            var visualSettings = GameSystem.settings.visual;
            var visualRefrences = GetComponentsInChildren<IReferenceUI<VisualSettingsHandle>>();
            for (int i = 0; i < visualRefrences.Length; i++)
            {
                visualRefrences[i].SetReference(visualSettings);
            }
            var audioSettings = GameSystem.settings.audio;
            var audioRefrences = GetComponentsInChildren<IReferenceUI<Configurations.AudioSettingsHandle>>();
            for (int i = 0; i < audioRefrences.Length; i++)
            {
                audioRefrences[i].SetReference(audioSettings);
            }

            var gameplaySettings = GameSystem.settings.gameplay;
            var gameplaySeRefrences = GetComponentsInChildren<IReferenceUI<GameplaySettings>>();
            for (int i = 0; i < gameplaySeRefrences.Length; i++)
            {
                gameplaySeRefrences[i].SetReference(gameplaySettings);
            }
        }
    }
}