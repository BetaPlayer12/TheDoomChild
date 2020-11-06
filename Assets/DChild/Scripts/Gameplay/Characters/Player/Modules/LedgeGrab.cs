﻿using DChild.Gameplay.Characters.Players.State;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class LedgeGrab : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private RaySensor m_grabbableWallSensor;
        [SerializeField]
        private RaySensor m_overheadSensor;
        [SerializeField]
        private RaySensor m_destinationSensor;
        [SerializeField, MinValue(0f)]
        private float m_destinationFromWallOffset;

        private int m_animation;
        private Character m_character;
        private Rigidbody2D m_rigidbody;
        private Animator m_animator;
        private ILedgeGrabState m_state;
        private Vector2 m_destination;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_rigidbody = info.rigidbody;
            m_state = info.state;
            m_animator = info.animator;
            m_animation = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.LedgeGrab);
        }

        public bool IsDoable()
        {
            m_grabbableWallSensor.Cast();
            if (m_grabbableWallSensor.allRaysDetecting)
            {
                m_overheadSensor.Cast();
                if (m_overheadSensor.isDetecting == false)
                {
                    var wallPoint = m_grabbableWallSensor.GetValidHits()[0].point;
                    var destinationPosition = m_destinationSensor.transform.position;
                    destinationPosition.x = wallPoint.x + (m_destinationFromWallOffset * (int)m_character.facing);
                    m_destinationSensor.transform.position = destinationPosition;
                    m_destinationSensor.Cast();
                    if (m_destinationSensor.isDetecting)
                    {
                        m_destination = m_destinationSensor.GetValidHits()[0].point;
                        return true;
                    }
                }
            }
            return false;
        }

        public void Execute()
        {
            m_rigidbody.position = m_destination;
            m_rigidbody.velocity = Vector2.zero;
            m_animator.SetTrigger(m_animation);
            m_state.waitForBehaviour = true;
        }

        public void EndExecution()
        {
            m_state.waitForBehaviour = false;
        }
    }
}
