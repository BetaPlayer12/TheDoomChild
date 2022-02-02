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
using DChild.Gameplay.Pathfinding;
using DarkTonic.MasterAudio;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/TomeOfSpells")]
    public class TomeOfSpellsAI : CombatAIBrain<TomeOfSpellsAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_patrol = new MovementInfo();
            public MovementInfo patrol => m_patrol;
            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            //Attack Behaviours
            [SerializeField, BoxGroup("Attack")]
            private SimpleAttackInfo m_attackFrost = new SimpleAttackInfo();
            public SimpleAttackInfo attackFrost => m_attackFrost;
            [SerializeField, BoxGroup("Attack"), ValueDropdown("GetAnimations")]
            private string m_attackFrostStartAnimation;
            public string attackFrostStartAnimation => m_attackFrostStartAnimation;
            [SerializeField, BoxGroup("Attack")]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            //
            [SerializeField, BoxGroup("Patience"), MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            [SerializeField, BoxGroup("Patience")]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField, BoxGroup("Patience")]
            private float m_patienceDistanceTolerance = 50f;
            public float patienceDistanceTolerance => m_patienceDistanceTolerance;

            //Animations
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_deathStartAnimation;
            public string deathStartAnimation => m_deathStartAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_deathLoopAnimation;
            public string deathLoopAnimation => m_deathLoopAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_deathEndAnimation;
            public string deathEndAnimation => m_deathEndAnimation;
            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;

            [SerializeField, BoxGroup("Projectile")]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attackFrost.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
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
            //Attack,
            AttackFrost,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Rigidbody2D m_rigidbody2D;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_roofSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_selfSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;

        private ProjectileLauncher m_projectileLauncher;

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        private float m_currentPatience;
        private float m_currentCD;
        private bool m_isDetecting;
        private bool m_enablePatience;
        private Vector2 m_lastTargetPos;
        private Vector2 m_startPos;

        private Coroutine m_executeMoveCoroutine;
        private Coroutine m_attackRoutine;
        private Coroutine m_patienceRoutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_currentAttack = Attack.Attack;
            m_flinchHandle.gameObject.SetActive(true);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
            m_attackDecider.hasDecidedOnAttack = false;
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
                    m_selfCollider.SetActive(true);
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
                //var patienceRoutine = PatienceRoutine();
                //StopCoroutine(patienceRoutine);
            }
        }

        private bool TargetBlocked()
        {
            Vector2 wat = m_selfSensor.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/wat, m_targetInfo.position - wat, 1000, LayerMask.GetMask("Player") + DChildUtility.GetEnvironmentMask());
            var eh = hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
            Debug.DrawRay(wat, m_targetInfo.position - wat);
            Debug.Log("Shot is " + eh + " by " + LayerMask.LayerToName(hit.transform.gameObject.layer));
            return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
        }

        public void SetAI(AITargetInfo targetInfo)
        {
            m_isDetecting = true;
            m_targetInfo = targetInfo;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation || m_stateHandle.currentState == State.Cooldown)
            {
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_flinchHandle.m_autoFlinch = true;
                m_agent.Stop();
                m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                m_stateHandle.Wait(State.ReevaluateSituation);
                StopAllCoroutines();
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_flinchHandle.m_autoFlinch = false;
                m_stateHandle.ApplyQueuedState();
            }
        }
        private Vector2 WallPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.centerMass.position, Vector2.right * transform.localScale.x, 1000, DChildUtility.GetEnvironmentMask());
            //if (hit.collider != null)
            //{
            //    return hit.point;
            //}
            return hit.point;
        }

        //Patience Handler
        private void Patience()
        {
            if (m_patienceRoutine == null)
            {
                m_patienceRoutine = StartCoroutine(PatienceRoutine());
            }
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private IEnumerator PatienceRoutine()
        {
            //if (m_enablePatience)
            //{
            //    while (m_currentPatience < m_info.patience)
            //    {
            //        m_currentPatience += m_character.isolatedObject.deltaTime;
            //        yield return null;
            //    }
            //}
            yield return new WaitForSeconds(m_info.patience);
            StopAllCoroutines();
            m_enablePatience = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.SetState(State.ReturnToPatrol);
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
            m_attackDecider.SetList(/*new AttackInfo<Attack>(Attack.Attack, m_info.attack.range),*/
                                    new AttackInfo<Attack>(Attack.AttackFrost, m_info.attackFrost.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
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

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_agent.Stop();
            m_bodyCollider.enabled = false;
            m_selfCollider.SetActive(false);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_animation.DisableRootMotion();
            var rb2d = GetComponent<Rigidbody2D>();
            rb2d.isKinematic = false;
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            m_hitbox.Disable();
            m_animation.SetEmptyAnimation(0, 0);
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            m_agent.Stop();
            Debug.Log("DIE HERE");
            m_animation.SetAnimation(0, m_info.deathStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathStartAnimation);
            m_character.physics.simulateGravity = true;
            m_animation.SetAnimation(0, m_info.deathLoopAnimation, true);
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            m_animation.SetAnimation(0, m_info.deathEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathEndAnimation);
            enabled = false;
            this.gameObject.SetActive(false);
            yield return null;
        }

        #region Attack
        private void ExecuteAttack(Attack m_attack)
        {
            m_agent.Stop();
            m_bodyCollider.enabled = true;
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            switch (/*m_attack*/ m_currentAttack)
            {
                //case Attack.Attack:
                //    StartCoroutine(AttackRoutine());
                //    break;
                case Attack.AttackFrost:
                    m_lastTargetPos = m_targetInfo.position;
                    //m_attackHandle.ExecuteAttack(m_info.attackFrost.animation, m_info.idleAnimation);
                    m_attackRoutine = StartCoroutine(FrostAttackRoutine());
                    break;
            }
        }

        private IEnumerator FrostAttackRoutine()
        {
            m_animation.SetAnimation(0, m_info.attackFrostStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackFrostStartAnimation);
            m_animation.SetAnimation(0, m_info.attackFrost.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackFrost.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_flinchHandle.gameObject.SetActive(true);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
            m_attackDecider.hasDecidedOnAttack = false;
            yield return null;
        }

        private void LaunchIcePattern(int numberOfProjectiles, int rotations)
        {
            for (int x = 0; x < rotations; x++)
            {
                float angleStep = 360f / numberOfProjectiles;
                float angle = 45f;
                for (int z = 0; z < numberOfProjectiles; z++)
                {
                    Vector2 startPoint = new Vector2(m_character.centerMass.position.x, m_character.centerMass.position.y);
                    float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * 5;
                    float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * 5;

                    Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
                    Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * m_info.projectile.projectileInfo.speed;

                    GameObject projectile = m_info.projectile.projectileInfo.projectile;
                    var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(projectile);
                    instance.transform.position = m_character.centerMass.position;
                    var component = instance.GetComponent<Projectile>();
                    component.ResetState();
                    component.GetComponent<Rigidbody2D>().velocity = projectileMoveDirection;
                    Vector2 v = component.GetComponent<Rigidbody2D>().velocity;
                    var projRotation = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                    component.transform.rotation = Quaternion.AngleAxis(projRotation, Vector3.forward);

                    angle += angleStep;
                }
            }
            //yield return null;
        }

        private void LaunchProjectile()
        {
            if (m_targetInfo.isValid)
            {
                //m_projectileLauncher.AimAt(m_lastTargetPos);
                //m_projectileLauncher.LaunchProjectile();
                LaunchIcePattern(4, 1);
            }
        }
        #endregion

        #region Movement
        private IEnumerator ExecuteMove(float attackRange, /*float heightOffset,*/ Attack attack)
        {
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            var moveSpeed = m_info.move.speed - UnityEngine.Random.Range(0, 3);
            while (!inRange || TargetBlocked())
            {

                bool xTargetInRange = Mathf.Abs(m_targetInfo.position.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(m_targetInfo.position.y - transform.position.y) < attackRange/*1*/ ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }
                DynamicMovement(m_targetInfo.position, moveSpeed);
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
        {
            m_agent.SetDestination(target);

            if (/*m_wallSensor.allRaysDetecting ||*/ m_selfSensor.isDetecting)
            {
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                //m_bodyCollider.SetActive(true);
                m_agent.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                return;
            }

            if (IsFacing(m_agent.hasPath && TargetBlocked() && !m_groundSensor.allRaysDetecting && !m_roofSensor.allRaysDetecting ? m_agent.segmentDestination : target))
            {
                if (m_animation.animationState.GetCurrent(0).IsComplete)
                {
                    var chosenMoveAnim = UnityEngine.Random.Range(0, 50) > 10 ? m_info.idleAnimation : m_info.move.animation;
                    m_animation.SetAnimation(0, chosenMoveAnim, true);
                }

                if (!m_wallSensor.allRaysDetecting && (m_groundSensor.allRaysDetecting || m_roofSensor.allRaysDetecting))
                {
                    //if (m_executeMoveCoroutine != null)
                    //{
                    //    StopCoroutine(m_executeMoveCoroutine);
                    //    m_executeMoveCoroutine = null;
                    //}
                    //StartCoroutine(DespawnRoutine());
                    //Vector3.MoveTowards(transform.position, m_targetInfo.position, m_info.move.speed);
                    m_bodyCollider.enabled = false;
                    m_agent.Stop();
                    Vector3 dir = (target - (Vector2)m_rigidbody2D.transform.position).normalized;
                    m_rigidbody2D.MovePosition(m_rigidbody2D.transform.position + dir * moveSpeed * Time.fixedDeltaTime);

                    m_animation.SetAnimation(0, m_info.move.animation, true);
                    return;
                }

                m_bodyCollider.enabled = true;
                var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
                var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
                m_agent.SetDestination(target);
                m_agent.Move(moveSpeed);
            }
            else
            {
                m_turnState = State.ReevaluateSituation;
                m_stateHandle.OverrideState(State.Turning);
            }
        }

        //private void DynamicMovement(Vector2 target, float moveSpeed)
        //{
        //    if (ShotBlocked())
        //    {
        //        VelocityTurn();
        //    }
        //    else
        //    {
        //        if (!IsFacingTarget())
        //        {
        //            CustomTurn();
        //        }
        //    }

        //    var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
        //    var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;

        //    bool isCloseToGround = false;
        //    bool isCloseToRoof = false;

        //    if (m_targetInfo.position.y < transform.position.y && m_groundSensor.allRaysDetecting)
        //    {
        //        isCloseToGround = Vector2.Distance(transform.position, GroundPosition()) < 2.5f ? true : false;
        //    }
        //    if (m_targetInfo.position.y > transform.position.y && m_roofSensor.allRaysDetecting)
        //    {
        //        isCloseToRoof = Vector2.Distance(transform.position, RoofPosition()) < 5f ? true : false;
        //    }

        //    if (!m_selfSensor.isDetecting && !isCloseToGround && !isCloseToRoof /*&& !GetComponentInChildren<NavigationTracker>().IsCurrentDestination(transform.position)*/)
        //    {
        //        if (Mathf.Abs(m_targetInfo.position.y - transform.position.y) > 5f && !m_wallSensor.isDetecting && !m_groundSensor.isDetecting)
        //        {
        //            m_agent.SetDestination(new Vector2(transform.position.x, target.y/* + 5*/));
        //        }
        //        else
        //        {
        //            m_agent.SetDestination(target);
        //        }
        //        //m_agent.SetDestination(target);
        //        m_agent.Move(moveSpeed);

        //        if (m_character.physics.velocity.y > .25f || m_character.physics.velocity.y < -.25f)
        //        {
        //            m_animation.SetAnimation(0, m_info.idleAnimation, true)/*.TimeScale = 2*/;
        //        }
        //        else
        //        {
        //            m_animation.SetAnimation(0, m_info.patrol.animation, true)/*.TimeScale = 2*/;
        //        }
        //    }
        //    else
        //    {
        //        m_agent.Stop();
        //        m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    }

        //    //if (IsFacingTarget())
        //    //{
        //    //}
        //    //else
        //    //{
        //    //    m_turnState = State.Attacking;
        //    //    m_stateHandle.OverrideState(State.Turning);
        //    //}
        //}

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }

        private Vector2 RoofPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }

        private bool ShotBlocked()
        {
            Vector2 wat = m_selfSensor.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/wat, m_targetInfo.position - wat, 1000, LayerMask.GetMask("Player") + DChildUtility.GetEnvironmentMask());
            var eh = hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
            Debug.DrawRay(wat, m_targetInfo.position - wat);
            Debug.Log("Shot is " + eh + " by " + LayerMask.LayerToName(hit.transform.gameObject.layer));
            return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
        }

        private Vector2 ShotPosition()
        {
            Vector2 startPoint = m_selfSensor.transform.position;
            Vector2 direction = (m_targetInfo.position - startPoint).normalized;

            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/startPoint, direction, 1000, DChildUtility.GetEnvironmentMask());
            Debug.DrawRay(startPoint, direction);
            return hit.point;
        }
        #endregion

        private void ChooseAttack()
        {
            if (!m_attackDecider.hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_attackCache.Count; i++)
                {
                    m_attackDecider.DecideOnAttack();
                    if (m_attackCache[i] != m_currentAttack && !m_attackUsed[i])
                    {
                        m_attackUsed[i] = true;
                        m_currentAttack = m_attackCache[i];
                        m_currentAttackRange = m_attackRangeCache[i];
                        return;
                    }
                }
            }
        }

        private void IsAllAttackComplete()
        {
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                if (!m_attackUsed[i])
                {
                    return;
                }
            }
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                m_attackUsed[i] = false;
            }
        }

        void AddToAttackCache(params Attack[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackCache.Add(list[i]);
            }
        }

        void AddToRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache.Add(list[i]);
            }
        }

        void UpdateRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache[i] = list[i];
            }
        }

        protected override void Start()
        {
            base.Start();
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            m_animation.DisableRootMotion();
            m_bodyCollider.enabled = false;
            m_startPos = transform.position;
            m_spineEventListener.Subscribe(m_info.projectile.launchOnEvent, LaunchProjectile);
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathStartAnimation);
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, transform);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.AttackFrost);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.attackFrost.range);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        private void Update()
        {

            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_agent.Stop();
                    if (IsFacingTarget())
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.ReturnToPatrol:
                    if (IsFacing(m_startPos))
                    {
                        if (Vector2.Distance(m_startPos, transform.position) > 5f)
                        {
                            var rb2d = GetComponent<Rigidbody2D>();
                            m_bodyCollider.enabled = false;
                            m_agent.Stop();
                            Vector3 dir = (m_startPos - (Vector2)rb2d.transform.position).normalized;
                            rb2d.MovePosition(rb2d.transform.position + dir * m_info.move.speed * Time.fixedDeltaTime);
                            m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        }
                        else
                        {
                            m_stateHandle.OverrideState(State.Patrol);
                        }
                    }
                    else
                    {
                        m_turnState = State.ReturnToPatrol;
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Patrol:
                    m_turnState = State.ReevaluateSituation;
                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    {
                        var chosenMoveAnim = UnityEngine.Random.Range(0, 100) < 25 ? m_info.idleAnimation : m_info.move.animation;
                        m_animation.SetAnimation(0, chosenMoveAnim, true);
                    }

                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                    if (m_executeMoveCoroutine != null)
                    {
                        StopCoroutine(m_executeMoveCoroutine);
                        m_executeMoveCoroutine = null;
                    }
                    m_agent.Stop();
                    m_turnHandle.Execute();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    m_executeMoveCoroutine = StartCoroutine(ExecuteMove(m_currentAttackRange, m_currentAttack));
                    m_attackDecider.hasDecidedOnAttack = false;
                    break;
                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.targetDistanceTolerance)
                        {
                            if (m_character.physics.velocity.y > 1 || m_character.physics.velocity.y < -1)
                            {
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            }
                            else
                            {
                                m_animation.SetAnimation(0, m_info.patrol.animation, true);
                            }
                            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                            CalculateRunPath();
                            m_agent.Move(m_info.move.speed);
                        }
                        else
                        {
                            m_agent.Stop();
                            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
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
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;
                case State.Chasing:
                    //m_attackDecider.DecideOnAttack();
                    m_attackDecider.hasDecidedOnAttack = false;
                    ChooseAttack();
                    m_currentAttack = Attack.AttackFrost;
                    m_currentAttackRange = m_info.attackFrost.range;
                    if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting*/)
                    {
                        m_agent.Stop();
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
            m_hitbox.gameObject.SetActive(true);
            m_isDetecting = false;
        }

        public void ResetAI()
        {
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
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
    }
}