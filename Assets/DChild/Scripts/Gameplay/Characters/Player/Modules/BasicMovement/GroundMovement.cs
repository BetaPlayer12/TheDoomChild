using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class GroundMovement : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField, ReadOnly]
        private MovementInfo m_info;

        private CharacterPhysics2D m_characterPhysics;
        protected Vector2 m_direction;

        private IMoveState m_state;
        private IPlayerState m_characterState;
        private Character m_character;
        private Animator m_animator;
        private string m_speedParameter;
        private int m_movingSpeedParameterValue;

        public void SetMovingSpeedParameter(int speedValue) => m_movingSpeedParameterValue = speedValue;
        public void SetInfo(MovementInfo info)
        {
            m_info = info;
            UpdateVelocity();
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_characterPhysics = info.physics;
            m_state = info.state;
            m_characterState = info.state;
            m_character = info.character;
            m_animator = info.animator;
            m_speedParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX);
        }

        public void Move(float direction)
        {
            if (direction == 0)
            {
                m_characterPhysics.SetVelocity(0);
                m_state.isMoving = false;
                m_animator.SetInteger(m_speedParameter, 0);
            }
            else
            {
                m_direction = direction > 0 ? Vector2.right : Vector2.left;
                Accelerate();
                m_state.isMoving = true;
                m_character.SetFacing(direction > 0 ? HorizontalDirection.Right : HorizontalDirection.Left);
                m_animator.SetInteger(m_speedParameter, m_movingSpeedParameterValue);
            }
        }

        public void UpdateVelocity()
        {
            m_direction = m_characterPhysics.velocity.x >= 0 ? Vector2.right : Vector2.left;
            var currentSpeed = Mathf.Abs(m_characterPhysics.velocity.x);
            if(currentSpeed > m_info.maxSpeed)
            {
                m_characterPhysics.SetVelocity(m_info.maxSpeed * m_direction.x);
            }
        }

        private bool IsInMaxSpeed()
        {
            if (m_direction.x > 0)
            {
                return m_characterPhysics.velocity.x < m_info.maxSpeed * m_characterPhysics.moveAlongGround.x;
            }
            else
            {
                return m_characterPhysics.velocity.x > -m_info.maxSpeed * m_characterPhysics.moveAlongGround.x;
            }
        }

        private Vector2 ConvertToMoveForce(float acceleration) => m_characterPhysics.moveAlongGround * (acceleration * m_direction.x);

        private void Accelerate()
        {
            if (IsInMaxSpeed())
            {
                var force = ConvertToMoveForce(m_info.acceleration);
                m_characterPhysics.AddForce(force);
            }
        }
    }
}