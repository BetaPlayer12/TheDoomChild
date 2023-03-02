using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class ChampionsUprising : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_championsUprisingCooldown;
        [SerializeField]
        private float m_championsUprisingMovementCooldown;
        [SerializeField]
        private float m_dashDuration;
        [SerializeField]
        private Info m_championsUprisingInfo;
        //TEST
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        private float m_cacheGravity;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_enemySensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_edgeSensor;
        //[SerializeField, BoxGroup("FX")]
        //private GameObject m_fxParent;
        //[SerializeField, BoxGroup("FX")]
        //private SpineFX m_fx;

        [SerializeField]
        private Vector2 m_dashForce;
        [SerializeField]
        private Vector2 m_impactForce;

        private bool m_canChampionsUprising;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_championsUprisingStateAnimationParameter;
        private float m_championsUprisingCooldownTimer;
        private float m_championsUprisingMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanChampionsUprising() => m_canChampionsUprising;
        public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_championsUprisingStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.ChampionsUprising);
            m_canChampionsUprising = true;
            m_canMove = true;
            m_championsUprisingMovementCooldownTimer = m_championsUprisingMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
            m_cacheGravity = m_physics.gravityScale;
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            base.Reset();
            m_championsUprisingInfo.ShowCollider(false);
            m_animator.SetBool(m_championsUprisingStateAnimationParameter, false);
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            StopAllCoroutines();
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canChampionsUprising = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_championsUprisingStateAnimationParameter, true);
            m_championsUprisingCooldownTimer = m_championsUprisingCooldown;
            m_championsUprisingMovementCooldownTimer = m_championsUprisingMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));

            m_physics.velocity = Vector2.zero;
            //m_fxParent.SetActive(true);
        }

        public void EndExecution()
        {
            m_championsUprisingInfo.ShowCollider(false);
            //m_canchampionsUprising = true;
            m_canMove = true;
            //m_championsUprisingAnimation.gameObject.SetActive(false);
            m_physics.gravityScale = m_cacheGravity;
            m_animator.SetBool(m_championsUprisingStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            m_championsUprisingInfo.ShowCollider(false);
            m_canMove = true;
            m_fxAnimator.Play("Buffer");
            StopAllCoroutines();
            //m_championsUprisingAnimation.gameObject.SetActive(false);
            m_physics.gravityScale = m_cacheGravity;
            m_animator.SetBool(m_championsUprisingStateAnimationParameter, false);
            base.Cancel();
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_championsUprisingInfo.ShowCollider(value);
            m_attackFX.transform.position = m_championsUprisingInfo.fxPosition.position;
        }

        public void StartDash()
        {
            StartCoroutine(DashRoutine());
        }

        public void StartUppercut()
        {
            m_physics.velocity = new Vector2(m_character.facing == HorizontalDirection.Right ? m_impactForce.x : -m_impactForce.x, m_impactForce.y);
        }

        public void HandleAttackTimer()
        {
            if (m_championsUprisingCooldownTimer > 0)
            {
                m_championsUprisingCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canChampionsUprising = false;
            }
            else
            {
                m_championsUprisingCooldownTimer = m_championsUprisingCooldown;
                //m_state.isAttacking = false;
                m_canChampionsUprising = true;
            }
        }

        //public void HandleMovementTimer()
        //{
        //    if (m_championsUprisingMovementCooldownTimer > 0)
        //    {
        //        m_championsUprisingMovementCooldownTimer -= GameplaySystem.time.deltaTime;
        //        m_canMove = false;
        //    }
        //    else
        //    {
        //        //Debug.Log("Can Move");
        //        m_championsUprisingMovementCooldownTimer = m_championsUprisingMovementCooldown;
        //        m_canMove = true;
        //    }
        //}

        private IEnumerator DashRoutine()
        {
            m_state.waitForBehaviour = true;
            //switch (m_currentState)
            //{
            //    case championsUprisingState.Grounded:
            //        m_championsUprisingAnimation.ImpactGrounded();
            //        break;
            //    case championsUprisingState.Midair:
            //        m_championsUprisingAnimation.ImpactMidair();
            //        break;
            //}
            //m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            var timer = m_dashDuration;
            m_wallSensor.Cast();
            m_enemySensor.Cast();
            m_edgeSensor.Cast();
            while (timer >= 0 && !m_wallSensor.allRaysDetecting && !m_enemySensor.isDetecting)
            {
                m_wallSensor.Cast();
                m_enemySensor.Cast();
                m_edgeSensor.Cast();
                m_physics.velocity = new Vector2(m_character.facing == HorizontalDirection.Right ? m_dashForce.x : -m_dashForce.x, m_physics.velocity.y);
                timer -= Time.deltaTime;
                if (!m_edgeSensor.isDetecting)
                {
                    timer = -1;
                }
                yield return null;
            }
            m_enemySensor.Cast();
            if (timer <= 0 && !m_enemySensor.isDetecting)
            {
                m_physics.velocity = new Vector2(0, m_physics.velocity.y);
                EndExecution();
                m_state.waitForBehaviour = true;
            }
            else
                m_animator.SetBool(m_championsUprisingStateAnimationParameter, false);
            yield return null;
        }
    }
}
