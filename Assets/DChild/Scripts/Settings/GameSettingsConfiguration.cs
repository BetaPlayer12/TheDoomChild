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
    }

}