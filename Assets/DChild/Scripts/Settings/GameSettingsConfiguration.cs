using DChild.Serialization;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Configurations
{
    [Serializable]
    public class GameSettingsConfiguration
    {
        [Serializable]
        public struct VisualConfiguration
        {
            public int resolutionIndex;
            public bool fullscreen;
            public bool vsync;
            [MinValue(0)]
            public float brightness;
            [MinValue(0)]
            public float contrast;
        }

        [Serializable]
        public struct AudioConfiguration
        {
            [MinValue(0)]
            public float masterVolume;
            [MinValue(0)]
            public float soundVolume;
            [MinValue(0)]
            public float musicVolume;
        }

        public VisualConfiguration visualConfiguration;
        public AudioConfiguration audioConfiguration;

        public GameSettingsConfiguration()
        {
            visualConfiguration = new VisualConfiguration
            {
                resolutionIndex = 0,
                fullscreen = true,
                vsync = true,
                brightness = 0.5f,
                contrast = 0.5f
            };

            audioConfiguration = new AudioConfiguration
            {
                masterVolume = 1,
                soundVolume = 1,
                musicVolume = 1,
            };
        }
    }
}