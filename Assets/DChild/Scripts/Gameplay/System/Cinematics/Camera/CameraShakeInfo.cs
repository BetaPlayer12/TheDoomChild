using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    [System.Serializable]
    public class CameraShakeInfo
    {
        [SerializeField]
        private AnimationCurve m_amplitude;
        [SerializeField]
        private AnimationCurve m_frequency;
        [SerializeField, MinValue(0f)]
        private float m_duration;

        public AnimationCurve amplitude => m_amplitude;
        public AnimationCurve frequency => m_frequency;
        public float duration => m_duration;
    }

}