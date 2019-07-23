using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class AirDash : Dash, IEventModule
    {
        [SerializeField]
        [MinValue(0.1)]
        protected float m_stopSpeed;
        public void ConnectEvents()
        {
            GetComponentInParent<IAirDashController>().DashCall += OnDashCall;
        }

        protected override void OnDashDurationEnd(object sender, EventActionArgs eventArgs)
        {
            CallDashEnd();
            enabled = false;
            m_physics.SetVelocity(m_direction.x * m_stopSpeed, 0);
            m_physics.simulateGravity = true;
            m_state.isDashing = false;
            m_state.canDash = false;
            if (m_ghosting != null)
                m_ghosting.enabled = false;
        }

        public void HandleDash()
        {
            CallDashStart();
            m_direction = m_facing.currentFacingDirection == HorizontalDirection.Left ? Vector2.left : Vector2.right;
            m_duration.Reset();
            enabled = true;
            m_state.isDashing = true;
            m_physics.simulateGravity = false;
            if (m_ghosting != null)
                m_ghosting.enabled = true;
        }

        private void Start()
        {
            enabled = false;
        }

        protected void FixedUpdate()
        {
            if(m_state.canDash)
            {
                m_physics.SetVelocity(m_direction.x * m_power, 0);
                m_state.canDash = false;
            }          
        }

        private void OnDashCall(object sender, EventActionArgs eventArgs)
        {
            if (m_state.canDash)
            {
                HandleDash();
            }
        }

#if UNITY_EDITOR
        public void Initialize(float stopPower)
        {
            m_stopSpeed = stopPower;
        }
#endif
    }
}
