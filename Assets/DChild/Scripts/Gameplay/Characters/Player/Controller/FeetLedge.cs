using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using DChild.Inputs;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class FeetLedge : MonoBehaviour, IPlayerExternalModule, IEventModule
    {
       
        private IFacing m_characterFacing;
        private RaySensor m_kneeSensor;
        private RaySensor m_feetLedgeSensor;
        private ILedgeGrabState m_state;
        private IPlatformDropState m_dropPlatform;
        private CharacterPhysics2D m_physics;
        private PlayerInput m_input;

        [SerializeField]
        private Vector2 m_offSet;


        public void Initialize(IPlayerModules player)
        {
            m_characterFacing = player;
            m_kneeSensor = player.sensors.kneeSensor;
            m_feetLedgeSensor = player.sensors.feetEdgeSensor;
            //m_state = player.characterState;
            
            m_physics = player.physics;
            m_dropPlatform = player.characterState;
        }

        public void ConnectEvents()
        {
            
           GetComponentInParent<IFeetLedgeController>().FeetLedgeCall += PullFromCliff;
        }

        public void PullFromCliff()
        {
            m_feetLedgeSensor.Cast();
            m_kneeSensor.Cast();
           
            if (m_feetLedgeSensor.isDetecting && m_kneeSensor.isDetecting == false  && ((m_input.direction.isLeftHeld == true && m_characterFacing.currentFacingDirection == HorizontalDirection.Left)|| (m_input.direction.isRightHeld == true && m_characterFacing.currentFacingDirection == HorizontalDirection.Right))){

               
                var currentPosition = m_physics.position;
                currentPosition.y = currentPosition.y - m_offSet.y;
                currentPosition.x = currentPosition.x + (m_offSet.x * (int)m_characterFacing.currentFacingDirection);
                m_physics.position = currentPosition;
            }

            
        }

        private void PullFromCliff(object sender, ControllerEventArgs eventArgs)
        {
            m_input = eventArgs.input;
            PullFromCliff();
        }
    }
}
