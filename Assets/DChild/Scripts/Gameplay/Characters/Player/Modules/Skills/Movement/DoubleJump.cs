using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Skill
{
    public class DoubleJump : Jump, IPlayerExternalModule, IEventModule
    {
        [SerializeField]
        private float m_delay;
        private float m_timer;

        private IHighJumpState m_state;
        private IDoubleJumpState m_doubleJumpState;
        private IPlacementState m_placementState;
        //private AirMoveDoubleJumpHandler m_handler;

        public override void Initialize(IPlayerModules player)
        {
            base.Initialize(player);
            m_state = player.characterState;
            m_doubleJumpState = player.characterState;
            m_placementState = player.characterState;
        }


      

        private void Update()
        {
            if (m_placementState.isGrounded)
            {
                m_doubleJumpState.hasDoubleJumped = false;
            }
        }

        public override void HandleJump()
        {
            m_character.StopCoyoteTime();
            //m_handler.enabled = true;
            base.HandleJump();
            m_character.AddForce(Vector2.up * m_power, ForceMode2D.Impulse);
            m_state.canHighJump = false;
            m_doubleJumpState.canDoubleJump = false; /////
            m_doubleJumpState.hasDoubleJumped = true;
            CallJumpStart(); 
        }

        public void ConnectEvents()
        {
            GetComponentInParent<IDoubleJumpController>().DoubleJumpCall += OnJumpCall;
            GetComponentInParent<IDoubleJumpController>().DoubleJumpReset += OnCallReset;
            GetComponentInParent<ILandController>().LandCall += OnLandCall;
        }

        private void OnLandCall(object sender, EventActionArgs eventArgs)
        {
            m_timer = m_delay;
            //m_handler.enabled = false;
            m_doubleJumpState.canDoubleJump = true;
            m_doubleJumpState.hasDoubleJumped = false;
        }

        private void OnCallReset(object sender, EventActionArgs eventArgs)
        {
            m_doubleJumpState.canDoubleJump = true;
        }

        private void OnJumpCall(object sender, EventActionArgs eventArgs)
        {
            HandleJump();
        }

        private void Start()
        {
            m_timer = m_delay;
            //m_handler = GetComponent<AirMoveDoubleJumpHandler>();
            //m_handler.enabled = false;
            m_doubleJumpState.canDoubleJump = true;
        }
    }
}