using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class DropPlatformDetector : MonoBehaviour, IPlayerExternalModule
    {
        private RaySensor m_groundSensor;
        private IPlatformDropState m_state;

        public void Initialize(IPlayerModules player)
        {
            m_groundSensor = player.sensors.groundSensor;
            m_state = player.characterState;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_state.isCrouched)
            {
                var hitCollider = m_groundSensor.GetProminentHitCollider();
                m_state.canPlatformDrop = hitCollider.CompareTag("Droppable");
            }
            else
            {
                m_state.canPlatformDrop = false;
            }
        }
    }

}