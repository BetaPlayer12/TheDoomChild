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

namespace DChild.Gameplay.Characters.Enemies
{
    public class MeleeBeeDroneAI : CombatAIBrain<MeleeBeeDroneAI.Info>, ISummonedEnemy
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

            [SerializeField]
            private int m_timePause;
            public int timePause => m_timePause;

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_meleeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo meleeAttack => m_meleeAttack;
            //
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_patienceDistanceTolerance = 50f;
            public float patienceDistanceTolerance => m_patienceDistanceTolerance;

            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idle2Animation = new BasicAnimationInfo();
            public BasicAnimationInfo idle2Animation => m_idle2Animation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_chargeAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo chargeAnimation => m_chargeAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                meleeAttack.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_idle2Animation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_chargeAnimation.SetData(m_skeletonDataAsset);

#endif
            }
        }

        private enum State
        {
            Idle,
            chargeIdle,
            ReturnToPatrol,
            Patrol,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyDamager;
        [SerializeField, TabGroup("Reference")]
        private Health m_health;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        
        private float timeCounter;
        private bool m_chargeOnce;
        private bool m_chargeFacing;
        private Vector2 m_startPos;

        private Coroutine m_executeMoveCoroutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
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
                
                base.SetTarget(damageable, m_target);
                
                if(m_chargeOnce == false)
                {
                    m_chargeOnce = true;
                    m_stateHandle.SetState(State.chargeIdle);
                }
                else
                {
                    m_stateHandle.SetState(State.Chasing);
                }

               
            }
        }
        public void SummonAt(Vector2 position, AITargetInfo target)
        {
            enabled = false;
            transform.position = position;
            m_character.physics.simulateGravity = false;
            m_hitbox.Enable();
            m_flinchHandle.gameObject.SetActive(true);
            m_health.SetHealthPercentage(1f);
            this.gameObject.SetActive(true);
            this.transform.SetParent(null);
            Awake();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }
        private bool TargetBlocked()
        {
            Vector2 wat = m_character.centerMass.position;
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/wat, m_targetInfo.position - wat, 1000, LayerMask.GetMask("Player") + DChildUtility.GetEnvironmentMask());
            var eh = hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
            Debug.DrawRay(wat, m_targetInfo.position - wat);
            Debug.Log("Shot is " + eh + " by " + LayerMask.LayerToName(hit.transform.gameObject.layer));
            return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.OverrideState(State.ReevaluateSituation);
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
            enabled = true;
        }

        private IEnumerator AttackRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            m_animation.SetAnimation(0, m_info.chargeAnimation, false);
            m_bodyDamager.enabled = false;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chargeAnimation);
            m_animation.SetAnimation(0, m_info.meleeAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.meleeAttack.animation);
            m_bodyDamager.enabled = true;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        #region Movement
        private IEnumerator ExecuteMove(float attackRange)
        {
            m_animation.DisableRootMotion();
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            var moveSpeed = m_info.move.speed - UnityEngine.Random.Range(0, 3);
            var newPos = Vector2.zero;
            while (!inRange || TargetBlocked())
            {
                newPos = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y);
                bool xTargetInRange = Mathf.Abs(m_targetInfo.position.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(m_targetInfo.position.y - transform.position.y) < 1 ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }
                m_turnState = State.ReevaluateSituation;
                DynamicMovement(newPos, moveSpeed);
                yield return null;
            }
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            StartCoroutine(AttackRoutine());
            yield return null;
        }

        private void DynamicMovement(Vector2 target, float movespeed)
        {
            var rb2d = GetComponent<Rigidbody2D>();
            m_agent.SetDestination(target);           

            if (IsFacing(target))
            {
                m_agent.Move(movespeed);
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
            base.OnDestroyed(sender, eventArgs);
            
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            m_bodyCollider.enabled = true;
            m_agent.Stop();
        }

        protected override void Start()
        {
            base.Start();
            m_startPos = transform.position;
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            timeCounter = 0;
            m_chargeOnce = false;
            m_chargeFacing = false;
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            
        }

        private void Update()
        {
          
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (m_targetInfo.isValid == false)
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;
                case State.chargeIdle:
                   if (IsFacingTarget())
                   {
                        m_animation.SetAnimation(0, m_info.chargeAnimation, true);
                    if (m_info.timePause <= timeCounter)
                    {
                            m_chargeFacing = false;
                        m_stateHandle.SetState(State.Chasing);
                    }
                    else
                    {
                        m_agent.Stop();
                        timeCounter += Time.deltaTime;
                       
                    }
                   }
                    else
                    {
                        m_turnState = State.chargeIdle;
                        m_chargeFacing = true;
                        m_stateHandle.SetState(State.Turning);
                       
                      
                    }
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
                    m_chargeOnce = false;
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    timeCounter = 0;
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    if (m_executeMoveCoroutine != null)
                    {
                        StopCoroutine(m_executeMoveCoroutine);
                        m_executeMoveCoroutine = null;
                    }
                    m_agent.Stop();
                    m_character.physics.SetVelocity(Vector2.zero);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_agent.Stop();
                    m_executeMoveCoroutine = StartCoroutine(ExecuteMove(m_info.meleeAttack.range));
                    break;

                case State.Chasing:
                    //m_attackDecider.DecideOnAttack();
                    m_agent.Stop();
                    m_character.physics.SetVelocity(Vector2.zero);
                    m_stateHandle.SetState(State.Attacking);
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_chargeFacing)
                    {
                        m_stateHandle.SetState(State.chargeIdle);
                    }
                    else
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
        }

        public void ResetAI()
        {
            m_targetInfo.Set(null, null);
            m_stateHandle.OverrideState(State.ReturnToPatrol);
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