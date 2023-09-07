using DChild.Gameplay.Characters.Players.Modules;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class IcarusWings : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_icarusWingsCooldown;
        //[SerializeField]
        //private float m_icarusWingsMovementCooldown;
        [SerializeField]
        private Info m_icarusWingsInfo;
        //TEST
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_cielingSensor;
        [SerializeField, BoxGroup("FX")]
        private ParticleSystem m_dustFeedbackFX;

        [SerializeField]
        private Vector2 m_pushForce;

        private bool m_canIcarusWings;
        //private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_icarusWingsStateAnimationParameter;
        private float m_icarusWingsCooldownTimer;
        private float m_icarusWingsMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanIcarusWings() => m_canIcarusWings;

        private Coroutine m_cielingCheckRoutine;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_icarusWingsStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IcarusWings);
            m_canIcarusWings = true;
            //m_canMove = true;
            //m_icarusWingsMovementCooldownTimer = m_icarusWingsMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            m_state.waitForBehaviour = false;
            m_state.isAttacking = false;
            m_canIcarusWings = true;
            //m_icarusWingsInfo.ShowCollider(false);
            m_icarusWingsInfo.PlayFX(false);
            m_animator.SetBool(m_icarusWingsStateAnimationParameter, false);
            base.Reset();
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canIcarusWings = false;
            //m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_icarusWingsStateAnimationParameter, true);
            m_icarusWingsCooldownTimer = m_icarusWingsCooldown;
            //m_icarusWingsMovementCooldownTimer = m_icarusWingsMovementCooldown;
            m_cielingCheckRoutine = StartCoroutine(CielingCheckRoutine());
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            //m_icarusWingsInfo.ShowCollider(false);
            //m_canicarusWings = true;
            //m_canMove = true;
            m_icarusWingsInfo.PlayFX(false);
            m_physics.velocity = Vector2.zero;
            if (m_cielingCheckRoutine != null)
            {
                StopCoroutine(m_cielingCheckRoutine);
                m_cielingCheckRoutine = null;
            }
            m_animator.SetBool(m_icarusWingsStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            if (m_cielingCheckRoutine != null)
            {
                StopCoroutine(m_cielingCheckRoutine);
                m_cielingCheckRoutine = null;
            }
            m_icarusWingsInfo.PlayFX(false);
            //m_icarusWingsInfo.ShowCollider(false);
            m_fxAnimator.Play("Buffer");
            m_animator.SetBool(m_icarusWingsStateAnimationParameter, false);
            base.Cancel();
        }

        //public void EnableCollision(bool value)
        //{
        //    m_rigidBody.WakeUp();
        //    m_icarusWingsInfo.ShowCollider(value);

        //    //TEST
        //    m_enemySensor.Cast();
        //    m_wallSensor.Cast();
        //    m_edgeSensor.Cast();
        //    if (/*!m_enemySensor.isDetecting && !m_wallSensor.allRaysDetecting && */m_edgeSensor.isDetecting && value)
        //    {
        //        m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
        //    }
        //    if (value)
        //    {
        //        m_icarusWingsInfo.fxPosition.localRotation = Quaternion.Euler(0, m_character.facing == HorizontalDirection.Right ? 180 : 0, 0);
        //        m_icarusWingsInfo.PlayFX(true);
        //    }
        //}

        public void Jump()
        {
            m_rigidBody.WakeUp();

            m_dustFeedbackFX.Play();
            m_icarusWingsInfo.PlayFX(true);
            m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            if (m_icarusWingsInfo.fxPosition != null)
            {
                m_icarusWingsInfo.fxPosition.localRotation = Quaternion.Euler(0, m_character.facing == HorizontalDirection.Right ? 180 : 0, 0);
                m_icarusWingsInfo.PlayFX(true);
            }
        }

        public void HandleAttackTimer()
        {
            if (m_icarusWingsCooldownTimer > 0)
            {
                m_icarusWingsCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canIcarusWings = false;
            }
            else
            {
                m_icarusWingsCooldownTimer = m_icarusWingsCooldown;
                //m_state.isAttacking = false;
                m_canIcarusWings = true;
            }
        }

        private IEnumerator CielingCheckRoutine()
        {
            m_cielingSensor.Cast();
            while (!m_cielingSensor.isDetecting)
            {
                m_cielingSensor.Cast();
                yield return null;
            }
            m_animator.SetBool(m_icarusWingsStateAnimationParameter, false);
            yield return null;
        }

        //public void HandleMovementTimer()
        //{
        //    if (m_icarusWingsMovementCooldownTimer > 0)
        //    {
        //        m_icarusWingsMovementCooldownTimer -= GameplaySystem.time.deltaTime;
        //        m_canMove = false;
        //    }
        //    else
        //    {
        //        //Debug.Log("Can Move");
        //        m_icarusWingsMovementCooldownTimer = m_icarusWingsMovementCooldown;
        //        m_canMove = true;
        //    }
        //}
    }
}
