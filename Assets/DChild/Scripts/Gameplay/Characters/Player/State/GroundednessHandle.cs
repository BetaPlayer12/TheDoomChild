using System;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class GroundednessHandle : MonoBehaviour, IComplexCharacterModule
    {
        private IPlacementState m_state;
        private CharacterPhysics2D m_physics;
        private Animator m_animator;
        private string m_midAirParamater;

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
            m_fallHandle.Initialize(info.animator, info.animationParametersData);
            m_landHandle = new LandHandle();
            m_landHandle.Initialize(info.animator, info.animationParametersData);
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
            }
            else
            {
                m_animator.SetBool(m_midAirParamater, true);
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

                var hasLanded = m_physics.onWalkableGround;
                if (hasLanded)
                {
                    m_state.isGrounded = true;
                    m_landHandle.Execute();
                    m_skillRequester.RequestSkillReset(PrimarySkill.DoubleJump, PrimarySkill.Dash);
                    m_fallHandle.ResetValue();
                    m_animator.SetBool(m_midAirParamater, false);
                }
            }
        }

    }

}