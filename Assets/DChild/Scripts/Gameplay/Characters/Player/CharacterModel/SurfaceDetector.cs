﻿using DChild.Gameplay.Environment;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class SurfaceDetector : MonoBehaviour
    {
        public struct SurfaceDetectedEventArgs : IEventActionArgs
        {
            public SurfaceDetectedEventArgs(SurfaceData m_surface) : this()
            {
                this.surface = m_surface;
            }

            public SurfaceData surface { get; }
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
                NewSurfaceDetected?.Invoke(this, new SurfaceDetectedEventArgs(m_currentSurface));
            }
        }
    }
}