﻿using System;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class GroundednessHandle : MonoBehaviour, IComplexCharacterModule
    {
        private IGroundednessState m_state;
        private CharacterPhysics2D m_physics;
        private Animator m_animator;
        private string m_midAirParamater;
        private bool m_isInMidAir;

        [SerializeField]
        private float m_groundGravity;
        [SerializeField]
        private float m_midAirGravity;

        [SerializeField]
        private FallHandle m_fallHandle;
        [SerializeField]
        private LandHandle m_landHandle;

        private SkillResetRequester m_skillRequester;
        public event EventAction<EventActionArgs> LandExecuted;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_physics = info.physics;
            m_state = info.state;
            m_animator = info.animator;
            m_midAirParamater = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsMidAir);
            m_fallHandle.Initialize(info);
            m_landHandle.Initialize(info);
            m_skillRequester = info.skillResetRequester;
        }

        private void Start()
        {
            m_state.isGrounded = m_physics.onWalkableGround;
        }

        public void FixedUpdate()
        {
            if (m_state.isGrounded)
            {
                m_state.isFalling = false;
                m_state.isGrounded = m_physics.onWalkableGround;
                if (m_isInMidAir)
                {
                    SetValuesToGround();
                }
                if (m_physics.inContactWithGround)
                {
                    m_physics.gravity.gravityScale = m_groundGravity;
                }
                else
                {
                    m_physics.gravity.gravityScale = m_midAirGravity;
                }
            }
            else
            {
                if (m_isInMidAir == false)
                {
                    m_isInMidAir = true;
                    m_animator.SetBool(m_midAirParamater, true);
                }
                var isFalling = m_fallHandle.isFalling(m_physics);
                if (isFalling)
                {
                    if (m_state.isFalling)
                    {
                        m_fallHandle.Execute(Time.deltaTime);
                    }
                    else
                    {
                        m_fallHandle.StartFall();
                    }
                }
                else
                {
                    m_physics.gravity.gravityScale = m_midAirGravity;
                    m_state.isFalling = false;
                }

                var hasLanded = m_physics.onWalkableGround;
                if (hasLanded)
                {
                    m_landHandle.Execute();
                    m_skillRequester.RequestSkillReset(PrimarySkill.DoubleJump, PrimarySkill.Dash);
                    m_fallHandle.ResetValue();
                    SetValuesToGround();
                }

                m_landHandle.RecordVelocity();
            }
        }

        private void SetValuesToGround()
        {
            m_isInMidAir = false;
            m_animator.SetBool(m_midAirParamater, false);
            m_physics.gravity.gravityScale = m_groundGravity;
            m_state.isGrounded = true;
        }

        private void OnDisable()
        {
            m_landHandle.SetRecordedVelocity(Vector2.zero);
        }
    }

}