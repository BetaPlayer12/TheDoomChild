using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class FoolsVerdict : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_foolsVerdictCooldown;
        [SerializeField]
        private float m_foolsVerdictMovementCooldown;
        [SerializeField]
        private Info m_foolsVerdictInfo;
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

        [SerializeField, BoxGroup("FoolsVerdict")]
        private GameObject m_foolsVerdictGO;
        [SerializeField, BoxGroup("FoolsVerdict")]
        private Transform m_foolsVerdictTF;
        [SerializeField, BoxGroup("FoolsVerdict")]
        private SpineFX m_foolsVerdictStartAnimation;
        private Vector2 m_foolsVerdictLocalPos;

        [SerializeField]
        private Vector2 m_pushForce;

        private bool m_canFoolsVerdict;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_foolsVerdictStateAnimationParameter;
        private float m_foolsVerdictCooldownTimer;
        private float m_foolsVerdictMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanFoolsVerdict() => m_canFoolsVerdict;
        public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_foolsVerdictStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.FoolsVerdict);
            m_canFoolsVerdict = true;
            m_canMove = true;
            m_foolsVerdictMovementCooldownTimer = m_foolsVerdictMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
            m_foolsVerdictLocalPos = m_foolsVerdictTF.localPosition;
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            base.Reset();
            //m_foolsVerdictInfo.ShowCollider(false);
            StopAllCoroutines();
            m_animator.SetBool(m_foolsVerdictStateAnimationParameter, false);
            if (m_foolsVerdictGO.activeSelf)
                m_foolsVerdictStartAnimation.Stop();
            m_foolsVerdictGO.SetActive(false);
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canFoolsVerdict = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_foolsVerdictStateAnimationParameter, true);
            m_foolsVerdictCooldownTimer = m_foolsVerdictCooldown;
            m_foolsVerdictMovementCooldownTimer = m_foolsVerdictMovementCooldown;
            m_foolsVerdictGO.SetActive(true);
            m_foolsVerdictStartAnimation.Play();
            m_foolsVerdictGO.GetComponent<Attacker>().SetParentAttacker(m_attacker);
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            //m_foolsVerdictInfo.ShowCollider(false);
            //m_canFoolsVerdict = true;
            m_canMove = true;
            StopAllCoroutines();
            if (m_foolsVerdictGO.activeSelf)
                m_foolsVerdictStartAnimation.Stop();
            m_foolsVerdictGO.SetActive(false);
            m_animator.SetBool(m_foolsVerdictStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            //m_foolsVerdictInfo.ShowCollider(false);
            StopAllCoroutines();
            m_fxAnimator.Play("Buffer");
            if (m_foolsVerdictGO.activeSelf)
                m_foolsVerdictStartAnimation.Stop();
            m_foolsVerdictGO.SetActive(false);
            m_animator.SetBool(m_foolsVerdictStateAnimationParameter, false);
            base.Cancel();
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_foolsVerdictInfo.ShowCollider(value);
            m_attackFX.transform.position = m_foolsVerdictInfo.fxPosition.position;

            //TEST
            m_enemySensor.Cast();
            m_wallSensor.Cast();
            m_edgeSensor.Cast();
            if (!m_enemySensor.isDetecting && !m_wallSensor.allRaysDetecting && m_edgeSensor.isDetecting && value)
            {
                m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
            }
        }

        public void HandleAttackTimer()
        {
            if (m_foolsVerdictCooldownTimer > 0)
            {
                m_foolsVerdictCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canFoolsVerdict = false;
            }
            else
            {
                m_foolsVerdictCooldownTimer = m_foolsVerdictCooldown;
                //m_state.isAttacking = false;
                m_canFoolsVerdict = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_foolsVerdictMovementCooldownTimer > 0)
            {
                m_foolsVerdictMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                //Debug.Log("Can Move");
                m_foolsVerdictMovementCooldownTimer = m_foolsVerdictMovementCooldown;
                m_canMove = true;
            }
        }

        public void Summon()
        {
            StartCoroutine(HammerRoutine());
        }


        private IEnumerator HammerRoutine()
        {
            //Reset
            m_foolsVerdictTF.SetParent(this.transform);
            m_foolsVerdictTF.localPosition = m_foolsVerdictLocalPos;
            //
            m_foolsVerdictTF.SetParent(null);
            m_foolsVerdictTF.gameObject.SetActive(true);

            m_foolsVerdictGO.SetActive(true);
            yield return new WaitForSeconds(.1f);
            m_foolsVerdictStartAnimation.Play();
            m_foolsVerdictTF.localScale = new Vector3(m_character.facing == HorizontalDirection.Right ? -1 : 1, 1, 1);
            //m_lichLordArmHurtBox.gameObject.SetActive(true);
            //m_foolsVerdictTF.GetComponentInChildren<SkeletonRenderer>().maskInteraction = SpriteMaskInteraction.None;
            //yield return new WaitForAnimationComplete(m_lichLordArmTF.GetComponentInChildren<SkeletonAnimation>().state, m_lichLordArmTF.GetComponentInChildren<SkeletonAnimation>().state.GetCurrent(0).Animation.ToString());
            //m_foolsVerdictTF.GetComponentInChildren<SkeletonRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            //m_foolsVerdictTF.gameObject.SetActive(false);
            yield return null;
        }
    }
}
