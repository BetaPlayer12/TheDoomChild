using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Collections;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class GroundDash : Dash
    {
        [SerializeField]
        private float m_adhesive;
        [SerializeField, Min(0)]
        private float m_cooldown;

        private CountdownTimer m_cooldownTimer;
        private bool m_isOnCooldown;

        public override void ConnectTo(IMainController controller)
        {
            base.ConnectTo(controller);
            controller.GetSubController<IGroundDashController>().DashCall += OnDashCall;
        }

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            m_state.canDash = true;
            info.groundednessHandle.LandExecuted += OnLandExecuted;
        }

        private void HandleDash()
        {
            m_direction = m_character.facing == HorizontalDirection.Left ? Vector2.left : Vector2.right;
            m_duration.Reset();
            TurnOnAnimation(true);
            enabled = true;
            m_state.isDashing = true;
            if (m_ghosting != null)
                m_ghosting.enabled = true;
        }

        protected override void OnDashCall(object sender, EventActionArgs eventArgs)
        {
            if (m_state.canDash && m_state.isDashing == false)
            {
                HandleDash();
                m_state.canDash = false;
            }
        }

        private void OnLandExecuted(object sender, EventActionArgs eventArgs)
        {
            AllowDash();
            m_cooldownTimer.EndTime(false);
        }

        protected override void StopDash()
        {
            m_physics.SetVelocity(Vector2.zero);
            m_state.isDashing = false;
            TurnOnAnimation(false);
            if (m_ghosting != null)
            {
                m_ghosting.enabled = false;
            }

            HandleCooldown();
        }

        protected override void OnDashDurationEnd(object sender, EventActionArgs eventArgs)
        {
            StopDash();
        }

        private void HandleCooldown()
        {
            if (m_cooldown > 0)
            {
                m_cooldownTimer.SetStartTime(m_cooldown);
                m_cooldownTimer.Reset();
                m_isOnCooldown = true;
            }
            else
            {
                AllowDash();
            }
        }

        private void OnCooldownEnd(object sender, EventActionArgs eventArgs)
        {
            AllowDash();
        }

        private void AllowDash()
        {
            m_state.canDash = true;
            m_isOnCooldown = false;
            enabled = false;
        }

        private void Start()
        {
            m_cooldownTimer = new CountdownTimer(m_cooldown);
            m_cooldownTimer.CountdownEnd += OnCooldownEnd;
            enabled = false;
        }

        protected override void Update()
        {
            if (m_isOnCooldown)
            {
                m_cooldownTimer.Tick(GameplaySystem.time.deltaTime);
            }
            else
            {
                base.Update();
            }
        }

        protected virtual void FixedUpdate()
        {
            if (m_state.isDashing)
            {
                m_physics.SetVelocity(m_direction.x * m_physics.moveAlongGround.x, m_physics.moveAlongGround.y);
            }
        }
    }
}
