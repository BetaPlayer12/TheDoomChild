using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Systems.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class EyeSentry : Minion, ITerrainPatroller
    {
        [SerializeField]
        private Damage m_damage;
        [SerializeField]
        [MinValue(0.1f)]
        private float m_speed;

        private EyeSentryAnimation m_animation;
        private PhysicsMovementHandler2D m_movement;

        protected override Damage startDamage => m_damage;

        protected override CombatCharacterAnimation animation => m_animation;

        public void Move()
        {
            var moveDirection = m_facing == HorizontalDirection.Left ? -transform.right : transform.right;
            m_movement.MoveTowards(moveDirection, m_speed);
            m_animation.DoIdle();
        }

        public void Turn()
        {
            TurnCharacter();
        }

        protected override void OnDeath()
        {
            m_animation.DoDeath();
        }

        protected override void Start()
        {
            base.Start();
            m_animation = GetComponent<EyeSentryAnimation>();
            m_movement = new PhysicsMovementHandler2D(GetComponent<IsolatedObjectPhysics2D>(), transform);
        }
    }

}