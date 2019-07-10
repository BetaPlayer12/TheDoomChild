using System;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Collections;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class GroundDash : Dash, IEventModule
    {
        [SerializeField]
        private float m_adhesive;
        [SerializeField, Min(0)]
        private float m_cooldown;
        private float m_modifiedDashPower;

        private IPlayerAnimationState m_animationState;

        private RaySensor m_slopeSensor;
        private CountdownTimer m_cooldownTimer;
        private bool m_isOnCooldown;



       

        public void ConnectEvents()
        {
            GetComponentInParent<IGroundDashController>().DashCall += OnDashCall;
            GetComponentInParent<ILandController>().LandCall += OnLandCall;
        }

        public override void Initialize(IPlayerModules player)
        {
            base.Initialize(player);
            m_slopeSensor = player.sensors.slopeSensor;
            m_animationState = player.animationState;
            m_state.canDash = true;
        }

        private void HandleDash()
        {
            CallDashStart();
            m_direction = m_facing.currentFacingDirection == HorizontalDirection.Left ? Vector2.left : Vector2.right;
            m_modifiedDashPower = m_power * m_modifier.dashDistance;
            m_duration.Reset();
            enabled = true;
            m_state.isDashing = true;
            if (m_ghosting != null)
                m_ghosting.enabled = true;
        }

        private void OnDashCall(object sender, EventActionArgs eventArgs)
        {
            if (m_state.canDash && m_state.isDashing == false)
            {
                HandleDash();
                m_state.canDash = false;
            }
        }

        private void OnLandCall(object sender, EventActionArgs eventArgs)
        {
            AllowDash();
            m_cooldownTimer.EndTime(false);
        }

        private void OnCooldownEnd(object sender, EventActionArgs eventArgs)
        {
            AllowDash();
        }

        protected override void OnDashDurationEnd(object sender, EventActionArgs eventArgs)
        {
            CallDashEnd();
            m_animationState.isFallingToJog = false;
            m_animationState.hasDashed = false;
            m_physics.SetVelocity(Vector2.zero);
            m_state.isDashing = false;
            if (m_ghosting != null)
            {
                m_ghosting.enabled = false;
            }

            HandleCooldown();
        }

        private void HandleCooldown()
        {
            var cooldown = m_cooldown * m_modifier.dashCooldown;
            if (cooldown > 0)
            {
                m_cooldownTimer.SetStartTime(m_cooldown * m_modifier.dashCooldown);
                m_cooldownTimer.Reset();
                m_isOnCooldown = true;
            }
            else
            {
                AllowDash();
            }
        }

        private void AllowDash()
        {
            m_state.canDash = true;
            m_isOnCooldown = false;
            enabled = false;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            m_cooldownTimer = new CountdownTimer(m_cooldown * m_modifier.dashCooldown);
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
                m_physics.SetVelocity(m_direction.x * m_modifiedDashPower * m_physics.moveAlongGround.x, m_physics.moveAlongGround.y * m_modifiedDashPower);
            }
            //if (Mathf.Abs(m_physics.groundAngle) != 0 && !m_slopeSensor.isDetecting)
            //{
            //    m_physics.SetVelocity(y: -m_adhesive);
            //}
        }

        
    }
}
