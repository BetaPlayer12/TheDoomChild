using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Skill
{
    public class DoubleJump : DChild.Gameplay.Characters.Players.Behaviour.Jump, IControllableModule
    {
        [SerializeField]
        private FXSpawner m_fXSpawner;

        private IHighJumpState m_state;
        private IDoubleJumpState m_doubleJumpState;

        private Animator m_animator;
        private string m_speedYParameter;
        private string m_doubleJumpParameter;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            m_state = info.state;
            m_doubleJumpState = info.state;
            m_doubleJumpState.canDoubleJump = true;

            m_animator = info.animator;
            m_speedYParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedY);
            m_doubleJumpParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.DoubleJump);
            info.skillResetRequester.SkillResetRequest += OnSkillReset;
        }

        private void OnSkillReset(object sender, ResetSkillRequestEventArgs eventArgs)
        {
            if (eventArgs.IsRequestedToReset(PrimarySkill.DoubleJump))
            {
                m_doubleJumpState.canDoubleJump = true;
            }
        }

        public override void HandleJump()
        {
           
            m_physics.StopCoyoteTime();
            base.HandleJump();
            m_physics.AddForce(Vector2.up * m_power, ForceMode2D.Impulse);
            m_state.canHighJump = false;
            m_doubleJumpState.canDoubleJump = false;
            
            m_animator.SetInteger(m_speedYParameter, 0);
            m_animator.SetTrigger(m_doubleJumpParameter);
            m_fXSpawner.SpawnFX(m_character.facing);
            

        }

        public void ConnectTo(IMainController controller)
        {
            controller.ControllerDisabled += OnControllerDisabled;
        }

        private void OnControllerDisabled(object sender, EventActionArgs eventArgs)
        {
            m_doubleJumpState.canDoubleJump = true;
        }

        private void Start()
        {
            m_doubleJumpState.canDoubleJump = true;
        }
    }
}