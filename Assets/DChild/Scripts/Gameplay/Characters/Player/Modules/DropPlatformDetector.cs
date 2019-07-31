using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class DropPlatformDetector : MonoBehaviour, IComplexCharacterModule
    {
        private RaySensor m_groundSensor;
        private IPlatformDropState m_state;

        private void OnSensorCast(object sender, RaySensorCastEventArgs eventArgs)
        {
            var hitCollider = m_groundSensor.GetProminentHitCollider();
            m_state.canPlatformDrop = hitCollider?.CompareTag("Droppable") ?? false;
        }

        private void OnLandCall(object sender, EventActionArgs eventArgs)
        {
            m_groundSensor.Cast();
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_groundSensor = info.GetSensor(PlayerSensorList.SensorType.Ground);
            m_groundSensor.SensorCast += OnSensorCast;
            m_state = info.state;
            info.groundednessHandle.LandExecuted += OnLandCall;
        }
    }

}