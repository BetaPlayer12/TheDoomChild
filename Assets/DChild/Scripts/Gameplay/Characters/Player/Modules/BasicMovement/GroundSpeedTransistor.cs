using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class GroundSpeedTransistor : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField, BoxGroup("Data")]
        private MovementData m_crouchData;
        [SerializeField, BoxGroup("Data")]
        private MovementData m_jogData;
        [SerializeField, BoxGroup("Data")]
        private MovementData m_sprintData;

        [SerializeField]
        private GroundMovement m_movement;
        [SerializeField]
        private CountdownTimer m_transistionToSprintTime;

        private IIsolatedTime m_time;
        private bool m_isSprinting;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_time = info.character.isolatedObject;
        }

        public void SwitchToCrouchSpeed()
        {
            m_movement.SetInfo(m_crouchData.info);
            m_movement.SetMovingSpeedParameter(1);
        }

        public void SwitchToJogSpeed()
        {
            m_movement.SetInfo(m_jogData.info);
            m_movement.SetMovingSpeedParameter(1);
            m_transistionToSprintTime.Reset();
            m_isSprinting = false;
        }

        public void HandleSprintTransistion()
        {
            if (m_isSprinting == false)
            {
                m_transistionToSprintTime.Tick(m_time.deltaTime);
            }
        }

        private void Awake()
        {
            m_transistionToSprintTime.CountdownEnd += OnCountdownEnd;
        }

        private void OnCountdownEnd(object sender, EventActionArgs eventArgs)
        {
            m_movement.SetInfo(m_sprintData.info);
            m_movement.SetMovingSpeedParameter(2);
            m_isSprinting = true;
        }


    }
}