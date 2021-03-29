using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class WhipAttack : AttackBehaviour
    {
        public enum Type
        {
            Ground_Forward,
            Ground_Overhead,
            MidAir_Forward,
            MidAir_Overhead,
            Crouch_Forward
        }

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

        private IPlayerModifer m_modifier;
        private int m_whipAttackAnimationParameter;
        private List<Type> m_executedTypes;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_executedTypes = new List<Type>();
            m_whipAttackAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.WhipAttack);
        }

        public override void Cancel()
        {
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
            m_state.canAttack = false;
            m_state.isAttacking = true;
            m_state.waitForBehaviour = true;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_whipAttackAnimationParameter, true);

            switch (type)
            {
                case Type.Ground_Forward:
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
                    break;
                case Type.MidAir_Overhead:
                    m_timer = m_midAirOverhead.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_midAirOverhead.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
                    break;
                case Type.Crouch_Forward:
                    m_timer = m_crouchForward.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_crouchForward.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
                    break;
            }
            Record(type);
        }

        public override void AttackOver()
        {
            base.AttackOver();

            m_animator.SetBool(m_whipAttackAnimationParameter, false);
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

        public void ClearExecutedCollision()
        {
            for (int i = 0; i < m_executedTypes.Count; i++)
            {
                var type = m_executedTypes[i];
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
    }
}
