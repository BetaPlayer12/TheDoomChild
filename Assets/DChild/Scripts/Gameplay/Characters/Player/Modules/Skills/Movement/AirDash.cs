using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class AirDash : Dash
    {
        [SerializeField]
        [MinValue(0.1)]
        protected float m_stopSpeed;

        protected override void OnDashDurationEnd(object sender, EventActionArgs eventArgs)
        {
            StopDash();
        }

        protected override void StopDash()
        {
            enabled = false;
            TurnOnAnimation(false);
            m_physics.SetVelocity(m_direction.x * m_stopSpeed, 0);
            m_physics.simulateGravity = true;
            m_state.isDashing = false;
            m_state.canDash = false;
            if (m_ghosting != null)
                m_ghosting.enabled = false;
        }

        public void HandleDash()
        {
            m_direction = m_character.facing == HorizontalDirection.Left ? Vector2.left : Vector2.right;
            m_duration.Reset();
            enabled = true;
            TurnOnAnimation(true);
            m_state.isDashing = true;
            m_physics.simulateGravity = false;
            if (m_ghosting != null)
                m_ghosting.enabled = true;
        }

        public override void ConnectTo(IMainController controller)
        {
            base.ConnectTo(controller);
            controller.GetSubController<IAirDashController>().DashCall += OnDashCall;
        }

        protected override void Awake()
        {
            base.Awake();
            enabled = false;
        }

        protected void FixedUpdate()
        {
            if (m_state.canDash)
            {
                m_physics.SetVelocity(m_direction.x * m_power, 0);
                m_state.canDash = false;
            }
        }

        protected override void OnDashCall(object sender, EventActionArgs eventArgs)
        {
            if (m_state.canDash)
            {
                HandleDash();
            }
        }
    }
}
