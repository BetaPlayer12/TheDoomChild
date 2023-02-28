using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class FireFist : AttackBehaviour
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_fireFistCooldown;
        [SerializeField]
        private float m_fireFistMovementCooldown;
        [SerializeField]
        private Info m_fireFistInfo;
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

        [SerializeField]
        private Vector2 m_pushForce;

        [SerializeField, BoxGroup("Temporary")]
        private Transform m_lichLordArmTF;
        [SerializeField, BoxGroup("Temporary")]
        private Transform m_lichLordArmHurtBox;
        [SerializeField, TabGroup("Temporary")]
        private ParticleSystem m_lichArmGroundFX;
        private Vector2 m_lichLordArmLocalPos;

        private bool m_canFireFist;
        private bool m_canMove;
        private IPlayerModifer m_modifier;
        private int m_fireFistStateAnimationParameter;
        private float m_fireFistCooldownTimer;
        private float m_fireFistMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanFireFist() => m_canFireFist;
        public bool CanMove() => m_canMove;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_fireFistStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.FireFist);
            m_canFireFist = true;
            m_canMove = true;
            m_fireFistMovementCooldownTimer = m_fireFistMovementCooldown;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
            m_lichLordArmLocalPos = m_lichLordArmTF.localPosition;
            m_lichLordArmTF.GetComponent<Attacker>().SetParentAttacker(m_attacker);
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            base.Reset();
            //m_fireFistInfo.ShowCollider(false);
            StopAllCoroutines();
            m_animator.SetBool(m_fireFistStateAnimationParameter, false);
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canFireFist = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_fireFistStateAnimationParameter, true);
            m_fireFistCooldownTimer = m_fireFistCooldown;
            m_fireFistMovementCooldownTimer = m_fireFistMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            //m_fireFistInfo.ShowCollider(false);
            //m_canFireFist = true;
            StopAllCoroutines();
            m_canMove = true;
            m_animator.SetBool(m_fireFistStateAnimationParameter, false);
            base.AttackOver();
        }

        public override void Cancel()
        {
            //m_fireFistInfo.ShowCollider(false);
            StopAllCoroutines();
            m_fxAnimator.Play("Buffer");
            m_animator.SetBool(m_fireFistStateAnimationParameter, false);
            base.Cancel();
        }

        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_fireFistInfo.ShowCollider(value);
            m_attackFX.transform.position = m_fireFistInfo.fxPosition.position;

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
            if (m_fireFistCooldownTimer > 0)
            {
                m_fireFistCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canFireFist = false;
            }
            else
            {
                m_fireFistCooldownTimer = m_fireFistCooldown;
                //m_state.isAttacking = false;
                m_canFireFist = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_fireFistMovementCooldownTimer > 0)
            {
                m_fireFistMovementCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canMove = false;
            }
            else
            {
                //Debug.Log("Can Move");
                m_fireFistMovementCooldownTimer = m_fireFistMovementCooldown;
                m_canMove = true;
            }
        }

        public void Summon()
        {
            StartCoroutine(SkeletalArmRoutine());
        }


        private IEnumerator SkeletalArmRoutine()
        {
            //Reset
            m_lichLordArmTF.SetParent(this.transform);
            m_lichLordArmTF.localPosition = m_lichLordArmLocalPos;
            m_lichArmGroundFX.Stop();
            //
            m_lichLordArmTF.SetParent(null);
            m_lichLordArmTF.gameObject.SetActive(true);
            m_lichLordArmTF.GetComponentInChildren<SkeletonAnimation>().state.SetAnimation(0, "Phase_1_Arm_Attack", false).MixDuration = 0;
            m_lichLordArmTF.localScale = new Vector3(m_character.facing == HorizontalDirection.Right ? 1 : -1, 1, 1);
            m_lichArmGroundFX.Play();
            yield return new WaitForSeconds(.1f);
            m_lichLordArmHurtBox.gameObject.SetActive(true);
            m_lichLordArmTF.GetComponentInChildren<SkeletonRenderer>().maskInteraction = SpriteMaskInteraction.None;
            yield return new WaitForAnimationComplete(m_lichLordArmTF.GetComponentInChildren<SkeletonAnimation>().state, m_lichLordArmTF.GetComponentInChildren<SkeletonAnimation>().state.GetCurrent(0).Animation.ToString());
            //yield return new WaitForSeconds(5f);
            //m_lichLordArmTF.gameObject.SetActive(false);
            m_lichLordArmTF.GetComponentInChildren<SkeletonRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            m_lichLordArmHurtBox.gameObject.SetActive(false);
            yield return null;
        }
    }
}
