using DChild.Configurations;
using DChild.UI;
using Holysoft.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu.Settings
{
    public class VisualAudioScreen : UICanvas
    {
        private IValueUI[] m_fields;

        public override void Hide()
        {           
            Disable();
            CallCanvasHide();
        }

        public override void Show()
        {
            Enable();
            for (int i = 0; i < (m_fields?.Length ?? 0); i++)
            {
                m_fields[i].UpdateUI();
            }
            CallCanvasShow();
        }

        private void Awake()
        {
            m_fields = GetComponentsInChildren<IValueUI>();

            var visualSettings = GameSystem.settings.visual;
            var visualRefrences = GetComponentsInChildren<IReferenceUI<VisualSettings>>();
            for (int i = 0; i < visualRefrences.Length; i++)
            {
                visualRefrences[i].SetReference(visualSettings);
            }
            var audioSettings = GameSystem.settings.audio;
            var audioRefrences = GetComponentsInChildren<IReferenceUI<Configurations.AudioSettings>>();
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