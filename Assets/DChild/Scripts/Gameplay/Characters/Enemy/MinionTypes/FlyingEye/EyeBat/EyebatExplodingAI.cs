using System;
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

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/EyebatExploding")]
    public class EyebatExplodingAI : CombatAIBrain<EyebatExplodingAI.Info>, ISummonedEnemy
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
            private float m_attackCD;
            public float attackCD => m_attackCD;
            [SerializeField]
            private float m_explosionDelay;
            public float explosionDelay => m_explosionDelay;
            [SerializeField]
            private float m_explosionRange;
            [SerializeField]
            private float m_chargeAttackTolerance;
            public float chargeAttack => m_chargeAttackTolerance;
            public float explosionRange => m_explosionRange;
            //
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_patienceDistanceTolerance = 50f;
            public float patienceDistanceTolerance => m_patienceDistanceTolerance;

            //Animations
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_attackCharge;
            public BasicAnimationInfo attackCharge => m_attackCharge;
            [SerializeField]
            private BasicAnimationInfo m_attackCharge2;
            public BasicAnimationInfo attackCharge2 => m_attackCharge2;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);

                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_attackCharge.SetData(m_skeletonDataAsset);
                m_attackCharge2.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Idle,
            //ReturnToPatrol,
            Patrol,
            Cooldown,
            Turning,
            Attacking,
            ChargeAttack,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Explosion,
            Charging,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Rigidbody2D m_rigidbody2D;
        [SerializeField, TabGroup("Reference")]
        private Health m_health;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodycollider;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_explodeBB;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        [SerializeField]
        private GameObject m_warningSignal;
        [SerializeField]
        private GameObject m_blast;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        /*[SerializeField]
        private GameObject m_explodieBB;*/

        private float m_currentCD;
        private bool m_isDetecting;
        private Vector2 m_startPos;
        private Vector2 m_lastTargetPos;
        [SerializeField]
        private float duration;

        private Coroutine m_executeMoveCoroutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_currentAttack = Attack.Attack;
            m_flinchHandle.gameObject.SetActive(true);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

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

        public void SummonAt(Vector2 position, AITargetInfo target)
        {
            //Lefix commento 3====D----
            enabled = false;
            m_targetInfo = target;
            m_isDetecting = true;
            transform.position = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y + 10f);
            m_character.physics.simulateGravity = false;
            m_hitbox.Enable();
            m_flinchHandle.gameObject.SetActive(true);
            m_health.SetHealthPercentage(1f);
            this.gameObject.SetActive(true);
            this.transform.SetParent(null);
            if (!IsFacingTarget())
                CustomTurn();
            Awake();
            m_stateHandle.OverrideState(State.Detect);
            enabled = true;
        }

        //public void SetAI(AITargetInfo targetInfo)
        //{
        //    m_isDetecting = true;
        //    m_targetInfo = targetInfo;
        //    m_stateHandle.OverrideState(State.ReevaluateSituation);
        //}

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation || m_stateHandle.currentState == State.Cooldown)
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
            StopAllCoroutines();
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_agent.Stop();
            //m_stateHandle.SetState(State.ReturnToPatrol);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
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
            //m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Explosion, m_info.explosionRange));
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Charging, m_info.chargeAttack));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            if (!IsFacingTarget())
                CustomTurn();
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
            base.OnDestroyed(sender, eventArgs);
            
            StopAllCoroutines();
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_agent.Stop();
            m_bodycollider.enabled = false;
            m_selfCollider.SetActive(false);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            //m_muzzleLoopFX.Stop();
            StartCoroutine(ExplodeRoutine());
        }
        private IEnumerator ExplodeRoutine()
        {
            m_hitbox.Disable();
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            StartCoroutine(ExplodeBBRoutine());
            yield return new WaitForSeconds(0.1f);
            m_blast.SetActive(true);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathAnimation);
            //m_explodieBB.SetActive(false);
            enabled = false;
            yield return new WaitForSeconds(1f);
            this.gameObject.SetActive(false);
            yield return null;
        }
        private IEnumerator Charge()
        {
            m_animation.SetAnimation(0, m_info.attackCharge, false);
            yield return new WaitForSeconds(0.4f);
            m_warningSignal.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            m_animation.SetAnimation(0, m_info.attackCharge2, true);
            m_lastTargetPos.x = m_lastTargetPos.x + 5;
            m_lastTargetPos.y = m_lastTargetPos.y - 11;
            if (IsFacing(m_lastTargetPos))
            {
                var lerpedValue = 0f;

                while(lerpedValue < duration){
                    float t = lerpedValue / duration;
                    transform.position = Vector2.Lerp(transform.position, m_lastTargetPos, t);
                    lerpedValue += Time.deltaTime;
                    StartCoroutine(ExplodeRoutine());
                    yield return null;
                }
                transform.position = m_lastTargetPos;
                //DynamicMovement(m_lastTargetPos, 75);
                //transform.position = Vector2.MoveTowards(m_startPos, m_lastTargetPos, 20);
                //m_agent.Move(75);
            }
            //transform.eulerAngles += Vector3.forward * Time.deltaTime * 75;
            //m_rigidbody2D.AddForce(transform.forward * 75);
            //DynamicMovement(m_targetInfo.position, 75);
            //yield return null;
        }
        private IEnumerator ExplodeBBRoutine()
        {
            yield return new WaitForSeconds(m_info.explosionDelay);
            m_explodeBB.enabled = true;
            yield return new WaitForSeconds(0.25f);
            m_explodeBB.enabled = false;
            yield return null;
        }
        /*private IEnumerator AttackCharge()
        {
            yield return new WaitForSeconds(m_info.chargeAttack);
            
        }*/

        #region Attack
        private void ExecuteAttack(Attack m_attack)
        {
            m_agent.Stop();
            m_bodycollider.enabled = true;
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            switch (m_attack)
            {
                //case Attack.Attack:
                //    StartCoroutine(AttackRoutine());
                //    break;
                /*case Attack.Explosion:
                    m_lastTargetPos = m_targetInfo.position;
                    StartCoroutine(ExplodeRoutine());
                    break;*/
                case Attack.Charging:
                    m_lastTargetPos = m_targetInfo.position;
                    m_agent.Stop();
                    StartCoroutine(Charge());
                    break;
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
            if (IsFacing(target))
            {
                m_agent.Move(moveSpeed);
            }
            else
            {
                m_stateHandle.OverrideState(State.Turning);
            }
        }

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
        #endregion

        protected override void Start()
        {
            base.Start();
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            m_animation.DisableRootMotion();
            m_bodycollider.enabled = false;
            m_startPos = transform.position;
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
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
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

                case State.Idle:
                    m_agent.Stop();
                    break;

                /*case State.ReturnToPatrol:
                    if (IsFacing(m_startPos))
                    {
                        if (Vector2.Distance(m_startPos, transform.position) > 10f)
                        {
                            var rb2d = GetComponent<Rigidbody2D>();
                            m_bodycollider.enabled = false;
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
                    break;*/

                case State.Patrol:
                    m_turnState = State.ReevaluateSituation;
                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    {
                        var chosenMoveAnim = UnityEngine.Random.Range(0, 100) < 25 ? m_info.idleAnimation.animation : m_info.move.animation;
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

                /*case State.ChargeAttack:
                    m_agent.Stop();
                    m_animation.SetAnimation(0, m_info.attackCharge, false);
                    break;*/

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    m_executeMoveCoroutine = StartCoroutine(ExecuteMove(m_info.chargeAttack, Attack.Charging));
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
                    m_attackDecider.DecideOnAttack();
                    //m_attackDecider.hasDecidedOnAttack = false;
                    if (m_attackDecider.hasDecidedOnAttack)
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
            //m_stateHandle.OverrideState(State.ReturnToPatrol);
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

        public void DestroyObject()
        {
        }
    }
}