using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class EarthShaker : AttackBehaviour
    {
        [SerializeField, MinValue(0.1f)]
        private float m_fallSpeed;
        [SerializeField]
        private ParticleSystem m_chargeFX;
        [SerializeField]
        private ParticleSystem m_preLoopFX;
        [SerializeField]
        private ParticleSystem m_fallLoopFX;
        [SerializeField]
        private Collider2D m_fallCollider;
        [SerializeField]
        private ParticleSystem m_impactFX;
        [SerializeField]
        private Collider2D m_impactCollider;
        [SerializeField, MinValue(0)]
        private float m_fallDamageModifier = 1;
        [SerializeField, MinValue(0)]
        private float m_impactDamageModifier = 1;

        private IPlayerModifer m_modifier;
        private Rigidbody2D m_rigidbody;
        private Damageable m_damageable; 
        private int m_earthShakerAnimationParameter;
        private float m_originalGravity;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            m_modifier = info.modifier;
            m_rigidbody = info.rigidbody;
            m_damageable = info.damageable;
            m_originalGravity = m_rigidbody.gravityScale;
            m_earthShakerAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.EarthShaker);
        }

        public override void Cancel()
        {
            base.Cancel();
            m_chargeFX?.Stop(true);
            m_preLoopFX?.Stop(true);
            m_fallLoopFX?.Stop(true);
            m_fallLoopFX?.Clear();
            m_fallCollider.enabled = false;
            m_impactFX?.Stop(true);
            m_impactCollider.enabled = false;
            m_rigidbody.gravityScale = m_originalGravity;
            m_rigidbody.velocity = Vector2.zero;
            m_animator.SetBool(m_earthShakerAnimationParameter, false);
        }

        public void Impact()
        {
            m_attacker.SetDamageModifier(m_impactDamageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
            m_rigidBody.WakeUp();
            m_fallLoopFX?.Stop(true);
            m_fallCollider.enabled = false;
            m_impactFX?.Play(true);
            m_impactCollider.enabled = true;
            m_rigidbody.velocity = Vector2.zero;
            m_animator.SetBool(m_earthShakerAnimationParameter, false);
            m_state.waitForBehaviour = true;
        }

        public void HandlePreFall()
        {
            m_chargeFX?.Stop(true);
            m_preLoopFX?.Play(true);
            m_fallCollider.enabled = true;
            m_rigidbody.gravityScale = m_originalGravity;
            m_rigidbody.velocity = Vector2.down * m_fallSpeed;
        }

        public void HandleFall()
        {
            if ((m_fallLoopFX?.isPlaying ?? false) == false)
            {
                m_preLoopFX?.Stop(true);
                m_fallLoopFX?.Play(true);
            }
            m_rigidbody.velocity = Vector2.down * m_fallSpeed;
        }

        public void StartExecution()
        {
            m_damageable.SetInvulnerability(Invulnerability.Level_1);
            m_attacker.SetDamageModifier(m_fallDamageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
            m_rigidbody.velocity = Vector2.zero;
            m_originalGravity = m_rigidbody.gravityScale;
            m_rigidbody.gravityScale = 0;
            m_chargeFX?.Play(true);
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_earthShakerAnimationParameter, true);
        }

        public void EndExecution()
        {
            m_damageable.SetInvulnerability(Invulnerability.None);
            m_impactFX?.Stop(true);
            m_animator.SetBool(m_animationParameter, false);
            m_animator.SetBool(m_earthShakerAnimationParameter, false);
            m_impactCollider.enabled = false;
            m_state.waitForBehaviour = false;
            m_state.canAttack = true;
            m_state.isAttacking = false;
            m_rigidbody.gravityScale = m_originalGravity;
        }
    }
}
