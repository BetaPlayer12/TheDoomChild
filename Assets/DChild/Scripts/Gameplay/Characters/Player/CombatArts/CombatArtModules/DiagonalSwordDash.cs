using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class DiagonalSwordDash : AttackBehaviour, IInterruptableCombatArtModule, IPlayerCritAttack
    {
        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_diagonalSwordDashCooldown;
        [SerializeField]
        private float m_diagonalSwordDashMovementCooldown;
        [SerializeField]
        private float m_dashDuration;
        [SerializeField]
        private Info m_diagonalSwordDashInfo;

        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        private float m_cacheGravity;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, BoxGroup("Sensors")]
        private RaySensor m_enemySensor;
        [SerializeField, BoxGroup("FX")]
        private Animator m_diagonalSwordDashFXAnimator;
        [SerializeField, BoxGroup("FX")]
        private GameObject m_diagonalSwordDashImpactFX;
        [SerializeField, BoxGroup("FX")]
        private GameObject m_diagonalSwordDashGroundImpactFX;

        [SerializeField]
        private Vector2 m_pushForce;
        [SerializeField]
        private Vector2 m_backForce;

        private bool m_canDiagonalSwordDash;
        private bool m_canMove;
        private bool m_canReset;
        private IPlayerModifer m_modifier;
        private int m_diagonalSwordDashStateAnimationParameter;
        private float m_diagonalSwordDashCooldownTimer;
        private float m_diagonalSwordDashMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanDiagonalSwordDash() => m_canDiagonalSwordDash;
        public bool CanMove() => m_canMove;
        public bool CanReset() => m_canReset;
        private Coroutine m_checkImpactCoroutine;
        private bool m_hasExecuted;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);
            
            m_modifier = info.modifier;
            m_diagonalSwordDashStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.DiagonalSwordDash);
            m_canDiagonalSwordDash = true;
            m_canMove = true;
            m_diagonalSwordDashMovementCooldownTimer = m_diagonalSwordDashMovementCooldown;

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
            m_state.waitForBehaviour = false;
            m_state.isAttacking = false;
            m_canDiagonalSwordDash = true;
            m_diagonalSwordDashInfo.ShowCollider(false);
            m_animator.SetBool(m_diagonalSwordDashStateAnimationParameter, false);
            base.Reset();
        }

        public void Execute()
        {
            m_hasExecuted = true;
            Debug.Log("Start Execute");
            //m_diagonalSwordDashFXAnimator.SetBool("WillPerformDSD", false);
            m_state.waitForBehaviour = true;
            m_state.isExecutingCombatArt = true;
            StopAllCoroutines();
            //m_physics.velocity = Vector2.zero;
            m_cacheGravity = m_physics.gravityScale;
            m_physics.gravityScale = 0;
            m_canReset = false;
            Debug.Log("Middle Execute");
            m_state.isAttacking = true;
            m_state.canAttack = false;
            m_canDiagonalSwordDash = false;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_diagonalSwordDashStateAnimationParameter, true);
            m_diagonalSwordDashCooldownTimer = m_diagonalSwordDashCooldown;
            m_diagonalSwordDashMovementCooldownTimer = m_diagonalSwordDashMovementCooldown;
            m_diagonalSwordDashFXAnimator.SetTrigger("ActiveTrigger");
            if (m_checkImpactCoroutine != null)
            {
                StopCoroutine(m_checkImpactCoroutine);
                m_checkImpactCoroutine = null;
            }
            Debug.Log("End Execute");
            m_physics.gravityScale = m_cacheGravity;
        }

        public void EndExecution()
        {
            Debug.Log("End Execute Start");
            if (m_dashCoroutine != null)
            {
                StopCoroutine(m_dashCoroutine);
                m_dashCoroutine = null;
            }
            // ALWAYS restore physics.
            RestoreDashPhysics();
            m_hasExecuted = false;
            m_dashStarted = false;
            m_diagonalSwordDashInfo.ShowCollider(false);
            m_diagonalSwordDashFXAnimator.SetTrigger("EndTrigger");
            m_state.isExecutingCombatArt = false;
            m_state.waitForBehaviour = false;
            Debug.Log("Middle End Execute");
            m_animator.SetBool(m_diagonalSwordDashStateAnimationParameter,false);
            m_canMove = true;
            if (m_checkImpactCoroutine != null)
            {
                StopCoroutine(m_checkImpactCoroutine);
                m_checkImpactCoroutine = null;
            }
            base.AttackOver();
            Debug.Log("End End Execute");
        }

        public override void Cancel()
        {
            if (!m_hasExecuted)
                return;
            Debug.Log("Start Cancel");
            m_hasExecuted = false;
            m_dashStarted = false;
            if (m_dashCoroutine != null)
            {
                StopCoroutine(m_dashCoroutine);
                m_dashCoroutine = null;
            }
            m_diagonalSwordDashInfo.ShowCollider(false);
            m_diagonalSwordDashFXAnimator.SetTrigger("EndTrigger");
            m_state.isExecutingCombatArt = false;
            m_state.waitForBehaviour = false;
            RestoreDashPhysics();
            m_fxAnimator.Play("Buffer");
            Debug.Log("Middle Cancel");
            m_animator.SetBool(m_diagonalSwordDashStateAnimationParameter,false);
            m_canMove = true;
            if (m_checkImpactCoroutine != null)
            {
                StopCoroutine(m_checkImpactCoroutine);
                m_checkImpactCoroutine = null;
            }
            base.Cancel();
            Debug.Log("End Cancel");
        }

        private void RestoreDashPhysics()
        {
            m_physics.gravityScale = m_cacheGravity;
        }
        public void EnableCollision(bool value)
        {
            m_rigidBody.WakeUp();
            m_diagonalSwordDashInfo.ShowCollider(value);
            m_attackFX.transform.position = m_diagonalSwordDashInfo.fxPosition.position;
            if (value)
                m_checkImpactCoroutine = StartCoroutine(CheckImpactRoutine());
        }

        private IEnumerator CheckImpactRoutine()
        {
            Debug.Log("Start CheckImpact");
            //m_diagonalSwordDashFXAnimator.SetBool("WillPerformDSD", true);
            var timer = 0.25f;
            var hasChecked = false;
            while (!hasChecked)
            {
                Debug.Log("Checking for Impact Point");
                m_enemySensor.Cast();
                if (m_enemySensor.isDetecting && timer >= 0.25f)
                {
                    Debug.Log("First If CheckImpact");
                    timer = 0f;
                    var hits = m_enemySensor.GetHits();
                    //var targetTransform = hits[1].transform;
                    int hitID = 0;
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (Vector2.Distance(m_character.centerMass.position, hits[i].transform.position) < 25f)
                        {
                            hitID = i;
                        }
                    }
                    var target = hits[hitID].point;
                    var instance = Instantiate(m_diagonalSwordDashImpactFX);
                    instance.transform.position = target;
                }
                else
                {
                    Debug.Log("Else CheckImpact");
                    if (timer < 0.25f)
                    {
                        timer += Time.deltaTime;
                    }
                }
                m_edgeSensor.Cast();
                if (m_edgeSensor.isDetecting)
                {
                    Debug.Log("Second If CheckImpact");
                    hasChecked = true;
                    var hits = m_edgeSensor.GetHits();
                    //var targetTransform = hits[1].transform;
                    int hitID = 0;
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (Vector2.Distance(m_character.centerMass.position, hits[i].transform.position) < 25f)
                        {
                            Debug.Log("Third If CheckImpact");
                            hitID = i;
                        }
                    }
                    var hitPoint = hits[hitID].point;
                    var instance = Instantiate(m_diagonalSwordDashImpactFX);
                    instance.transform.position = hitPoint;
                    var instanceGround = Instantiate(m_diagonalSwordDashGroundImpactFX);
                    instanceGround.transform.position = hitPoint;
                }
                yield return null;
            }
            yield return null;
            Debug.Log("End CheckImpact");
        }
        private Coroutine m_dashCoroutine;
        private bool m_dashStarted;
        public void StartDash()
        {
            if (!m_hasExecuted) { return; }
            if (m_dashStarted) { return; }
            m_dashStarted = true;
            m_cacheGravity = m_physics.gravityScale;
            m_physics.gravityScale = 0f;
            m_dashCoroutine =  StartCoroutine(DashRoutine());
        }

        public void HandleAttackTimer()
        {
            if (m_diagonalSwordDashCooldownTimer > 0)
            {
                m_diagonalSwordDashCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canDiagonalSwordDash = false;
            }
            else
            {
                m_diagonalSwordDashCooldownTimer = m_diagonalSwordDashCooldown;
                //m_state.isAttacking = false;
                m_canDiagonalSwordDash = true;
            }
        }

        public void HandleMovementTimer()
        {
            if (m_diagonalSwordDashMovementCooldownTimer > 0)
            {
                m_diagonalSwordDashMovementCooldownTimer -=
                    GameplaySystem.time.deltaTime;

                m_canMove = false;
            }
            else
            {
                m_diagonalSwordDashMovementCooldownTimer =
                    m_diagonalSwordDashMovementCooldown;

                m_canMove = true;
            }
        }

        public void HandleResetTimer()
        {
            if (m_diagonalSwordDashCooldownTimer > 0)
            {
                m_diagonalSwordDashCooldownTimer -= GameplaySystem.time.deltaTime;
                m_canReset = true;
            }
            else
            {
                m_diagonalSwordDashCooldownTimer = m_diagonalSwordDashCooldown;
                m_canReset = false;
                //EndExecution();
            }
        }

        public bool CanExecuteDash()
        {
            m_groundSensor.Cast();
            return !m_groundSensor.isDetecting;
        }

        private IEnumerator DashRoutine()
        {
            Debug.Log("Start Dash");
            m_state.waitForBehaviour = true;
            float timer = m_dashDuration;
            while (m_hasExecuted && timer > 0f)
            {
                m_wallSensor.Cast();
                m_groundSensor.Cast();
                if (m_wallSensor.isDetecting || m_groundSensor.isDetecting)
                {
                    break;
                }
                m_physics.velocity = new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y);
                timer -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            RestoreDashPhysics();
            if (m_hasExecuted)
            {
                m_physics.velocity = new Vector2(m_character.facing == HorizontalDirection.Right ? -m_backForce.x : m_backForce.x, m_backForce.y);
            }
            m_dashStarted = false;
            m_dashCoroutine = null;
            Debug.Log("End Dash");
        }

        public void SetCritConfiguration(PlayerCritStatsInfo info)
        {
            m_diagonalSwordDashInfo.SetCritConfiguration(info);
        }

        public void SetCritConfiguration(List<PlayerCritStatsInfo> info)
        {
        }

        public void SetCritConfiguration(PlayerCritStatsInfo overheadInfo, PlayerCritStatsInfo midairForwardInfo, PlayerCritStatsInfo midairOverheadInfo, PlayerCritStatsInfo crouchInfo)
        {
        }

        public void SetCritConfiguration(PlayerCritStatsInfo forwardInfo, PlayerCritStatsInfo overheadInfo, PlayerCritStatsInfo midairForwardInfo, PlayerCritStatsInfo midairOverheadInfo, PlayerCritStatsInfo crouchInfo)
        {
        }

        public override void IncreaseCritChance(float critChance)
        {
            m_diagonalSwordDashInfo.IncreaseCritChance(critChance);
        }

        public override void IncreaseCritDamage(float critDamage)
        {
            m_diagonalSwordDashInfo.IncreaseCritDamage(critDamage);
        }
    }
}
