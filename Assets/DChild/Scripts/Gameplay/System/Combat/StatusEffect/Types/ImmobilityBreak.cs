using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Systems.WorldComponents;
using DChild.Gameplay.Characters.Players;
using Holysoft.Collections;
using Holysoft.Event;
using UnityEngine;
using DChild.Inputs;
using Sirenix.OdinInspector;
using DChild.Gameplay.Combat.StatusInfliction;
using System;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ImmobilityBreak : MonoBehaviour
    {
        [SerializeField, MinValue(1)]
        private int m_breakThreshold;
        [SerializeField, MinValue(1)]
        private int m_breakSpeed;
        [SerializeField]
        private CountdownTimer m_inputResetTimer;
        [SerializeField, MinValue(1)]
        private int m_breakResetSpeed;

        private int m_breakMeter;
        private bool m_isBreaking;
        private bool m_requiresRightButton;

        private IStatusReciever m_reciever;

        //public void Initialize(IPlayerModules player)
        //{
        //    m_reciever = (IStatusReciever)player;
        //    GetComponentInParent<IPetrifyController>().CallPetrifyHandler += OnPetrifyHandle;
        //    GetComponentInParent<IFrozenController>().CallFrozenHandler += OnFrozenHandle;
        //    m_inputResetTimer.CountdownEnd += OnInputReset;
        //}

        private void OnInputReset(object sender, EventActionArgs eventArgs)
        {
            m_isBreaking = false;
        }

        private void OnFrozenHandle(object sender, ControllerEventArgs eventArgs)
        {
            HandleBreak(eventArgs.input.direction, StatusEffectType.Frozen);
        }

        private void OnPetrifyHandle(object sender, ControllerEventArgs eventArgs)
        {
            HandleBreak(eventArgs.input.direction, StatusEffectType.Petrify);
        }

        private void HandleBreak(DirectionalInput playerInput, StatusEffectType breakFrom)
        {
            if (m_isBreaking)
            {
                bool increaseBreakMeter = false;
                if (m_requiresRightButton)
                {
                    increaseBreakMeter = playerInput.isRightPressed;
                }
                else
                {
                    increaseBreakMeter = playerInput.isLeftPressed;
                }
                if (increaseBreakMeter)
                {
                    m_inputResetTimer.Reset();
                    m_breakMeter += m_breakSpeed;
                    if (m_breakMeter >= m_breakThreshold)
                    {
                        GameplaySystem.combatManager.CureStatusOf(m_reciever, breakFrom);
                        m_breakMeter = 0;
                        enabled = false;
                    }               
                }
                enabled = true;
            }
            else
            {
                m_requiresRightButton = playerInput.isLeftPressed;
                m_isBreaking = true;
            }         
        }

        private void Awake()
        {
            enabled = false;
        }

        private void LateUpdate()
        {
            if (m_isBreaking)
            {
                m_inputResetTimer.Tick(Time.deltaTime);
            }
            else
            {
                m_breakMeter -= m_breakResetSpeed;
                if(m_breakMeter <= 0)
                {
                    m_breakMeter = 0;
                    enabled = false;
                }
            }
        }
    }

}