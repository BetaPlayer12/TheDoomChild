using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class DropPlatformDetector : MonoBehaviour, IPlayerExternalModule
    {
        [SerializeField]
        private RaySensor m_groundSensor;
        private IPlatformDropState m_state;
        

        public void Initialize(IPlayerModules player)
        {
            m_groundSensor = player.sensors.groundSensor;
            m_groundSensor.SensorCast += OnSensorCast;
            m_state = player.characterState;
            
            GetComponentInParent<ILandController>().LandCall += OnLandCall;
        }

        private void OnSensorCast(object sender, RaySensorCastEventArgs eventArgs)
        {

            var hitCollider = m_groundSensor.GetProminentHitCollider();
           // Debug.Log($"{hitCollider.gameObject.name} = " + hitCollider.tag);
            m_state.canPlatformDrop = hitCollider?.CompareTag("Droppable") ?? false;
        }

        private void OnLandCall(object sender, EventActionArgs eventArgs)
        {
            
            m_groundSensor.Cast();
        }
    }

}