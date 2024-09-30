using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class BasicSlashes : AttackBehaviour
    {
        public struct BasicSlashEventArgs : IEventActionArgs
        {
            public Type type;

            public BasicSlashEventArgs(Type type)
            {
                this.type = type;
            }
        }

        public enum Type
        {
            Ground_Overhead,
            Crouch,
            MidAir_Forward,
            MidAir_Overhead
        }

        [SerializeField, HideLabel]
        private BasicSlashesStatsInfo m_configuration;

        //[SerializeField]
        //private Vector2 m_momentumVelocity;
        [SerializeField]
        private SkeletonAnimation m_attackFX;
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
        private bool m_canAirAttack;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public event EventAction<BasicSlashEventArgs> OnSlash;

        public bool CanAirAttack() => m_canAirAttack;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            m_rigidbody = info.rigidbody;
            m_modifier = info.modifier;
            m_executedTypes = new List<Type>();
            m_cacheGravity = m_rigidbody.gravityScale;
            m_adjustGravity = true;
            m_canAirAttack = true;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
        }

        public void SetConfiguration(BasicSlashesStatsInfo info)
        {
            m_configuration.CopyInfo(info);
        }

        public override void Cancel()
        {
            m_rigidbody.gravityScale = m_cacheGravity;
            m_adjustGravity = true;
            m_canAirAttack = false;

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

            m_rigidBody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
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
                    m_rigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    break;
                case Type.Crouch:
                    m_timer = m_crouch.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_crouch.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
                    break;
                case Type.MidAir_Forward:
                    m_timer = m_midAirForward.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_midAirForward.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
                    m_canAirAttack = false;

                    if (m_adjustGravity == true)
                    {
                        m_cacheGravity = m_rigidbody.gravityScale;
                        m_rigidbody.gravityScale = m_aerialGravity;
                        //m_rigidbody.velocity = new Vector2(m_rigidBody.velocity.x, 0);
                        m_rigidbody.velocity = /*Vector2.zero*/new Vector2(m_rigidbody.velocity.x * m_configuration.momentumVelocity.x, m_rigidbody.velocity.y * m_configuration.momentumVelocity.y);
                    }
                    break;
                case Type.MidAir_Overhead:
                    m_timer = m_midAirOverhead.nextAttackDelay;
                    m_attacker.SetDamageModifier(m_midAirOverhead.damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
                    m_canAirAttack = false;

                    if (m_adjustGravity == true)
                    {
                        m_cacheGravity = m_rigidbody.gravityScale;
                        m_rigidbody.gravityScale = m_aerialGravity;
                        //m_rigidbody.velocity = new Vector2(m_rigidBody.velocity.x, 0);
                        m_rigidbody.velocity = /*Vector2.zero*/new Vector2(m_rigidbody.velocity.x * m_configuration.momentumVelocity.x, m_rigidbody.velocity.y * m_configuration.momentumVelocity.y);
                    }
                    break;
            }
            Record(type);

            OnSlash?.Invoke(this, new BasicSlashEventArgs(type));
        }

        public void PlayFXFor(Type type, bool play)
        {
            switch (type)
            {
                case Type.Ground_Overhead:
                    m_groundOverhead.PlayFX(play);
                    m_attackFX.transform.position = m_groundOverhead.fxPosition.position;
                    m_fxAnimator.SetTrigger("GroundOverhead");
                    break;
                case Type.Crouch:
                    m_crouch.PlayFX(play);
                    m_attackFX.transform.position = m_crouch.fxPosition.position;
                    m_fxAnimator.SetTrigger("Crouch");
                    break;
                case Type.MidAir_Forward:
                    m_midAirForward.PlayFX(play);
                    m_attackFX.transform.position = m_midAirForward.fxPosition.position;
                    m_fxAnimator.Play("JumpSlash");
                    break;
                case Type.MidAir_Overhead:
                    m_midAirOverhead.PlayFX(play);
                    m_attackFX.transform.position = m_midAirOverhead.fxPosition.position;
                    m_fxAnimator.SetTrigger("JumpOverhead");
                    break;
            }
        }

        public override void AttackOver()
        {
            base.AttackOver();

            //Debug.Log("BASIC SLASHES ATTACK OVER");
            if (m_state.isDoingCombo == true)
            {
                m_state.isDoingCombo = false;
            }

            m_rigidbody.gravityScale = m_cacheGravity;
            m_adjustGravity = false;
            //m_fxAnimator.Play("Buffer");
            //test.state.AddEmptyAnimation(0, 0, 0);
            //test.state.ClearTrack(0);
            m_skeletonAnimation.state.SetEmptyAnimation(0, 0);
            //m_fxAnimator.Play("Buffer");
            m_rigidBody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
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

        public void ResetAttackDelay()
        {
            m_timer = 1;
            m_state.canAttack = true;
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
            m_canAirAttack = true;
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

            //Debug.Log(m_executedTypes.Count);
        }

        public (int index, float value) getSomething() { return (0, 1f); }
    }
}
