using DChild.Gameplay;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    public class PlatformPatrolSensor : MonoBehaviour
    {
        [SerializeField]
        private RaySensor m_groundSensor;
        [SerializeField]
        private RaySensor m_edgeWallSensor;
        [Space]
        [SerializeField]
        private RaySensor m_wallSensor;

        private Vector2 m_snapToWallPosition;
        private bool m_shouldSnapToWall;
        private Vector2 m_snapToLedgePosition;
        private bool m_shouldSnapToLedge;

        public bool shouldSnapToWall => m_shouldSnapToWall;
        public Vector2 snapToWallPosition => m_snapToWallPosition;

        public Vector2 snapToLedgePosition => m_snapToLedgePosition;
        public bool shouldSnapToLedge => m_shouldSnapToLedge;

        public void ResetSensor()
        {
            m_shouldSnapToWall = false;
            m_shouldSnapToLedge = false;
        }

        private void OnWallensorCast(object sender, RaySensorCastEventArgs eventArgs)
        {
            if (m_wallSensor.isDetecting)
            {
                m_shouldSnapToWall = true;
                m_snapToWallPosition = m_wallSensor.GetHits()[0].point;
            }
            else
            {
                m_shouldSnapToWall = false;
            }
        }

        private void OnGroundSensorCast(object sender, RaySensorCastEventArgs eventArgs)
        {
            if (m_groundSensor.isDetecting == false)
            {
                m_edgeWallSensor.Cast();
                if (m_edgeWallSensor.isDetecting)
                {
                    m_shouldSnapToLedge = true;
                    m_snapToLedgePosition = m_edgeWallSensor.GetHits()[0].point;
                }
            }
            else
            {
                m_shouldSnapToLedge = false;
            }
        }

        private void Start()
        {
            if (m_groundSensor)
            {
                m_groundSensor.SensorCast += OnGroundSensorCast;
            }
            if (m_wallSensor)
            {
                m_wallSensor.SensorCast += OnWallensorCast;
            }
        }
    }
}