using DChild.Gameplay.Characters.Players.Modules;
using Sirenix.OdinInspector;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.BattleAbilityModule
{
    public class EdgedFury : AttackBehaviour, IInterruptableCombatArtModule, IPlayerCritAttack
    {
        private const string EdgedFuryRevealPlayerParameter = "EdgedFuryRevealPlayer";

        [SerializeField]
        private SkeletonAnimation m_attackFX;

        [SerializeField]
        private float m_edgedFuryCooldown;
        [SerializeField, MinValue(0f)]
        private float m_executionDuration = 2.5f;
        //[SerializeField]
        //private float m_edgedFuryMovementCooldown;
        [SerializeField]
        private Info m_edgedFuryInfo;
        //TEST
        [SerializeField, BoxGroup("Physics")]
        private Character m_character;
        [SerializeField, BoxGroup("Physics")]
        private Rigidbody2D m_physics;
        private float m_cacheGravity;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_enemySensor;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_wallSensor;
        //[SerializeField, BoxGroup("Sensors")]
        //private RaySensor m_edgeSensor;
        [SerializeField, BoxGroup("FX")]
        private ParticleSystem m_fx;
        [SerializeField, BoxGroup("FX")]
        private GameObject m_fxGO;

        [SerializeField]
        private Vector2 m_pushForce;
        private bool m_canEdgedFury;
        private bool m_canMove;
        private bool m_isExecuting;
        private bool m_hasPhysicsCache;
        private bool m_cacheSimulateGravity;
        private bool m_isWaitingForPlayerReveal;
        private bool m_isPlayerVisualHidden;
        private IPlayerModifer m_modifier;
        private CharacterPhysics2D m_characterPhysics;
        private int m_edgedFuryStateAnimationParameter;
        private int m_edgedFuryRevealPlayerAnimationParameter = Animator.StringToHash(EdgedFuryRevealPlayerParameter);
        private float m_edgedFuryCooldownTimer;
        private float m_executionTimer;
        private float m_cacheCharacterGravity;
        private Coroutine m_executionRoutine;
        private RigidbodyConstraints2D m_cacheConstraints;
        private SpineRootAnimation m_playerSpineAnimation;
        private bool m_isRestoringPlayerVisuals;
        //private float m_edgedFuryMovementCooldownTimer;

        private Animator m_fxAnimator;
        private SkeletonAnimation m_skeletonAnimation;

        public bool CanEdgedFury() => m_canEdgedFury;
        public bool CanMove() => m_canMove;
        public bool IsExecuting() => m_isExecuting || m_isWaitingForPlayerReveal;

        public override void Initialize(ComplexCharacterInfo info)
        {
            base.Initialize(info);

            m_modifier = info.modifier;
            m_edgedFuryStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.EdgedFury);
            m_edgedFuryRevealPlayerAnimationParameter = Animator.StringToHash(EdgedFuryRevealPlayerParameter);
            m_canEdgedFury = true;
            m_canMove = true;
            //m_edgedFuryMovementCooldownTimer = m_edgedFuryMovementCooldown;
            m_cacheGravity = m_physics.gravityScale;
            m_characterPhysics = m_character != null ? m_character.physics : m_physics.GetComponent<CharacterPhysics2D>();
            m_playerSpineAnimation = info.character != null ? info.character.GetComponentInChildren<SpineRootAnimation>(true) : null;

            m_fxAnimator = m_attackFX.gameObject.GetComponentInChildren<Animator>();
            m_skeletonAnimation = m_attackFX.gameObject.GetComponent<SkeletonAnimation>();
            SetEdgedFuryRevealPlayer(false);
        }

        //public void SetConfiguration(SlashComboStatsInfo info)
        //{
        //    m_configuration.CopyInfo(info);
        //}

        public override void Reset()
        {
            //m_edgedFuryInfo.PlayFX(false);
            //m_fx.gameObject.SetActive(false);
            //m_fx.Stop();
            StopExecutionRoutine();
            StopPlayerRevealWait();
            m_edgedFuryInfo.ShowCollider(false);
            m_canMove = true;
            m_isExecuting = false;
            m_executionTimer = 0;
            m_state.isExecutingCombatArt = false;
            m_state.waitForBehaviour = false;
            m_state.isAttacking = false;
            HideFX(false);
            RestorePhysicsLock();
            base.Reset();
        }

        public void Execute()
        {
            if (IsExecuting())
            {
                return;
            }

            m_isExecuting = true;
            StopPlayerRevealWait();
            SetEdgedFuryRevealPlayer(false);
            m_state.waitForBehaviour = true;
            m_state.isExecutingCombatArt = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            CachePhysicsState();
            ApplyPhysicsLock();
            m_canEdgedFury = false;
            m_edgedFuryCooldownTimer = m_edgedFuryCooldown;
            m_canMove = false;
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetBool(m_edgedFuryStateAnimationParameter, true);
            m_isPlayerVisualHidden = true;
            m_executionTimer = 0;
            m_executionRoutine = StartCoroutine(ExecutionRoutine());
            //m_edgedFuryInfo.PlayFX(true);
            //m_fx.gameObject.SetActive(true);
            //m_fx.Play();
            //m_fxGO.SetActive(true);
            //m_edgedFuryMovementCooldownTimer = m_edgedFuryMovementCooldown;
            //m_attacker.SetDamageModifier(m_slashComboInfo[m_currentSlashState].damageModifier * m_modifier.Get(PlayerModifier.AttackDamage));
        }

        public void EndExecution()
        {
            if (m_isExecuting == false)
            {
                return;
            }

            if (m_executionTimer < m_executionDuration)
            {
                return;
            }

            CompleteExecution();
        }

        public void EnforceExecutionLock()
        {
            if (m_isExecuting || m_isWaitingForPlayerReveal)
            {
                m_canMove = false;
                m_state.waitForBehaviour = true;
                m_state.isExecutingCombatArt = true;
                m_state.isAttacking = true;
                m_state.canAttack = false;
                ApplyPhysicsLock();
            }
        }

        private void FixedUpdate()
        {
            EnforceExecutionLock();
        }

        private void LateUpdate()
        {
            EnforceExecutionLock();
            TryRevealPlayerFromAnimator();
        }

        private IEnumerator ExecutionRoutine()
        {
            while (m_executionTimer < m_executionDuration)
            {
                ApplyPhysicsLock();
                m_executionTimer += GameplaySystem.time.deltaTime;
                yield return null;
            }

            SetEdgedFuryRevealPlayer(true);
            m_executionRoutine = null;
            CompleteExecution();
        }

        private void CompleteExecution()
        {
            if (m_isExecuting == false)
            {
                return;
            }

            StopExecutionRoutine();
            m_edgedFuryInfo.ShowCollider(false);
            m_canMove = false;
            m_isExecuting = false;
            m_executionTimer = 0;
            m_state.isExecutingCombatArt = true;
            m_state.waitForBehaviour = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
            HideFX(true);
            if (m_isWaitingForPlayerReveal == false)
            {
                ReleaseExecutionLock();
            }
        }

        private void StopExecutionRoutine()
        {
            if (m_executionRoutine != null)
            {
                StopCoroutine(m_executionRoutine);
                m_executionRoutine = null;
            }
        }

        private void StopPlayerRevealWait()
        {
            m_isWaitingForPlayerReveal = false;
        }

        private void CachePhysicsState()
        {
            if (m_hasPhysicsCache)
            {
                return;
            }

            m_cacheGravity = m_physics.gravityScale;
            m_cacheConstraints = m_physics.constraints;
            if (m_characterPhysics != null)
            {
                m_cacheSimulateGravity = m_characterPhysics.simulateGravity;
                m_cacheCharacterGravity = m_characterPhysics.gravity.gravityScale;
            }
            m_hasPhysicsCache = true;
        }

        private void ApplyPhysicsLock()
        {
            CachePhysicsState();
            m_physics.constraints = m_cacheConstraints | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            m_physics.gravityScale = 0;
            m_physics.velocity = Vector2.zero;
            if (m_characterPhysics != null)
            {
                m_characterPhysics.simulateGravity = false;
                m_characterPhysics.gravity.gravityScale = 0;
                m_characterPhysics.SetVelocity(Vector2.zero);
            }
        }

        private void RestorePhysicsLock()
        {
            if (m_hasPhysicsCache)
            {
                m_physics.constraints = m_cacheConstraints;
                if (m_characterPhysics != null)
                {
                    m_characterPhysics.simulateGravity = m_cacheSimulateGravity;
                    m_characterPhysics.gravity.gravityScale = m_cacheCharacterGravity;
                }
                m_hasPhysicsCache = false;
            }

            m_physics.gravityScale = m_cacheGravity;
            m_physics.velocity = Vector2.zero;
            m_characterPhysics?.SetVelocity(Vector2.zero);
        }

        public override void Cancel()
        {
            //m_edgedFuryInfo.PlayFX(false);
            //m_fx.gameObject.SetActive(false);
            //m_fx.Stop();
            StopExecutionRoutine();
            StopPlayerRevealWait();
            m_edgedFuryInfo.ShowCollider(false);
            m_canMove = true;
            m_isExecuting = false;
            m_executionTimer = 0;
            m_state.isExecutingCombatArt = false;
            m_state.waitForBehaviour = false;
            m_state.isAttacking = false;
            m_state.canAttack = true;
            HideFX(false);
            RestorePhysicsLock();
            m_fxAnimator.Play("Buffer");
            base.Cancel();
        }

        public void EnableCollision(bool value)
        {
            m_edgedFuryInfo.ShowCollider(value);
            if (value)
            {
                m_fxGO.SetActive(true);
                m_rigidBody.WakeUp();
                m_attackFX.transform.position = m_edgedFuryInfo.fxPosition.position;
                m_physics.velocity = Vector2.zero;
                m_edgedFuryInfo.ShowCollider(value);
                m_physics.velocity = Vector2.zero;
            }
            else
            {
                HideFXOnly();
                m_physics.velocity = new Vector2(0, m_physics.velocity.y);
            }

            //m_physics.AddForce(new Vector2(m_character.facing == HorizontalDirection.Right ? m_pushForce.x : -m_pushForce.x, m_pushForce.y), ForceMode2D.Impulse);
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

        private void HideFX(bool delayPlayerReveal)
        {
            HideFXOnly();
            if (delayPlayerReveal && m_isPlayerVisualHidden)
            {
                StartPlayerRevealWait();
            }
            else
            {
                StopPlayerRevealWait();
                CompletePlayerReveal();
            }
        }

        private void HideFXOnly()
        {
            m_fxGO.SetActive(false);
        }

        private void StartPlayerRevealWait()
        {
            if (m_isWaitingForPlayerReveal)
            {
                return;
            }

            m_isWaitingForPlayerReveal = true;
            m_canMove = false;
            m_state.waitForBehaviour = true;
            m_state.isExecutingCombatArt = true;
            m_state.isAttacking = true;
            m_state.canAttack = false;
        }

        private void TryRevealPlayerFromAnimator()
        {
            if (m_isWaitingForPlayerReveal == false || m_animator == null)
            {
                return;
            }

            if (m_animator.GetBool(m_edgedFuryRevealPlayerAnimationParameter))
            {
                CompletePlayerReveal();
            }
        }

        private void CompletePlayerReveal()
        {
            m_isWaitingForPlayerReveal = false;
            RestorePlayerVisualState();
            SetEdgedFuryRevealPlayer(false);
            if (m_isExecuting == false)
            {
                ReleaseExecutionLock();
            }
        }

        private void ReleaseExecutionLock()
        {
            RestorePhysicsLock();
            m_canMove = true;
            m_state.isExecutingCombatArt = false;
            m_state.waitForBehaviour = false;
            m_state.isAttacking = false;
            base.AttackOver();
        }

        private void RestorePlayerVisualState()
        {
            ClearPlayerAnimationState();
            m_isPlayerVisualHidden = false;
        }

        private void SetEdgedFuryRevealPlayer(bool value)
        {
            if (m_animator == null)
            {
                return;
            }

            m_animator.SetBool(m_edgedFuryRevealPlayerAnimationParameter, value);
        }

        private void ClearPlayerAnimationState()
        {
            if (m_isRestoringPlayerVisuals || m_animator == null)
            {
                return;
            }

            m_isRestoringPlayerVisuals = true;
            try
            {
                m_animator.SetBool(m_edgedFuryStateAnimationParameter, false);
                m_animator.SetBool(m_animationParameter, false);
                if (m_animator.isActiveAndEnabled)
                {
                    m_animator.Update(0f);
                }

                m_playerSpineAnimation?.UpdateAnimation(0f);
                m_playerSpineAnimation?.LateUpdateAnimation();
            }
            finally
            {
                m_isRestoringPlayerVisuals = false;
            }
        }

        public void HandleAttackTimer()
        {
            if (m_canEdgedFury) return;
            m_edgedFuryCooldownTimer -= GameplaySystem.time.deltaTime;
            if (m_edgedFuryCooldownTimer <= 0)
            {
                m_edgedFuryCooldownTimer = 0;
                m_canEdgedFury = true;
            }
        }

        public void SetCritConfiguration(PlayerCritStatsInfo info)
        {
            m_edgedFuryInfo.SetCritConfiguration(info);
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
            m_edgedFuryInfo.IncreaseCritChance(critChance);
        }

        public override void IncreaseCritDamage(float critDamage)
        {
            m_edgedFuryInfo.IncreaseCritDamage(critDamage);
        }

        //public void HandleMovementTimer()
        //{
        //    if (m_edgedFuryMovementCooldownTimer > 0)
        //    {
        //        m_edgedFuryMovementCooldownTimer -= GameplaySystem.time.deltaTime;
        //        m_canMove = false;
        //    }
        //    else
        //    {
        //        //Debug.Log("Can Move");
        //        m_edgedFuryMovementCooldownTimer = m_edgedFuryMovementCooldown;
        //        m_canMove = true;
        //    }
        //}
    }
}
