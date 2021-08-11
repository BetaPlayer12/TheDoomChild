using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public class CameraPeekHandle : MonoBehaviour
    {


        [System.Serializable]
        private struct ConfigurationData
        {
            [SerializeField, MinValue(0)]
            private float m_upYOffset;
            [SerializeField, MinValue(0)]
            private float m_downYOffset;

            public float upYOffset => m_upYOffset;
            public float downYOffset => m_downYOffset;
        }

        private struct InterpolationData
        {
            public float animationTime;
            public float time;
            public Vector3 previousOffset;

            public void Initialize(Vector3 offset)
            {
                animationTime = 0;
                time = 0;
                previousOffset = offset;
            }
        }

        [SerializeField]
        private ConfigurationData m_normalConfigurationData;
        [SerializeField]
        private ConfigurationData m_highConfigurationData;
        [SerializeField]
        private ConfigurationData m_lowConfigurationData;

        [SerializeField]
        private float m_duration;
        [SerializeField]
        private AnimationCurve m_interpolation;

        private CameraPeekConfiguration m_currentConfigType;
        private ConfigurationData m_currentConfigData;
        private Vector3 m_currentOffset;
        private IVirtualCamera m_currentCamera;
        private InterpolationData m_interpolationData;
        private ConfigurationData m_disabledPeekonfigData;

        public void SetConfiguration(CameraPeekConfiguration configType)
        {
            m_currentConfigType = configType;
            switch (m_currentConfigType)
            {
                case CameraPeekConfiguration.Normal:
                    m_currentConfigData = m_normalConfigurationData;
                    break;
                case CameraPeekConfiguration.ExtremeHighOnly:
                    m_currentConfigData = m_highConfigurationData;
                    break;
                case CameraPeekConfiguration.ExtremeLowOnly:
                    m_currentConfigData = m_lowConfigurationData;
                    break;
                case CameraPeekConfiguration.None:
                    m_currentConfigData = m_disabledPeekonfigData;
                    break;
            }
        }

        public void ApplyOffset(IVirtualCamera camera, CameraPeekMode lookAhead)
        {
            Vector3 offset = Vector3.zero;
            switch (lookAhead)
            {
                case CameraPeekMode.None:
                    offset = Vector3.zero;
                    break;
                case CameraPeekMode.Up:
                    offset = Vector3.up * m_currentConfigData.upYOffset;
                    break;
                case CameraPeekMode.Down:
                    offset = Vector3.down * m_currentConfigData.downYOffset;
                    break;
            }
            m_currentCamera = camera;
            if (m_currentOffset != offset)
            {
                m_currentOffset = offset;
                m_interpolationData.Initialize(m_currentCamera.currentOffset);
                StopAllCoroutines();
                StartCoroutine(InterpolationRoutine(offset));
            }
        }

        public void CopyOffset(IVirtualCamera from, IVirtualCamera to)
        {
            to.ApplyOffset(from.currentOffset);
        }

        private IEnumerator InterpolationRoutine(Vector3 destination)
        {
            do
            {
                if (GameplaySystem.isGamePaused == false)
                {
                    m_interpolationData.time += Time.unscaledDeltaTime;
                    m_interpolationData.animationTime = Mathf.Clamp01(m_interpolationData.time / m_duration);
                    m_currentCamera.ApplyOffset(Vector3.Lerp(m_interpolationData.previousOffset, destination, m_interpolationData.animationTime));
                }
                yield return null;
            } while (m_interpolationData.animationTime < 1);
        }

        private void Awake()
        {
            m_disabledPeekonfigData = new ConfigurationData();
            SetConfiguration(CameraPeekConfiguration.Normal);
        }
    }
}