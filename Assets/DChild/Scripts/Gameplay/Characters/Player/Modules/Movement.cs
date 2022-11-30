﻿using DChild.Gameplay.Characters.Players.Behaviour;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{

    public class Movement : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        public enum Type
        {
            Crouch,
            Jog,
            MidAir,
            Pull,
            Push
        }

        [SerializeField]
        private float m_jogSpeed;
        [SerializeField]
        private float m_crouchSpeed;
        [SerializeField]
        private float m_midAirSpeed;
        [SerializeField]
        private float m_pullSpeed;
        [SerializeField]
        private float m_pushSpeed;

        private float m_currentSpeed;
        private IPlayerModifer m_modifier;
        private Rigidbody2D m_rigidbody;
        private Character m_character;
        private Animator m_animator;
        private int m_speedAnimationParameter;
        private int m_xInputAnimationParameter;
        private bool m_isTurning;

        [ShowInInspector, ReadOnly, HideInEditorMode]
        protected float speed => m_currentSpeed * m_modifier.Get(PlayerModifier.MoveSpeed);

        public bool isTurning => m_isTurning;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_modifier = info.modifier;
            m_character = info.character;
            m_rigidbody = info.rigidbody;
            SwitchConfigTo(Type.Jog);
            m_animator = info.animator;
            m_speedAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX);
            m_xInputAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.XInput);
        }

        public void Cancel()
        {
            m_animator.SetFloat(m_speedAnimationParameter, 0);
            m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
        }

        public void SwitchConfigTo(Type type)
        {
            switch (type)
            {
                case Type.Crouch:
                    m_currentSpeed = m_crouchSpeed;
                    break;
                case Type.Jog:
                    m_currentSpeed = m_jogSpeed;
                    break;
                case Type.MidAir:
                    m_currentSpeed = m_midAirSpeed;
                    break;
                case Type.Pull:
                    m_currentSpeed = m_pullSpeed;
                    break;
                case Type.Push:
                    m_currentSpeed = m_pushSpeed;
                    break;
            }
        }

        public void UpdateFaceDirection(float direction)
        {
            m_isTurning = true;
            if (Mathf.Sign(direction) != (int)m_character.facing)
            {
                var otherFacing = m_character.facing == HorizontalDirection.Right ? HorizontalDirection.Left : HorizontalDirection.Right;
                m_character.SetFacing(otherFacing);
            }
            m_isTurning = false;
        }

        public void Move(float direction, bool faceDirection)
        {
            if (direction == 0)
            {
                m_animator.SetFloat(m_speedAnimationParameter, 0);
                m_animator.SetInteger(m_xInputAnimationParameter, 0);
            }
            else
            {
                if (faceDirection == true)
                {
                    UpdateFaceDirection(direction);
                }

                m_animator.SetFloat(m_speedAnimationParameter, 1);
                m_animator.SetInteger(m_xInputAnimationParameter, direction > 0 ? 1 : -1);
            }
            var xVelocity = speed * direction;
            m_rigidbody.velocity = new Vector2(xVelocity, m_rigidbody.velocity.y);
        }
    }
}
