using DChild.Gameplay.Characters.Players.Modules;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class AirLunge : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_airLungeCooldown;
        [SerializeField]
        private float m_airLungeMovementCooldown;
        [SerializeField]
        private Info m_airLungeInfo;
        //TEST
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_enemySensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_cielingSensor;

        [SerializeField]
        private Vector2 m_pushForce;

        private bool m_canAirLunge;
        //private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_airLungeStateAnimationParameter;
        private float m_airLungeCooldownTimer;
        private float m_airLungeMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanAirLunge() => m_canAirLunge;

        private Coroutine m_cielingCheckRoutine;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_airLungeStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.AirLunge);
            m_canAirLunge = true;
            //m_canMove = true;
            m_airLungeMovementCooldownTimer = m_airLungeMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            base.Reset();
            m_airLungeInfo.ShowCollider(false);
            m_animator.SetBool(m_airLungeStateAnimationParameter, false);
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canAirLunge = false;
            //m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_airLungeStateAnimationParameter, true);
            m_airLungeCooldownTimer = m_airLungeCooldown;
            m_airLungeMovementCooldownTimer = m_airLungeMovementCooldown;
            m_cielingCheckRoutine = StartCoroutine(CielingCheckRoutine());
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            m_airLungeInfo.ShowCollider(false);
            //m_canAirLunge = true;
            //m_canMove = true;
            if (m_cielingCheckRoutine != null)
            {
                StopCoroutine(m_cielingCheckRoutine);
                m_cielingCheckRoutine = null;
            }
            m_animator.SetBool(m_airLungeStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            if (m_cielingCheckRoutine != null)
            {
                StopCoroutine(m_cielingCheckRoutine);
                m_cielingCheckRoutine = null;
            }
            m_airLungeInfo.ShowCollider(false);
            m_fxAnimator.Play("Buffer");
            m_animator.SetBool(m_airLungeStateAnimationParameter, false);
            base.Cancel();
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_airLungeInfo.ShowCollider(value);

            //TEST
            m_enemySensor.Cast();
            m_wallSensor.Cast();
            m_edgeSensor.Cast();
            if (/*!m_enemySensor.isDetecting && !m_wallSensor.allRaysDetecting && */m_edgeSensor.isDetecting && value)
            {
                m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            }
            if (value)
            {
                m_airLungeInfo.fxPosition.localRotation = Quaternion.Euler(0, m_character.facing == HorizontalDirection.Right ? 180 : 0, 0);
                m_airLungeInfo.PlayFX(true);
            }
        }

        public void HandleAttackTimer()
        {
            if (m_airLungeCooldownTimer > 0)
            {
                m_airLungeCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canAirLunge = false;
            }
            else
            {
                m_airLungeCooldownTimer = m_airLungeCooldown;
                m_state.isAttacking = false;
                m_canAirLunge = true;
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
            m_animator.SetBool(m_airLungeStateAnimationParameter, false);
            yield return null;
        }

        //public void HandleMovementTimer()
        //{
        //    if (m_airLungeMovementCooldownTimer > 0)
        //    {
        //        m_airLungeMovementCooldownTimer -= GameplaySystem.time.deltaTime;
        //        m_canMove = false;
        //    }
        //    else
        //    {
        //        //Debug.Log("Can Move");
        //        m_airLungeMovementCooldownTimer = m_airLungeMovementCooldown;
        //        m_canMove = true;
        //    }
        //}
    }
}
