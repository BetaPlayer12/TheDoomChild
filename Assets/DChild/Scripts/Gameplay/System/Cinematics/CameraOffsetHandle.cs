using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public class CameraOffsetHandle : MonoBehaviour
    {
        private struct Data
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

        [SerializeField, MinValue(0)]
        private float m_yOffset;
        [SerializeField]
        private float m_duration;
        [SerializeField]
        private AnimationCurve m_interpolation;

        private Vector3 m_currentOffset;
        private IVirtualCamera m_currentCamera;
        private Data m_data;

        public void ApplyOffset(IVirtualCamera camera, Cinema.LookAhead lookAhead)
        {
            Vector3 offset = Vector3.zero;
            switch (lookAhead)
            {
                case Cinema.LookAhead.None:
                    offset = Vector3.zero;
                    break;
                case Cinema.LookAhead.Up:
                    offset = Vector3.up * m_yOffset;
                    break;
                case Cinema.LookAhead.Down:
                    offset = Vector3.down * m_yOffset;
                    break;
            }
            m_currentCamera = camera;
            if (m_currentOffset != offset)
            {
                m_currentOffset = offset;
                m_data.Initialize(m_currentCamera.currentOffset);
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
                    m_data.time += Time.unscaledDeltaTime;
                    m_data.animationTime = Mathf.Clamp01(m_data.time / m_duration);
                    m_currentCamera.ApplyOffset(Vector3.Lerp(m_data.previousOffset, destination, m_data.animationTime));
                }
                yield return null;
            } while (m_data.animationTime < 1);
        }
    }
}