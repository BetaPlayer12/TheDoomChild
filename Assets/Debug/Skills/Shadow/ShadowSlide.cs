using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Skill
{
    public class ShadowSlide : ShadowSkill
    {
        [SerializeField]
        [MinValue(0.1)]
        private float m_power;
        [SerializeField]
        private CountdownTimer m_slideDuration;
        [SerializeField]
        private CountdownTimer m_coolOffDuration;
        [ShowInInspector]
        private bool m_canShadowSlide;

        private Animator m_animator;
        private CharacterPhysics2D m_physics;
        private IFacing m_facing;
        private IShadow m_shadow;
        private IIsolatedTime m_time;

        private bool m_isSliding;
        private bool m_isCoolingOff;
        private Vector2 m_speed;

        public override void Initialize(IPlayerModules player)
        {
            base.Initialize(player);
            m_facing = player;
            m_physics = player.physics;
            m_time = player.isolatedObject;
            //m_animator = player.animation.GetComponentInChildren<Animator>();
        }

        protected override void UseSkill()
        {
            if (m_canShadowSlide)
            {
                base.UseSkill();
                m_animator.SetBool("Crouch", true);
                m_shadow.BecomeAShadow(false);
                var direction = m_facing.currentFacingDirection == HorizontalDirection.Left ? Vector2.left : Vector2.right;
                m_speed = new Vector2(direction.x * m_power * m_physics.moveAlongGround.x, m_physics.moveAlongGround.y * m_power);
                m_isSliding = true;
                enabled = true;
            }
        }

        private void Update()
        {
            if (m_isCoolingOff)
            {
                m_animator.SetBool("Crouch", false);
                m_coolOffDuration.Tick(m_time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            if (m_isSliding)
            {
                m_physics.SetVelocity(m_speed);
                m_slideDuration.Tick(m_time.deltaTime);
            }
        }

        private void OnSlideDurationEnd(object sender, EventActionArgs eventArgs)
        {
            m_shadow.BecomeNormal();
            m_slideDuration.Reset();
            m_isSliding = false;
            m_isCoolingOff = true;
        }

        private void OnCoolOffEnd(object sender, EventActionArgs eventArgs)
        {
            m_isCoolingOff = false;
            m_coolOffDuration.Reset();
            enabled = false;
        }

        private void Start()
        {
            m_shadow = GetComponent<IShadow>();
            m_canShadowSlide = true;
            m_slideDuration.CountdownEnd += OnSlideDurationEnd;
            m_coolOffDuration.CountdownEnd += OnCoolOffEnd;
        }

    }
}
