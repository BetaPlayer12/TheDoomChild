using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using DChild.Inputs;
using Holysoft.Collections;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Skill
{
    public class WallJump : Jump, IPlayerExternalModule, IEventModule
    {
        [SerializeField]
        private float m_forwardPower;
        [SerializeField]
        private CountdownTimer m_waitBehaviourDuration;

        private PlayerInput m_input;
        private IWallJumpState m_state;
        private IIsolatedTime m_time;

        public void ConnectEvents()
        {
            var controller = GetComponentInParent<IWallJumpController>();
            controller.UpdateCall += OnUpdateCall;
            controller.WallJumpCall += OnWallJumpCall;
        }

        public override void Initialize(IPlayerModules player)
        {
            base.Initialize(player);
            m_state = player.characterState;
            m_time = player.isolatedObject;
            //m_input = player.animation.GetComponent<PlayerInput>();
        }

        public override void HandleJump()
        {
            base.HandleJump();
            m_character.AddForce(Vector2.up * m_power, ForceMode2D.Impulse);
            var forceDirection = m_facing.currentFacingDirection == HorizontalDirection.Left ? 1 : -1;
            m_character.SetVelocity(forceDirection * m_forwardPower);
            m_character.simulateGravity = true;
            CallJumpStart();

            m_state.waitForBehaviour = true;
            /*if(m_input.direction.isHorizontalHeld) */m_state.isWallJumping = true;
            m_waitBehaviourDuration.Reset();
            enabled = true;
            m_facing.TurnCharacter();
        }

        private void OnUpdateCall(object sender, ControllerEventArgs eventArgs)
        {
            m_state.canWallJump = m_state.isStickingToWall;
        }

        private void OnWallJumpCall(object sender, EventActionArgs eventArgs)
        {
            if (m_state.canWallJump)
            {
                HandleJump();
            }
        }

        private void OnCountdownEnd(object sender, EventActionArgs eventArgs)
        {
            m_state.waitForBehaviour = false;
        }

        private void Awake()
        {
            m_waitBehaviourDuration.CountdownEnd += OnCountdownEnd;
            enabled = false;
        }

        private void Update()
        {
            m_waitBehaviourDuration.Tick(m_time.deltaTime);
        }

#if UNITY_EDITOR
        public void Initialize(float power, float waitBehaviourDuration)
        {
            m_forwardPower = power;
            m_waitBehaviourDuration = new CountdownTimer(waitBehaviourDuration);
        }
#endif
    }

}