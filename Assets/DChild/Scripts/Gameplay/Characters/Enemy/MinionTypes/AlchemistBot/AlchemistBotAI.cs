﻿using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Environment;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/AlchemistBot")]
    public class AlchemistBotAI : CombatAIBrain<AlchemistBotAI.Info>, IResetableAIBrain
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_patrol = new MovementInfo();
            public MovementInfo patrol => m_patrol;
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
            [SerializeField]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            [SerializeField]
            private float m_attackBodyLightningDuration;
            public float attackBodyLightningDuration => m_attackBodyLightningDuration;
            //
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_patienceDistanceTolerance = 50f;
            public float patienceDistanceTolerance => m_patienceDistanceTolerance;

            [SerializeField]
            private float m_detectionTime;
            public float detectionTime => m_detectionTime;
            [SerializeField]
            private float m_awakenTime;
            public float awakenTime => m_awakenTime;

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathStartAnimation;
            public string deathStartAnimation => m_deathStartAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathFallLoopAnimation;
            public string deathFallLoopAnimation => m_deathFallLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathFallImpact1Animation;
            public string deathFallImpact1Animation => m_deathFallImpact1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathFallImpact2Animation;
            public string deathFallImpact2Animation => m_deathFallImpact2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_awakenAnimation;
            public string awakenAnimation => m_awakenAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_dormantAnimation;
            public string dormantAnimation => m_dormantAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_prepAnimation;
            public string prepAnimation => m_prepAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_spawnBlobEvent;
            public string spawnBlobEvent => m_spawnBlobEvent;

            [SerializeField]
            private GameObject m_blobAcid;
            public GameObject blobAcid => m_blobAcid;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);

#endif
            }
        }

        private enum State
        {
            Dormant,
            Detect,
            ReturnToPatrol,
            Patrol,
            Cooldown,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Attack1,
            Attack2,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Rigidbody2D m_rigidbody2D;
        [SerializeField, TabGroup("Reference")]
        private IsolatedObjectPhysics2D m_objectPhysics2D;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Transform m_projectilePoint;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_aggroCollider;
        [SerializeField, TabGroup("Reference")]
        private BoxCollider2D m_attackBB;
        [SerializeField, TabGroup("Reference")]
        private BoxCollider2D m_attackSideBB;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_bodylightningFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_glowFX;
        [SerializeField, TabGroup("BB")]
        private Collider2D m_bodylightningBB;
        [SerializeField, TabGroup("Foreground")]
        private AlchemistBotForeground m_foregroundController;
        [SerializeField, TabGroup("Foreground")]
        private GameObject m_spriteMask;
        [SerializeField, TabGroup("Foreground")]
        private SkeletonAnimation m_skeletomAnimation;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_chosenAttack;

        [SerializeField, SpineBone, TabGroup("Bone")]
        private string m_boneName;
        [SerializeField, TabGroup("Bone")]
        private Bone m_bone;
        private Vector2 m_boneDefaultPos;

        [SerializeField]
        private bool m_willPatrol;

        private float m_currentCD;
        private bool m_isDetecting;
        private bool m_canAttack2;
        private Vector2 m_startPos;
        private Coroutine m_bodylightningCoroutine;
        private Coroutine m_deathCoroutine;
        private Coroutine m_executeMoveCoroutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            //transform.localScale = new Vector3(m_chosenAttack == Attack.Attack2 ? -transform.localScale.x : transform.localScale.x, 1, 1);
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_selfCollider.enabled = true;
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
                //var patienceRoutine = PatienceRoutine();
                //StopCoroutine(patienceRoutine);
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (!m_bodylightningBB.enabled && m_deathCoroutine == null)
            {
                m_attackBB.enabled = false;
                m_attackSideBB.enabled = false;
                m_attackBB.offset = Vector2.zero;
                m_attackBB.size = new Vector2(.25f, .25f);
                //m_flinchHandle.m_autoFlinch = true;
                m_hitbox.Disable();
                StopAllCoroutines();
                m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                StartCoroutine(FlinchRoutine());
            }
        }

        private IEnumerator FlinchRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            //m_flinchHandle.m_autoFlinch = false;
            if (!m_bodylightningBB.enabled)
            {
                m_bodylightningCoroutine = StartCoroutine(BodyLightningRoutine());
            }
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        //private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        //{
        //    //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
        //    //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        //    if (!m_bodylightningBB.enabled && m_deathCoroutine == null)
        //    {
        //        m_attackBB.enabled = false;
        //        m_attackSideBB.enabled = false;
        //        m_attackBB.offset = Vector2.zero;
        //        m_attackBB.size = new Vector2(.25f, .25f);
        //        m_flinchHandle.m_autoFlinch = true;
        //        m_hitbox.Disable();
        //        StopAllCoroutines();
        //        m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        //        m_stateHandle.Wait(State.ReevaluateSituation);
        //    }
        //}

        //private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        //{
        //    if (!m_bodylightningBB.enabled && m_deathCoroutine == null)
        //    {
        //        m_flinchHandle.m_autoFlinch = false;
        //        m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //        if (!m_bodylightningBB.enabled)
        //        {
        //            m_bodylightningCoroutine = StartCoroutine(BodyLightningRoutine());
        //        }
        //        m_stateHandle.ApplyQueuedState();
        //    }
        //}

        private void SpawnBlob()
        {
            StartCoroutine(SpawnBlobRoutine());
        }

        private IEnumerator SpawnBlobRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            var blob = Instantiate(m_info.blobAcid, transform.position, Quaternion.identity);
            blob.GetComponent<BlobAcidAI>().SetTargetInfo(m_targetInfo);
            yield return null;
        }

        private Vector2 GroundPosition()
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(m_projectilePoint.position, Vector2.down, 1000, true, out hitCount, true);
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].point;
        }

        private static ContactFilter2D m_contactFilter;
        private static RaycastHit2D[] m_hitResults;
        private static bool m_isInitialized;

        private static void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_contactFilter.useLayerMask = true;
                m_contactFilter.SetLayerMask(DChildUtility.GetEnvironmentMask());
                //m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(DChildUtility.GetEnvironmentMask()));
                m_hitResults = new RaycastHit2D[16];
                m_isInitialized = true;
            }
        }

        public static RaycastHit2D[] Cast(Vector2 origin, Vector2 direction, float distance, bool ignoreTriggers, out int hitCount, bool debugMode = false)
        {
            Initialize();
            m_contactFilter.useTriggers = !ignoreTriggers;
            hitCount = Physics2D.Raycast(origin, direction, m_contactFilter, m_hitResults, distance);
#if UNITY_EDITOR
            if (debugMode)
            {
                if (hitCount > 0)
                {
                    Debug.DrawRay(origin, direction * m_hitResults[0].distance, Color.cyan, 1f);
                }
                else
                {
                    Debug.DrawRay(origin, direction * distance, Color.cyan, 1f);
                }
            }
#endif
            return m_hitResults;
        }

        private Vector2 WallPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.right * transform.localScale.x, 1000, DChildUtility.GetEnvironmentMask());
            //if (hit.collider != null)
            //{
            //    return hit.point;
            //}
            return hit.point;
        }

        //Patience Handler
        private void Patience()
        {
            enabled = false;
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_stateHandle.SetState(State.ReturnToPatrol);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            enabled = true;
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range)
                                  /*, new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range)*/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private void ExecuteAttack(Attack m_attack)
        {
            m_agent.Stop();
            StartCoroutine(AttackBBSize());
            //m_character.physics.SetVelocity(Vector2.zero);
            m_bodyCollider.enabled = true;
            //m_selfCollider.SetActive(true);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            switch (m_attack)
            {
                case Attack.Attack1:
                    m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimation);
                    break;
                case Attack.Attack2:
                    transform.localScale = new Vector3(m_targetInfo.position.x > transform.position.x ? -1 : 1, 1, 1);
                    StartCoroutine(Attack2CooldownRoutine());
                    m_attackHandle.ExecuteAttack(m_info.attack2.animation, m_info.idleAnimation);
                    break;
            }
        }

        private IEnumerator DetectRoutine()
        {
            if (m_foregroundController.gameObject.activeSelf)
            {
                m_foregroundController.Awaken();
                //m_animation.EnableRootMotion(true, true);
                //m_animation.SetAnimation(0, m_info.awakenAnimation, false);
                m_animation.SetAnimation(0, m_info.idleAnimation, true).MixDuration = 0;
                m_animation.DisableRootMotion();
                yield return new WaitForSeconds(m_info.awakenTime);
                //m_foregroundController.gameObject.SetActive(false);
                m_foregroundController.transform.SetParent(null);
                m_spriteMask.SetActive(false);
                m_skeletomAnimation.maskInteraction = SpriteMaskInteraction.None;
                //transform.position = new Vector2(transform.position.x, CielingPos().y - 5f);
                var initialPos = transform.position;
                transform.position = new Vector2(transform.position.x, transform.position.y + 25f);
                var scrollSpeed = 1f;
                while (Vector2.Distance(initialPos, transform.position) > 1f)
                {
                    //transform.position = new Vector2(transform.position.x, transform.position.y - scrollSpeed);
                    transform.position = Vector2.MoveTowards(transform.position, initialPos, scrollSpeed);
                    yield return null;
                }
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.awakenAnimation);
                //m_animation.SetAnimation(0, m_info.prepAnimation, false).MixDuration = 0;

                //m_animation.SetAnimation(0, m_info.prepAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.prepAnimation);
                m_hitbox.Enable();
                //yield return new WaitForSeconds(m_info.detectionTime);
            }
            else
            {
                m_animation.DisableRootMotion();
                m_hitbox.Enable();
            }
            m_startPos = transform.position;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator DeathRoutine()
        {
            m_animation.SetAnimation(0, m_info.deathStartAnimation, false);
            m_animation.EnableRootMotion(true, false);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathStartAnimation);
            yield return new WaitForSeconds(1.6f);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            //m_animation.DisableRootMotion();
            m_objectPhysics2D.simulateGravity = true;
            m_animation.SetAnimation(0, m_info.deathFallLoopAnimation, true);
            m_bodyCollider.enabled = true;
            yield return new WaitUntil(() => m_bodyCollider.IsTouchingLayers(DChildUtility.GetEnvironmentMask()));
            var animation = UnityEngine.Random.Range(0, 2) == 1 ? m_info.deathFallImpact1Animation : m_info.deathFallImpact2Animation;
            m_animation.SetAnimation(0, animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, animation);
            enabled = false;
            this.gameObject.SetActive(false);
            yield return null;
        }

        private IEnumerator AttackBBSize()
        {
            yield return new WaitForSeconds(/*.35f*/ .5f);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            if (m_chosenAttack == Attack.Attack1)
            {
                var distance = Vector2.Distance(m_projectilePoint.position, GroundPosition());
                m_attackBB.enabled = true;
                m_attackBB.offset = new Vector2(0,(-distance * 0.5f));
                m_attackBB.size = new Vector2(.25f, distance);
            }
            else
            {
                m_attackSideBB.enabled = true;
            }
            yield return new WaitForSeconds(.75f);
            m_attackBB.enabled = false;
            m_attackSideBB.enabled = false;
            m_attackBB.offset = Vector2.zero;
            m_attackBB.size = new Vector2(.25f, .25f);
            yield return null;
        }

        private IEnumerator Attack2CooldownRoutine()
        {
            m_canAttack2 = false;
            yield return new WaitForSeconds(3);
            m_canAttack2 = true;
            yield return null;
        }

        private IEnumerator BodyLightningRoutine()
        {
            yield return new WaitForSeconds(0.25f);
            m_hitbox.Enable();
            m_bodylightningFX.Play();
            m_glowFX.Play();
            m_bodylightningBB.enabled = true;
            yield return new WaitForSeconds(m_info.attackBodyLightningDuration);
            m_bodylightningFX.Stop();
            m_glowFX.Stop();
            m_bodylightningBB.enabled = false;
            yield return null;
        }

        private Vector2 CielingPos()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.centerMass.position, Vector2.up, 1000, DChildUtility.GetEnvironmentMask());
            //if (hit.collider != null)
            //{
            //    return hit.point;
            //}
            return hit.point;
        }

        private void CalculateRunPath()
        {
            bool isRight = m_targetInfo.position.x >= transform.position.x;
            var movePos = new Vector2(transform.position.x + (isRight ? -3 : 3), m_targetInfo.position.y + 10);
            while (Vector2.Distance(transform.position, WallPosition()) <= 5)
            {
                movePos = new Vector2(movePos.x + 0.1f, movePos.y);
                break;
            }
            m_agent.SetDestination(movePos);
        }

        private bool IsInRange(Vector2 position, float distance) => Vector2.Distance(position, m_character.centerMass.position) <= distance;

        #region Movement
        private IEnumerator ExecuteMove(float attackRange, /*float heightOffset,*/ Attack attack)
        {
            m_animation.DisableRootMotion();
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            var moveSpeed = m_info.move.speed - UnityEngine.Random.Range(0, 3);
            var newPos = Vector2.zero;
            var randXPos = UnityEngine.Random.Range(-2f, 2f);
            var randYPos = UnityEngine.Random.Range(10f, 15f); ;
            while (!inRange || TargetBlocked())
            {
                newPos = new Vector2(m_targetInfo.position.x + randXPos, m_targetInfo.position.y + randYPos);
                bool xTargetInRange = Mathf.Abs(/*m_targetInfo.position.x*/newPos.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(/*m_targetInfo.position.y*/newPos.y - transform.position.y) < 1 ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }
                //DynamicMovement(/*new Vector2(m_targetInfo.position.x, m_targetInfo.position.y)*/ newPos);
                m_turnState = State.ReevaluateSituation;
                DynamicMovement(newPos, moveSpeed);
                yield return null;
            }
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            ExecuteAttack(attack);
            yield return null;
        }

        private void DynamicMovement(Vector2 target, float moveSpeed)
        {;
            m_agent.SetDestination(target);
            if (IsFacing(target))
            {
                m_agent.Move(moveSpeed);
            }
            else
            {
                m_stateHandle.OverrideState(State.Turning);
            }
        }
        #endregion

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            GameplaySystem.minionManager.Unregister(GetComponent<ICombatAIBrain>());
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            if (m_bodylightningCoroutine != null)
            {
                StopCoroutine(m_bodylightningCoroutine);
                m_bodylightningCoroutine = null;
            }
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            //var rb2d = GetComponent<Rigidbody2D>();
            m_agent.Stop();
            m_selfCollider.enabled = false;
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_hitbox.Disable();
            m_attackBB.enabled = false;
            m_attackSideBB.enabled = false;
            m_attackBB.offset = Vector2.zero;
            m_attackBB.size = new Vector2(.25f, .25f);
            m_bodylightningBB.enabled = false;
            m_bodylightningFX.Stop();
            m_glowFX.Stop();
            if (m_bodylightningCoroutine != null)
            {
                StopCoroutine(m_bodylightningCoroutine);
                m_bodylightningCoroutine = null;
            }
            m_deathCoroutine = StartCoroutine(DeathRoutine());
            Debug.Log("ALCHEMIST BOT DEATHHHH");
        }

        private void SkeletonAnimation_UpdateLocal(ISkeletonAnimation animated)
        {
            //Debug.Log("FKING AIM");
            if (m_targetInfo.isValid)
            {
                var targetPos = GroundPosition();
                var localPositon = transform.InverseTransformPoint(targetPos);
                localPositon = new Vector2(-localPositon.x, localPositon.y);
                m_bone.SetLocalPosition(localPositon);
            }
        }

        protected override void Start()
        {
            base.Start();

            //if (!m_willPatrol)
            //{
            //    m_hitbox.Disable();
            //}
            m_startPos = transform.position;
            m_character.physics.simulateGravity = m_willPatrol ? true : false;
            //m_aggroCollider.enabled = m_willPatrol ? true : false;
            if (m_willPatrol)
            {
                m_hitbox.Enable();
                m_animation.DisableRootMotion();
            }
            m_spineEventListener.Subscribe(m_info.spawnBlobEvent, SpawnBlob);
        }

        protected override void Awake()
        {
            //Debug.Log(m_info);
            base.Awake();
            GameplaySystem.minionManager.Register(GetComponent<ICombatAIBrain>());
            //Debug.Log("ALCHEMIST BOT BASE AWAKE");
            m_patrolHandle.TurnRequest += OnTurnRequest;
            //m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_damageable.DamageTaken += OnDamageTaken;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            //m_deathHandle.SetAnimation(m_info.deathFallImpact1Animation);
            //m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Dormant, State.WaitBehaviourEnd);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_bone = m_animation.skeletonAnimation.Skeleton.FindBone(m_boneName);
            m_boneDefaultPos = m_bone.GetLocalPosition();
            m_animation.skeletonAnimation.UpdateLocal += SkeletonAnimation_UpdateLocal;

            m_canAttack2 = true;
        }

        private void Update()
        {

            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_agent.Stop();

                    //if (!IsFacingTarget() && m_willPatrol)
                    //{
                    //    m_turnState = State.Detect;
                    //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    //        m_stateHandle.SetState(State.Turning);
                    //}
                    //else
                    //{
                    //    m_stateHandle.Wait(State.ReevaluateSituation);
                    //    StartCoroutine(DetectRoutine());
                    //}
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(DetectRoutine());
                    break;

                case State.ReturnToPatrol:
                    if (Vector2.Distance(m_startPos, transform.position) > 10f)
                    {
                        m_turnState = State.ReturnToPatrol;
                        DynamicMovement(m_startPos, m_info.move.speed);
                    }
                    else
                    {
                        m_stateHandle.OverrideState(State.Patrol);
                    }
                    break;

                case State.Patrol:
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.dormantAnimation)
                    {
                        if (m_foregroundController.gameObject.activeSelf)
                        {
                            m_foregroundController.gameObject.SetActive(false);
                            m_spriteMask.SetActive(false);
                        }
                        m_turnState = State.ReevaluateSituation;
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    }
                    break;

                case State.Dormant:
                    m_animation.SetAnimation(0, m_info.dormantAnimation, true);
                    m_agent.Stop();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    if (m_executeMoveCoroutine != null)
                    {
                        StopCoroutine(m_executeMoveCoroutine);
                        m_executeMoveCoroutine = null;
                    }
                    m_agent.Stop();
                    m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    //m_selfCollider.SetActive(false);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);

                    //m_animation.EnableRootMotion(true, true);
                    m_agent.Stop();
                    //m_character.physics.SetVelocity(Vector2.zero);
                    m_executeMoveCoroutine = StartCoroutine(ExecuteMove(m_attackDecider.chosenAttack.range, m_chosenAttack));
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;
                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.targetDistanceTolerance)
                        {
                            //m_selfCollider.SetActive(true);
                            m_animation.EnableRootMotion(false, false);
                            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                            m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = 1f;
                            CalculateRunPath();
                            m_agent.Move(m_info.move.speed);
                        }
                        else
                        {
                            m_agent.Stop();
                            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                            //m_selfCollider.SetActive(false);
                            m_animation.SetAnimation(0, m_info.idleAnimation, true).TimeScale = 1f;
                        }
                    }

                    if (m_currentCD <= m_info.attackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                        //m_selfCollider.SetActive(true);
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;
                case State.Chasing:
                    m_attackDecider.DecideOnAttack();
                    if (m_attackDecider.hasDecidedOnAttack)
                    {
                        m_chosenAttack = m_attackDecider.chosenAttack.attack;
                        m_animation.EnableRootMotion(true, true);
                        m_agent.Stop();
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        m_stateHandle.SetState(State.Attacking);
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {

                        m_stateHandle.OverrideState(State.Chasing);
                    }
                    else

                    {
                        m_stateHandle.SetState(State.Patrol);
                        //timeCounter = 0;
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_targetInfo.isValid)
            {
                if (TargetBlocked())
                {
                    if (Vector2.Distance(m_targetInfo.position, transform.position) > m_info.patienceDistanceTolerance)
                    {
                        Patience();
                    }
                }
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.ReturnToPatrol);
            m_isDetecting = false;
            m_selfCollider.enabled = false;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_hitbox.Enable();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            Patience();
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }

        public void Activated(bool m_instant)
        {
            enabled = true;
            m_aggroCollider.enabled = true;
            if (m_instant)
            {
                m_stateHandle.OverrideState(State.Patrol);
            }
            else
            {
                m_stateHandle.OverrideState(State.Dormant);
            }
            //StartCoroutine(ActivateRoutine(m_instant));
        }

        //private IEnumerator ActivateRoutine(bool instant)
        //{
        //    yield return new WaitForSeconds(3f);
        //    yield return null;
        //}

        public void Deactivated(bool m_instant)
        {
            enabled = false;
            StopAllCoroutines();
            m_hitbox.Disable();
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.dormantAnimation, true);
            m_stateHandle.OverrideState(State.Dormant);
        }
    }
}