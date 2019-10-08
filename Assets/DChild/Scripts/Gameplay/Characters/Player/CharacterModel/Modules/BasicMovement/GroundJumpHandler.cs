using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using DChild.Inputs;
using UnityEngine;
using System;
using DChild.Gameplay.Characters.Players;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class GroundJumpHandler : Jump, IControllableModule
    {
        [SerializeField]
        private FXSpawner m_fXSpawner;

        private IHighJumpState m_highJumpState;
        private Animator m_animator;
        private string m_jumpParamater;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            m_highJumpState = info.state;
            m_animator = info.animator;
            m_character = info.character;
            m_jumpParamater = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Jump);
            info.groundednessHandle.LandExecuted += OnLand;
        }

        public override void HandleJump()
        {
            m_highJumpState.canHighJump = true;
            if (m_physics.onWalkableGround)
            {
                m_physics.StopCoyoteTime();
                m_physics.SetVelocity(x: 0);
                base.HandleJump();
                m_physics.AddForce(Vector2.up * m_power, ForceMode2D.Impulse);
                m_animator.SetTrigger(m_jumpParamater);
                m_highJumpState.hasJumped = true;
                m_fXSpawner.SpawnFX(m_character.facing);
               
            }
            //m_character.transform.eulerAngles = Vector3.zero;
          
        }

        private void OnLand(object sender, EventActionArgs eventArgs)
        {
            Debug.Log("Hass jump set to false");
            m_highJumpState.hasJumped = false;        
        }
       
        public void ConnectTo(IMainController controller)
        {
            controller.ControllerDisabled += OnControllerDisabled;
        }

        private void OnControllerDisabled(object sender, EventActionArgs eventArgs)
        {
            m_highJumpState.canHighJump = false;
            m_physics.SetVelocity(x: 0);
        }

    }
}