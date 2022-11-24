using DChild.Gameplay.Environment;
using Holysoft.Event;
using Sirenix.Utilities;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{

    public class SurfaceDetector : MonoBehaviour
    {
        public class SurfaceDetectedEventArgs : IEventActionArgs
        {
            public void Initialize(SurfaceData m_surface)
            {
                this.surface = m_surface;
            }

            public SurfaceData surface { get; private set; }
        }

        [SerializeField]
        private RaySensor m_sensor;
        [SerializeField]
        private SurfaceData m_currentSurface;

        public SurfaceData currentSurface => m_currentSurface;

        public event EventAction<SurfaceDetectedEventArgs> NewSurfaceDetected;

        private void Awake()
        {
            m_sensor.SensorCast += OnSensorCast;
        }

        private void OnSensorCast(object sender, RaySensorCastEventArgs eventArgs)
        {
            var surfaceData = m_sensor.GetProminentHitCollider()?.GetComponentInParent<Surface>()?.data ?? null;
            if (surfaceData != null && m_currentSurface != surfaceData)
            {
                m_currentSurface = surfaceData;
                using (Cache<SurfaceDetectedEventArgs> cacheEventArgs = Cache<SurfaceDetectedEventArgs>.Claim())
                {
                    cacheEventArgs.Value.Initialize(m_currentSurface);
                    NewSurfaceDetected?.Invoke(this, cacheEventArgs.Value);
                    cacheEventArgs.Release();
                }
            }
        }
    }
}