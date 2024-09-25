using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class WhipAttack : AttackBehaviour
    {
        public struct WhipAttackEventArgs : IEventActionArgs
        {
            public Type type;

            public WhipAttackEventArgs(Type type)
            {
                this.type = type;
            }
        }
        public enum Type
        {
            Ground_Forward,
            Ground_Overhead,
            MidAir_Forward,
            MidAir_Overhead,
            Crouch_Forward
        }

        [SerializeField, HideLabel]
        private WhipAttackStatsInfo m_configuration;
        //[SerializeField]
        //private float m_whipMovementCooldown;
        //[SerializeField]
        //private Vector2 m_momentumVelocity;
        [SerializeField]
        private Info m_groundForward;
        [SerializeField]
        private Info m_groundOverhead;
        [SerializeField]
        private Info m_midAirForward;
        [SerializeField]
        private Info m_midAirOverhead;
        [SerializeField]
        private Info m_crouchForward;
        //[SerializeField]
        //private float m_aerialGravity;

        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_whipAttackAnimationParameter;
        private List<Type> m_executedTypes;
        private Rigidbody2D m_rigidbody;
        private float m_cacheGravity;
        private bool m_adjustGravity;
        private bool m_canAirWhip;
        private float m_whipMovementCooldownTimer;

        public event EventAction<WhipAttackEventArgs> OnWhip;

        public bool CanMove() => m_canMove;
        public bool CanAirWhip() => m_canAirWhip;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_executedTypes = new List<Type>();
            m_whipAttackAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.WhipAttack);
            m_rigidbody = info.rigidbody;
            m_cacheGravity = m_rigidbody.gravityScale;
            m_adjustGravity = true;
            m_canAirWhip = true;
        }

        public void SetConfiguration(WhipAttackStatsInfo info)
        {
            m_configuration.CopyInfo(info);
        }

        public override void Cancel()
        {
            m_rigidbody.gravityScale = m_cacheGravity;
            m_adjustGravity = true;

            if (m_executedTypes.Count > 0)
            {
                base.Cancel();
                m_animator.SetBool(m_whipAttackAnimationParameter, false);

                for (int i = 0; i < m_executedTypes.Count; i++)
                {
                    var type = m_executedTypes[i];
                    EnableCollision(type, false);
                }

                m_executedTypes.Clear();
            }
        }

        public void EnableCollision(Type type, bool value)
        {
            m_rigidBody.WakeUp();

            switch (type)
            {
                case Type.Ground_Forward:
                    m_groundForward.ShowCollider(value);
                    break;
                case Type.Ground_Overhead:
                    m_groundOverhead.ShowCollider(value);
                    break;
                case Type.MidAir_Forward:
                    m_midAirForward.ShowCollider(value);
                    break;
                case Type.MidAir_Overhead:
                    m_midAirOverhead.ShowCollider(value);
                    break;
                case Type.Crouch_Forward:
                    m_crouchForward.ShowCollider(value);
                    break;
            }

            if (value)
            {
                Record(type);
            }
            else
            {
                m_executedTypes.Remove(type);
            }
        }

        public void Execute(Type type)
        {
            m_canMove = false;
            m_state.canAttack = false;
            m_state.isAttacking = true;
            m_state.waitForBehaviour = true;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_whipAttackAnimationParameter, true);

            switch (type)
            {
                case Type.Ground_Forward:
                    //m_state.canAttack = true;
                    //m_state.isAttacking = false;
                    //m_state.waitForBehaviour = false;
                    m_timer = m_groundForward.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_groundForward.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
                    break;
                case Type.Ground_Overhead:
                    m_timer = m_groundOverhead.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_groundOverhead.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
                    break;
                case Type.MidAir_Forward:
                    m_timer = m_midAirForward.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_midAirForward.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
                    m_canAirWhip = false;

                    if (m_adjustGravity == true)
                    {
                        m_cacheGravity = m_rigidbody.gravityScale;
                        m_rigidbody.gravityScale = /*m_aerialGravity*/m_configuration.aerialGravity;
                        m_rigidbody.velocity = /*Vector2.zero*/new Vector2(m_rigidbody.velocity.x * m_configuration.momentumVelocity.x, m_rigidbody.velocity.y * m_configuration.momentumVelocity.y);
                    }

                    break;
                case Type.MidAir_Overhead:
                    m_timer = m_midAirOverhead.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_midAirOverhead.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
                    m_canAirWhip = false;

                    if (m_adjustGravity == true)
                    {
                        m_cacheGravity = m_rigidbody.gravityScale;
                        m_rigidbody.gravityScale = /*m_aerialGravity*/m_configuration.aerialGravity;
                        m_rigidbody.velocity = /*Vector2.zero*/new Vector2(m_rigidbody.velocity.x * m_configuration.momentumVelocity.x, m_rigidbody.velocity.y * m_configuration.momentumVelocity.y);
                    }

                    break;
                case Type.Crouch_Forward:
                    m_timer = m_crouchForward.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_crouchForward.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
                    break;
            }
            Record(type);
            OnWhip?.Invoke(this, new WhipAttackEventArgs(type));
        }

        public override void AttackOver()
        {
            base.AttackOver();

            m_canMove = true;
            if (m_state.isDoingCombo == true)
            {
                m_state.isDoingCombo = false;
            }

            m_animator.SetBool(m_whipAttackAnimationParameter, false);
            m_rigidbody.gravityScale = m_cacheGravity;
            m_adjustGravity = false;
        }

        public void HandleNextAttackDelay()
        {
            if (m_timer >= 0)
            {
                m_timer -= GameplaySystem.time.deltaTime;
                if (m_timer <= 0)
                {
                    m_timer = 1;
                    m_state.canAttack = true;
                }
            }
        }

        public void ResetAerialGravityControl()
        {
            m_adjustGravity = true;
        }

        public void ResetAirAttacks()
        {
            m_canAirWhip = true;
        }

        public void ClearExecutedCollision()
        {
            //for (int i = 0; i < m_executedTypes.Count; i++)
            //{
            //    var type = m_executedTypes[i];
            //    EnableCollision(type, false);
            //}

            foreach (Type type in Enum.GetValues(typeof(Type)))
            {
                EnableCollision(type, false);
            }

            m_executedTypes.Clear();
        }

        private void Record(Type type)
        {
            if (m_executedTypes.Contains(type) == false)
            {
                m_executedTypes.Add(type);
            }
        }

        public void HandleMovementTimer()
        {
            if (m_whipMovementCooldownTimer > 0)
            {
                m_whipMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                if (!m_animator.GetBool(m_whipAttackAnimationParameter))
                {
                    //Debug.Log("Can Move");
                    m_whipMovementCooldownTimer = /*m_whipMovementCooldown*/m_configuration.whipMovementCooldown;
                    m_canMove = true;
                }
            }
        }
    }
}
