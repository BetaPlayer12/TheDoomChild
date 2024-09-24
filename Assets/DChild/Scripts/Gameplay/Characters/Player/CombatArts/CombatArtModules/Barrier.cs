using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Spine.Unity;
using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class Barrier : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        //[SerializeField]
        //private float m_barrierCooldown;
        [SerializeField]
        private float m_barrierMovementCooldown;
        [SerializeField]
        private Info m_barrierInfo;
        //TEST
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        [SerializeField]
        private Hitbox m_hitbox;
        //[SerializeField, BoxGroup("FX")]
        //private ParticleSystem m_fx;
        //[SerializeField, BoxGroup("FX")]
        //private ParticleSystem m_endFx;
        [SerializeField, BoxGroup("FX")]
        private Animator m_barrierFX;
        [SerializeField, BoxGroup("FX")]
        private MaterialReplacementExample m_materialReplacement;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_enemySensor;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_wallSensor;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_edgeSensor;

        [SerializeField]
        private Vector2 m_pushForce;

        //private bool m_canbarrier;
        private bool m_isDoingBarrier;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_barrierStateAnimationParameter;
        //private float m_barrierCooldownTimer;
        private float m_barrierMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        //public bool Canbarrier() => m_canbarrier;
        public bool CanMove() => m_canMove;
        public bool IsDoingBarrier() => m_isDoingBarrier;

        private Coroutine m_barrierHoldRoutine;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_barrierStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Barrier);
            //m_canbarrier = true;
            m_canMove = true;
            m_barrierMovementCooldownTimer = m_barrierMovementCooldown;

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
            //m_barrierInfo.ShowCollider(false);
            m_animator.SetBool(m_barrierStateAnimationParameter, false);
            base.Reset();
        }

        public void Execute()
        {
            m_barrierHoldRoutine = StartCoroutine(BarrierHoldRoutine());
            m_state.waitForBehaviour = false;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            //m_physics.velocity = Vector2.zero;
            //m_canbarrier = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_barrierStateAnimationParameter, true);
            //m_barrierCooldownTimer = m_barrierCooldown;
            m_barrierMovementCooldownTimer = m_barrierMovementCooldown;
            m_isDoingBarrier = true;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            if (m_barrierHoldRoutine != null)
            {
                StopCoroutine(m_barrierHoldRoutine);
                m_barrierHoldRoutine = null;
            }
            //Debug.Log("Barrier End");
            //m_state.waitForBehaviour = false;
            //m_barrierInfo.ShowCollider(false);

            m_barrierFX.SetBool("BarrierIsOn", false);
            m_materialReplacement.replacementEnabled = false;
            m_isDoingBarrier = false;
            m_animator.SetBool(m_barrierStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            if (m_barrierHoldRoutine != null)
            {
                StopCoroutine(m_barrierHoldRoutine);
                m_barrierHoldRoutine = null;
            }
            m_physics.velocity = Vector2.zero;
            //m_barrierInfo.ShowCollider(false);

            m_barrierFX.SetBool("BarrierIsOn", false);
            m_materialReplacement.replacementEnabled = false;
            m_isDoingBarrier = false;
            m_animator.SetBool(m_barrierStateAnimationParameter, false);
            base.Cancel();
        }

        public void EnableShield(bool value)
        {
            m_rigidBody.WakeUp();
            //m_barrierInfo.ShowCollider(value);
            m_attackFX.transform.position = m_barrierInfo.fxPosition.position;
            m_physics.velocity = Vector2.zero;

            m_hitbox.SetCanBlockDamageState(value);
            if (value)
            {
                m_barrierFX.SetBool("BarrierIsOn", true);
                m_materialReplacement.replacementEnabled = true;
            }

            m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            //TEST
            //m_enemySensor.Cast();
            //m_wallSensor.Cast();
            //m_edgeSensor.Cast();
            //if (!m_enemySensor.isDetecting && !m_wallSensor.allRaysDetecting && m_edgeSensor.isDetecting && value)
            //{
            //    m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            //}
            //else if (!value)
            //{
            //    m_physics.velocity = new Vector2(0, m_physics.velocity.y);
            //}
        }

        //public void HandleAttackTimer()
        //{
        //    if (m_barrierCooldownTimer > 0)
        //    {
        //        m_barrierCooldownTimer -= GameplaySystem.time.deltaTime;
        //        m_canbarrier = false;
        //    }
        //    else
        //    {
        //        m_barrierCooldownTimer = m_barrierCooldown;
        //        m_state.isAttacking = false;
        //        m_canbarrier = true;
        //    }
        //}

        public void HandleMovementTimer()
        {
            if (m_barrierMovementCooldownTimer > 0)
            {
                m_barrierMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                //Debug.Log("Can Move");
                m_barrierMovementCooldownTimer = m_barrierMovementCooldown;
                m_canMove = true;
            }
        }

        private IEnumerator BarrierHoldRoutine()
        {
            while (true)
            {
                m_state.waitForBehaviour = false;
                m_state.isAttacking = true;
                yield return null;
            }
        }
    }
}
