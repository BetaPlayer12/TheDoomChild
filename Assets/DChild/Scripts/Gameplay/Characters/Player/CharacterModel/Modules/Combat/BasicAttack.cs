﻿using System;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using DChild.Inputs;
using Holysoft.Collections;
using Holysoft.Event;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using Spine;
using UnityEngine;
using DChild.Gameplay.Combat;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class BasicAttack : MonoBehaviour, IComplexCharacterModule, IControllableModule
    {
        private ICombatState m_state;
        private Animator m_animator;
        private Character m_character;
        private IsolatedPhysics2D m_physics;
        private string m_attackTriggerParameter;
        private string m_attackDirectionParameter;
        [SerializeField]
        private ParticleSystem m_forwardSlashFx1;
        [SerializeField]
        private ParticleSystem m_forwardSlashFx2;
        [SerializeField]
        private ParticleSystem m_upwardSlashfx;
        [SerializeField]
        private ParticleSystem m_downardSlashfx;

        [SerializeField,MinValue(1)]
        private float m_force;
        private Vector2 m_forceDirection;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_state = info.state;
            m_character = info.character;
            m_animator = info.animator;
            m_physics = info.physics;
            m_attackTriggerParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Attack);
            m_attackDirectionParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.AttackYDirection);
            info.attacker.BreakableObjectDamage += OnBreakableObjectDamage;
        }
      
        public void ConnectTo(IMainController controller)
        {
            controller.ControllerDisabled += OnControllerDisabled;
        }

        public void Execute()
        {
            m_animator.SetTrigger(m_attackTriggerParameter);
            m_state.canAttack = false;
            m_state.waitForBehaviour = true;
            if (m_state.isGrounded)
            {
                m_physics.SetVelocity(0, 0);
            }
        }

        public void SetAttackDirection(DirectionalInput input)
        {

            if (input.isDownHeld)
            {
                m_forceDirection = Vector2.down;
                m_animator.SetInteger(m_attackDirectionParameter, -1);
                m_downardSlashfx.Play();
            }
            else if (input.isUpHeld)
            {
                m_forceDirection = Vector2.up;
                m_animator.SetInteger(m_attackDirectionParameter, 1);
                m_upwardSlashfx.Play();
            }
            else
            {
                m_forceDirection = m_character.facing == HorizontalDirection.Right ? Vector2.right : Vector2.left;
                m_animator.SetInteger(m_attackDirectionParameter, 0);
                m_forwardSlashFx1.Play();
            }
        }

        private void OnControllerDisabled(object sender, EventActionArgs eventArgs)
        {
            m_state.canAttack = true;
            m_state.waitForBehaviour = false;
        }

        private void OnBreakableObjectDamage(object sender, BreakableObjectEventArgs eventArgs)
        {
            eventArgs.instance.RecordForceReceived(m_forceDirection, m_force);
        }
    }
}