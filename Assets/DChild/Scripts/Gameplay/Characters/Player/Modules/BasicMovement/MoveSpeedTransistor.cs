using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class MoveSpeedTransistor : MonoBehaviour, IComplexCharacterModule, IControllableModule
    {
        [SerializeField, BoxGroup("Data")]
        private MovementData m_crouchData;
        [SerializeField, BoxGroup("Data")]
        private MovementData m_jogData;
        [SerializeField, BoxGroup("Data")]
        private MovementData m_sprintData;
        [SerializeField, BoxGroup("Data")]
        private MovementData m_airMoveData;

        [SerializeField]
        private MovementHandle m_movement;
        [SerializeField]
        private CountdownTimer m_transistionToSprintTime;

        private IIsolatedTime m_time;
        private IMoveState m_state;
        private bool m_isSprinting;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_time = info.character.isolatedObject;
            m_state = info.state;
        }

        public void ConnectTo(IMainController controller)
        {
            controller.ControllerDisabled += OnControllDisabled;
        }

        private void OnControllDisabled(object sender, EventActionArgs eventArgs)
        {
            DisableSprint();
        }

        public void SwitchToCrouchSpeed()
        {
            m_movement?.SetInfo(m_crouchData.info);
            DisableSprint();
        }

        public void SwitchToAirMoveSpeed()
        {
            m_movement?.SetInfo(m_airMoveData.info);
            DisableSprint();
        }

        public void SwitchToJogSpeed()
        {
            m_movement?.SetInfo(m_jogData.info);
            DisableSprint();
        }

        public void HandleSprintTransistion()
        {
           
            if (m_isSprinting == false && m_state.isMoving)
            {
                m_transistionToSprintTime.Tick(m_time.deltaTime);
            }
                
          
           
            
        }

        private void DisableSprint()
        {
            m_movement?.SetMovingSpeedParameter(1);
            m_transistionToSprintTime.Reset();
            m_isSprinting = false;
        }

        private void Awake()
        {
            m_transistionToSprintTime.Reset();
            m_transistionToSprintTime.CountdownEnd += OnCountdownEnd;
        }

        private void OnCountdownEnd(object sender, EventActionArgs eventArgs)
        {
            m_movement?.SetInfo(m_sprintData.info);
            m_movement?.SetMovingSpeedParameter(2);
            m_isSprinting = true;
        }


    }
}