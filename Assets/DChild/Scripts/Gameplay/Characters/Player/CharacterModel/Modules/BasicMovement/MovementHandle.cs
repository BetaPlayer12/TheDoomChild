﻿using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class MovementHandle : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField, ReadOnly]
        private MovementInfo m_info;

        protected const float TOLERABLESTOPVELOCITY = 0.1f;
        private CharacterPhysics2D m_characterPhysics;
        protected Vector2 m_direction;

        private IMoveState m_state;
        private IPlayerState m_characterState;
        private Character m_character;
        private Animator m_animator;
        private RaySensor m_groundSensor;
        //private RaySensor m_slopeSensor;

        private string m_speedParameter;
        private string m_ySpeedParameter;
        private string m_turnParameter;
        private string m_landParameter;
        private int m_movingSpeedParameterValue;
        private bool m_hasStopped;
        // private bool m_groundedParameter;
        private float oldDirection;
        private bool m_Play;
        private bool m_ToggleChange;


        public void SetMovingSpeedParameter(int speedValue) => m_movingSpeedParameterValue = speedValue;
        public void SetInfo(MovementInfo info)
        {
            if (m_info != info)
            {
                m_info = info;
                UpdateVelocity();
            }
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_characterPhysics = info.physics;
            m_state = info.state;
            m_characterState = info.state;
            m_character = info.character;
            m_animator = info.animator;
            m_speedParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX);
            m_ySpeedParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedY);
            m_turnParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Turn);
            m_landParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Land);
            // m_groundedParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsGrounded);
            //m_slopeSensor = info.GetSensor(PlayerSensorList.SensorType.Slope);
            m_groundSensor = info.GetSensor(PlayerSensorList.SensorType.Ground);
            info.groundednessHandle.LandExecuted += OnLand;
        }

        private void OnLand(object sender, EventActionArgs eventArgs)
        {
            //m_state.isMoving = false;
            m_animator.SetInteger(m_ySpeedParameter, 0);
            //m_animator.SetInteger(m_speedParameter, 0);
            m_characterPhysics.SetVelocity(Vector2.zero);
        }

        public void Move(float direction)
        {
            if (direction == 0)
            {
                if (m_characterPhysics.inContactWithGround)
                {
                    if (m_hasStopped)
                    {
                        m_characterPhysics.SetVelocity(0, 0);
                    }
                    else
                    {
                        m_characterPhysics.SetVelocity(y: 0);
                    }
                }

                if (m_hasStopped ==false && m_characterPhysics.velocity.x != 0)
                {
                    Deccelerate();
                }
                m_state.isMoving = false;
                m_animator.SetInteger(m_speedParameter, 0);
            }
            else
            {
                var newDirection = direction > 0 ? Vector2.right : Vector2.left;

                if (newDirection != m_direction && m_hasStopped == false)
                {
                    m_direction = newDirection;
                    var currentSpeed = Mathf.Abs(m_characterPhysics.velocity.x);
                    m_characterPhysics.SetVelocity(currentSpeed * m_direction.x);
                }

                if (IsInMaxSpeed())
                {
                    m_characterPhysics.SetVelocity(m_info.maxSpeed * m_direction.x);
                }
                else
                {
                    Accelerate();
                }

                m_state.isMoving = true;
                m_hasStopped = false;

                var newFacing = direction > 0 ? HorizontalDirection.Right : HorizontalDirection.Left;

                if (m_character.facing != newFacing)
                {
                    var characterTransform = m_character.transform;
                    characterTransform.localScale = new Vector3(direction, characterTransform.localScale.y, characterTransform.localScale.z);
                }
                m_character.SetFacing(newFacing);

                m_animator.SetInteger(m_speedParameter, m_movingSpeedParameterValue);
            }


        }

        public void UpdateVelocity()
        {
            m_direction = m_characterPhysics.velocity.x >= 0 ? Vector2.right : Vector2.left;
            var currentSpeed = Mathf.Abs(m_characterPhysics.velocity.x);
            if (currentSpeed > m_info.maxSpeed)
            {
                m_characterPhysics.SetVelocity(m_info.maxSpeed * m_direction.x);
            }
        }

        private bool IsInMaxSpeed()
        {
            if (m_direction.x > 0)
            {
                return m_characterPhysics.velocity.x >= m_info.maxSpeed * m_characterPhysics.moveAlongGround.x;
            }
            else
            {
                return m_characterPhysics.velocity.x <= -m_info.maxSpeed * m_characterPhysics.moveAlongGround.x;
            }
        }

        private Vector2 ConvertToMoveForce(float acceleration) => m_characterPhysics.moveAlongGround * (acceleration * m_direction.x);

        private void Accelerate()
        {
            var force = ConvertToMoveForce(m_info.acceleration);
            m_characterPhysics.AddForce(force);
        }

        public void Deccelerate()
        {
            if (m_direction.x > 0)
            {
                if (m_characterPhysics.velocity.x > TOLERABLESTOPVELOCITY)
                {
                    m_characterPhysics.AddForce(Vector2.left * m_info.decceleration);

                }
                if (m_hasStopped == false && m_characterPhysics.velocity.x <= 0)
                {
                    m_characterPhysics.SetVelocity(0);
                    m_hasStopped = true;
                }
            }
            else
            {
                if (m_characterPhysics.velocity.x < -TOLERABLESTOPVELOCITY)
                {
                    m_characterPhysics.AddForce(Vector2.right * m_info.decceleration);
                }

                if (m_hasStopped == false && m_characterPhysics.velocity.x >= 0)
                {
                    m_characterPhysics.SetVelocity(0);
                    m_hasStopped = true;
                }
            }
        }
    }
}