using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pathfinding;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minions/SeedOfTheOne")]
    public class SeedOfTheOneAI : CombatAIBrain<SeedOfTheOneAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_idle = new MovementInfo();
            public MovementInfo idle => m_idle;

            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_patrol = new MovementInfo();
            public MovementInfo patrol => m_patrol;
            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_panic = new MovementInfo();
            public MovementInfo panic => m_panic;

            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;

            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_movingAnimation;
            public string movingAnimation => m_movingAnimation;

            [SerializeField, BoxGroup("Animation"), ValueDropdown("GetAnimations")]
            private string m_panicAnimation;
            public string panicAnimation => m_panicAnimation;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_idle.SetData(m_skeletonDataAsset);
                m_patrol.SetData(m_skeletonDataAsset);
                m_panic.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Patrol,
            Panic,
            Turning,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Rigidbody2D m_rigidbody2D;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Health m_health;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Panic Points")]
        private List<Vector2> m_panicPoints;
        [SerializeField, TabGroup("Panic Points")]
        private SeedOfTheOneRetreatPointsConfiguration m_retreatPoints;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;
        private Vector2 m_startPos;

        private Coroutine m_executeMoveCoroutine;

        [SerializeField]
        private float m_waitToReturnToPatrolTime;

        [SerializeField]
        private bool m_isRetreating;
        private Vector2 m_currentRetreatPoint;

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        private void OnTakesDamage(object sender, Damageable.DamageEventArgs eventArgs) => m_stateHandle.SetState(State.Panic);

        public void SetAI(AITargetInfo targetInfo)
        {
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
            m_flinchHandle.m_autoFlinch = true;
            m_agent.Stop();
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_stateHandle.Wait(State.ReevaluateSituation);
            StopAllCoroutines();
            //if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation)
            //{
            //    //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            //}
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_flinchHandle.m_autoFlinch = false;
                m_stateHandle.ApplyQueuedState();
            }

            m_stateHandle.SetState(State.Panic);
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

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            if (m_executeMoveCoroutine != null)
            {
                StopCoroutine(m_executeMoveCoroutine);
                m_executeMoveCoroutine = null;
            }
            if (IsFacingTarget())
                CustomTurn();

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
            //m_animation.SetAnimation(0, m_info.deathStartAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathStartAnimation);
            m_character.physics.simulateGravity = true;
            //m_animation.SetAnimation(0, m_info.deathLoopAnimation, true);
            //m_animation.SetAnimation(0, m_info.deathEndAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathEndAnimation);
            enabled = false;
            m_bodyCollider.enabled = false;
            this.gameObject.SetActive(false);
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            yield return null;
        }

        protected override void Start()
        {
            base.Start();
            m_animation.SetAnimation(0, m_info.patrol.animation, true);
            //m_animation.DisableRootMotion();
            m_bodyCollider.enabled = false;
            m_startPos = transform.position;

            for (int i = 0; i < m_retreatPoints.retreatPoints.Length; i++)
            {
                m_panicPoints.Add(m_retreatPoints.retreatPoints[i]);
            }
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_hitbox.damageable.DamageTaken += OnTakesDamage;

            //m_deathHandle?.SetAnimation(m_info.deathStartAnimation);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Patrol:
                    m_turnState = State.ReevaluateSituation;
                    m_isRetreating = false;

                    m_animation.SetAnimation(0, m_info.movingAnimation, true);

                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);

                    m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                    if (m_executeMoveCoroutine != null)
                    {
                        StopCoroutine(m_executeMoveCoroutine);
                        m_executeMoveCoroutine = null;
                    }
                    m_agent.Stop();
                    m_turnHandle.Execute();
                    break;

                case State.Panic:
                    m_turnState = State.ReevaluateSituation;

                    if (m_targetInfo.isValid)
                    {
                        StartCoroutine(PanicRoutine(SetRetreatPosition()));
                    }

                    break;

                case State.ReevaluateSituation:
                    if (m_targetInfo.isValid)
                    {
                        if (m_isRetreating)
                        {
                            m_stateHandle.SetState(State.Panic);
                        }
                        else
                        {
                            m_stateHandle.SetState(State.Patrol);
                        }
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;

                case State.WaitBehaviourEnd:
                    return;
            }
        }

        private IEnumerator PanicRoutine(Vector2 retreatPoint)
        {
            m_agent.OnDestinationReached += OnPanicPointReach;
            m_agent.SetDestination(retreatPoint);
            m_isRetreating = true;

            while (m_isRetreating)
            {
                m_agent.Move(m_info.panic.speed);
                yield return null;
            }

            m_agent.OnDestinationReached -= OnPanicPointReach;

            yield return null;

            void OnPanicPointReach(object sender, EventActionArgs eventArgs)
            {
                Debug.Log("Panic Point Reached");
                m_isRetreating = false;
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                StartCoroutine(ReturnToPatrolPathRoutine());
            }
        }
        
        private IEnumerator ReturnToPatrolPathRoutine()
        {
            bool canPanic = true;

            for (int i = 0; i < m_waitToReturnToPatrolTime; i++)
            {
                yield return new WaitForSeconds(1f);

                if (canPanic)
                {
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Panic);
                    }
                }
            }

            canPanic = false;

            if (!canPanic)
            {
                m_stateHandle.OverrideState(State.ReevaluateSituation);
            }
        }

        private Vector2 SetRetreatPosition()
        {
            int chosenIndex = Random.Range(0, m_panicPoints.Count);

            m_currentRetreatPoint = m_panicPoints[chosenIndex];

            m_isRetreating = true;
            m_animation.SetAnimation(0, m_info.panicAnimation, true);

            return m_currentRetreatPoint;
        }

        public override void ReturnToSpawnPoint()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnForbidFromAttackTarget()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnTargetDisappeared()
        {
            throw new System.NotImplementedException();
        }

    }
}

