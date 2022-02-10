using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{

    public class BasicSlashes : AttackBehaviour
    {
        public enum Type
        {
            Ground_Overhead,
            Crouch,
            MidAir_Forward,
            MidAir_Overhead
        }

        [SerializeField]
        private Info m_groundOverhead;
        [SerializeField]
        private Info m_crouch;
        [SerializeField]
        private Info m_midAirForward;
        [SerializeField]
        private Info m_midAirOverhead;
        [SerializeField]
        private float m_aerialGravity;

        private IPlayerModifer m_modifier;
        private List<Type> m_executedTypes;
        private Rigidbody2D m_rigidbody;
        private float m_cacheGravity;
        private bool m_adjustGravity;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            m_rigidbody = info.rigidbody;
            m_modifier = info.modifier;
            m_executedTypes = new List<Type>();
            m_cacheGravity = m_rigidbody.gravityScale;
            m_adjustGravity = true;
        }

        public override void Cancel()
        {
            m_rigidbody.gravityScale = m_cacheGravity;
            m_adjustGravity = true;

            if (m_executedTypes.Count > 0)
            {
                base.Cancel();
                for (int i = 0; i < m_executedTypes.Count; i++)
                {
                    var type = m_executedTypes[i];
                    EnableCollision(type, false);
                    PlayFXFor(type, false);
                    //ClearFXFor(type);
                }
                m_executedTypes.Clear();
            }
        }

        public void EnableCollision(Type type, bool value)
        {
            m_rigidBody.WakeUp();

            switch (type)
            {
                case Type.Ground_Overhead:
                    m_groundOverhead.ShowCollider(value);
                    break;
                case Type.Crouch:
                    m_crouch.ShowCollider(value);
                    break;
                case Type.MidAir_Forward:
                    m_midAirForward.ShowCollider(value);
                    break;
                case Type.MidAir_Overhead:
                    m_midAirOverhead.ShowCollider(value);
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
            m_state.canAttack = false;
            m_state.isAttacking = true;
            m_state.waitForBehaviour = true;
            m_animator.SetBool(m_animationParameter, true);

            switch (type)
            {
                case Type.Ground_Overhead:
                    m_timer = m_groundOverhead.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_groundOverhead.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
                    break;
                case Type.Crouch:
                    m_timer = m_crouch.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_crouch.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
                    break;
                case Type.MidAir_Forward:
                    m_timer = m_midAirForward.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_midAirForward.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));

                    if (m_adjustGravity == true)
                    {
                        m_cacheGravity = m_rigidbody.gravityScale;
                        m_rigidbody.gravityScale = m_aerialGravity;
                        m_rigidbody.velocity = new Vector2(m_rigidBody.velocity.x, 0);
                    }
                    break;
                case Type.MidAir_Overhead:
                    m_timer = m_midAirOverhead.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_midAirOverhead.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));

                    if (m_adjustGravity == true)
                    {
                        m_cacheGravity = m_rigidbody.gravityScale;
                        m_rigidbody.gravityScale = m_aerialGravity;
                        m_rigidbody.velocity = new Vector2(m_rigidBody.velocity.x, 0);
                    }
                    break;
            }
            Record(type);
        }

        public void PlayFXFor(Type type, bool play)
        {
            switch (type)
            {
                case Type.Ground_Overhead:
                    m_groundOverhead.PlayFX(play);
                    break;
                case Type.Crouch:
                    m_crouch.PlayFX(play);
                    break;
                case Type.MidAir_Forward:
                    m_midAirForward.PlayFX(play);
                    break;
                case Type.MidAir_Overhead:
                    m_midAirOverhead.PlayFX(play);
                    break;
            }
        }

        public override void AttackOver()
        {
            base.AttackOver();

            if (m_state.isDoingCombo == true)
            {
                m_state.isDoingCombo = false;
            }

            m_rigidbody.gravityScale = m_cacheGravity;
            m_adjustGravity = false;
        }

        public void ClearFXFor(Type type)
        {
            switch (type)
            {
                case Type.Ground_Overhead:
                    m_groundOverhead.ClearFX();
                    break;
                case Type.Crouch:
                    m_crouch.ClearFX();
                    break;
                case Type.MidAir_Forward:
                    m_midAirForward.ClearFX();
                    break;
                case Type.MidAir_Overhead:
                    m_midAirOverhead.ClearFX();
                    break;
            }
        }

        public void HandleNextAttackDelay()
        {
            if (m_timer >= 0)
            {
                m_timer -= GameplaySystem.time.deltaTime;
                if (m_timer <= 0)
                {
                    m_timer = -1;
                    m_state.canAttack = true;
                }
            }
        }

        public void ResetAerialGravityControl()
        {
            m_adjustGravity = true;
        }

        public void ClearExecutedCollision()
        {
            //for (int i = 0; i < m_executedTypes.Count; i++)
            //{
            //    var type = m_executedTypes[i];
            //    EnableCollision(type, false);
            //    Debug.Log("Clear");
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

            Debug.Log(m_executedTypes.Count);
        }

        public (int index, float value) getSomething() { return (0, 1f); }
    }
}
